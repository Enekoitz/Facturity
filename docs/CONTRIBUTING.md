# Contributing

Thanks for contributing! This guide takes you from a fresh clone to a merged pull request.

## Development Setup

1. Fork and clone the repository.
2. Install dependencies:

```bash
dotnet restore
```

3. Run the test suite to confirm everything works:

```bash
dotnet test
```

## Branch Naming

All branches are created from `main` and follow the pattern:

```
<type>/<issue#>-<short-description>
```

| Type       | Purpose                               | Example                      |
| ---------- | ------------------------------------- | ---------------------------- |
| `feat`     | New feature                           | `feat/42-add-csv-export`     |
| `fix`      | Bug fix                               | `fix/89-header-overflow`     |
| `hotfix`   | Urgent production fix                 | `hotfix/101-crash-on-login`  |
| `refactor` | Code restructure (no behavior change) | `refactor/56-optimize-query` |
| `docs`     | Documentation only                    | `docs/12-update-api`         |
| `test`     | Test additions or fixes               | `test/77-add-e2e-coverage`   |
| `chore`    | Maintenance, deps, config             | `chore/30-update-deps`       |

**Rules:**

- Lowercase, kebab-case (hyphens, no underscores or spaces).
    
- Include the issue number (no `#`).
    
- Keep the description to 2–5 words.
    
- Delete the branch after merge.
    

## Commit Messages

We follow [Conventional Commits](https://www.conventionalcommits.org/):

```
<type>[optional scope]: <description>

[optional body]

[optional footer — e.g. Closes #42]
```

**Examples:**

```
feat(auth): add OAuth2 login flow 

Implements the OAuth2 provider requested in #42.

Closes #42
```

```
fix: correct off-by-one in pagination

The last page was skipped when total % perPage === 0.
```

**Rules:**

- Use the imperative mood: "add", not "added".
    
- Keep the subject line under 72 characters.
    
- One logical change per commit.
    
- Reference related issues in the footer.
    

## Pull Requests

Before opening a PR, make sure:

- [ ] Branch is up to date with `main`.
    
- [ ] All tests pass locally (`dotnet test`).
    
- [ ] Lint and format checks pass (`dotnet format --verify-no-changes`).
    
- [ ] The PR is focused on a single concern.
    
- [ ] The description links the issue (`Closes #42`).
    

**PR description should include:**

- What changed and why.
    
- How to test / verify the change.
    
- Screenshots or recordings for UI changes.
    

**Merge policy:** Squash and merge only. The PR title becomes the commit message, so keep it in Conventional Commits format.

## Code Style

- Follow the existing code style (enforced by linter).
    
- Run `dotnet format` before committing.
    
- Keep functions small and focused.
    
- Add tests for new behavior.
    

## Code of Conduct

Please read our Code of Conduct. Be kind and respectful in all interactions.

## Questions?

- For small fixes (typos, obvious bugs), go ahead and open a PR.
    
- For larger changes or new features, open an issue first to discuss the approach.
    