Here's your update prompt, ready to paste:

---

> Read my existing `context.md`. Then scan my project for anything that has changed or is missing — new files, dependencies, stack changes, shifted priorities, or new conventions.
>
> Update rules:
> - **Do not rewrite** sections that are still accurate
> - **Append or replace** only what has changed
> - **Add to Current State** — what's now working, broken, or in progress
> - **Flag** anything in the file that looks outdated or contradicted by what you see
>
> Ask me only what you can't infer — in one consolidated list. Then output the full updated `context.md` as a single clean markdown code block.

---

**When to run this:**
| Trigger | Frequency |
|---|---|
| After finishing a major feature | Every time |
| Start of a new work session (big projects) | Weekly |
| After onboarding a new tool/library | Every time |
| Before handing off to someone else | Every time |

---

**Suggested workflow for your Pro account:**

```
Session start  →  paste context.md manually into Project instructions
                        ↓
Do your work
                        ↓
Session end    →  run the update prompt in Claude Code
                        ↓
Commit updated context.md to GitHub
```

This keeps your vault lean and accurate without bloating it over time.

Want me to also write a `log.md` update prompt to pair with this — so you always have a running history of what was done each session?