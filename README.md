Stores user & system data, updated via Grpc, published via Kalfka.

## Flyway usage for db initialization
#### Download :
```bash
curl.exe -L -o flyway.zip https://repo1.maven.org/maven2/org/flywaydb/flyway-commandline/9.22.0/flyway-commandline-9.22.0-windows-x64.zip
```

#### Extract contents
```bash
Expand-Archive flyway.zip .\
```

##### Inside:
```
flyway.cmd
conf\
sql\
```

#### Set env path
```bash
setx PATH "$Env:PATH;$($PWD.Path)\flyway-9.22.0" /M
```