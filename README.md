# BuildVision — Construction Project Demo

A compact, recruiter‑friendly demo that shows how we build modern, maintainable software: a full‑stack app for managing and visualizing construction projects — projects, costs, materials, and progress tracking.

Tech Stack (at a glance)

- Backend: .NET 10, microservices, Azure Service Bus (emulator locally), .NET Aspire orchestration.
- Architecture: Presentation, Application, Domain, Infrastructure + a shared kernel.
- Frontend: Next.js (App Router, TypeScript), Material UI, Sass modules.
- Testing (planned): xUnit, NSubstitute (unit), Bogus + WireMock (integration via Aspire test host).

Repository Layout

- `Backend/` — Microservices (Projects, Costs & Materials, Gateway) and `SharedKernel` library.
- `Frontend/` — Next.js SPA (TypeScript, ESLint, `src/` layout, alias `@/*`).
- `Aspire/` — App host that orchestrates local services.

Notes

- Local development uses the Azure Service Bus emulator and the Aspire app host.
- This repo is a demo — focused on architecture and developer experience over feature breadth.
