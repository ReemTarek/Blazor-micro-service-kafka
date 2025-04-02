# Ecommerce Product Service

## Overview

This project is a part of an e-commerce application that handles product-related operations. It is built using .NET 9 and Blazor, and it includes a Kafka consumer for processing orders.

## Features

- **Product Management**: Provides APIs to manage products.
- **Kafka Integration**: Consumes messages from a Kafka topic to update product quantities.
- **Swagger**: API documentation and testing using Swagger.

## Prerequisites

- .NET 9 SDK
- Docker (for running Kafka and Zookeeper)
- SQL Server (or any other database supported by Entity Framework Core)

## Getting Started

### Setting Up the Database

1. Update the connection string in `appsettings.json` to point to your database.
2. Run the following commands to apply migrations and create the database:
dotnet ef database update

### Running Kafka and Zookeeper

1. Ensure Docker is installed and running.
2. Navigate to the directory containing `kafka.yml`.
3. Run the following command to start Kafka and Zookeeper:

docker-compose -f kafka.yml up

### Running the Application

1. Navigate to the project directory.
2. Run the application using the following command:
dotnet run

3. The application will be available at `https://localhost:5001`.

### Accessing Swagger

Navigate to `https://localhost:5001/swagger` to access the Swagger UI for API documentation and testing.

## Project Structure

- **Controllers**: Contains API controllers for managing products.
- **Data**: Contains the `ProductDbContext` for Entity Framework Core.
- **Kafka**: Contains the Kafka consumer for processing orders.
- **Models**: Contains the data models used in the application.

## Kafka Consumer

The Kafka consumer listens to the `order-topic` and updates product quantities based on the orders received. It is implemented in the `KafkaConsumer` class.

## Refrence:
https://www.youtube.com/watch?v=CbDgOlqBvrs
   
