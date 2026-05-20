# CLAUDE.md — Fitness Tracker App

This file defines the project context, architecture rules, and agent system.
Read this file before every task. All decisions here are final unless explicitly changed.

---

## Project Overview

| | |
|---|---|
| **Name** | Fitness Tracker App |
| **Goal** | Track workouts, exercises, and personal progress |
| **Developer** | Solo (learning project) |
| **Repository** | Monorepo |
| **PRD** | See `/docs/PRD.md` for full business requirements |

---

## Repository Structure

```
fitness/
├── CLAUDE.md                        ← this file
├── .editorconfig                    ← C# formatting rules
├── .gitignore
├── docker-compose.yml
├── README.md
│
├── .claude/
│   ├── agents/
│   │   ├── architect-agent.md
│   │   ├── business-analyst-agent.md
│   │   ├── code-review-agent.md
│   │   ├── testing-agent.md
│   │   ├── devops-agent.md
│   │   └── frontend-agent.md
│   └── skills/                      ← created during Phase 1 as patterns emerge
│       ├── csharp-conventions.md    ← C# patterns and examples
│       ├── expo-conventions.md      ← Expo/TypeScript patterns
│       ├── testing-patterns.md      ← xUnit test templates
│       └── git-workflow.md          ← branch + commit checklist
│
├── .github/
│   └── workflows/                   ← GitHub Actions CI/CD
│
├── .vscode/
│   └── settings.json                ← format on save (committed to repo)
│
├── docs/
│   ├── PRD.md                       ← Product Requirements Document
│   └── adr/                         ← Architecture Decision Records
│                                       (why we made key decisions)
│
├── server/                          ← ASP.NET Core Web API (C#)
│   ├── Fitness.sln
│   └── src/
│       ├── Fitness.API/
│       ├── Fitness.Application/
│       ├── Fitness.Domain/
│       └── Fitness.Infrastructure/
│
├── client/                          ← Expo + TypeScript (iOS, Android, Web)
│   ├── app/                         ← Expo Router pages (file = route)
│   │   ├── (auth)/
│   │   │   ├── _layout.tsx
│   │   │   └── login.tsx
│   │   └── (app)/
│   │       ├── _layout.tsx          ← protected routes
│   │       ├── index.tsx            ← dashboard
│   │       ├── templates/
│   │       │   ├── index.tsx
│   │       │   ├── [id].tsx
│   │       │   └── create.tsx
│   │       └── sessions/
│   │           ├── index.tsx
│   │           └── [id].tsx
│   │
│   ├── features/                    ← feature-based logic
│   │   ├── auth/
│   │   │   ├── components/          ← LoginForm, GoogleButton
│   │   │   ├── hooks/               ← useLogin, useGoogleAuth
│   │   │   └── services/            ← auth.service.ts
│   │   ├── templates/
│   │   │   ├── components/          ← TemplateCard, TemplateForm
│   │   │   ├── hooks/               ← useTemplates, useCreateTemplate
│   │   │   └── services/            ← templates.service.ts
│   │   ├── sessions/
│   │   │   ├── components/          ← SessionCard, ExerciseLog, SetLogItem
│   │   │   ├── hooks/               ← useActiveSession, useCompleteSession
│   │   │   └── services/            ← sessions.service.ts
│   │   └── exercises/
│   │       ├── components/          ← ExercisePicker, ExerciseCard
│   │       ├── hooks/               ← useExercises
│   │       └── services/            ← exercises.service.ts
│   │
│   ├── shared/                      ← reusable across features
│   │   ├── components/
│   │   │   ├── ui/                  ← Button, Input, Card, Modal, Spinner
│   │   │   └── layout/              ← Screen, Header, SafeArea
│   │   ├── hooks/                   ← useDebounce, useRefresh, usePagination
│   │   ├── stores/                  ← Zustand (auth.store, session.store)
│   │   ├── services/
│   │   │   └── api.ts               ← Axios instance with interceptors
│   │   ├── types/                   ← interfaces mirroring backend DTOs
│   │   ├── utils/                   ← formatDate, calculateVolume
│   │   └── constants/               ← theme.ts, routes.ts
│   │
│   └── localization/                ← i18n (setup from day one)
│       ├── index.ts
│       └── translations/
│           ├── en.json
│           └── uk.json
│
└── infra/
    ├── docker/
    ├── nginx/
    └── monitoring/                  ← Prometheus + Grafana
```

### Component placement rule
```
Used in only one feature  → features/[name]/components/
Used in 2+ features       → shared/components/ui/
Page / screen             → app/
```

---

## Tech Stack

### Backend
| Technology | Purpose |
|---|---|
| ASP.NET Core Web API | REST API |
| C# 12 | Language |
| Entity Framework Core | ORM |
| PostgreSQL | Database |
| JWT + Refresh Tokens | Authentication |
| FluentValidation | Input validation |
| AutoMapper | Object mapping |
| Serilog | Structured logging |
| xUnit + Moq | Testing |

### Client (iOS, Android, Web)
| Technology | Purpose |
|---|---|
| Expo SDK 54 | Cross-platform framework |
| TypeScript (strict) | Language |
| Expo Router v3 | File-based navigation |
| Zustand | Global client state |
| TanStack Query v5 | Server state + caching |
| NativeWind v4 | Styling (Tailwind for React Native) |
| React Hook Form + Zod | Forms and validation |
| Axios | HTTP client with interceptors |
| i18n-js + expo-localization | Localization (setup from day one) |

