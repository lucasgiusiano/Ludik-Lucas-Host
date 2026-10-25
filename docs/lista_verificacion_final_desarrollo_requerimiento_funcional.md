# **Lista de Verificación** Para Final de Cada Funcionalidad

## 1. Estado de compilación y errores

- [ ] Compila sin errores.
- [ ] No hay _warnings_ suprimidos en el proyecto.
- [ ] La funcionalidad implementada funciona según lo esperado.

## 2. Calidad y estilo de código

- [ ] Nombres de clases, metodos y constantes en PascalCase.
- [ ] Nombres de parametros en metodos y variables locales en camelCase.
- [ ] No hay código muerto ni duplicado; se extrae lógica común.
- [ ] No hay `Console.WriteLine`, bloques de depuración ni código comentado innecesario.

## 3. Arquitectura y mantenibilidad

- [ ] Separación de capas: API ↔ Aplicación ↔ Dominio ↔ Infraestructura, con dependencias unidireccionales.
- [ ] Las interfaces están en la capa de Aplicación; las implementaciones, en Infraestructura.
- [ ] Se inyectan dependencias vía contenedor (no `new` en controladores).
- [ ] Se parametriza con abstracciones (interfaces/abstractas) en lugar de clases concretas.

## 4. Pruebas

- [ ] Cobertura ≥ 80 % en dominio y aplicación.
- [ ] Cada caso de uso tiene tests unitarios (positivos/negativos).

## 5. Documentación y API

- [ ] Cada metodo en tiene pre y pos condicion.
- [ ] Controladores documentados con XML `<summary>` para Swagger.
- [ ] DTOs anotados (`[Required]`, `[MaxLength]`).
- [ ] Comentarios solo para aclarar el “por qué”, no el “cómo”.

## 6. Rendimiento y seguridad

- [ ] Validación de entrada centralizada (FluentValidation o DataAnnotations).
- [ ] Manejo de excepciones.
- [ ] Autorización aplicada (`[Authorize]`, roles, claims).
- [ ] Paginación o streaming (`IAsyncEnumerable`) para colecciones grandes.
