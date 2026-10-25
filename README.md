# Ludik — Plataforma web de ludificación para educación secundaria

>  Proyecto full-stack que convierte participación y logros de estudiantes en **medallas, monedas, rankings y recompensas**, con **barra de progreso** y **tabla de equivalencias (GPS de calificaciones)**, para dar **feedback inmediato** y elevar la motivación. Backend **ASP.NET Core 8 (Clean Architecture)**, frontend **React**, y despliegue en **Azure**.

---

## Problema & propuesta de valor

En secundaria, el feedback llega tarde y los alumnos no ven su progreso al día; eso baja el compromiso. **Ludik** aporta retroalimentación continua, visible y accionable (progreso, metas, logros y recompensas) que **reduce carga docente** y **mejora la motivación** del alumnado.

---

## Arquitectura (Clean Architecture + SPA)

- **Frontend**: React (SPA), consume API REST y actualiza el estado en cliente.  
- **API**: ASP.NET Core 8, controladores REST; **CQRS con MediatR** para casos de uso.  
- **Dominio / Aplicación**: `LogicaNegocio` (entidades, reglas) y `LogicaAplicacion` (casos de uso, DTOs).  
- **Infra / Datos**: `AccesoDatos` con **EF Core 8** sobre **SQL Server**.  
- **Jobs**: **Hangfire** para tareas en segundo plano (p. ej., reseteo semanal de “Kudos”).  
- **Auth**: **ASP.NET Identity + JWT**.  
- **DevOps**: Swagger/OpenAPI; GitHub Actions; Azure (App Service + Static Web Apps + Azure SQL + Blob Storage).

---

## Funcionalidades clave

- **Medallas** (docente y entre pares, con validación), **monedas** y **recompensas** (tienda).  
- **Rankings** por tipo de medalla; **hitos** y **potenciadores** (multiplicadores temporales).  
- **Metas personales** por asignatura + **barra de progreso** con próximos pasos.  
- **Tabla de equivalencias (GPS)** que traduce medallas a **nota** de forma objetiva.  
- **Gestión de grupos/asignaturas** (códigos/QR), perfiles y avatares, recuperación de contraseña.

---

## Tecnologías principales

- **Backend**: ASP.NET Core 8, CQRS (MediatR), EF Core 8, Identity + JWT, Hangfire, Swagger.  
- **Frontend**: React (SPA).  
- **Base de datos**: SQL Server / Azure SQL.  
- **Cloud**: Azure App Service, Static Web Apps, Blob Storage.  
- **Testing**: xUnit + Moq.

---

## Estructura del repositorio

~~~plaintext
Ludik-main/
├── LudikAPI/                    # Solución backend .NET 8 (Clean Architecture)
│   ├── AccesoDatos/             # EF Core, migraciones, repositorios
│   ├── LogicaNegocio/           # Entidades y reglas de dominio
│   ├── LogicaAplicacion/        # Casos de uso (CQRS/MediatR), DTOs
│   ├── WebApi/                  # ASP.NET Core API, controladores, Identity/JWT, Swagger
│   ├── PruebasUnitarias/        # Tests con xUnit + Moq
│   └── PruebasIntegracion/      # Tests de integración
│
├── ludikclient/                 # Frontend React (SPA)
│   ├── src/
│   │   ├── components/          # Componentes reutilizables
│   │   ├── features/            # Módulos (grupos, medallas, recompensas, etc.)
│   │   ├── hooks/               # Hooks (React Query)
│   │   ├── lib/                 # Configuración de axios y utilidades
│   │   └── pages/               # Páginas principales
│   └── package.json
│
├── docs/                        # Diagramas y documentación técnica
├── .github/workflows/           # CI/CD: build, test y deploy en Azure
└── README.md                    # Este documento
~~~

**Controladores expuestos (ejemplos)**:  
`Login`, `Profesor`, `Estudiante`, `Grupo`, `Medalla`, `AsignacionMedallas`, `TablaEquivalencia`, `TablaClasificacion`, `Tienda`, `Recompensa`, `BarraProgreso`, `Avatar`, `Kudo`, `RecuperarContrasena`.

---

## Puesta en marcha (local)

### Requisitos

- **.NET SDK 8**  
- **Node.js ≥ 20** / **npm ≥ 10**  
- **SQL Server** (local o contenedor)  
- Cuenta o contenedor de **Azure Blob Storage** (opcional)

### Backend (API)

Configurar `LudikAPI/WebApi/appsettings.json`:

- `ConnectionStrings:DefaultConnection` → cadena de conexión a SQL Server.  
- `JwtSettings` → clave, issuer y audience para JWT.  
- *(Opcional)* `AzureStorage` y `Storage*` si se usan blobs reales.

Ejecutar migraciones y correr el servidor:

~~~bash
cd LudikAPI/WebApi
dotnet build
dotnet ef database update
dotnet run
~~~

La API estará disponible en `https://localhost:5001` (Swagger en `/swagger`).

### Frontend (SPA)

Configurar la URL base de la API en `ludikclient/src/lib/axios.js`.

Instalar dependencias y ejecutar:

~~~bash
cd ludikclient
npm ci
npm start
~~~

Generar build de producción:

~~~bash
npm run build
~~~

### Pruebas

~~~bash
cd LudikAPI
dotnet test
~~~

---

## Módulos destacados (relación con el negocio)

- **GPS de Calificaciones (Tabla de equivalencias)**: define escalones de nota basados en combinaciones crecientes de medallas; alimenta la **barra de progreso** y hace la evaluación **objetiva y transparente**.  
- **Economía interna**: medallas → monedas → **tienda de recompensas** (beneficios configurables).  
- **Rankings** y **Kudos** (reseteables) para ciclos cortos de competencia sana.  
- **Avatares** con atributos (imágenes gestionadas en Blob Storage).

> La motivación no es decorativa: la **UX** es un requisito funcional para que las recompensas “valgan” a ojos del estudiante.

---

## Despliegue y CI/CD

- **GitHub Actions**: build, test y despliegue automatizado.  
- **Azure**: App Service para la API, Static Web Apps para el frontend, **Azure SQL** y **Blob Storage** para datos y archivos.

---

## Por qué importa para un reclutador

- **Producto real orientado a usuarios** (docentes y estudiantes) que soluciona un problema educativo con **feedback inmediato**.  
- **Arquitectura mantenible y escalable** (Clean Architecture + CQRS + jobs en background).  
- **Prácticas de ingeniería modernas**: pruebas automatizadas, documentación (Swagger), CI/CD y despliegue cloud.

---

## Roadmap (sugerido)

- MFA y rotación periódica de JWT.  
- Métricas/telemetría (Application Insights) y panel de analítica docente.  
- Internacionalización (i18n) y accesibilidad (WCAG).  
- Modo offline para aulas con conectividad limitada.

---

## Créditos

Proyecto académico (Universidad ORT Uruguay) desarrollado por **Renato Ríos**, **Lucas Giusiano** y **Manuel Martínez**.

> Para el detalle completo de requerimientos, decisiones de diseño y resultados del proyecto, ver el documento técnico incluido en el repositorio.
