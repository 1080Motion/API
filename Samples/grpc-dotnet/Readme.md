# 1080 Public API Sample Client (gRPC in .NET)

This is a sample client for the 1080 Motion customer API.

It shows how to build a basic client using C#/.NET 6 

> The following guide from Microsoft explains how to create a new Grpc client project in Visual Studio or VS Code:
> 
> [https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start](https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start#create-the-grpc-client-in-a-net-console-app)


## How it works

The code expects the **API key** to be passed on the command line. If you don't have one, contact 1080 Motion support.

For example (*the API key here is not valid - replace with your own working API key*)

    CustomerApiClientSample.exe EF3515B6-D6A5-46E2-A925-5006D8FFC2E2

The API is linked to a specific instructor account, and all calls using that API key grants access to data
under that instructor.


## Code Structure

* The `API\Protos` folder contains a copy of the official proto files. 
    * They are compiled using the protobuf compiler `protoc` during build time. 
      The compiler produces both the payload classes and the client side service classes 
      that are used by the `CustomerApiClient` class.
    * This is done by selecting properties of the .proto file and specifying "Protobuf Compiler" as "Build Target".
      * Only `Client` code need to be generated
      * In the `.csproj` file, this is shown as `<protobuf Include="someprotofile.proto" GrpcServices="Client" />`
* The `CustomerApiClient` class contains the code that sets up the gRPC channel 
  and communicates with the 1080 customer api.
  * See comments on the individual methods for more details.
* `Program.cs` contains the entry point of the program. It parses the command line args uses the CustomerApiClient to 
  login, then download the list of clients for the current instructor.

