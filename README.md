# FoodFile
FoodFile is going to be decentralized food traceability solution. To find out more about the project visit [foodfile.org](https://foodfile.org).

## Command-Line options
All command line arguments are optional. The defaults (appSettings.json) are chosen to work with the [elasticsearch docker image](https://hub.docker.com/_/elasticsearch). It is recommended to provide your member-ID (--this).

### --mode
Specifies the execution mode of FoodFile. Can be one of regular, member or combined. The regular mode is for data recording and tracing. In member mode, the FoodFile instance serves as a membership provider (foodfile.org is a FoodFile instance running in member mode). As a participant of the FoodFile netowrk, choose regular mode. Combined mode is primarily intended as a convinience feature for development. Defaults to regular.

### --this
Specifies the member-ID of this FoodFile instance. This is recommended in regular and combined mode; It does not have any effect in member mode. In regular and combined mode, FoodFile will work without this information but features will be limited. You can obtain a member-ID from [foodfile.org/membership](https://foodfile.org/membership) or the membership service specified with the --members option.

### --members
Specifies the URL to the trusted membership provider. The provided URL is used in regular and combined mode; It does not have any effect in member mode. Defaults to https://foodfile.org/api/members/.

### --elastic
Specifies the Elasticsearch URL. Defaults to http://elasticsearch:9200/.

### --delay
Specifies a startup delay in seconds for the FoodFile application. Elasticsearch does not properly respond to requests right after startup. When executed simultaneously (as with docker-compose), the delay ensures that elasticsearch is ready when FoodFile sends the first request. Defaults to 60.