# FoodFile
FoodFile is a decentralized food traceability solution. To find out more about the project visit [foodfile.org](https://foodfile.org).

## Technologies
FoodFile is written in F# for ASP.Net Core 3 and implements the MVC pattern. Data is stored in Elasticsearch; Every FoodFile instance must be tied together with an Elasticsearch cluster.
The frontend is built with React, Bootstrap and d3 for tree visualizations.

## Command-Line options
The defaults (appSettings.json) are chosen to work with the [elasticsearch docker image](https://hub.docker.com/_/elasticsearch). It is recommended to provide your member-ID (--this) and salt (--salt).

### --mode
Specifies the execution mode of FoodFile. Can be one of regular, member or combined. The regular mode is for data recording and tracing. In member mode, the FoodFile instance serves as a membership provider (foodfile.org is a FoodFile instance running in member mode). As a participant of the FoodFile netowrk, choose regular mode. Combined mode is primarily intended as a convinience feature for development. Defaults to regular.

### --this
Specifies the member-ID of this FoodFile instance. This is recommended in regular and combined mode; It does not have any effect in member mode. In regular and combined mode, FoodFile will work without this information but features will be limited. You can obtain a member-ID from [foodfile.org/membership](https://foodfile.org/membership) or the membership service specified with the --members option.

### --salt
Specifies the salt to use for sharing tokens. Provide a random string, semantically tied to your member-ID. Token based sharing os only secure if a salt is provided.

### --members
Specifies the URL to the trusted membership provider. The provided URL is used in regular and combined mode; It does not have any effect in member mode. Defaults to https://foodfile.org/api/members/.

### --elastic
Specifies the Elasticsearch URL. Defaults to http://elasticsearch:9200/.

### --delay
Specifies a startup delay in seconds for the FoodFile application. Elasticsearch does not properly respond to requests right after startup. When executed simultaneously (as with docker-compose), the delay ensures that elasticsearch is ready when FoodFile sends the first request. Defaults to 60.

## Run with docker-compose

The repository contains an example docker-compose.yml which can be downloaded and used with
```
docker-compose up
```
Note:
* In the yml, use the environment variables and the volume mount (commented out) to provide your SSL certificate to FoodFile / Kestrel.
* If you don't want to clone the repositry and build the container locally, you can comment the build option and uncomment the image option to fetch the image from Docker Hub instead.

If you want to start FoodFile in regular mode with docker-compose, make sure to provide your member-ID:
```
command: --this=<member-ID>
```
If you want to start FoodFile in member mode with docker-compose, set mode to member:
```
command: --mode=member
```

## Run with Docker
Before you run FoodFile with Docker, make sure that Elasticsearch is up and responsive.

To start in regular mode:
```
docker run leonardkoll/foodfile --this <member-ID> --elastic http://localhost:9200/ --delay 0
```

To start in member mode:
```
docker run leonardkoll/foodfile --mode member --elastic http://localhost:9200/ --delay 0
```

Since the Dockerfile is contained in the repository you may alternatively build the image locally:
```
git clone https://github.com/leonardkoll/FoodFile.git
docker build ./FoodFile
```

## Run as a native app
You will need [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) (SDK) or later to build and run the app. Before you run FoodFile, make sure that Elasticsearch is up and responsive.
```
git clone https://github.com/leonardkoll/FoodFile.git
dotnet build ./FoodFile
cd ./FoodFile/bin/Release/netcoreapp3.1
```

To start in regular mode:
```
dotnet FoodFile.dll --this <member-ID> --elastic http://localhost:9200/ --delay 0
```

To start in member mode:
```
dotnet FoodFile.dll --mode member --elastic http://localhost:9200/ --delay 0
```

## Notes for a production environment
* Be careful.
* This project is under developement. Password security does not comply with best practices, error handling is insufficient, features are missing, etc.
* The Elasticsearch setting "discovery.type=single-node" (docker-compose.yml) will skip bootstrap checks in most cases and is not suitable for production.
* The default indexcreationcmd setting (appSettings.json) specifies that the Elasticsearch indices for entities and members are created with zero replicas. Consider changing this to prevent data loss.
