# 1080Motion API

Welcome to the official 1080Motion API! 

The API provides programmatic access to training data recorded on 1080 sprint and quantum machines. 
Using the API customers and/or third party athlete management systems can easily integrate the API into their platforms.

For more details and instructions of how to use the API please visit the [wiki](https://github.com/1080Motion/API/wiki) pages

## Accessing the API

> **Note:** There are two flavors of the API: a REST based API using basic HTTP requests, and one using gRPC and protocol buffers. 
> The recommended flavor is the REST API because it is easier to interact with. 

The HTTP/REST API is hosted at [https://publicapi.1080motion.com](https://publicapi.1080motion.com).

The gRPC API is hosted at [https://publicapi-grpc.1080motion.com/](https://publicapi-grpc.1080motion.com/).

### API key
To access data for an account you also need an api key related to that account. These keys can be created
by the administrator for the instructor account from the [1080 web app](https://webapp.1080motion.com). 
After logging in as an administrator, click on the name in the top right corner and select "API Keys" in the drop-down menu.

If you have questions or cannot generate keys, contact 1080Motion support.

The same API key is used on both flavors APIs

See the [wiki page on authentication](https://github.com/1080Motion/API/wiki/Authentication) for more details.

### Getting started

The HTTP API can be tested without any additional tooling by 
accessing [the swagger page](https://publicapi.1080motion.com/swagger/index.html) hosted on the API Server.

It's also possible to use a tool such as [Postman](https://www.postman.com/) to query the API.

To use the gRPC API, you need to [generate code from the .proto files](https://github.com/1080Motion/API/wiki/Client-code-generation-(grpc))

## Why two APIs?

Historical reasons. Technically, the gRPC based API has better performance thanks to efficient data serialization and
code generation.

The negative side is that it's a steep learning curve to get started, especially from higher level languages such as 
python or R which many want to use.

This lead us to develop the new, simpler web API.

For the time being, both flavors of the API will continue to live side by side.

Read more about gRPC over at: 
- https://grpc.io
- https://developers.google.com/protocol-buffers/docs/proto3

