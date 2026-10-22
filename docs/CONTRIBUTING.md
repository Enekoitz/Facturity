# How to Contribute

## Workflow (GitHub flow)

1. Create a branch from `main` with a prefix matching the type of change:
   - `feat/...`, `fix/...`, `docs/...`

2. Make small commits with clear messages.

3. Open a PR using the template.

4. Merge after review (or self-review if you work alone).

## Documentation rules

- **One branch per unit of change**, not per thinking session.

- If a code change affects the docs, **update them in the same PR** (`docs-as-code`).

- Changes to scope, architecture or important decisions → **new ADR** (Architecture Decision Record).

- Minor fixes (typos, glossary) can go straight to `main` if you work alone.

- Backlog and tasks are **not** documented here: they go to the task manager, **GitHub Issues**.

## Conventions

- Commits: `type: description` (`docs: update scope`, `feat(api): add global exception handler`, etc.).

- File names in lowercase with hyphens.

- Diagrams with Mermaid whenever possible.

- Every document ends with `Last reviewed: date`.
