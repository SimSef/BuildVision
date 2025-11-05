# BuildVision — Construction Project Demo

A compact, recruiter‑friendly demo that shows how we build modern, maintainable software: a full‑stack app for managing and visualizing construction projects — projects, costs, materials, and progress tracking.

Highlights

- Clean Architecture + Microservices: clear boundaries and testable layers.
- Event‑Driven Design: services communicate via an event bus for loose coupling.
- Orchestrated Locally with .NET Aspire: simple, unified developer experience.
- Modern Frontend: Next.js (TypeScript), Material UI, Sass — SPA‑first.

What It Demonstrates

- Manage projects, track site progress, and organize materials and costs.
- A gateway (API facade) that serves the SPA and coordinates the backend.
- Internal services that own their data and react to events instead of tight coupling.

Tech Stack (at a glance)

- Backend: .NET 10, microservices, Azure Service Bus (emulator locally), .NET Aspire orchestration.
- Architecture: Presentation, Application, Domain, Infrastructure + a shared kernel.
- Frontend: Next.js (App Router, TypeScript), Material UI, Sass modules.
- Testing (planned): xUnit, NSubstitute (unit), Bogus + WireMock (integration via Aspire test host).

Repository Layout

- `Backend/` — Microservices (Projects, Costs & Materials, Gateway) and `SharedKernel` library.
- `Frontend/` — Next.js SPA (TypeScript, ESLint, `src/` layout, alias `@/*`).
- `Aspire/` — App host that orchestrates local services.

Why It Matters

- Maintainability first: clean boundaries mean easier changes and safer refactors.
- Resilient by design: event‑driven flow reduces cascading failures and tight coupling.
- Production‑style patterns in a small, understandable codebase.

Notes

- Local development uses the Azure Service Bus emulator and the Aspire app host.
- This repo is a demo — focused on architecture and developer experience over feature breadth.

