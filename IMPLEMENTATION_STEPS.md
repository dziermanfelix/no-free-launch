# No-Free-Launch: Implementation Steps by Phase

Use this with the main plan. Each step is something **you** implement; the step describes *what* to do and *what* to learn, not full code.

---

## Phase 1: Backend foundation and SpaceX integration

### 1.1 – Environment and solution

- Install (if needed) and verify: .NET 8 SDK (`dotnet --version`), your IDE (VS Code / Rider / Visual Studio for Mac).
- In your repo root (e.g. `no-free-launch`), create a new solution: `dotnet new sln -n NoFreeLaunch`.
- Create the API project: `dotnet new webapi -n NoFreeLaunch.Api` (use minimal APIs or MVC; minimal is fine). Add it to the solution.
- Run the template app and confirm it starts (e.g. `dotnet run --project NoFreeLaunch.Api`).

### 1.2 – SpaceX HTTP client

- Add a folder or project for "infrastructure" or "clients" (e.g. `NoFreeLaunch.Infrastructure` class library, or a `Clients` folder inside the API project).
- Create an interface, e.g. `ISpaceXLaunchClient`, with a method like `Task<IReadOnlyList<LaunchDto>> GetLaunchesAsync(CancellationToken ct)` (and optionally `GetLaunchByIdAsync(string id)`).
- Implement it with `HttpClient`: call `GET https://api.spacexdata.com/v5/launches`. Use `System.Text.Json` to deserialize the response into a DTO/model that matches the API (or a subset of fields you care about).
- Register the client in `Program.cs`: register `HttpClient` for the SpaceX base address, register your interface as a typed client or scoped service. Use dependency injection to get `ISpaceXLaunchClient` in a controller or minimal endpoint and return raw JSON to verify the pipeline works.

### 1.3 – GraphQL with HotChocolate

- Add NuGet packages to the API project: `HotChocolate.AspNetCore`, `HotChocolate.Data` (if you want filtering later).
- In `Program.cs`, add `AddGraphQLServer()` and `MapGraphQL()`. Use the default path `/graphql`.
- Define a GraphQL type (e.g. `LaunchType`) that maps from your DTO: properties like `Id`, `Name`, `DateUtc`, `Success`, etc. You can start with a class and use HotChocolate's convention (property names become field names), or use the descriptor API.
- Create a query class (e.g. `LaunchQueries`) with a method `GetLaunches()` that returns `IEnumerable<LaunchType>` (or `IQueryable` if you use Data). Resolve the data by injecting `ISpaceXLaunchClient` and calling `GetLaunchesAsync()`.
- Add a second query, e.g. `GetLaunch(string id)`, that calls your client's get-by-id method.
- Start the app and open the GraphQL endpoint (e.g. `https://localhost:7xxx/graphql`). Use Banana Cake Pop (built into HotChocolate) or any GraphQL client to run `{ launches { id name dateUtc success } }` and `{ launch(id: "...") { id name } }` and confirm they return SpaceX data.

### 1.4 – Optional cleanup

- Move DTOs and the GraphQL type into a shared place (e.g. `NoFreeLaunch.Core` or a `Models` folder) if you want a cleaner structure. Ensure the API still runs and GraphQL still returns data.

**Phase 1 done when:** The .NET app runs, calls SpaceX REST API, and exposes `launches` and `launch(id)` via GraphQL; you can query it from Banana Cake Pop or Postman.

---

## Phase 2: SQL Server and "your" data

### 2.1 – SQL Server in Docker

- Install Docker Desktop (if not already). Pull and run SQL Server for Linux (e.g. `mcr.microsoft.com/mssql/server:2022-latest`) with required env vars (`ACCEPT_EULA`, `MSSQL_SA_PASSWORD`). Expose port 1433.
- Connect with a SQL client (Azure Data Studio, SSMS, or `sqlcmd`) and confirm you can run a simple query. Note the connection string (server, user, password, database name).

### 2.2 – Database schema

