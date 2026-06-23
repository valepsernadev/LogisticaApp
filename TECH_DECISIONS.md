# Decisiones Técnicas — LogisticaApp

Este archivo documenta las decisiones técnicas, versiones y errores conocidos del proyecto
para evitar repetir problemas en nuevas sesiones o chats.

---

## Stack

- **Runtime:** .NET 9 (`net9.0`) — NO usar net10.0, los paquetes no son compatibles
- **Base de datos:** PostgreSQL 16
- **ORM:** Entity Framework Core 9.0.0
- **Documentación API:** Scalar
- **Autenticación:** JWT con BCrypt para hash de passwords

---

## Paquetes NuGet y versiones exactas

| Paquete | Versión | Notas |
|---|---|---|
| `Npgsql.EntityFrameworkCore.PostgreSQL` | 9.0.4 | Conector de EF Core con PostgreSQL |
| `Microsoft.EntityFrameworkCore` | 9.0.0 | ORM principal |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 9.0.0 | Autenticación JWT |
| `Microsoft.AspNetCore.OpenApi` | 9.0.0 | Requerido para que Scalar funcione |
| `Scalar.AspNetCore` | 2.16.5 | Documentación de API |
| `BCrypt.Net-Next` | 4.2.0 | Hash de passwords |
| `EFCore.NamingConventions` | 9.0.0 | Convierte PascalCase a snake_case automáticamente |

---

## Errores conocidos y soluciones

### 1. Paquetes en versión 10.x
**Error:** `NETSDK1045: The current .NET SDK does not support targeting .NET 10.0`
**Causa:** Al instalar paquetes sin especificar versión, NuGet instala la última (10.x) que requiere .NET 10.
**Solución:** Siempre especificar la versión al instalar: `dotnet add package NombrePaquete --version 9.0.0`

### 2. Columnas no encontradas en PostgreSQL
**Error:** `column u.Email does not exist`
**Causa:** EF Core usa PascalCase pero PostgreSQL espera snake_case.
**Solución:** Agregar `UseSnakeCaseNamingConvention()` en el DbContext y tener instalado `EFCore.NamingConventions 9.0.0`.
```csharp
options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()
```

### 3. Scalar muestra 0.0.0.0 como servidor
**Error:** Scalar intenta hacer peticiones a `http://0.0.0.0:8080` y falla con `ERR_ADDRESS_INVALID`.

**Causa:** Kestrel escucha en `0.0.0.0` dentro del contenedor pero el navegador no puede alcanzar esa dirección.

**Solución:** Configurar el servidor en Scalar explícitamente:
```csharp
app.MapScalarApiReference(options =>
{
    options.WithTitle("LogisticaApp API");
    options.AddServer(new ScalarServer("http://localhost:8080"));
}).AllowAnonymous();
```
**Importante:** El método `WithServerUrl()` NO existe en Scalar 2.16.5, usar `AddServer()`.

### 4. Docker no resuelve mcr.microsoft.com (Windows)
**Error:** `dial tcp: lookup mcr.microsoft.com: no such host`

**Causa:** Docker Desktop en Windows con WSL2 no hereda el DNS del sistema operativo.

**Solución Windows:** Agregar las IPs directamente en el hosts de Docker Desktop:
```bash
wsl -d docker-desktop -u root sh -c "echo '150.171.70.10 mcr.microsoft.com' >> /etc/hosts && echo '150.171.70.10 eastus.data.mcr.microsoft.com' >> /etc/hosts"
```
**Nota:** En Linux este problema no existe, Docker usa el DNS del sistema directamente.

### 5. AddOpenApi() no encontrado
**Error:** `'IServiceCollection' does not contain a definition for 'AddOpenApi'`

**Causa:** Falta el paquete `Microsoft.AspNetCore.OpenApi`.

**Solución:** `dotnet add package Microsoft.AspNetCore.OpenApi --version 9.0.0`

---

## Configuración de Docker

### Imágenes
Siempre usar el prefijo completo `mcr.microsoft.com`:
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
```
**Nunca** usar `FROM dotnet/sdk:9.0` sin el prefijo, no existe en Docker Hub.

### Variables de entorno requeridas
```yaml
ASPNETCORE_ENVIRONMENT: Development
ASPNETCORE_URLS: http://+:8080
ConnectionStrings__DefaultConnection: "Host=db;Port=5432;Database=logistica_db;Username=logistica_user;Password=logistica_pass"
```

### Healthcheck de PostgreSQL
Siempre usar healthcheck para que la API espere a que la BD esté lista:
```yaml
healthcheck:
  test: ["CMD-SHELL", "pg_isready -U logistica_user -d logistica_db"]
  interval: 5s
  timeout: 5s
  retries: 5
depends_on:
  db:
    condition: service_healthy
```

---

## Convenciones del proyecto

- **Nombres de tablas:** snake_case (manejado automáticamente por EFCore.NamingConventions)
- **Primary keys:** UUID con `gen_random_uuid()` en PostgreSQL
- **Timestamps:** `created_at TIMESTAMP DEFAULT NOW()` en todas las tablas
- **Sin migraciones:** El esquema se define en `init.sql` y se monta en Docker
- **Interfaces:** Todos los servicios tienen su interfaz en `Services/Interfaces/`
- **Respuestas:** Siempre usar `ApiResponseDto<T>` para respuestas consistentes
- **Errores:** Manejados globalmente por `ErrorHandlerMiddleware`
- **Nivel:** Mid — buenas prácticas, SOLID, sin sobreingeniería

---

## URLs del proyecto

- **API:** http://localhost:8080
- **Scalar:** http://localhost:8080/scalar/v1

---

## Comando para levantar el proyecto
```bash
docker compose up --build
```