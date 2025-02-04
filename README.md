<div style="display: inline-block; padding: 0px 10px;" align="center">
    <img src="https://miro.medium.com/v2/resize:fit:1024/1*3391_Gz2SDKmo50hWvzfUg.png" width="50" height="50">
    <img src="![image](https://github.com/user-attachments/assets/df39fd4c-a8e7-46a0-8fe0-9c3c8593755d)" width="50" height="50">
    <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSt0WIiLfY2fqHKOWO_jDOoHVhFU_t9QSZKEg&s" width="50" height="50">
    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Git_icon.svg/2048px-Git_icon.svg.png" width="50" height="50">
    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/9/9a/Visual_Studio_Code_1.35_icon.svg/2048px-Visual_Studio_Code_1.35_icon.svg.png" width="50" height="50">
</div>

## For development (Localhost)

Dowonload and install all dependency and follow the steps.

1) Downloads

    - [.Net SDK 9.0.102](https://dotnet.microsoft.com/es-es/download/dotnet/9.0)

    - [MySQL Community 9.1.0](https://downloads.mysql.com/archives/community/)

    - [GIT Lastest](https://git-scm.com/downloads)
    
    Cloud for all documents
    - [SharePoint](#)

    We recommend use Visual Studio Code IDE
    - [Visual Studio Code (lastest version)](https://code.visualstudio.com/download).

    For testing Back End API's
    - [Postaman (lastest version)](https://www.postman.com/downloads/).

2) After installations, run MySQL Workbench on localhost.

3) Make sure don't have a database created "nom_db"

3) Clone this repository on your pc

        $ git clone https://github.com/ISCLuisRamirez/BENom.git

    switch to "develop" branch 

        $ git checkout develop

    or make it local

        $ git branch develop -m

    last pull from origin

        $ git pull origin develop

4) Create file appsettings.json on "/" with the data of the database 

        ${
        $    "ConnectionStrings": {
        $        "DefaultConnection": "server=localhost;port=3306;database=nom_db;user=YOURLOCALUSER;password=YOURLOCALDATABASEPASSWORD"
        $    },
        $    "Jwt": {
        $        "Key": "C97cWeuVUvDwqMdRBC+fzo4Egiz2qE95KtWVSVQM0hI=",
        $        "Issuer": "BENom"
        $    },
        $    "Logging": {
        $        "LogLevel": {
        $        "Default": "Information",
        $        "Microsoft.AspNetCore": "Warning"
        $        }
        $    },
        $    "AllowedHosts": "*"
        $}

4) Install EntityFramework Tool

        $ dotnet tool install --global dotnet-ef

4) Install all dependencies on the project

        $ dotnet add package Microsoft.EntityFrameworkCore --version 6.0
        $ dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0
        $ dotnet add package Microsoft.EntityFrameworkCore.Tools --version 6.0
        $ dotnet add package Pomelo.EntityFrameworkCore.MySql --version 6.0

5) Execute on terminal

        $ dotnet build