- Create a database (e.g. `NoFreeLaunchDb`). Design one or two tables, e.g. `Favorites` (e.g. `Id`, `LaunchId`, `UserId` or `CreatedAt`) or `SavedLaunches` with a similar idea. Add a primary key; add an index on `LaunchId` (and `UserId` if you have it) for lookups.
- Optionally write a stored procedure, e.g. `dbo.AddFavorite(@LaunchId, @UserId)` and `dbo.RemoveFavorite(@LaunchId, @UserId)`, and a procedure or view to list favorites. Run the scripts so the schema and procedures exist.

### 2.3 – .NET data access

- Add Entity Framework Core (with SQL Server provider) or Dapper to the API (or to `NoFreeLaunch.Infrastructure`). Create a DbContext (EF) or a repository/service that runs raw SQL (Dapper).
- Add a connection string to `appsettings.json` (and optionally user secrets for local dev). Register the DbContext or repository in DI.
- Implement a service, e.g. `IFavoritesService`, that adds a favorite, removes a favorite, and lists favorites (by user id or globally for now). Under the hood it uses EF/Dapper and optionally calls your stored procedures.

### 2.4 – GraphQL mutations and queries

- In HotChocolate, add a mutation, e.g. `AddFavorite(launchId: string)` and `RemoveFavorite(launchId: string)`, that call your `IFavoritesService`. Return something simple (e.g. success boolean or the updated list).
- Add a query, e.g. `favorites`, that returns the list of saved launch IDs (or a list of launch details if you join with in-memory SpaceX data). Resolve it via `IFavoritesService` and optionally combine with `ISpaceXLaunchClient` to enrich with launch details.
- Test in Banana Cake Pop: run the mutations, then the `favorites` query, and confirm data is stored in SQL Server.

**Phase 2 done when:** SQL Server runs in Docker, your app persists favorites (or similar) and exposes them via GraphQL mutations and queries; you can add/remove/list favorites and see changes in the database.

---

## Phase 3: Frontend (React + TypeScript + Vite + Apollo)

### 3.1 – Project setup

- In the repo (e.g. `frontend/` or `src/web`), run `npm create vite@latest . -- --template react-ts`. Install dependencies (`npm install`).
- Add Apollo Client: `@apollo/client`, `graphql`. Create an Apollo Client instance (with `ApolloClient`, `InMemoryCache`, `HttpLink` pointing at your backend GraphQL URL, e.g. `http://localhost:5xxx/graphql`). Wrap the app with `ApolloProvider` in `main.tsx`.
- Run the Vite dev server and confirm the default page loads. Ensure the backend is running and CORS allows the frontend origin (configure CORS in the .NET API if needed).

### 3.2 – GraphQL operations

- Define a query for launches (e.g. `GetLaunches`) using `gql` and the same field names as your backend schema. Optionally define `GetLaunch(id)` and, if you built Phase 2, `GetFavorites` and mutations `AddFavorite`, `RemoveFavorite`.
- Use Apollo's `useQuery` in a component to fetch launches and render the list (e.g. in a simple table or cards). Handle loading and error states.
- If you have favorites, use `useMutation` for add/remove and refetch or update the cache so the UI updates after a mutation.

### 3.3 – UI structure

- Build a minimal layout: e.g. a list view of launches and a detail view (by id or row click). Use React Router if you want (e.g. `/` for list, `/launch/:id` for detail).
- Optionally add React Bootstrap (or another library): `npm install react-bootstrap bootstrap`, wrap the app with the Bootstrap provider, and use `Container`, `Row`, `Col`, `Card`, `Button` (or similar) to style the list and detail views.
- Ensure the app is responsive and accessible (e.g. semantic HTML, keyboard navigation, ARIA where needed).

**Phase 3 done when:** The React app loads launch data via Apollo from your GraphQL API, displays it, and (if you did Phase 2) can add/remove favorites and see the updated list.

---

## Phase 4: Docker and optional Kubernetes

### 4.1 – Dockerfile for the API

