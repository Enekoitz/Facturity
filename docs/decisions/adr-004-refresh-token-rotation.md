---
status: accepted
date: 2026-10-03
---
# ADR 004: Estrategia de rotación de refresh token

## Contexto

El acceso a la API se autentica mediante JWT (ver ADR 003). Los access tokens tienen una vida corta (15 min) para limitar el impacto de una posible fuga. Esto exige un mecanismo para que el cliente obtenga un access token nuevo sin forzar al usuario a volver a introducir sus credenciales cada 15 minutos.

Ese mecanismo es el refresh token: una credencial de vida más larga (21 días) que el cliente intercambia por un access token nuevo cuando el actual expira.

Un refresh token de larga duración introduce su propio riesgo: si se filtra, da acceso indefinido (hasta su expiración) sin necesidad de conocer la contraseña del usuario. Había que decidir cómo mitigar ese riesgo sin renunciar a la comodidad de no re-loguear constantemente.

## Decisión

Se implementa la opción 3: rotación en cada uso, con los tokens de una misma sesión agrupados bajo un FamilyId, y revocación de toda la familia si se detecta el reuso de un token ya gastado.

## Alternativas consideradas

- Refresh token único y estático durante toda su vida (21 días), validado contra un hash en BD, sin rotación.
	
- Refresh token con rotación simple: cada uso invalida el token y emite uno nuevo, pero sin mecanismo de detección de reutilización.
	
- Refresh token con rotación + detección de reutilización (reuse detection) agrupado por familia de sesión — opción elegida.
	
- Sin refresh token: access tokens de vida larga directamente.

## Consecuencias

- (+) Un escenario de robo de token se autocontiene: en el peor caso, el atacante obtiene acceso durante, como mucho, una rotación, y el usuario legítimo es forzado a volver a loguearse — un coste aceptable frente a una sesión comprometida indefinidamente.
	
- (+) Introduce una ventana de inconsistencia aceptada: si el proceso falla entre revocar el token viejo y emitir el nuevo, el usuario pierde la sesión y debe volver a loguearse. Se considera preferible fallar cerrando la sesión a fallar dejándola abierta.
	
- (-) Aumento de operaciones I/O y consumo de recursos.
	
- (=) Requiere una tabla RefreshTokens con limpieza periódica pendiente (tokens expirados/revocados acumulan filas indefinidamente si no se purgan — tarea futura, no bloqueante).
	
- (+) Cada dispositivo/sesión tiene su propia familia; comprometer una sesión no afecta a las demás sesiones activas del mismo usuario.

### Notas adicionales
(Ninguna actualmente)

## Referencias

- [ADR 03](adr-003-identity-jwt-strategy.md): Estrategia de gestión de identidades, autenticación y autorización
	
- OWASP: [Refresh Token Rotation](https://datatracker.ietf.org/doc/html/draft-ietf-oauth-security-topics)