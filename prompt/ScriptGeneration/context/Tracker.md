
---

> Read my `context.md` and `log.md` from my `.claude/` folder. Generate a `tracker.yaml` file that reflects my project's current state.
>
> Structure it with these sections:
>
> ```yaml
> project: (project name)
> last_updated: (today's date YYYY-MM-DD)
>
> features:
>   - name: (feature name)
>     status: done | in-progress | planned
>     notes: (brief detail or "none")
>     last_updated: (date or null)
>
> pages:
>   - url: (route or path)
>     status: done | in-progress | planned
>     notes: (brief detail or "none")
>     last_updated: (date or null)
>
> bugs:
>   - description: (what's broken)
>     severity: low | medium | high
>     status: open | in-progress | resolved
>     last_updated: (date or null)
>
> tasks:
>   - description: (task detail)
>     priority: low | medium | high
>     status: planned | in-progress | done
>     last_updated: (date or null)
> ```
>
> Rules:
> - Infer as much as possible from `context.md` and `log.md`
> - Only include sections relevant to my project — skip empty ones
> - Ask me in one consolidated list for anything you can't infer
> - Output the final file as a single clean YAML code block, ready to save as `.claude/tracker.yaml`

---