- In the backend folder (or repo root), add a `Dockerfile`. Use the .NET 8 SDK image to restore and publish the API; use the ASP.NET runtime image to run the published app. Set the working directory and entry point so `dotnet NoFreeLaunch.Api.dll` runs. Build the image and run it locally with `docker run -p 8080:8080 <image>`. Confirm the GraphQL endpoint responds (you may need to set `ASPNETCORE_URLS` or expose the correct port).

### 4.2 – Dockerfile for the frontend

- Add a Dockerfile for the React app: stage 1 – use Node to run `npm ci` and `npm run build`; stage 2 – use nginx (or another static host) to serve the built files from `dist`. Build the image and run it; confirm the app loads and can call the API (use env or build-time variable for the API URL so the container points at the backend).

### 4.3 – Docker Compose

- In the repo root, add `docker-compose.yml`. Define services: `api` (build from the API Dockerfile, expose port), `web` (build from the frontend Dockerfile, expose port 80), `sqlserver` (use the official SQL Server image, env vars, port 1433). Set the API's connection string to the `sqlserver` service hostname. Optionally use a shared network so `api` can reach `sqlserver` and `web` can be reached by the browser.
- Run `docker compose up --build`. Open the frontend in the browser and verify: launches load, and if you have favorites, they persist (data in the SQL Server container).

### 4.4 – Optional: Kubernetes

- Enable Kubernetes in Docker Desktop (or install minikube). Write minimal manifests: a Deployment for the API, a Deployment for the frontend, a Service for each, and (optional) an Ingress or NodePort so you can access the app. Use the same Docker images. Apply with `kubectl apply -f ...` and confirm pods run and the app is reachable. This step is for learning pod/service concepts, not production tuning.

**Phase 4 done when:** `docker compose up` runs the full stack (API, frontend, SQL Server) and you can use the app end-to-end; optionally, the same images run in a local Kubernetes cluster.

---

## Phase 5: Tests and (optional) CI/CD

### 5.1 – Backend unit/integration tests

- Create an xUnit project (e.g. `NoFreeLaunch.Api.Tests`). Add a reference to the API project (or to the project that contains your services).
- Write a few tests for your SpaceX client or favorites service: e.g. mock `HttpClient` (with `HttpMessageHandler` or a test server) and assert that the client returns expected DTOs; or use an in-memory SQLite database for the DbContext and test that the favorites service adds/removes correctly.
- Optionally add an integration test that starts the API (e.g. `WebApplicationFactory`) and sends a GraphQL request (HTTP POST to `/graphql` with a query) and asserts on the response. Run tests with `dotnet test`.

### 5.2 – Frontend tests

- Add Vitest and React Testing Library to the frontend: `npm install -D vitest @testing-library/react @testing-library/jest-dom jsdom` (and any Vite Vitest config you need). Configure Vitest to use jsdom for the environment.
- Write one or two component tests: e.g. a component that displays a list of launches (mock `useQuery` or wrap with a mock Apollo provider and pass mock data) and assert that items render; optionally test a button that triggers a mutation.
- Run tests with `npm run test` (or `npx vitest run`).

### 5.3 – Optional E2E with Playwright

- Add Playwright: `npm init playwright@latest` in the frontend or in a separate e2e folder. Write one flow: e.g. navigate to the app, wait for the launches list to load, and assert that at least one launch is visible (or that a specific text appears). Run against the locally running app (or a Compose stack).

### 5.4 – CI pipeline

- Add a pipeline file (e.g. `.gitlab-ci.yml` or `.github/workflows/ci.yml`). Define jobs: build and test the .NET solution (`dotnet restore`, `dotnet build`, `dotnet test`), build and test the frontend (`npm ci`, `npm run build`, `npm run test`). Optionally add a job that builds the Docker images (and push to a registry if you have one). Run the pipeline on push and confirm all jobs pass.

**Phase 5 done when:** Backend and frontend tests run locally and in CI; optionally one E2E test and Docker build in the pipeline.
