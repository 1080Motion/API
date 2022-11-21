using Grpc.Core;
using Grpc.Net.Client;

using TEM.Proto;

namespace CustomerApiClientSample;

public class CustomerApiClient
{
    private readonly Uri _baseUri;
    /// <summary>
    /// The authenticated communication channel to the customer API services
    /// </summary>
    private GrpcChannel? _channel;

    public CustomerApiClient()
     : this(new Uri("https://cgrpc.1080motion.com"))
    { }
    
    public CustomerApiClient(Uri apiUri)
    {
        _baseUri = apiUri;
    }

    /// <summary>
    /// Connects to the gRPC service and performs a login using the specified API key.
    /// </summary>
    /// <param name="apiKey">The API key that provides access ot 1080 data.</param>
    public async Task Initialize(string apiKey)
    {
        using var channel = GrpcChannel.ForAddress(_baseUri, new GrpcChannelOptions()
        {
            HttpClient = new HttpClient()
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
    
}