
using System.Net;

using WebApiSample;


string defaultHost = "https://publicapi.1080motion.com/";

// Allow the user to specify the API key and host on the command line, but use the default host if its not specified 
// API key must always be specified.

if (args.Length < 1 || args.Any(a => a is "-h" or "--help"))
{
    Console.Error.WriteLine("Usage: {0} <ApiKey> [host]", Path.GetFileName(Environment.ProcessPath));
    Console.Error.WriteLine("If no host is specified, the client will connect to {0}", defaultHost);
    return -1;
}

string apiKey = args[0];

try
{
    // Setup a client and login to the API
    var apiHost = new Uri(args.Length > 1 ? args[1] : defaultHost);
    var client = new ApiClient(apiHost, apiKey);
    await client.PrintAllClients();
    await client.PrintAllExercises();
    await client.PrintSessionsFromToday();

    // Download some training data and save it to CSV files
    var csvClient = new CsvApiClient(apiHost, apiKey);
    await csvClient.DownloadSetSummaryToCsv();
    await csvClient.DownloadRepSamplesToCsv();
    
    return 0;
}
catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.ServiceUnavailable)
{
    Console.Error.WriteLine("The 1080 Web API is not available. Check your internet connection");
    return -2;
}
catch (HttpRequestException ex) 
{
    Console.Error.WriteLine("Error talking to the 1080 Web API: {0}", ex.Message);
    return -2;
}
catch (Exception ex)
{
    Console.Error.WriteLine("Unhandled exception: {0}", ex.Message);
    return -2;
}