# Fitness Tracker

This is a full stack fitness tracking application for structured workout logging, daily check ins, and user account management. It is built with a production-style RESTful API backend with a React frontend.

I am an avid gym goer, so I wanted to build a custom application that meets MY needs when tracking workouts in the gym. There are apps like MyFitnessPal that are great for calorie tracking but don't have as robust of workout logging. There are other workout apps that have great workout logging but lackluster food / nutrient logging. My goal is to build an application that does both exceptionally. 

In addition to that, I wanted to build an app that showcases a variety of my skillset as a software engineer. This application serves as an exercise in building a production style RESTful API with the kind of structure and discipline I'd expect in a professional codebase. The backend is organized using a layered architecture. Controllers handle routing and request/response concerns, services encapsulate business logic, and data access is managed through Entity Framework Core. This separation keeps the codebase testable and easy to extend as new features are added. Authentication is handled through ASP.NET Identity and JWT, following patterns I would expect to see and maintain in a real production environment.

I modeled the WorkoutLogging endpoints to match my typical workout flow. In a workout, I have exercises that I do. I do more than one set of the same exercise. For example, in a lifting session, I could do an exercise, like bench press, for four sets of 5 reps. That's how I decided on the flow Session -> Exercise -> Set.

Please note that the project is actively under development. My next goal is to add nutrition logging.

---

## Features

- JWT-based user registration and authentication
- User profiles and account management
- Workout session creation and structured exercise/set logging
- Daily check-in tracking for weight and progress
- Aggregation endpoints for dashboard-style summaries

---

## Tech Stack

| Layer          | Technology               |
|----------------|--------------------------|
| Backend API    | ASP.NET Core Web API     |
| ORM            | Entity Framework Core    |
| Database       | PostgreSQL               |
| Auth           | Identity + JWT           |
| Frontend       | React + TypeScript       |
| Infrastructure | Docker                   |

---

## API Endpoints

### Auth

| Method | Endpoint           | Auth Required | Description                        |
|--------|--------------------|---------------|------------------------------------|
| `POST` | `/auth/register`   | No            | Register a new user                |
| `POST` | `/auth/login`      | No            | Authenticate and receive a JWT     |

---

### Users

| Method | Endpoint    | Auth Required | Description                              |
|--------|-------------|---------------|------------------------------------------|
| `GET`  | `/users/me` | Yes           | Get the authenticated user's profile     |

---

### Workout Sessions

| Method   | Endpoint                          | Auth Required | Description                        |
|----------|-----------------------------------|---------------|------------------------------------|
| `POST`   | `/workout-sessions`               | Yes           | Create a new workout session       |
| `GET`    | `/workout-sessions`               | Yes           | List all sessions for the user     |
| `GET`    | `/workout-sessions/{sessionId}`   | Yes           | Get a specific session by ID       |
| `PATCH`  | `/workout-sessions/{sessionId}`   | Yes           | Update a workout session           |
| `DELETE` | `/workout-sessions/{sessionId}`   | Yes           | Delete a workout session           |

---

### Workout Exercises

Exercises are nested under a workout session.

| Method   | Endpoint                                                    | Auth Required | Description                        |
|----------|-------------------------------------------------------------|---------------|------------------------------------|
| `POST`   | `/workout-sessions/{sessionId}/exercises`                   | Yes           | Add an exercise to a session       |
| `PATCH`  | `/workout-sessions/{sessionId}/exercises/{exerciseId}`      | Yes           | Update an exercise                 |
| `DELETE` | `/workout-sessions/{sessionId}/exercises/{exerciseId}`      | Yes           | Remove an exercise from a session  |

---

### Workout Sets

Sets are nested under a session's exercise.

| Method   | Endpoint                                                                          | Auth Required | Description                  |
|----------|-----------------------------------------------------------------------------------|---------------|------------------------------|
| `POST`   | `/workout-sessions/{sessionId}/exercises/{exerciseId}/sets`                       | Yes           | Add a set to an exercise     |
| `PATCH`  | `/workout-sessions/{sessionId}/exercises/{exerciseId}/sets/{setId}`               | Yes           | Update a set                 |
| `DELETE` | `/workout-sessions/{sessionId}/exercises/{exerciseId}/sets/{setId}`               | Yes           | Delete a set                 |

---

> All endpoints except `/auth/register` and `/auth/login` require a valid JWT passed as a Bearer token in the `Authorization` header:
> ```
> Authorization: Bearer <your_token>
> ```

---

## Development Status

**Implemented**
- Authentication and authorization (JWT)
- Database schema and migrations
- Core entity modeling
- Workout session, exercise, and set logging
- Containerized development environment (Docker)

**Planned**
- Refresh token rotation
- Request rate limiting
- Daily check-in endpoints
- Expanded analytics endpoints for dashboard summaries
