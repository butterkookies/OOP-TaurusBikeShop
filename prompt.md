Before we do anything else, I want you to scan this entire codebase and
build a complete picture of the project. Save everything you find to
structured files in a Memory/ folder at the project root.

—— Step 1: Scan the codebase ————————————————————————————————————————

Read everything that reveals project intent and structure:

  STACK & RUNTIME
  · manifest files: package.json, composer.json, go.mod, Cargo.toml,
    requirements.txt, Gemfile, build.gradle, pom.xml — whatever exists
  · lock files for exact version pinning: package-lock.json, yarn.lock,
    composer.lock, Pipfile.lock, etc.
  · runtime config: .nvmrc, .python-version, .tool-versions

  FRAMEWORK & ARCHITECTURE
  · entry points: index.js, main.ts, app.py, main.go, src/main.*, etc.
  · directory structure: what lives in src/, lib/, app/, pages/, api/,
    components/, services/, models/, routes/, etc.
  · config files: next.config.js, vite.config.ts, webpack.config.js,
    tailwind.config.js, tsconfig.json, .babelrc, etc.

  DEPLOYMENT & INFRASTRUCTURE
  · Dockerfile, docker-compose.yml
  · fly.toml, vercel.json, netlify.toml, railway.toml, render.yaml,
    .platform.app.yaml, serverless.yml, amplify.yml
  · .github/workflows/*.yml, .gitlab-ci.yml, Jenkinsfile, bitbucket-pipelines.yml
  · any cloud-specific config (AWS SAM, CDK, Terraform, Pulumi)

  PROJECT INTENT
  · README.md, CONTRIBUTING.md, CHANGELOG.md, docs/
  · any existing FEATURES.md, TODO.md, ROADMAP.md
  · comments at the top of main entry files
  · package.json "description", "scripts", "engines" fields

  CODE CONVENTIONS
  · .eslintrc*, .prettierrc*, .editorconfig, phpcs.xml, .rubocop.yml,
    pyproject.toml [tool.black], .golangci.yml — whatever linting exists
  · test setup: jest.config.*, vitest.config.*, pytest.ini, phpunit.xml,
    go test patterns, spec/ or test/ directory structure
  · git config: .gitignore, .gitattributes, branch naming in recent commits

  DEPENDENCIES (top-level only, don't enumerate all transitive deps)
  · production dependencies and their purpose
  · dev dependencies that reveal tooling choices
  · any notable peer dependencies or version constraints

  CURRENT STATE
  · run: git log --oneline -20
  · run: git status
  · run: git branch -a
  · run: git tag --sort=-version:refname | head -10
  · note the current version if one exists

—— Step 2: Create the Memory/ folder and write these files ——————————

Create Memory/ at the project root if it doesn't exist.
Write each file below. Use only what you found — no placeholders,
no "TBD", no guessing. If something genuinely cannot be inferred,
write "NOT FOUND — needs manual input" for that field.

———————————————————————————————————————————————————————————————————————
FILE: Memory/project_overview.md
———————————————————————————————————————————————————————————————————————

# Project Overview

## Identity
- **Name**: [from package.json, README, or repo name]
- **Description**: [from README or package.json description]
- **Current Version**: [from package.json, Cargo.toml, or latest git tag]
- **Current State**: [active development / maintenance / pre-release / stable]

## Stack
- **Language(s)**: [with versions where pinned]
- **Runtime**: [Node.js vX, Python 3.X, Go 1.X, PHP 8.X, etc.]
- **Framework(s)**: [primary framework and version]
- **UI Layer**: [React, Vue, Svelte, plain HTML, none, etc.]
- **Styling**: [Tailwind, CSS Modules, SCSS, styled-components, etc.]
- **State Management**: [Redux, Zustand, Pinia, none, etc.]

## Data
- **Database**: [Postgres, MySQL, SQLite, MongoDB, none — infer from deps/config]
- **ORM / Query Layer**: [Prisma, Eloquent, GORM, SQLAlchemy, etc.]
- **Cache**: [Redis, Memcached, in-memory, none]
- **Storage**: [S3, local filesystem, Cloudflare R2, none]

## Architecture
- **Type**: [monolith / monorepo / microservices / serverless / static / CLI tool / library / plugin / desktop app / embedded firmware]
- **Entry Points**: [list the main entry files]
- **Key Directories**: [annotated list of what lives where]
- **API Style**: [REST / GraphQL / tRPC / WebSocket / none]

## Deployment
- **Target Platform**: [Fly.io / Vercel / Netlify / AWS / GCP / VPS / self-hosted / not yet deployed]
- **Deploy Method**: [git push / CI trigger / manual script / Docker / none found]
- **CI/CD**: [GitHub Actions / GitLab CI / none — name the workflow files found]
- **Environments**: [production / staging / dev — infer from config files and env examples]

## Repository State
- **Current Branch**: [output of git branch --show-current]
- **Latest Commits**: [paste last 10 from git log --oneline]
- **Latest Tags**: [paste from git tag output]
- **Working Tree**: [clean / dirty — from git status]
- **Open Branches**: [list from git branch -a]

---

———————————————————————————————————————————————————————————————————————
FILE: Memory/project_quality.md
———————————————————————————————————————————————————————————————————————

# Quality Contract

## Test Setup
- **Test Framework**: [Jest, Vitest, pytest, PHPUnit, go test, etc.]
- **Test Directory**: [where tests live]
- **Test Command**: [exact command to run all tests]
- **Coverage Tool**: [if configured — Istanbul, coverage.py, etc.]
- **Coverage Threshold**: [if set in config — otherwise: NOT SET, needs decision]

## Code Quality
- **Linter**: [ESLint, Pylint, PHPStan, golangci-lint, etc. — with config file]
- **Formatter**: [Prettier, Black, gofmt, etc. — with config file]
- **Lint Command**: [exact command]
- **Format Command**: [exact command]
- **Pre-commit Hooks**: [if .husky/, .pre-commit-config.yaml, etc. — list what runs]

## Performance Budgets
- **Bundle Size Limit**: [if set in config — otherwise: NOT SET]
- **Lighthouse Score Target**: [if documented — otherwise: NOT SET]
- **Core Web Vitals Target**: [if documented — otherwise: NOT SET]

## Definition of Done
> To be filled in by the developer — Claude cannot infer this.
> Example: "Feature branch passes all tests, lint clean, PR reviewed,
> deployed to staging, smoke test passes, FEATURES.md updated."

---

———————————————————————————————————————————————————————————————————————
FILE: Memory/project_danger_map.md
———————————————————————————————————————————————————————————————————————

# Danger Map

> High-risk files and areas. Updated as new risks are discovered.
> If you touch any file listed here, stop and re-read its entry first.

## Inferred from codebase scan

[For each dangerous file or pattern you find, write an entry like this:]

### [filename or pattern]
- **Why dangerous**: [auth handling / payment processing / shared DB state /
  expensive API call / many dependents / destructive operation / etc.]
- **Safe approach**: [what to do before touching it]
- **Known dependents**: [files or systems that depend on this]

[If nothing dangerous is clearly identifiable from the scan alone, write:]
> No high-risk files identified from static scan. This file will populate
> as the project develops. Add entries here when you discover expensive
> operations, fragile dependencies, or files that are risky to modify.

---

———————————————————————————————————————————————————————————————————————
FILE: Memory/reference_commands.md
———————————————————————————————————————————————————————————————————————

# Reference Commands

> Every command that matters for this project. Inferred from package.json
> scripts, Makefile, Dockerfile, CI config, and README. Verified where possible.

## Development