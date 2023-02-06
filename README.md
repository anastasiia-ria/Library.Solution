# Libary

#### Trackc location and status of user’s books in their home library.

#### By [Anastasiia Riabets](https://github.com/anastasiia-ria)

## Technologies Used

- CSS
- HTML
- C#
- .NET 5.0
- ASP.Net
- Razor
- dotnet script REPL
- MySQL
- Workbench
- EF Core

## Description

An application to track location and status of user’s books in their home library.

## Setup/Installation Requirements

- Clone this repository to your Desktop:
  1. Your computer will need to have GIT installed. If you do not currently have GIT installed, follow [these](https://docs.github.com/en/get-started/quickstart/set-up-git) directions for installing and setting up GIT.
  2. Once GIT is installed, clone this repository by typing following commands in your command line:
     ```
     ~ $ cd Desktop
     ~/Desktop $ git clone https://github.com/anastasiia-ria/Library.Solution.git
     ~/Desktop $ cd Library.Solution
     ```
- Install the [.NET 5 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
- Install packages and tools:
  ```
  ~/Desktop/Library.Solution/ $ dotnet restore
  ```
- Create appsettings.json file:
  ```
   ~/Desktop/Library.Solution $ cd Library
   ~/Desktop/Library.Solution/Library $ touch appsettings.json
   ~/Desktop/Library.Solution/Library $ echo '{
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Port=3306;database=library;uid=root;pwd=[PASSWORD];"
      }
    }' > appsettings.json
  ```
  [PASSWORD] is your password
- Create Database:
  ```
  ~/Desktop/Library.Solution/Library $ dotnet ef database update
  ```
- Build the project:
  ```
   ~/Desktop/Library.Solution/Library $ dotnet build
  ```
- Run the project
  ```
   ~/Desktop/Library.Solution/Library $ dotnet run
  ```
- Visit [http://library.dlinds.com:6001/](http://library.dlinds.com:6001/) to try this application

## Known Bugs

- Layout is not adjusted for the small screens

## Research & Planning Log

### Friday, 08/13

- 8:20: research Google Books API (they have different kinds of authorization, and I needed to figure out which one works best for this project).
- 9:30: research on how to let user either draw their bookshelves to mark their physical location, or how to drag shelves and books around.
- 10:30: work on proposal
- 11:00: work on README
- 11:15: research autocomplete from API in .NET (https://www.encodedna.com/webapi/aspdotnet-webapi-example-autocomplete-text-using-jquery.htm)
- 12:00: lunch
- 1:00: research on how to make MVC app live (Azure)
- 2:00: start working on MVP

## License

[ISC](https://opensource.org/licenses/ISC)

Copyright (c) 4/29/2022 Anastasiia Riabets
