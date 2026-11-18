# University Explorer Portal

## Overview

University Explorer Portal is a web application built with Blazor as part of the Full Stack Development CA2 assignment.
the idea behind the project is to bring university information and student applications together in one place. Users can explore universities in different countries, find out more about each institution and keep track of their applications without having to navigate between different websites.
The application combines information from the Countries API and the Universities API. It also connects to the Edutots Student Application API, which is used to retrieve, create and update student applications.

## What can you do with the application?
The main part of the application is the University Explorer. Users can select a country from a dropdown menu and immediately see relevant information, including its capital, region, population and flag. The universities available in that country are displayed alongside the country details.
There is also a search feature, making it easier to find a particular institution without having to go through the entire list. Selecting a university opens its details, where users can find its website and associated domains.
The application also includes a section for student applications. Existing applications can be viewed in a table, while new ones can be submitted through a form. Previously submitted applications can also be edited.

## External APIs

Three external API endpoints are used throughout the application.

| API | Purpose |
|---|---|
| [Countries API](https://countries.dev/countries) | Provides country information used in the country selector and details section. |
| [Universities API](https://universities.hipolabs.com/) | Returns universities according to the selected country. |
| [Edutots Student Application API](https://edutots.net/api/studentapplication) | Handles student application records. |

- **GET:** Retrieves the existing applications.
- **POST:** Sends a new application.
- **PUT:** Updates an existing application using its ID.

## Technologies and Tools

The application was developed using C# and Blazor, with Razor components handling the different sections of the interface.
Other technologies and tools used include:

- .NET
- HTML and CSS
- Bootstrap
- HttpClient for API requests
- REST APIs
- Git and GitHub for version control

## Application Structure

The project is divided into components, models and services to keep the code organised and make individual parts easier to maintain.

The `Components` folder contains the Razor files used to build the interface. Some of the main components are:

| Component | What it does |
|---|---|
| `CountrySelector.razor` | Handles country selection. |
| `CountryDetails.razor` | Displays information about the selected country. |
| `UniversityCard.razor` | Presents basic university information. |
| `UniversityDetails.razor` | Shows further details about a selected university. |
| `StudentApplicationForm.razor` | Provides the form used to create and edit applications. |
| `StudentApplicationTable.razor` | Displays the available student applications. |



## Running the Application

To run the project locally, you will need Visual Studio or the .NET SDK installed on your machine.

First, clone the repository:

```bash
git clone YOUR_GITHUB_REPOSITORY_URL
```

Open the project folder and restore the dependencies:

```bash
dotnet restore
```

Then start the application:

```bash
dotnet run
```

Once the application is running, open the local address provided in the terminal.

An internet connection is needed to retrieve information from the external APIs.

