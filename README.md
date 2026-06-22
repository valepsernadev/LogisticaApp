  # LogisticaApp

## Descripción
MVP de gestión logística de envíos.

## Arquitectura
**Layered Architecture (N-Capas)** — separación clara de responsabilidades en capas independientes donde cada una solo se comunica con la capa inmediatamente inferior.

## Capas

| Capa | Carpeta | Responsabilidad |
|------|---------|-----------------|
| Presentación | `Controllers/` | Recibe las peticiones HTTP y devuelve las respuestas al cliente |
| Lógica de negocio | `Services/` | Contiene las reglas y flujos de negocio de la aplicación |
| Dominio | `Models/` | Define las entidades que representan las tablas de la base de datos |
| Acceso a datos | `Data/` | Gestiona la conexión y configuración de Entity Framework Core |
| Transferencia | `DTOs/` | Objetos para transferir datos entre capas sin exponer entidades internas |

## Stack

- **Runtime:** .NET 9
- **Base de datos:** PostgreSQL
- **Contenedores:** Docker / Docker Compose

## Patrones

- **Service Layer** — encapsula la lógica de negocio en servicios reutilizables
- **Unit of Work** — agrupa operaciones de base de datos en una sola transacción
