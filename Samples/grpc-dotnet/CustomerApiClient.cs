using Grpc.Core;
using Grpc.Net.Client;

using TEM.Proto;

namespace CustomerApiClientSample;

public class CustomerApiClient
{
    public const string DefaultHost = "https://publicapi-grpc.1080motion.com";
    private readonly Uri _baseUri;
    /// <summary>
    /// The authenticated communication channel to the customer API services
    /// </summary>
    private GrpcChannel? _channel;

    public CustomerApiClient()
     : this(new Uri(DefaultHost))
    { }
    
    public CustomerApiClient(Uri apiUri)
    {
        _baseUri = apiUri;
    }

    public string Host => _baseUri.ToString();
    /// <summary>
    /// Connects to the gRPC service and performs a login using the specified API key.
    /// </summary>
    /// <param name="apiKey">The API key that provides access ot 1080 data.</param>
    public async Task Initialize(string apiKey)
    {
        using var channel = GrpcChannel.ForAddress(_baseUri, new GrpcChannelOptions()
        {
            // By specifying a handler here we get around some misbehaving proxies that block HTTP2 traffic
            // The downside is we don't get load balancing, but the 1080 API does not yet have that 
            // enabled so it's not an actual problem.
            HttpHandler = new HttpClientHandler()
        });
    
        // Login first
        var authClient = new AuthService.AuthServiceClient(channel);
        var authResponse = await authClient.GetTokenAsync(new GetTokenRequest() { ApiKey = apiKey });

        _channel = CreateAuthenticatedGrpcChannel(authResponse.Token);
    }

    private GrpcChannel CreateAuthenticatedGrpcChannel(string token)
    {
        // The credential
        var credentials = CallCredentials.FromInterceptor((context, metadata) =>
        {
            if (!string.IsNullOrEmpty(token))
            {
                // Add the "Authorization: ..." header and attach to all HTTP requests
                metadata.Add("Authorization", $"1080Motion {token}");
            }
            return Task.CompletedTask;
        });

        var channel = GrpcChannel.ForAddress(_baseUri, new GrpcChannelOptions
        {
            Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
        });
        return channel;
    }

    /// <summary>
    /// Makes an asynchronous call to the GetInstructor remote procedure.
    /// The actual instructor downloaded is given by the API key used during initialization: Each API key grants access to one instructor. 
    /// </summary>
    public async Task<Instructor> DownloadInstructor()
    {
        if (_channel == null)
            throw new InvalidOperationException("Not yet initialized");

        var client = new InstructorService.InstructorServiceClient(_channel);
        var response = await client.GetInstructorAsync(new GetInstructorRequest());
        return response.Instructor;
    }

    /// <summary>
    /// Makes an asynchronous call to the GetAllClients remote procedure.
    /// All clients for the currently logged in instructor will be downloaded.
    /// </summary>
    public async Task<IList<Client>> DownloadAllClients()
    {
        if (_channel == null)
            throw new InvalidOperationException("Not yet initialized");

        var client = new ClientService.ClientServiceClient(_channel);
        var response = await client.GetAllClientsAsync(new GetAllClientsRequest());
        return response.Clients;
    }


    public async Task<IList<ExerciseType>> DownloadExerciseLibrary()
    {
        if (_channel == null)
            throw new InvalidOperationException("Not yet initialized");

        var client = new ExerciseTypeService.ExerciseTypeServiceClient(_channel);
        var response = await client.GetAllExerciseTypesAsync(new GetAllExerciseTypesRequest());
        return response.ExerciseTypes;
    }
    
    public async Task<SessionInfo?> DownloadMostRecentSessionForClient(Client client, bool downloadReps)
    {
        if (_channel == null)
            throw new InvalidOperationException("Not yet initialized");
        
        string clientId = client.Common.Guid;
        
        var sessionServiceClient = new SessionService.SessionServiceClient(_channel);
        var req = new GetAllSessionsForClientRequest()
            { ClientGuid = clientId };
        // First get the list of sessions for this client. The returned data does only includes the basic session info (no list of exercises or sets or reps)
        var response = await sessionServiceClient.GetAllSessionsForClientAsync(req);
        if (response.Sessions.Count == 0)
            return null;

        // Download the exercise library so we can match exercise type ids with the name of the exercise
        var allExercises = await DownloadExerciseLibrary();
        var sessionId = response.Sessions[0].Common.Guid;
        
        // This will download more detailed information for a session (but does not include the actual reps)
        var data = await sessionServiceClient.GetDataForSessionAsync(new GetDataForSessionRequest(){SessionGuid = sessionId});
        var sessionInfo = new SessionInfo( response.Sessions[0], data, allExercises);

        if (downloadReps && sessionInfo.HasAnySets())
        {
            // Finally, download the actual training data (reps)
            var setServiceClient = new SetService.SetServiceClient(_channel);
            var sets = await setServiceClient.GetSetsForSessionAsync(new GetSetsForSessionRequest
                { SessionGuid = sessionId });

            sessionInfo.SetTrainingData(sets);
        }

        return sessionInfo;
    }
}


public class SessionInfo
{
    public SessionInfo(
        Session session,
        GetDataForSessionResponse sessionData,
        IList<ExerciseType> exerciseLibrary)
    {
        Id = new Guid(session.Common.Guid);
        Created = session.Common.Created.ToDateTimeOffset();
        foreach (var exercise in sessionData.Exercises)
        {
            var exerciseType = exerciseLibrary.FirstOrDefault(et => et.Common.Guid == exercise.ExerciseTypeGuid);
            if (exerciseType is null)
                continue;
            IEnumerable<Set> sets = sessionData.Sets.Where(s => s.ExerciseGuid == exercise.Common.Guid);
            var exerciseInfo = new ExerciseInfo(exercise, exerciseType, sets);
            Exercises.Add(exerciseInfo);
        }
    }
    
    public List<ExerciseInfo> Exercises { get; } = new();
    public Guid Id { get; }
    public DateTimeOffset Created { get; }

    public void SetTrainingData(GetSetsForSessionResponse trainingData)
    {
        foreach (Set s in trainingData.Sets)
        {
            foreach (ExerciseInfo exercise in Exercises)
            {
                if (exercise.TryUpdateSet(s))
                    break;
            }
        }
    }

    public bool HasAnySets()
    {
        return Exercises.Any(e => e.Sets.Count > 0);
    }
}

public class ExerciseInfo
{
    public ExerciseInfo(Exercise e, ExerciseType type, IEnumerable<Set> sets)
    {
        ExerciseId = new Guid(e.Common.Guid);
        ExerciseName = type.Name;
        Sets.AddRange(sets);
    }
    public Guid ExerciseId { get; }
    public string ExerciseName { get; }
    
    public List<Set> Sets { get; } = new();

    public bool TryUpdateSet(Set newSet)
    {
        for (var i = 0; i < Sets.Count; i++)
        {
            Set? existingSet = Sets[i];
            if (existingSet.Common.Guid == newSet.Common.Guid)
            {
                Sets[i] = newSet;
                return true;
            }
        }

        return false;
    }
}
//
// public class SetInfo
// {
//     public SetInfo(Set s)
//     {
//         s.
//     }
//     public IList<MotionGroup> Reps { get; }
// }