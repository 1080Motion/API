
using CustomerApiClientSample;

using Grpc.Core;

if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: CustomerApiClientSample.exe <ApiKey>");
    return -1;
}

string apiKey = args[0];
try
{
    // Setup a client and login to the API
    //string apiHost = "https://localhost:8585";
    //string apiHost = "https://cgrpc.1080motion.com";
    string apiHost = "https://1080cgrpc.azurewebsites.net/";
    var apiClient = new CustomerApiClient(new Uri(apiHost));
    await apiClient.Initialize(apiKey);

    Console.WriteLine("Downloading instructor & clients");
    var instructor = await apiClient.DownloadInstructor();
    Console.WriteLine("Instructor = {0} {1}", instructor.Firstname, instructor.Lastname);
    var clients = await apiClient.DownloadAllClients();
    Console.WriteLine("{0} clients received", clients.Count);
    foreach (var client in clients)
    {
        Console.WriteLine("  Client: {0} ({1})", client.DisplayName, client.Gender);
    }

    return 0;
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
{
    Console.Error.WriteLine("The 1080 API is not available. Check your internet connection");
    return -2;
}
catch (Exception ex)
{
    Console.Error.WriteLine("Error talking to the 1080 API: {0}", ex.Message);
    return -2;
}

