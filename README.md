# 🎓 University Explorer Portal

A Blazor Web Application (.NET 8, Interactive Server) that **combines three web APIs** so a prospective student can explore universities around the world and manage university applications.

This project was built for the *API Mashup* Blazor coursework. It is written to be simple and easy to read.

---

## Table of Contents
1. [Project Overview](#1-project-overview)
2. [Technologies Used](#2-technologies-used)
3. [APIs Used](#3-apis-used)
4. [Main Features](#4-main-features)
5. [Project Structure](#5-project-structure)
6. [How to Open and Run in Visual Studio 2022](#6-how-to-open-and-run-in-visual-studio-2022)
7. [How Each API Is Used](#7-how-each-api-is-used)
8. [How the Code Works (for explaining to your lecturer)](#8-how-the-code-works)
9. [Design Decisions and Notes](#9-design-decisions-and-notes)
10. [Screenshots](#10-screenshots)
11. [Uploading to GitHub with Visual Studio](#11-uploading-to-github-with-visual-studio)
12. [Troubleshooting](#12-troubleshooting)

---

## 1. Project Overview

The website lets a user:

1. Pick a **country** and see its details (flag, capital, region, population).
2. See the **universities** in that country, on the same page (this is the **API Mashup**: the Countries API + the Universities API).
3. **Search** universities while typing and open a university's **details**.
4. **Create**, **view** and **edit** university applications using the Edutots Student Application API.

## 2. Technologies Used

| Technology | Purpose |
|---|---|
| C# / .NET 8 | Programming language and framework |
| Blazor Web App (Interactive Server) | Building the website with Razor components |
| `HttpClient` (via `AddHttpClient`) | Calling the REST APIs |
| `EditForm` + Data Annotations | Forms and validation |
| Bootstrap 5 | Layout, cards, tables, navbar and responsive design |
| Visual Studio 2022 | Development environment |

No extra NuGet packages are needed.

## 3. APIs Used

| API | URL | Used for |
|---|---|---|
| Countries API | `https://countries.dev/countries` | Country dropdown and country details |
| Universities API | `http://universities.hipolabs.com/search?country={country}` | Universities of the selected country |
| Student Application API | `https://edutots.net/api/studentapplication` | Get, create and update applications |

## 4. Main Features

- **University Explorer** (`/university-explorer`): country dropdown → country details card → list of universities → live search → university details panel.
- **Student Applications** (`/student-applications`): table of all applications with an **Edit** button on every row.
- **Create Student Application** (`/student-applications/create`): validated form (POST) with a success message.
- **Edit Application** (`/student-applications/edit/{id}`): loads the current values, lets you change them (PUT) and redirects back to the list.
- **Validation** with Data Annotations (`[Required]`, `[EmailAddress]`, `[StringLength]`) and messages shown under each field.
- **Loading indicators** on countries, universities and applications.
- **Error handling**: a red "Unable to retrieve data" message (with a *Try again* button) if an API cannot be reached.
- **Reusable components** that talk to their parent page with `[Parameter]` and `EventCallback`.
- **Responsive design** (the menu collapses into a "hamburger" button on phones).

## 5. Project Structure

```
UniversityExplorerPortal.sln
README.md
Screenshots/                      <- put your 11 screenshots here
UniversityExplorerPortal/
├── Program.cs                    <- registers the 3 HttpClients (one per API)
├── Models/
│   ├── Country.cs
│   ├── University.cs
│   └── StudentApplication.cs     <- also holds the validation rules
├── Services/
│   ├── CountryService.cs
│   ├── UniversityService.cs
│   └── StudentApplicationService.cs
├── Components/
│   ├── CountryDetails.razor          <- shows the selected country
│   ├── UniversityCard.razor          <- summary of one university
│   ├── UniversityDetails.razor       <- full details of the selected university
│   ├── StudentApplicationForm.razor  <- used by BOTH Create and Edit pages
│   ├── CountrySelector.razor         <- the country dropdown
│   ├── StudentApplicationTable.razor <- the applications table
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor             <- navigation bar
│   └── Pages/
│       ├── Home.razor                    (/)
│       ├── UniversityExplorer.razor      (/university-explorer)
│       ├── StudentApplications.razor     (/student-applications)
│       ├── CreateStudentApplication.razor(/student-applications/create)
│       └── EditStudentApplication.razor  (/student-applications/edit/{id})
└── wwwroot/                      <- Bootstrap and app.css
```

## 6. How to Open and Run in Visual Studio 2022

**You need:** Visual Studio 2022 (version **17.8 or newer**) with the **"ASP.NET and web development"** workload installed (it includes .NET 8), and an **internet connection** (the project calls live APIs).

1. Unzip the project folder.
2. Open Visual Studio 2022 → **File → Open → Project/Solution…**
3. Select **`UniversityExplorerPortal.sln`** and click **Open**.
4. Wait for Visual Studio to finish restoring (a few seconds).
5. Press **F5** (or click the green ▶ button). Your browser opens on the Home page.
6. If Visual Studio asks you to trust the development certificate, click **Yes**.

To stop the app, close the browser tab and press **Shift + F5** in Visual Studio.

## 7. How Each API Is Used

| Action | Method | URL | Service method | Page |
|---|---|---|---|---|
| Load all countries | GET | `https://countries.dev/countries` | `CountryService.GetCountriesAsync()` | University Explorer |
| Load universities for a country | GET | `http://universities.hipolabs.com/search?country=Ireland` | `UniversityService.GetUniversitiesAsync(country)` | University Explorer |
| List applications | GET | `https://edutots.net/api/studentapplication` | `StudentApplicationService.GetApplicationsAsync()` | Student Applications |
| Load one application to edit | GET | `https://edutots.net/api/studentapplication` (then pick the matching Id) | `StudentApplicationService.GetApplicationAsync(id)` | Edit Application |
| Create an application | POST | `https://edutots.net/api/studentapplication` | `StudentApplicationService.CreateApplicationAsync(app)` | Create Student Application |
| Update an application | PUT | `https://edutots.net/api/studentapplication/{id}` e.g. `/5` | `StudentApplicationService.UpdateApplicationAsync(app)` | Edit Application |

Each API has its **own `HttpClient`**, set up in `Program.cs` with `AddHttpClient<...>()`. Every request also sends a `User-Agent` header, because some web servers reject requests without one.

## 8. How the Code Works

**The mashup (`UniversityExplorer.razor`)**
1. When the page opens, `OnInitializedAsync` calls `CountryService` (while a "Loading countries..." message is shown).
2. The `CountrySelector` component shows the dropdown. When the user picks a country it fires the `OnCountrySelected` **EventCallback** to the page.
3. The page finds that country in the list and shows it in `CountryDetails`.
4. The page then calls `UniversityService` with the country name and shows the result as `UniversityCard` components.
5. The search box uses `@bind="SearchText" @bind:event="oninput"`, so the list is filtered on every key press.
6. Clicking **View Details** on a card fires the `OnSelect` EventCallback; the page then shows `UniversityDetails`.

**Create and Edit share one form (`StudentApplicationForm.razor`)**
- The parent page passes in the `StudentApplication` object (a blank one for Create, the loaded one for Edit), the button text and an `OnSubmit` callback.
- The form is an `EditForm` with `DataAnnotationsValidator`. `OnSubmit` is only fired when every rule passes.
- Create → `POST`, then a green success message. Edit → `PUT`, then `NavigateTo("/student-applications")`.

**Loading and errors**: every page has `isLoading` and `hasError` variables. `try / catch / finally` blocks around each API call set them, and the page shows a spinner or a red alert accordingly.

## 9. Design Decisions and Notes

These are the places where the coursework brief and the real APIs differed, and what was decided:

- **Application Date.** The brief asks for an *Application Date*, but the live Edutots API currently returns no date field (it returns `id, studentName, email, country, university, course, personalStatement`). The form therefore asks for the date and **sends it** with every POST/PUT; the table shows it when the API returns one and shows `-` otherwise.
- **Personal Statement.** The API stores a `personalStatement` for every application, so the form includes it (required, max 500 characters).
- **One application by Id.** The brief only lists "GET all", so the Edit page loads the list and picks the application with the matching Id.
- **Country spellings.** The two country APIs do not always spell a country the same way (e.g. "United States of America" vs "United States"). `UniversityService` contains a small list of fixes and, if nothing is found, retries without any text in brackets. If a country still returns nothing, the page says *"No universities were found"* instead of crashing.
- **Only the first 50 universities are drawn** (use the search box to narrow down). Countries such as the USA have thousands of universities and drawing them all would be very slow.
- **Flags** are shown with the flag *image* from the Countries API. Emoji flags do not display on Windows (they appear as letters such as "IE").
- **The Universities API uses `http://`**. This is fine because the call is made by the server, not by the browser.
- **Prerendering is turned off** (`prerender: false` in `App.razor`) so each page loads its data once and the loading messages behave simply.
- **Namespace:** the project is called `UniversityExplorerPortal` because a page called `UniversityExplorer.razor` cannot live inside a namespace that is also called `UniversityExplorer`.
- The validation attribute for e-mail addresses in .NET is `[EmailAddress]` (the brief's `[Email]` does not exist).

## 10. Screenshots

> **How to add them:** run the app, press **Win + Shift + S** (Windows snipping tool), save each image into the **`Screenshots`** folder using the file name shown below, then commit and push. The images will then appear in this README automatically.

### 1. Home Page
`Screenshots/01-home-page.png`

![Home Page](Screenshots/01-home-page.png)

### 2. University Explorer Page
`Screenshots/02-university-explorer.png`

![University Explorer Page](Screenshots/02-university-explorer.png)

### 3. Country selected and details displayed
`Screenshots/03-country-details.png`

![Country selected and details displayed](Screenshots/03-country-details.png)

### 4. Universities loaded for a selected country
`Screenshots/04-universities-loaded.png`

![Universities loaded](Screenshots/04-universities-loaded.png)

### 5. University search functionality
`Screenshots/05-university-search.png`

![University search](Screenshots/05-university-search.png)

### 6. University details displayed
`Screenshots/06-university-details.png`

![University details](Screenshots/06-university-details.png)

### 7. Student Applications page (GET)
`Screenshots/07-student-applications-get.png`

![Student Applications (GET)](Screenshots/07-student-applications-get.png)

### 8. Create Student Application page (POST)
`Screenshots/08-create-application-post.png`

![Create Student Application (POST)](Screenshots/08-create-application-post.png)

### 9. Edit Student Application page (PUT)
`Screenshots/09-edit-application-put.png`

![Edit Application (PUT)](Screenshots/09-edit-application-put.png)

### 10. Loading state
`Screenshots/10-loading-state.png`

![Loading state](Screenshots/10-loading-state.png)

*How to capture it:* the "Loading..." message only shows for a moment. To get a screenshot, temporarily add `await Task.Delay(5000);` as the first line inside `CountryService.GetCountriesAsync()`, run the app, open **University Explorer** and take the screenshot. **Delete that line afterwards.**

### 11. Error handling state
`Screenshots/11-error-handling.png`

![Error handling state](Screenshots/11-error-handling.png)

*How to capture it:* temporarily change the Countries API address in `Program.cs` to `https://countries.invalid/`, run the app, open **University Explorer** and take the screenshot (you will see the red "Unable to retrieve data" message). **Change the address back afterwards.**

## 11. Uploading to GitHub with Visual Studio

The coursework needs a **private** repository, with commit history, shared with your lecturer.

### A) Create the private repository from Visual Studio
1. Open the solution in Visual Studio 2022.
2. In the top menu choose **Git → Create Git Repository…**
3. Sign in to your GitHub account when asked.
4. Fill in the dialog:
   - **Owner:** your GitHub username
   - **Repository name:** `UniversityExplorerPortal`
   - **Make sure "Private repository" is ticked.** ✅
   - Leave "Push your code to a new remote repository" ticked.
5. Click **Create and Push**.

A `.gitignore` file is already included, so build folders (`bin`, `obj`, `.vs`) are not uploaded.

### B) Invite your lecturer
1. Go to your repository on github.com → **Settings → Collaborators → Add people**.
2. Enter **chisom.anuforom@dorset.ie** and send the invitation.
3. Invite **only** this person.

### C) Make regular commits (the coursework marks the commit history)
After each piece of work (for example after finishing the explorer page):
1. Open **Git → Commit or Stash…** (the *Git Changes* window).
2. Type a short message, e.g. `Add university search`.
3. Click **Commit All** (or **Commit All and Push** to upload at once).
4. Use **Git → Push** to upload any commits that have not been pushed yet.

Example commit messages: `Create project and models` → `Add API services` → `Add University Explorer page` → `Add Student Applications pages` → `Add validation and error handling` → `Add README and screenshots`.

> When your screenshots are saved in the `Screenshots` folder, commit and push them too, otherwise they will not show in the README on GitHub.

## 12. Troubleshooting

| Problem | What to do |
|---|---|
| Visual Studio cannot open the project / .NET 8 missing | Open the **Visual Studio Installer** → *Modify* → tick **ASP.NET and web development** and update Visual Studio to 17.8 or newer. |
| "Unable to retrieve data" appears | Check your internet connection. The free public APIs are sometimes slow or offline; click **Try again** after a moment. |
| "No universities were found" for a country | The Universities API may have no data for that country, or spell its name differently. Try another country (e.g. Ireland, France, Canada). |
| Browser warns about the certificate | Click **Yes** when Visual Studio asks to trust the ASP.NET Core development certificate. |
| Port already in use | Stop the app (Shift + F5) and run it again. |
