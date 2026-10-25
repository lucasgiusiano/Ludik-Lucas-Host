using LogicaAplicacion.DTOs.TablaEquivalenciaDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.TablaEquivalencia;
using LogicaAplicacion.InterfacesCasosUsos.TablaEquivalencia;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EsProfesor")]
    public class TablaEquivalenciaController : Controller
    {
        private readonly IAltaTablaEquivalencia _altaTablaEquivalencia;
        private readonly IEditarTablaEquivalencia _editarTablaEquivalencia;
        private readonly IObtenerTablasEquivalenciaDelProfesor _obtenerTablasEquivalenciaDelProfesor;

        public TablaEquivalenciaController(IAltaTablaEquivalencia altaTablaEquivalencia, 
            IEditarTablaEquivalencia editarTablaEquivalencia,
            IObtenerTablasEquivalenciaDelProfesor obtenerTablasEquivalenciaDelProfesor)
        {
            _altaTablaEquivalencia = altaTablaEquivalencia;
            _editarTablaEquivalencia = editarTablaEquivalencia;
            _obtenerTablasEquivalenciaDelProfesor = obtenerTablasEquivalenciaDelProfesor;
        }


        /// <summary>
        /// Crea una nueva tabla de equivalencia de calificaciones.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite registrar una nueva "Tabla de Equivalencia" o "GPS de Calificaciones".
        /// La tabla asocia notas numéricas (1, 2, 3...) con un conjunto de medallas requeridas.
        /// 
        /// **Reglas de negocio aplicadas:**
        /// - Las notas deben ser una secuencia consecutiva desde 1.
        /// - Las medallas para una nota superior deben incluir todas las medallas de la nota inmediatamente anterior.
        /// </remarks>
        /// <param name="tablaDto">Objeto con los datos de la tabla a crear. Incluye el nombre y la lista de equivalencias (nota y medallas).</param>
        /// <response code="201">**Creado.** La tabla de equivalencia fue creada exitosamente.</response>
        /// <response code="400">**Solicitud incorrecta.** Los datos proporcionados no superaron las validaciones de negocio (ej. notas no secuenciales, medallas faltantes, etc.).</response>
        /// <response code="401">**No autorizado.** El usuario no está autenticado.</response>
        /// <response code="403">**Prohibido.** El usuario no tiene el rol de "Profesor".</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CrearTablaEquivalencia([FromBody] TablaEquivalenciaAltaDto tablaDto)
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(profesorId))
                return Unauthorized(new Error("Error.Unauthorized", "No se pudo identificar al profesor a partir del token."));


            var resultado = await _altaTablaEquivalencia.EjecutarAsync(tablaDto, profesorId);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return StatusCode(StatusCodes.Status201Created, "La tabla de equivalencia fue creada con exito.");
        }

        /// <summary>
        /// Edita una tabla de equivalencia existente.
        /// </summary>
        /// <param name="id">El ID de la tabla de equivalencia a modificar.</param>
        /// <param name="tablaDto">Objeto con los nuevos datos para la tabla.</param>
        /// <response code="200">**OK.** La tabla se actualizó exitosamente.</response>
        /// <response code="400">**Solicitud Incorrecta.** Los datos proporcionados son inválidos o el ID de la ruta no coincide con el del cuerpo.</response>
        /// <response code="403">**Prohibido.** El usuario no es el propietario de la tabla.</response>
        /// <response code="404">**No Encontrado.** No se encontró una tabla con el ID especificado.</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditarTablaEquivalencia([FromBody] TablaEquivalenciaDto tablaDto)
        {
          
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(profesorId))
                return Unauthorized(new Error("Error.Unauthorized", "No se pudo identificar al profesor a partir del token."));
            

            var resultado = await _editarTablaEquivalencia.EjecutarAsync(tablaDto, profesorId);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);
            

            return StatusCode(StatusCodes.Status200OK, "La tabla de equivalencia fue editada correctamente.");

        }

        /// <summary>
        /// Obtiene todas las tablas de equivalencia de un profesor.
        /// </summary>
        /// <response code="200">**OK.** Operacion exitosa, retorna las lista de tablas del profesor.</response>
        /// <response code="400">**Solicitud Incorrecta.** El id proporcionado no pertenece a ningún profesor.</response>
        /// <response code="403">**Prohibido.** El usuario no tiene el rol para acceder a este recurso.</response>
        /// <response code="404">**No Encontrado.** No se encontró ninguna tabla con el ID especificado.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTablasEquivalenciaDelProfesor()
        {

            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(profesorId))
                return Unauthorized(new Error("Error.Unauthorized", "No se pudo identificar al profesor a partir del token."));

            var resultado = await _obtenerTablasEquivalenciaDelProfesor.EjecutarAsync(profesorId);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return Ok(resultado.Valor);

        }
    }
}
