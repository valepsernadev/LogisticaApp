# LogisticaApp

## Descripción
MVP de sistema de gestión logística de envíos. Permite gestionar envíos, conductores, rutas y validación de entregas asistida por IA.

## Stack
- .NET 9 Web API
- PostgreSQL 16
- Docker + Docker Compose
- Scalar (documentación de API)

## Arquitectura
Layered Architecture (N-Capas) dentro de un monolito:
- Controllers: recibe peticiones HTTP, nada de lógica aquí
- Services: toda la lógica de negocio va aquí
- Models: entidades de la base de datos
- Data: AppDbContext y configuración de EF Core
- DTOs: objetos para transferir datos entre capas

## Patrón
Service Layer + Unit of Work (el DbContext ES el Unit of Work)

## Base de datos
PostgreSQL con init.sql. Sin migraciones.

## Roles del sistema
- Cliente: crea solicitudes y hace seguimiento de envíos
- Conductor: gestiona entregas y sube evidencias
- Operador Logístico: asigna envíos y gestiona conductores y vehículos
- Administrador: dashboards financieros y gestión de usuarios

## Reglas importantes
- Nivel junior: código claro, sin abstracciones innecesarias
- Sin Repository Pattern
- Sin migraciones de EF Core, usar init.sql
- Documentación con Scalar, no Swagger
- Todos los endpoints protegidos con JWT excepto login y register
- Manejo de errores con try/catch y códigos HTTP correctos
- Nombres de clases y métodos en inglés, comentarios en español

## Cómo correr el proyecto
```bash
docker compose up --build