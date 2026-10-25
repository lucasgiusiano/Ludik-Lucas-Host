using LogicaAplicacion.DTOs.GrupoDTOs;
using System.Security.Claims;
using LogicaAplicacion.InterfacesCasosUsos.Recompensa;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using Microsoft.AspNetCore.Authorization;
using LogicaAplicacion.ImplementacionCasosUsos.Recompensa;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EsProfesor")]
    public class RecompensaController : ControllerBase
    {
        private readonly IAltaRecompensa _altaRecompensa;
        private readonly IEditarRecompensa _editarRecompensa;
        private readonly IBajaRecompensa _bajaRecompensa;
        private readonly IAsignarRecompensaTiendas _asignarRecompensa;
        private readonly IObtenerRecompensasDelProfesor _obtenerRecompensasDelProfesor;

        public RecompensaController(IAltaRecompensa altaRecompensa,
            IEditarRecompensa editarRecompensa,
            IBajaRecompensa bajaRecompensa,
            IAsignarRecompensaTiendas asignarRecompensaTiendas,
            IObtenerRecompensasDelProfesor obtenerRecompensasDelProfesor)
        {
            _altaRecompensa = altaRecompensa;
            _editarRecompensa = editarRecompensa;
            _bajaRecompensa = bajaRecompensa;
            _asignarRecompensa = asignarRecompensaTiendas;
            _obtenerRecompensasDelProfesor = obtenerRecompensasDelProfesor;
        }

        /// <summary>
        /// Este endpoint permite a un profesor crear una recompensa.
        /// </summary>
        /// <returns>
        /// 201 Created: Si la Recompensa se creo correctamente.
        /// 400 Bad Request: Si faltan datos o alguno es inválido.
        /// 401 Unauthorized: Si quien lo intenta hacer no es una persona autorizada (alguien que no sea un profesor).
        /// 500 Internal Server Error: Si ocurre un error inesperado durante el procesamiento.
        /// </returns>
        [HttpPost("alta")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AltaRecompensa([FromBody] RecompensaSimpleAltaDto recompensaRequest)
        {

            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(profesorId))
                return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

            var resultado = await _altaRecompensa.EjecutarAsync(recompensaRequest, profesorId);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return StatusCode(StatusCodes.Status201Created, "La Recompensa ha sido creado correctamente.");

        }

        /// <summary>
        /// Este endpoint permite a un profesor editar una recompensa.
        /// </summary>
        /// <returns>
        /// 201 Created: Si la Recompensa se edito correctamente.
        /// 400 Bad Request: Si faltan datos o alguno es inválido.
        /// 401 Unauthorized: Si quien lo intenta hacer no es una persona autorizada (alguien que no sea un profesor).
        /// 500 Internal Server Error: Si ocurre un error inesperado durante el procesamiento.
        /// </returns>
        [HttpPut("{recompensaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarRecompensa([FromRoute] string recompensaId,
            [FromBody] RecompensaSimpleEditarDto dto)
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(profesorId))
                return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

            var resultado = await _editarRecompensa.EjecutarAsync(recompensaId, dto, profesorId);
            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return Ok("Recompensa actualizada correctamente.");
        }

        /// <summary>
        /// Este endpoint permite a un profesor eliminar una recompensa.
        /// </summary>
        /// <returns>
        /// 201 Created: Si la Recompensa se elimino correctamente.
        /// 400 Bad Request: Si faltan datos o alguno es inválido.
        /// 401 Unauthorized: Si quien lo intenta hacer no es una persona autorizada (alguien que no sea un profesor).
        /// 500 Internal Server Error: Si ocurre un error inesperado durante el procesamiento.
        /// </returns>
        [HttpDelete("{recompensaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BajaRecompensa([FromRoute] string recompensaId)
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(profesorId))
                return this.ManejarFallo(Resultado.Falla(new Error("Error.Unauthorized", "No autenticado.")));

            var resultado = await _bajaRecompensa.EjecutarAsync(recompensaId, profesorId);
            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return Ok("Recompensa eliminada correctamente.");
        }

        /// <summary>
        /// Asigna una recompensa existente a las tiendas de uno o más grupos del profesor.
        /// </summary>
        /// <param name="dto">DTO que contiene el Id de la recompensa y la lista de Ids de los grupos.</param>
        /// <returns>
        /// 200 OK: Si la asignación se completó.
        /// 400 Bad Request: Si los datos de entrada son inválidos o si alguna regla de negocio no se cumple (ej. recompensa ya asignada).
        /// 401 Unauthorized: Si el usuario no está autenticado.
        /// 403 Forbidden: Si el usuario no es un profesor.
        /// 404 Not Found: Si la recompensa o alguno de los grupos no se encuentra.
        /// </returns>
        [HttpPost("asignar-a-grupos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AsignarRecompensaTiendasGrupos([FromBody] RecompensaYGruposDto dto)
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(profesorId))
                return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

            var resultado = await _asignarRecompensa.EjecutarAsync(dto, profesorId);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);


            return Ok("Recompensa asignada correctamente a las tiendas de los grupos seleccionados.");
        }
        /// <summary>
        /// Retona las recompensas creadas por el profesor autenticado.
        /// </summary>
        /// <returns>Si es exitoso retorna la lista de recompensas que creo el profesor, si falla devuelve el error por el cual falla.</returns>
        [HttpGet("obtener-recompensas-profesor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> ObtenerRecompensasDelProfesor()
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(profesorId))
                return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

            var resultado = await _obtenerRecompensasDelProfesor.EjecutarAsync(profesorId);

            return resultado.EsExitoso ? Ok(resultado.Valor) : this.ManejarFallo(resultado);
        }
    }
}
