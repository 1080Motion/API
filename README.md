# 1080Motion API (beta)
Welcome to the official 1080Motion API! Please note the API is still in beta but but we are working on making it production ready as soon as possible.

The API url is currently located at:  
[https://cgrpc.1080motion.com](https://cgrpc.1080motion.com)

## Introduction
The 1080Motion API allows authenticated clients to query data that is stored in our databases. Using the API customers and/or third party athlete management systems can easily integrate the API into their platforms.

## Design
The API is built using [gRPC](https://grpc.io) and [Protocol buffers](https://developers.google.com/protocol-buffers/docs/proto3) (proto 3).


## Usage
To start using the API you first need to download the proto files found in the [Protos](https://github.com/1080Motion/API/tree/master/Protos) folder. Based on these proto files you then need to generate client code for the specific programming language you will be using. After this setup is complete you are ready to start making queries!  

## API key
To access data for an account you also need an api key related  to that account. To obtain this key please contact 1080Motion.

## Documentation
For more details and instructions of how to use the API please visit the [wiki](https://github.com/1080Motion/API/wiki) pages
