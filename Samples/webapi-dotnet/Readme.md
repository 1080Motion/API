# 1080 Public API Sample Client (Web API using .NET)

This is a sample client using the 1080 Motion web based public API.

All calls are made using .NET `HttpClient` and payloads are deserialized from Json into C# objects defined in the `DataTransferObjects.cs` file. 
This file is generated from the swagger definition of the API using the [NSwagStudio tool](https://github.com/RicoSuter/NSwag/wiki/NSwagStudio).

## How it works

The code expects the **API key** to be passed on the command line. If you don't have one, contact 1080 Motion support.

For example (*the API key here is not valid - replace with your own working API key*)

    WebApiSample.exe EF3515B6-D6A5-46E2-A925-5006D8FFC2E2

The API is linked to a specific instructor account, and all calls using that API key grants access to data
under that instructor.


## Code Structure

* The `DataTransferObjects.cs` file contains the classes that are used to deserialize the Json payloads from the API.
* The `ApiClient.cs` file contains some example code to query the API and return the results as C# objects to the caller
* The `CsvApiClient.cs` file contains some example code that downloads training data (reps from sessions) and writes them to CSV files on disk.
  * This can be used as a starting point for customers who want to automate CSV exports instead of using the web app to manually do this
* `Program.cs` contains the entry point of the program. 

