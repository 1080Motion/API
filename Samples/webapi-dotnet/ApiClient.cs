using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json;

using WebApiSample.DataTransferObjects;

namespace WebApiSample;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    
    public ApiClient(Uri apiHost, string apiKey)
    {
        // Setup the HttpClient that is pre-configured with the address of the public API and
        // make it send the API key with every request.
        // This means we only need to specify the relative path when we make HTTP requests
        _httpClient = new HttpClient()
        {
            BaseAddress = apiHost,
        };
        _httpClient.DefaultRequestHeaders.Add("X-1080-API-Key", apiKey);
    }

    public async Task PrintAllClients()
    {
        // Get all clients and deserialize them into an array of PublicClient objects
        var clients = await _httpClient.GetFromJsonAsync<PublicClient[]>("Client");
        if (clients is null || clients.Length == 0)
        {
            Console.WriteLine("No clients found");
            return;
        }

        Console.WriteLine("*** Client Roster ***");
        foreach (var client in clients)
        {
            Console.WriteLine("Name: {0}, Id: {1}", client.DisplayName, client.Id);
        }
    }
    
    
    public async Task PrintAllExercises(bool includePublic = true, bool includePrivate = true)
    {
        // Get all exercises and deserialize them into an array of PublicExerciseType objects.
        // The parameters passed to this function is passed directly to the API in the HTTP request.
        var exerciseTypes = await _httpClient.GetFromJsonAsync<PublicExerciseType[]>($"ExerciseType?includePublic={includePublic}&includePrivate={includePrivate}");
        if (IsEmpty(exerciseTypes))
        {
            Console.WriteLine("No exercises found");
            return;
        }

        Console.WriteLine("*** {0} exercises found in exercise library ***", exerciseTypes.Length);
        foreach (var exercise in exerciseTypes)
        {
            Console.WriteLine("Name: {0}, Id: {1} ({2})", exercise.Name, exercise.Id, exercise.IsPublic ? "Public" : "Private");
        }
    }
    
    
    public async Task PrintSessionsFromToday()
    {
        // First get the list of sessions that are less than 1 day old
        var sessions = await _httpClient.GetFromJsonAsync<SessionInfo[]>("Session?maxAgeDays=1");
        if (sessions is null || sessions.Length == 0)
        {
            Console.WriteLine("No sessions found today");
            return;
        }

        // Get list of clients so we can print the name of the client in the output (otherwise we only have the id)
        PublicClient[]? clients = await _httpClient.GetFromJsonAsync<PublicClient[]>("Client");
        
        Console.WriteLine("*** Sessions from the last 24 hours ***");
        foreach (var session in sessions)
        {
            // Download more information for this session. This includes the list of exercises and sets inside the session
            PublicSession? details = await _httpClient.GetFromJsonAsync<PublicSession>($"Session/{session.Id}");
            if (details is null)
            {
                // This should never happen since we just got the session info from the same API
                Console.WriteLine("ERROR: Could not get session details for session {0}", session.Id);
                continue;
            }
            
            var client = clients?.FirstOrDefault(c => c.Id == details.ClientId);
            Console.WriteLine("Session {0} for client {1}:", client?.DisplayName ?? "<???>", details.Created);
            if (IsEmpty(details.Exercises))
            {
                Console.WriteLine("  No exercises");
                continue;
            }
            
            Console.WriteLine("  {0} exercises, with {1} sets in total", details.Exercises.Count, details.Exercises.Sum(e => e.Sets?.Count ?? 0));
            foreach (var e in details.Exercises)
            {
                Console.WriteLine("    Exercise {0} ({1} sets)", e.Id, e.Sets?.Count ?? 0);
            }
        }
    }
    
    private static bool IsEmpty<T>([NotNullWhen(false)] ICollection<T>? collection) => collection is null || collection.Count == 0;
}