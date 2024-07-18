﻿
using CustomerApiClientSample;

using Grpc.Core;

string defaultHost = "https://publicapi-grpc.1080motion.com";

if (args.Length < 1 || args.Any(a => a == "-h" || a == "--help"))
{
    Console.Error.WriteLine("Usage: CustomerApiClientSample.exe <ApiKey> [host]");
    Console.Error.WriteLine("If no host is specified, the client will connect to {0}", defaultHost);
    return -1;
}

string apiKey = args[0];
try
{
    // Setup a client and login to the API
    //string apiHost = "https://localhost:8585";
    string apiHost = args.Length > 1 ? args[1] : "https://cgrpc.1080motion.com";
    var apiClient = new CustomerApiClient(new Uri(apiHost));
    Console.WriteLine("Connecting to API at {0}", apiHost);
    await apiClient.Initialize(apiKey);

    Console.WriteLine("Downloading instructor & clients");
    var instructor = await apiClient.DownloadInstructor();
    Console.WriteLine("Instructor = {0} {1}", instructor.Firstname, instructor.Lastname);
    var clients = await apiClient.DownloadAllClients();
    Console.WriteLine("{0} clients received", clients.Count);
    foreach (var client in clients)
    {
        Console.WriteLine("  Client: {0} ({1})", client.DisplayName, client.Gender);


        // Demo how to download and process a session including all its training data:
        var mostRecentSession = await apiClient.DownloadMostRecentSessionForClient(client, downloadReps: true);
        Console.WriteLine("   - Most recent session: {0} ({1}, {2} exercises):", mostRecentSession.Id,
            mostRecentSession.Created, mostRecentSession.Exercises.Count);
        foreach (var exercise in mostRecentSession.Exercises)
        {
            Console.WriteLine("     - Exercise {0} ({1}) - {2} sets", exercise.ExerciseId, exercise.ExerciseName,
                exercise.Sets.Count);
            foreach (var set in exercise.Sets)
            {
                Console.WriteLine("       - Set {0} ({1} reps)", set.Common.Guid, set.MotionGroups.Count);
            }
        }
    }


    return 0;
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
{
    Console.Error.WriteLine("The 1080 API is not available. Check your internet connection");
    return -2;
}
catch (RpcException ex)
{
    Console.Error.WriteLine("Error talking to the 1080 API: {0}", ex.Message);
    return -2;
}
catch (Exception ex)
{
    Console.Error.WriteLine("Unhandled exception: {0}", ex.Message);
    return -2;
}

