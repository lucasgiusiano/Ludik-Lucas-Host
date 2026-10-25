using System.Security.Claims;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaAplicacion.InterfacesCasosUsos.Medalla;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedallaController : ControllerBase
    {
        private IAltaMedalla _altaMedalla;
        private readonly IObtenerTodasLasMedallas _obtenerTodasLasMedallas;
        private readonly IObtenerMedallaPorId _obtenerMedallaPorId;
        private readonly IModificarMedalla _modificarMedalla;
        private readonly IBajaMedalla _bajaMedalla;
        private readonly IObtenerMedallasProfesorPorGrupo _obtenerMedallasProfesorPorGrupo;

        public MedallaController(
            IAltaMedalla altaMedalla,
            IObtenerTodasLasMedallas obtenerTodasLasMedallas,
            IObtenerMedallaPorId obtenerMedallaPorId,
            IModificarMedalla modificarMedalla,
            IBajaMedalla bajaMedalla,
            IObtenerMedallasProfesorPorGrupo obtenerMedallasProfesorPorGrupo)
        {
            _altaMedalla = altaMedalla;
            _obtenerTodasLasMedallas = obtenerTodasLasMedallas;
            _obtenerMedallaPorId = obtenerMedallaPorId;
            _modificarMedalla = modificarMedalla;
            _bajaMedalla = bajaMedalla;
            _obtenerMedallasProfesorPorGrupo = obtenerMedallasProfesorPorGrupo;
        }

        /// <summary>
        /// Este endpoint permite obtener todas las medallas que tiene el profesor y el sistema precargadas.
        /// </summary>
        /// <response code="200">Devuelve la lista de medallas.</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [Authorize(Policy = "EsProfesor")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MedallaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Resultado<IEnumerable<MedallaDto>> resultado = await _obtenerTodasLasMedallas.EjecutarAsync(profesorId);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Obtiene una medalla específica por su ID.
        /// </summary>
        /// <param name="id">ID de la medalla a obtener.</param>
        /// <response code="200">Devuelve la medalla solicitada.</response>
        /// <response code="400">ID de medalla inválido (ej. no positivo).</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="404">Medalla no encontrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [Authorize(Policy = "EsProfesor")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MedallaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var resultado = await _obtenerMedallaPorId.EjecutarAsync(id, profesorId);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Este endpoint permite a un profesor crear una medalla.
        /// </summary>
        /// <returns>
        /// 201 Created: Si la medalla se creo correctamente.
        /// 400 Bad Request: Si faltan datos o alguno es inválido.
        /// 401 Unauthorized: Si quien lo intenta hacer no es una persona autorizada (alguien que no sea un profesor).
        /// 500 Internal Server Error: Si ocurre un error inesperado durante el procesamiento.
        /// </returns>
        [Authorize(Policy = "EsProfesor")]
        [HttpPost("alta")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] MedallaAltaDto medallaDto)
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(profesorId))
                return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));
            Resultado resultado = await _altaMedalla.EjecutarAsync(medallaDto, profesorId);
            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return StatusCode(StatusCodes.Status201Created, "Medalla creada correctamente.");
        }

        /// <summary>
        /// Modifica una medalla existente.
        /// </summary>
        /// <param name="id">ID de la medalla a modificar.</param>
        /// <param name="medallaDto">Datos de la medalla para modificar.</param>
        /// <response code="204">Medalla modificada correctamente.</response>
        /// <response code="400">ID inválido o datos inválidos.</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="404">Medalla no encontrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [Authorize(Policy = "EsProfesor")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] MedallaEditarDto medallaDto)
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Resultado resultado = await _modificarMedalla.EjecutarAsync(id, medallaDto, profesorId);
            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);
            return NoContent();
        }

        /// <summary>
        /// Elimina una medalla por su ID.
        /// </summary>
        /// <param name="id">ID de la medalla a eliminar.</param>
        /// <response code="204">Medalla eliminada correctamente.</response>
        /// <response code="400">ID inválido.</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="404">Medalla no encontrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [Authorize(Policy = "EsProfesor")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(profesorId))
                return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

            Resultado resultado = await _bajaMedalla.EjecutarAsync(id, profesorId);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return NoContent();
        }

        /// <summary>
        /// Obtiene las medallas del profesor asociadas al grupo indicado.
        /// </summary>
        /// <param name="grupoId">Id del grupo.</param>
        /// <response code="200">Devuelve la lista de medallas del profesor para el grupo.</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [Authorize(Policy = "EsProfesorOEstudiante")]
        [HttpGet("grupo/{grupoId:int}")]
        [ProducesResponseType(typeof(IEnumerable<MedallaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPorGrupo(int grupoId)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
                return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

            var resultado = await _obtenerMedallasProfesorPorGrupo.EjecutarAsync(grupoId);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return Ok(resultado.Valor);
        }
    }
}
