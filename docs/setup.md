# Development Environment Setup

This guide walks you through setting up the development environment for the Fitness Tracker App.
Follow every step before starting development.

---

## Prerequisites

| Tool | Version | Download |
|---|---|---|
| .NET SDK | 8.0+ | https://dotnet.microsoft.com/download |
| Node.js | 20+ LTS | https://nodejs.org |
| Docker Desktop | latest | https://www.docker.com/products/docker-desktop |
| Git | latest | https://git-scm.com |
| PostgreSQL | 16+ | via Docker (see below) |

---

## 1. Clone the Repository

```bash
git clone https://github.com/your-username/fitness.git
cd fitness
```

---

## 2. Visual Studio 2022 Setup (Backend)

### Install Visual Studio 2022
Download Community Edition (free):
https://visualstudio.microsoft.com/vs/community/

During installation select workloads:
- **ASP.NET and web development**
- **Data storage and processing** (for EF Core tools)

### Install Extensions
Open Visual Studio → `Extensions` → `Manage Extensions` → search and install:

| Extension | Purpose | Link |
|---|---|---|
| **Roslynator** | 500+ C# analyzers and refactorings | marketplace |
| **SonarLint** | Bug and vulnerability detection | marketplace |
| **GitLens** | Advanced git integration | marketplace |
| **Error Lens** | Inline error display | marketplace |

### Open the Backend Project
```
File → Open → Project/Solution
Select: fitness/server/Fitness.sln
```

---

## 3. VS Code Setup (Client)

### Install VS Code
https://code.visualstudio.com

### Install Extensions
VS Code will automatically suggest installing recommended extensions
when you open the project (via `.vscode/extensions.json`).

Click **Install All** when prompted, or install manually:

| Extension | Purpose |
|---|---|
| ESLint | TypeScript linting |
| Prettier | Code formatting |
| Expo Tools | Expo project support |
| React Native Tools | Debugging |
| Tailwind CSS IntelliSense | NativeWind autocomplete |
| GitLens | Advanced git integration |
| Error Lens | Inline error display |
| Path Intellisense | Import path autocomplete |
| Auto Rename Tag | Sync JSX tag renaming |
| Thunder Client | API testing inside VS Code |

### Open the Client Project
```
File → Open Folder
Select: fitness/client/
```

---

## 4. Backend — First Run

```bash
cd server

# Restore dependencies
dotnet restore

# Apply database migrations (requires PostgreSQL running)
dotnet ef database update --project src/Fitness.Infrastructure --startup-project src/Fitness.API

# Run the API
dotnet run --project src/Fitness.API
```

API will be available at: `https://localhost:5001`
Swagger UI: `https://localhost:5001/swagger`

---

## 5. Client — First Run

```bash
cd client

# Install dependencies
npm install

# Start Expo development server
npx expo start
```

- Press `i` for iOS simulator
- Press `a` for Android emulator
- Press `w` for web browser
- Scan QR code with Expo Go app on your phone

---

## 6. Database — Docker

Run PostgreSQL locally via Docker:

```bash
# From project root
docker-compose up fitness-db -d
```

Connection string (for local development):
```
Host=localhost;Port=5432;Database=fitness;Username=postgres;Password=postgres
```

---

## 7. Git Setup

### Configure Git identity
```bash
git config --global user.name "Your Name"
git config --global user.email "your@email.com"
```

### Install Husky (pre-commit hooks)
```bash
cd client
npm run prepare
```

Husky will now run automatically on every commit and push.

### Verify hooks are working
```bash
# Should trigger pre-commit hook
git commit --allow-empty -m "test(infra): verify husky hooks"
```

---

## 8. Environment Variables

### Backend
Create `server/src/Fitness.API/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=fitness;Username=postgres;Password=postgres"
  },
  "JWT": {
    "Secret": "your-local-dev-secret-min-32-chars",
    "Issuer": "fitness-api",
    "Audience": "fitness-client",
    "ExpiryMinutes": 60
  },
  "Google": {
    "ClientId": "your-google-client-id",
    "ClientSecret": "your-google-client-secret"
  }
}
```

### Client
Create `client/.env.local`:
```
EXPO_PUBLIC_API_URL=https://localhost:5001/api
```

⚠️ Never commit these files — they are in `.gitignore`

---

## 9. Verify Everything Works

```bash
# Backend compiles with zero warnings
cd server
dotnet build

# Backend tests pass
dotnet test

# Client lints with zero warnings
cd client
npm run lint

# Client formats correctly
npm run format:check
```

All commands must pass before starting development.

---

## 10. Git Workflow Reminder

Always follow this workflow:

```bash
# 1. Create a branch before any work
git checkout -b feat/backend/your-feature-name

# 2. Make your changes

# 3. Stage and commit (Husky runs automatically)
git add .
git commit -m "feat(backend): your feature description"

# 4. Push
git push origin feat/backend/your-feature-name

# 5. Open Pull Request to main
```

Branch naming: `type/scope/short-description`
Commit format: `type(scope): short description`

See `CLAUDE.md` for full Git conventions.
