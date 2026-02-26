# .NET 10.0 Upgrade Plan — HtmlToPdfCore

## Table of Contents

1. [Executive Summary](#1-executive-summary)
2. [Migration Strategy](#2-migration-strategy)
3. [Detailed Dependency Analysis](#3-detailed-dependency-analysis)
4. [Project-by-Project Plans](#4-project-by-project-plans)
   - [src/HtmlToPdfCore.csproj](#41-srchtmltopdfcorecsproj)
5. [Package Update Reference](#5-package-update-reference)
6. [Breaking Changes Catalog](#6-breaking-changes-catalog)
7. [Testing & Validation Strategy](#7-testing--validation-strategy)
8. [Risk Management](#8-risk-management)
9. [Complexity & Effort Assessment](#9-complexity--effort-assessment)
10. [Source Control Strategy](#10-source-control-strategy)
11. [Success Criteria](#11-success-criteria)

---

## 1. Executive Summary

### Scenario Description

Upgrade the **HtmlToPdfCore** solution from **.NET Core 3.1** to **.NET 10.0 (LTS)**. This involves updating the project target framework, and updating compatible NuGet packages to their .NET 10.0-aligned versions.

### Scope

| Item | Detail |
|------|--------|
| **Projects affected** | 1 (`src/HtmlToPdfCore.csproj`) |
| **Current framework** | `netcoreapp3.1` |
| **Target framework** | `net10.0` |
| **Total LOC** | 243 |
| **Total packages** | 3 (2 require updates) |
| **Security vulnerabilities** | None |
| **API issues** | None (263/263 APIs fully compatible) |

### Selected Strategy

**All-At-Once Strategy** — All projects upgraded simultaneously in a single atomic operation.

**Rationale**:
- Single project solution — no dependency ordering complexity
- Very small codebase (243 LOC across 4 files)
- Zero API breaking changes detected
- No security vulnerabilities to isolate
- All packages have known compatible target versions
- Low difficulty rating from assessment

### Complexity Classification

**Simple** — 1 project, depth 1, no high-risk factors, no vulnerabilities. Fast batch execution applies.

### Critical Issues

None identified. This is a straightforward framework version bump with package updates.

---

## 2. Migration Strategy

### Approach: All-At-Once

All project file changes, package reference updates, and compilation fixes are applied in a **single coordinated operation** with no intermediate states.

**Justification**:

| Criteria | Value | Assessment |
|----------|-------|------------|
| Project count | 1 | ✅ Well below 5-project threshold |
| Codebase size | 243 LOC | ✅ Very small |
| Dependency depth | 1 | ✅ Minimal |
| API breaking changes | 0 | ✅ None detected |
| Security vulnerabilities | 0 | ✅ Clean |
| Package updates | 2 | ✅ Straightforward version bumps |
| Difficulty rating | 🟢 Low | ✅ Ideal for All-At-Once |

### Execution Sequence

The atomic upgrade follows this order within a single operation:

1. Update `TargetFramework` in `src/HtmlToPdfCore.csproj` from `netcoreapp3.1` → `net10.0`
2. Update NuGet package references (see §5 Package Update Reference)
3. Restore dependencies (`dotnet restore`)
4. Build solution and fix all compilation errors (see §6 Breaking Changes Catalog)
5. Verify solution builds with 0 errors

Testing follows after the atomic upgrade completes successfully.

### Phase Definitions

| Phase | Scope | Goal |
|-------|-------|------|
| **Phase 0: Prerequisites** | SDK verification | Confirm .NET 10 SDK is installed |
| **Phase 1: Atomic Upgrade** | All project + package updates | Solution builds with 0 errors |
| **Phase 2: Validation** | Build & runtime checks | All validation criteria met |

---

## 3. Detailed Dependency Analysis

### Dependency Graph

```
HtmlToPdfCore.csproj  (netcoreapp3.1 → net10.0)
    └── [No project dependencies]
```

The solution contains a **single standalone project** with no project-to-project dependencies. It is simultaneously a leaf node and the root node.

### Project Groupings

Since there is only one project and it has no inter-project dependencies, the entire solution is upgraded in a single atomic operation with no phase sequencing required.

| Phase | Projects | Reason |
|-------|----------|--------|
| Phase 1 (Atomic Upgrade) | `src/HtmlToPdfCore.csproj` | Only project; no dependencies to respect |

### Critical Path

`src/HtmlToPdfCore.csproj` (sole project) → Build → Validate

### Circular Dependencies

None. No inter-project dependencies exist.

---

## 4. Project-by-Project Plans

## 4. Project-by-Project Plans

### 4.1 src/HtmlToPdfCore.csproj

#### Current State

| Attribute | Value |
|-----------|-------|
| **Target Framework** | `netcoreapp3.1` |
| **Project Kind** | DotNetCoreApp (SDK-style) |
| **Dependencies (projects)** | 0 |
| **Dependants (projects)** | 0 |
| **Files** | 4 (1 with incidents) |
| **Lines of Code** | 243 |
| **NuGet Packages** | 3 (2 require update) |
| **Risk Level** | 🟢 Low |

**Current Packages**:

| Package | Current Version |
|---------|----------------|
| `Microsoft.Extensions.Hosting` | 3.1.0 |
| `Microsoft.Extensions.Hosting.Abstractions` | 3.1.0 |
| `Wkhtmltopdf.NetCore` | 3.0.0 |

#### Target State

| Attribute | Value |
|-----------|-------|
| **Target Framework** | `net10.0` |
| **Packages to update** | 2 |
| **Packages compatible (no update needed)** | 1 (`Wkhtmltopdf.NetCore`) |

#### Migration Steps

**Step 1: Prerequisites**
- Verify .NET 10 SDK is installed on the build machine
- Confirm `upgrade-to-NET10` branch is active

**Step 2: Update Target Framework**

In `src/HtmlToPdfCore.csproj`, change:
```xml
<!-- Before -->
<TargetFramework>netcoreapp3.1</TargetFramework>

<!-- After -->
<TargetFramework>net10.0</TargetFramework>
```

**Step 3: Update Package References**

In `src/HtmlToPdfCore.csproj`, update:

```xml
<!-- Before -->
<PackageReference Include="Microsoft.Extensions.Hosting" Version="3.1.0" />
<PackageReference Include="Microsoft.Extensions.Hosting.Abstractions" Version="3.1.0" />

<!-- After -->
<PackageReference Include="Microsoft.Extensions.Hosting" Version="10.0.3" />
<PackageReference Include="Microsoft.Extensions.Hosting.Abstractions" Version="10.0.3" />
```

`Wkhtmltopdf.NetCore` at version `3.0.0` is marked as ✅ Compatible — **no update required**.

**Step 4: Restore Dependencies**

```bash
dotnet restore src/HtmlToPdfCore.csproj
```

**Step 5: Build and Fix Compilation Errors**

```bash
dotnet build src/HtmlToPdfCore.csproj
```

Address any compilation errors using the [Breaking Changes Catalog](#6-breaking-changes-catalog). Rebuild after fixes to verify 0 errors.

**Step 6: Validation**

- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] No unresolved package dependency conflicts
- [ ] Application runs as expected

---

## 5. Package Update Reference

### Common Package Updates (all projects)

| Package | Current Version | Target Version | Projects Affected | Update Reason |
|---------|----------------|---------------|-------------------|---------------|
| `Microsoft.Extensions.Hosting` | 3.1.0 | 10.0.3 | `HtmlToPdfCore.csproj` (1 project) | Framework alignment — version 3.1.0 targets netcoreapp3.1; update to 10.0.3 for net10.0 compatibility |
| `Microsoft.Extensions.Hosting.Abstractions` | 3.1.0 | 10.0.3 | `HtmlToPdfCore.csproj` (1 project) | Framework alignment — same rationale as above; these two packages are released in lockstep |

### Compatible Packages (no update required)

| Package | Current Version | Status | Reason |
|---------|----------------|--------|--------|
| `Wkhtmltopdf.NetCore` | 3.0.0 | ✅ Compatible | Assessment confirms compatibility with net10.0; no update needed |

### Summary

| Category | Count |
|----------|-------|
| Packages requiring update | 2 |
| Compatible packages (no change) | 1 |
| **Total packages** | **3** |

---

## 6. Breaking Changes Catalog

### API Compatibility Summary

The assessment reports **zero breaking changes** across all 263 analyzed APIs:

| Category | Count |
|----------|-------|
| 🔴 Binary Incompatible | 0 |
| 🟡 Source Incompatible | 0 |
| 🔵 Behavioral Change | 0 |
| ✅ Compatible | 263 |

### Expected Areas to Monitor

Although the automated analysis reports zero issues, the following areas are worth manual review after the framework bump, given the major version jump from .NET Core 3.1 to .NET 10:

| Area | Concern | Likelihood | Action |
|------|---------|------------|--------|
| `Microsoft.Extensions.Hosting` API changes | Minor API surface changes across major versions | Low | Build will reveal any issues; fix as found |
| Nullable reference types | .NET 5+ enables NRT by default in new SDK templates | Low | Review if project enables `<Nullable>enable</Nullable>` and suppress or fix warnings |
| `Wkhtmltopdf.NetCore` runtime behavior | Native library wrapper — runtime behavior unchanged but verify binary is compatible with .NET 10 runtime | Low | Perform a smoke test of PDF generation post-upgrade |
| `Program.cs` / `Startup.cs` hosting model | .NET 6+ introduced minimal hosting; .NET Core 3.1 pattern still compiles but consider modernizing | Informational | Optional modernization, not a blocker |

> ⚠️ **Note**: All listed concerns are low-likelihood and informational. The assessment confirms full compatibility. Any issues will be surfaced during `dotnet build` in Phase 1.

---

## 7. Testing & Validation Strategy

### Phase 1: Build Validation (after atomic upgrade)

| Check | Command | Expected Outcome |
|-------|---------|------------------|
| Dependency restore | `dotnet restore` | No unresolved packages |
| Solution build | `dotnet build` | 0 errors, 0 warnings |

### Phase 2: Runtime Validation

The solution does not include automated test projects. Runtime validation should cover:

| Scenario | Validation Method | Expected Outcome |
|----------|------------------|------------------|
| Application starts | Run the application | No startup exceptions |
| PDF generation | Exercise HTML-to-PDF conversion with a sample input | PDF output generated correctly |
| `Wkhtmltopdf.NetCore` native binding | Verify wkhtmltopdf native binaries load | No `DllNotFoundException` or native interop errors |

### Validation Checklist

- [ ] `.NET 10 SDK` confirmed installed
- [ ] `dotnet restore` completes with no errors
- [ ] `dotnet build` completes with **0 errors**
- [ ] `dotnet build` completes with **0 warnings** (or all warnings acknowledged)
- [ ] Application starts without exceptions
- [ ] PDF generation produces expected output
- [ ] No native library loading errors from `Wkhtmltopdf.NetCore`

---

## 8. Risk Management

### Risk Register

| Risk | Level | Description | Mitigation |
|------|-------|-------------|------------|
| `Wkhtmltopdf.NetCore` native binary incompatibility | 🟡 Medium | This package wraps a native binary (`wkhtmltopdf`). While marked compatible, the native binary may not behave as expected under .NET 10's runtime. | Perform explicit PDF generation smoke test after upgrade. If issues arise, check for updated versions or alternative packages (e.g., `PuppeteerSharp`, `DinkToPdf`). |
| `Microsoft.Extensions.Hosting` API surface changes | 🟢 Low | Major version bump (3.1.0 → 10.0.3) may introduce minor API changes not caught by the automated scan. | Build will reveal any compilation errors. Fix as found — assessment confirms all used APIs are compatible. |
| .NET 10 SDK not installed | 🟢 Low | Build will fail if .NET 10 SDK is absent on the build machine. | Verify SDK installation before starting (Phase 0). Download from https://dot.net if needed. |
| Nullable reference type warnings | 🟢 Low | Enabling NRT in .NET 10 SDK default settings may produce new warnings. | Review `<Nullable>` property in project file; suppress or fix warnings as appropriate. |

### Contingency Plans

| Scenario | Contingency |
|----------|-------------|
| `Wkhtmltopdf.NetCore` fails at runtime | Evaluate upgrading to a newer version or migrating to `PuppeteerSharp` for headless Chrome-based PDF generation |
| Unexpected compilation errors | Use the Breaking Changes Catalog (§6) as a starting reference; consult .NET migration guides at https://learn.microsoft.com/dotnet/core/compatibility |
| Build machine missing .NET 10 SDK | Download and install from https://dotnet.microsoft.com/download/dotnet/10.0 |

---

## 9. Complexity & Effort Assessment

### Per-Project Complexity

| Project | Complexity | LOC | Package Updates | API Issues | Risk | Notes |
|---------|-----------|-----|----------------|------------|------|-------|
| `src/HtmlToPdfCore.csproj` | 🟢 Low | 243 | 2 | 0 | 🟢 Low | Straightforward version bump; no code changes expected |

### Phase Complexity

| Phase | Complexity | Description |
|-------|-----------|-------------|
| Phase 0: Prerequisites | 🟢 Low | SDK check only |
| Phase 1: Atomic Upgrade | 🟢 Low | Single project; 2 package updates; no breaking changes |
| Phase 2: Validation | 🟢 Low | Build + smoke test |

### Resource Requirements

| Resource | Requirement |
|----------|-------------|
| .NET Skills | Basic — project file editing and `dotnet` CLI usage |
| Parallel capacity | Single developer sufficient |
| Environment | .NET 10 SDK on build machine |

---

## 10. Source Control Strategy

### Branching

| Item | Value |
|------|-------|
| Source branch | `master` |
| Upgrade branch | `upgrade-to-NET10` |
| Merge target | `master` (via Pull Request) |

All upgrade changes are made on the `upgrade-to-NET10` branch, which was created as part of the assessment stage.

### Commit Strategy

**All-At-Once Strategy: Single Commit Approach**

Because this is an atomic single-project upgrade, all changes (framework update, package updates, any compilation fixes) should be made in a **single commit** on the upgrade branch:

```
commit: chore: upgrade HtmlToPdfCore from netcoreapp3.1 to net10.0

- Update TargetFramework: netcoreapp3.1 → net10.0
- Update Microsoft.Extensions.Hosting: 3.1.0 → 10.0.3
- Update Microsoft.Extensions.Hosting.Abstractions: 3.1.0 → 10.0.3
- Fix any compilation issues from framework upgrade
```

### Review & Merge Process

1. Ensure `upgrade-to-NET10` branch builds with 0 errors
2. Ensure runtime smoke test passes (PDF generation)
3. Open a Pull Request from `upgrade-to-NET10` → `master`
4. PR checklist:
   - [ ] Build passes in CI
   - [ ] No new warnings introduced
   - [ ] Smoke test verified locally
5. Merge using standard merge or squash commit

---

## 11. Success Criteria

### Technical Criteria

| Criterion | Expected Result |
|-----------|----------------|
| Target framework updated | `src/HtmlToPdfCore.csproj` targets `net10.0` |
| Package updates applied | `Microsoft.Extensions.Hosting` = 10.0.3, `Microsoft.Extensions.Hosting.Abstractions` = 10.0.3 |
| Solution restores successfully | `dotnet restore` completes with 0 errors |
| Solution builds successfully | `dotnet build` completes with **0 errors** |
| No package dependency conflicts | No version resolution warnings or conflicts in build output |
| No security vulnerabilities | Confirmed — 0 vulnerabilities identified in assessment |

### Quality Criteria

| Criterion | Expected Result |
|-----------|----------------|
| Code quality maintained | No new code-quality degradation introduced |
| Runtime behavior preserved | PDF generation produces correct output |
| Native library loads | `Wkhtmltopdf.NetCore` binaries load without errors under .NET 10 |

### Process Criteria

| Criterion | Expected Result |
|-----------|----------------|
| All-At-Once strategy followed | Single atomic upgrade; no intermediate partial states |
| Source control followed | All changes on `upgrade-to-NET10` branch; single commit; PR opened to `master` |
| Plan executed as documented | Steps performed in order per §2 and §4.1 |

### Definition of Done

The migration is **complete** when:

1. ✅ `src/HtmlToPdfCore.csproj` targets `net10.0`
2. ✅ All 2 flagged packages updated to their suggested versions
3. ✅ `dotnet build` produces 0 errors
4. ✅ Application starts and PDF generation works correctly
5. ✅ Changes committed to `upgrade-to-NET10` branch and PR opened to `master`
