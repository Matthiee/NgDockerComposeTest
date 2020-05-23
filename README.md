# NgDockerComposeTest
Docker, Angular, ASP.NET Core, Redis, MySQL - Basic Setup

## Description

This project runs an Angular front-end and ASP.NET Core back-end. 
It uses MySQL for data storage and Redis to cache api responses for 10 seconds. 

Docker-compose is used to spin up the Site, MySQL and Redis. 

## Setup
 - `cd DockerTest/Client`
 - `npm i`
 - `build-prod`
 - `cd ..`
 - `docker-compose up`
 - `docker-compose ps` to find the port the API/Angular is running on
 - Navigate to `https://localhost:[port]/`
 - To shut down `docker-compose down`

### Technologies used
| Technology            | Version |
|:----------------------|--------:|
| Angular               | 9.0.5   |
| ASP.NET Core          | 3.1     |
| Redis                 | redis:alpine3.11  |	
| MySQL                 | mysql:5.7  |		