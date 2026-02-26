# HtmlToPdfCore .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the HtmlToPdfCore solution upgrade from .NET Core 3.1 to .NET 10.0. The single project will be upgraded in one atomic operation.

**Progress**: 0/3 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [▶] TASK-001: Verify prerequisites
**References**: Plan §Phase 0

- [▶] (1) Verify .NET 10 SDK is installed on the build machine
- [ ] (2) .NET 10 SDK installation confirmed (**Verify**)

---

### [ ] TASK-002: Atomic framework and package upgrade
**References**: Plan §Phase 1, Plan §4.1, Plan §5 Package Update Reference, Plan §6 Breaking Changes Catalog

- [ ] (1) Update TargetFramework in src/HtmlToPdfCore.csproj from `netcoreapp3.1` to `net10.0`
- [ ] (2) TargetFramework updated to net10.0 (**Verify**)
- [ ] (3) Update Microsoft.Extensions.Hosting from 3.1.0 to 10.0.3 in src/HtmlToPdfCore.csproj
- [ ] (4) Update Microsoft.Extensions.Hosting.Abstractions from 3.1.0 to 10.0.3 in src/HtmlToPdfCore.csproj
- [ ] (5) Both package references updated to version 10.0.3 (**Verify**)
- [ ] (6) Restore dependencies with `dotnet restore src/HtmlToPdfCore.csproj`
- [ ] (7) Dependencies restored successfully (**Verify**)
- [ ] (8) Build solution with `dotnet build src/HtmlToPdfCore.csproj` and fix any compilation errors per Plan §6 Breaking Changes Catalog
- [ ] (9) Solution builds with 0 errors (**Verify**)

---

### [ ] TASK-003: Final commit
**References**: Plan §10 Source Control Strategy

- [ ] (1) Commit all changes with message: "chore: upgrade HtmlToPdfCore from netcoreapp3.1 to net10.0"

---
