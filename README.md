# Real-Time Chat App (.NET 8 + React + SignalR)

Production-oriented full-stack chat application with JWT auth, MongoDB persistence, and SignalR real-time messaging.

## Project Structure

- `server`: ASP.NET Core 8 Web API + SignalR + MongoDB
- `client`: React (Vite) + Tailwind + SignalR client

## Backend Highlights

- Minimal hosting model in `Program.cs`
- JWT Authentication with SignalR token support (`access_token` query string)
- BCrypt password hashing
- MongoDB repositories for users/messages
- SignalR `ChatHub` with:
  - `OnConnectedAsync`
  - `OnDisconnectedAsync`
  - `JoinUserRoom`
  - `SendMessage`
  - `BroadcastOnlineUsers`

## Frontend Highlights

- Login, Register, Chat routes
- JWT + user stored in localStorage
- Online/offline indicators
- Conversation history loading
- Real-time send/receive via SignalR
- Auto-scroll to latest message
- Responsive WhatsApp-like split layout

## Environment Variables

### Backend (`server/.env.example`)

- `JWT_SECRET`
- `DB_CONNECTION`
- `PORT`

### Frontend (`client/.env.example`)

- `API_BASE_URL`
- `SIGNALR_URL`

## Run Instructions

### Backend

```bash
cd server
dotnet restore
dotnet run
```

### Frontend

```bash
cd client
npm install
npm run dev
```

## API Endpoints

- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/users` (auth required)
- `GET /api/messages/{userId}` (auth required)
- `POST /api/messages` (auth required)

## SignalR Hub

- Endpoint: `/chatHub`
- Client invokes:
  - `JoinUserRoom(userId)`
  - `SendMessage(receiverId, content)`
- Client listens:
  - `ReceiveMessage`
  - `OnlineUsers`
