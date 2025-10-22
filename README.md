# TODO App (Angular 20 + .NET 8)

Simple TODO list: view, add, delete.

## Stack
- Frontend: Angular 20 (standalone)
- Backend: .NET 8 Minimal API, in-memory store, Swagger
- Tests: xUnit (API), Jest (frontend)

## Prerequisites
- Node 22+ and npm
- .NET SDK 8.x

## Running locally
### Backend
```bash
cd backend/Todo.Api
dotnet restore
dotnet run
```
### Frontend
```bash
cd frontend/todo-app
npm install
npm start
```
