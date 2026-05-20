# Fitness Tracker App — Product Requirements Document (MVP)

## Overview

A fitness tracking application that allows users to create workout templates,
execute workout sessions based on those templates, and track real results.
The app targets users who train both at the gym and at home.

---

## Platforms

| Platform | Technology |
|---|---|
| Mobile (iOS + Android) | Expo + TypeScript |
| Web | Expo Web |
| Backend | ASP.NET Core Web API (C#) |

---

## Domain Entities

### User
Represents a registered user of the application.

| Field | Type | Description |
|---|---|---|
| Id | Guid | Unique identifier |
| Email | string | User email |
| WorkoutTemplates | Collection | User's workout templates |
| WorkoutSessions | Collection | User's workout sessions |
| CreatedAt | DateTimeOffset | — |
| UpdatedAt | DateTimeOffset | — |

### UserCredentials
Stores authentication data separately from business data.

| Field | Type | Description |
|---|---|---|
| Id | Guid | Unique identifier |
| UserId | Guid | Reference to User |
| Provider | enum | Google / Email |
| RefreshToken | string? | JWT refresh token |
| PasswordHash | string? | For Email auth (future) |

### Exercise (system catalog)
Global exercise catalog managed by the system, not users.

| Field | Type | Description |
|---|---|---|
| Id | Guid | Unique identifier |
| Name | string | Exercise name |
| Description | string | Exercise description |
| MuscleGroup | enum | Target muscle group |

### WorkoutTemplate
A workout program created by the user.

| Field | Type | Description |
|---|---|---|
| Id | Guid | Unique identifier |
| UserId | Guid | Owner |
| Name | string | Template name |
| WorkoutTemplateExercises | Collection | Planned exercises |
| WorkoutSessions | Collection | Sessions created from this template |

### WorkoutTemplateExercise
An exercise within a workout template with target goals.

| Field | Type | Description |
|---|---|---|
| Id | Guid | Unique identifier |
| WorkoutTemplateId | Guid | Parent template |
| ExerciseId | Guid | Reference to system exercise |
| Order | int | Exercise order in template |
| TargetSets | int? | Target number of sets |
| TargetReps | int? | Target number of reps |
| TargetWeight | decimal? | Target weight |

### WorkoutSession
A concrete workout execution created from a template.

| Field | Type | Description |
|---|---|---|
| Id | Guid | Unique identifier |
| UserId | Guid | Owner |
| WorkoutTemplateId | Guid | Source template reference |
| Status | enum | ToDo / InProgress / Done / Canceled |
| ExerciseLogs | Collection | Snapshot of exercises |

### ExerciseLog
A snapshot of a template exercise at session creation time.
Also used for spontaneous exercises added during session.

| Field | Type | Description |
|---|---|---|
| Id | Guid | Unique identifier |
| WorkoutSessionId | Guid | Parent session |
| ExerciseId | Guid | Reference to system exercise |
| WorkoutTemplateExerciseId | Guid? | null = spontaneous exercise |
| Order | int | Exercise order (can be changed during session) |
| TargetSets | int? | Copied from template (null if spontaneous) |
| TargetReps | int? | Copied from template (null if spontaneous) |
| TargetWeight | decimal? | Copied from template (null if spontaneous) |
| SetLogs | Collection | Actual performed sets |

### SetLog
A single performed set with actual results.

| Field | Type | Description |
|---|---|---|
| Id | Guid | Unique identifier |
| ExerciseLogId | Guid | Parent exercise log |
| Order | int | Set order |
| Reps | int | Actual reps performed |
| Weight | decimal | Actual weight used |

---

## Session Lifecycle

```
ToDo
 └─→ InProgress
       ├─→ Done
       └─→ Canceled
```

### ToDo
- Session is created from a WorkoutTemplate
- All WorkoutTemplateExercises are copied into ExerciseLogs (snapshot)
- Template is NOT modified

### InProgress
- User performs the workout
- User can edit ExerciseLogs: change order, targets, add/remove exercises
- User can add spontaneous exercises (WorkoutTemplateExerciseId = null)
- User adds SetLogs (reps + weight) to each ExerciseLog
- Template is NOT modified during session

### Done
- ExerciseLog changes are applied to WorkoutTemplate (one-time sync)
- Session is frozen — only SetLog editing is allowed (correct mistakes)
- Template is updated to reflect latest session structure

### Canceled
- All ExerciseLog changes are discarded
- Template is NOT modified (it was never touched)
- Session data is preserved for history but marked Canceled

---

## Business Rules

### Templates
- Only the owner can create, edit, or delete their templates
- Template is updated only when session status changes to Done
- User cannot switch templates during an active session

### Sessions
- Only one active session (InProgress) per user at a time
- Session is always created from a template (WorkoutTemplateId required)
- Spontaneous exercises can be added during InProgress

### Exercises
- System exercise catalog is managed by admins only
- Users cannot create custom exercises in MVP
- Users select exercises from the catalog when building templates

### SetLog editing after Done
- User can update Reps and Weight on existing SetLogs
- User cannot add new SetLogs after Done
- User cannot remove SetLogs after Done

---

## Authentication

### MVP (Google OAuth)
- Login / Register via Google
- JWT Access Token + Refresh Token

### Future (Email + Password)
- Register with email and password
- Login with email and password
- Password reset flow
- UserCredentials entity supports both providers via Provider enum

---

## API Endpoints (overview)

### Auth
| Method | Endpoint | Description |
|---|---|---|
| POST | /auth/google | Google OAuth login |
| POST | /auth/refresh | Refresh access token |
| POST | /auth/logout | Logout |

### Exercises (system catalog)
| Method | Endpoint | Description |
|---|---|---|
| GET | /exercises | Get all exercises (filter by MuscleGroup) |
| GET | /exercises/{id} | Get exercise by id |

### Workout Templates
| Method | Endpoint | Description |
|---|---|---|
| GET | /workout-templates | Get user's templates |
| GET | /workout-templates/{id} | Get template by id |
| POST | /workout-templates | Create template |
| PUT | /workout-templates/{id} | Update template |
| DELETE | /workout-templates/{id} | Delete template |
| POST | /workout-templates/{id}/exercises | Add exercise to template |
| PUT | /workout-templates/{id}/exercises/{exerciseId} | Update exercise in template |
| DELETE | /workout-templates/{id}/exercises/{exerciseId} | Remove exercise from template |

### Workout Sessions
| Method | Endpoint | Description |
|---|---|---|
| GET | /workout-sessions | Get user's sessions |
| GET | /workout-sessions/{id} | Get session by id |
| POST | /workout-sessions | Create session from template |
| PATCH | /workout-sessions/{id}/start | ToDo → InProgress |
| PATCH | /workout-sessions/{id}/complete | InProgress → Done |
| PATCH | /workout-sessions/{id}/cancel | InProgress → Canceled |

### Exercise Logs (during InProgress)
| Method | Endpoint | Description |
|---|---|---|
| PUT | /exercise-logs/{id} | Update exercise log (order, targets) |
| POST | /exercise-logs | Add spontaneous exercise to session |
| DELETE | /exercise-logs/{id} | Remove exercise from session |

### Set Logs
| Method | Endpoint | Description |
|---|---|---|
| POST | /set-logs | Add set to exercise log |
| PUT | /set-logs/{id} | Update set (allowed after Done) |
| DELETE | /set-logs/{id} | Delete set (only InProgress) |

---

## Out of Scope (MVP)

- Custom user exercises
- Social features (sharing, following)
- Nutrition tracking
- Rest timer
- Progress charts and statistics
- Push notifications
- Admin panel for exercise catalog
