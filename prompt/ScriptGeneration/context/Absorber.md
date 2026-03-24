Here's your ready-to-paste prompt:

---

> Read `project-context.md` from my project root. Then read `.claude/context.md` if it already exists.
>
> Absorb everything from `project-context.md` into a structured `context.md` and save it to `.claude/context.md`.
>
> Rules:
> - Do not delete or ignore any information from `project-context.md` — absorb it all
> - Restructure and clean it into these sections if not already organized:
>   1. Identity
>   2. Tech Stack
>   3. Architecture
>   4. Coding Conventions
>   5. Current State
>   6. My Preferences
>   7. Constraints
> - If `.claude/context.md` already exists, merge both files — no duplicate info
> - Remove redundant or outdated entries during merge
> - Output the final result as a single clean markdown code block AND save it directly to `.claude/context.md`
> - This is a one-time migration — confirm when done

---
