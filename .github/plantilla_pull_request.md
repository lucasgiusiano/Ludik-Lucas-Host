# Lista de verificación para Pull Request

## 1. Descripción y contexto
- [ ] ¿El título del PR es claro y descriptivo?  
- [ ] ¿La descripción explica el propósito del cambio y su contexto?  

## 2. Calidad del código

### Convenciones y estilo
- [ ] Verificar linters (ESLint, StyleCop, RuboCop) sin errores.  
- [ ] Alineación de indentación, espacios y tabuladores uniformes.   

### Legibilidad y nomenclatura
- [ ] Variables y funciones con nombres claros y descriptivos (e.g. `calcularTotal()` vs `ct()`).  
- [ ] Evitar abreviaturas oscuras; preferir `ObtenerResultadosDeBusqueda` en lugar de `GetRslt`. Claridad sobre brevedad.
- [ ] Clases, métodos y constantes tienen nombres en PascalCase. Ejemplo: UsuarioService, CalcularPromedio
- [ ] Parámetros de métodos y variables locales en camelCase. Ejemplo: cantidadUsuarios, nombreArchivo
- [ ] El nombre de un interfaz debe comenzar con una "I" mayúscula. Ejemplo: IUsuario
- [ ] Agrupar propiedades relacionadas en objetos o estructuras.  

### Modularidad y responsabilidad única
- [ ] Cada función/módulo cumple un único propósito (Single Responsibility).  
- [ ] Máximo ~30 líneas por función para facilitar pruebas y mantenimiento.  
- [ ] Validar que no existan “God objects” o clases que hagan todo.  

### Complejidad y acoplamiento
- [ ] No hay codigo duplicado (DRY).  

### Código muerto y comentarios
- [ ] Eliminar bloques comentados o variables no usadas.  
- [ ] Hay comentarios solo para explicar “por qué”, no “qué”.  
- [ ] Documentar intenciones complejas o algoritmos no triviales. 
- [ ] Cada metodo tiene pre y pos condición. 
 
### Performance y recursos
- [ ] No hay loops costosos o recursiones profundas.  
- [ ] Se reviso el uso de memoria en colecciones grandes.  

## 3. Pruebas y validación

### Cobertura y alcance
- [ ] Hay una cobertura mínima del 70 % de líneas críticas; priorizar lógica de negocio.  
- [ ] Hay tests para rutas felices, bordes (nulos, límites de arrays, etc) y casos de error.  

### Tipos de pruebas
- [ ] Hay pruebas Unitarias.  
- [ ] Hay pruebas de Integración (interacción entre módulos, base de datos).  

### Revisión de resultados
- [ ] Validar logs de test en busca de warnings o errores silenciosos.  
- [ ] No hay pruebas que fallan intermitentemente (si las hay, deben corregirse o marcarse).   