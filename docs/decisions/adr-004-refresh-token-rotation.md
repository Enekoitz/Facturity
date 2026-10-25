---
status: accepted
date: 2026-10-03
---
# ADR 004: Refresh Token Rotation Strategy

## Context

API access is authenticated via JWT (see ADR 003). Access tokens have a short lifespan (15 minutes) to limit the impact of a potential leak. This necessitates a mechanism for the client to obtain a new access token without forcing the user to re-enter their credentials every 15 minutes.

This mechanism is the "refresh token"—a longer-lived credential (21 days) that the client exchanges for a new access token when the current one expires.

A long-lived refresh token introduces its own risk: if leaked, it grants indefinite access (until it expires) without requiring the user's password. We needed to determine how to mitigate this risk while maintaining the convenience of not having to constantly re-authenticate.

## Decision

We are implementing Option 3: rotation upon every use, with tokens from the same session grouped under a `FamilyId`. If the reuse of an already-spent token is detected, the entire family of tokens is revoked.

## Alternatives Considered

- A single, static refresh token for its entire lifespan (21 days), validated against a hash in the database, with no rotation.

- Refresh token with simple rotation: each use invalidates the token and issues a new one, but without a mechanism to detect reuse.

- Refresh token with rotation and reuse detection, grouped by session family — the chosen option.

- No refresh token: using long-lived access tokens directly.

## Consequences

- (+) A token theft scenario is self-contained: in the worst case, an attacker gains access for, at most, the duration of a single rotation cycle, and the legitimate user is forced to log in again—an acceptable trade-off compared to having a session compromised indefinitely. - (+) Introduces a window of acceptable inconsistency: if the process fails between revoking the old token and issuing the new one, the user loses their session and must log in again. It is considered preferable for a failure to result in a closed session rather than an open one.

- (-) Increased I/O operations and resource consumption.

- (=) Requires a `RefreshTokens` table that necessitates periodic cleanup (expired/revoked tokens will accumulate rows indefinitely if not purged — a non-blocking, future task).

- (+) Each device/session has its own family; compromising one session does not affect other active sessions belonging to the same user.

## Additional Notes
(None at present)

## References

- [ADR 03](adr-003-identity-jwt-strategy.md): Identity management, authentication, and authorization strategy

- OWASP: [Refresh Token Rotation](https://datatracker.ietf.org/doc/html/draft-ietf-oauth-security-topics)