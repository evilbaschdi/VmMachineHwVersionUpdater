---
name: stage-all-and-commit
description: 'Stage all changed files and create a concise commit message from the complete staged diff. Use when asked to stage all changes, prepare a commit message, or commit the current work.'
---

# Stage All Changes and Commit

Use this workflow only when the user asks to stage or commit changes.

1. Run `git status --short` and identify the current branch. Do not discard or overwrite changes.
2. Stage every tracked and untracked change with `git add --all`.
3. Inspect `git diff --cached --check` and the complete staged diff. Resolve whitespace errors and ensure the staged changes are in scope; never silently omit changed files.
4. Generate a short commit subject that accurately describes all staged changes. Prefer the repository's recent commit style; do not invent a scope or claim tests that were not run.
5. Show the proposed subject and staged file summary. If the user asked only for a message, stop here. If the user asked to commit, create the commit with that subject.
6. Verify the resulting commit and confirm the worktree status. Report any errors instead of implying success.

Never use `git add -u` or path-specific staging when the request is to stage all changed files. Never amend a commit unless explicitly requested.