### Infrastructure
| Technology | Purpose |
|---|---|
| Docker + docker-compose | Containerization |
| Nginx | Reverse proxy |
| GitHub Actions | CI/CD pipeline |
| Prometheus | Metrics |
| Grafana | Monitoring dashboards |

---

## Backend Architecture — Clean Architecture

```
Fitness.Domain/
  ├── Entities/        ← business entities (pure C#, zero dependencies)
  ├── Enums/           ← Status, MuscleGroup, AuthProvider
  ├── Common/          ← BaseEntity (Id, CreatedAt, UpdatedAt)
  └── Exceptions/      ← NotFoundException, DomainException

Fitness.Application/
  ├── DTOs/            ← request/response objects
  ├── Interfaces/      ← IRepository, IService contracts
  ├── Services/        ← business logic (Service Pattern)
  ├── Mapping/         ← AutoMapper profiles
  ├── Validators/      ← FluentValidation rules
  └── Common/          ← shared DTOs (PagedResult<T> etc.)

Fitness.Infrastructure/
  ├── Persistence/     ← AppDbContext (not "Data")
  ├── Migrations/      ← EF Core migrations
  └── Repositories/   ← IRepository implementations

Fitness.API/
  ├── Controllers/     ← thin HTTP layer only
  ├── Middlewares/     ← error handling, logging (NOT in Infrastructure)
  └── Swagger/         ← API documentation
```

### Dependency direction (never violate)
```
API → Application → Domain
Infrastructure → Application → Domain
```

### Forbidden (always flag)
- Business logic in controllers
- Direct DB access from API layer
- `new` for services inside classes — use DI
- Returning `null` from services — use exceptions or Result<T>
- Middleware in Infrastructure layer
- Domain exceptions in Application layer
- Setting `UpdatedAt` outside the entity — entity manages its own state
- Exposing database port publicly in Docker

---

## Code Standards

### Modern syntax (always prefer latest)
**C# 12:** primary constructors, pattern matching, records for DTOs,
collection expressions `[]`, `required` modifier, `?.` and `??` operators.

**TypeScript:** `async/await` over `.then()`, optional chaining `?.`,
nullish coalescing `??`, `satisfies` operator, destructuring everywhere.

When older syntax is unavoidable — always add a comment explaining why.

### Zero warnings policy
No compiler warnings, no linter warnings, no runtime warnings — ever.
If a warning cannot be fixed (third-party library), suppress it explicitly with a comment.

### Formatting
| Setting | Value |
|---|---|
| Indentation | Tabs |
| TypeScript quotes | Double `""` |
| Trailing comma | None |
| Semicolons | Yes |
| Print width | 100 |
| Format on save | Yes (via `.vscode/settings.json`) |

---

## Git Conventions

### Commit message format
```
type(scope): short description

feat(backend): add workout session creation
fix(frontend): correct navigation after login
refactor(backend): move middleware to API layer
test(backend): add ExerciseService unit tests
chore(infra): add docker-compose setup
```

**Types:** feat, fix, refactor, test, docs, chore, style, perf
**Scopes:** backend, frontend, infra, monitoring, db

### Branch naming
```
type/scope/short-description

feat/backend/workout-session
fix/frontend/login-navigation
chore/infra/docker-setup
```

### Husky hooks
```
pre-commit  → lint-staged: ESLint + Prettier (TS), dotnet format (C#)
commit-msg  → Conventional Commits format + scope validation
pre-push    → dotnet test (blocks push if any test fails)
```

---

## Agent System

Read the corresponding agent file before starting a task.
Invoke agents with: "work as @agent-name"

| Agent | File | When to use |
|---|---|---|
| **Architect** | `.claude/agents/architect-agent.md` | Architecture decisions, layer structure, SOLID, patterns |
| **Business Analyst** | `.claude/agents/business-analyst-agent.md` | Feature requirements, business logic, PRD updates |
| **Code Review** | `.claude/agents/code-review-agent.md` | Review any code before considering a task done |
| **Testing** | `.claude/agents/testing-agent.md` | Writing and reviewing tests |
| **DevOps** | `.claude/agents/devops-agent.md` | Docker, Nginx, CI/CD, Prometheus, Grafana, Kubernetes |
| **Frontend** | `.claude/agents/frontend-agent.md` | Expo components, hooks, navigation, TypeScript |

### Teaching mode (all agents)
If the developer says "I don't understand" or "explain this concept" —
switch to teaching mode: explain from scratch with a simple analogy,
then show a concrete example from the fitness project.

---

## Development Roadmap

- [x] Initial backend (needs Clean Architecture refactor)
- [x] Agent system defined
- [x] PRD written
- [ ] Phase 1 — Project setup + Backend refactor
  - [ ] Step 1 — Husky + ESLint + Prettier + EditorConfig
  - [ ] Step 2 — Repository restructure (server/, client/, infra/)
  - [ ] Step 3 — Clean Architecture refactor
- [ ] Phase 2 — Unit tests (xUnit + Moq)
- [ ] Phase 3 — Expo client
- [ ] Phase 4 — Docker + Nginx + GitHub Actions CI/CD
- [ ] Phase 5 — Prometheus + Grafana monitoring
- [ ] Phase 6 — Kubernetes (stretch goal, after Phase 5)
