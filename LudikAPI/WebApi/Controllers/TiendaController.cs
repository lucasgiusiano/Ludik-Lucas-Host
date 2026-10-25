using LogicaAplicacion.InterfacesCasosUsos.Tienda;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LogicaNegocio.Resultados;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EsProfesorOEstudiante")]
    public class TiendaController : ControllerBase
    {
        private readonly IObtenerListadoRecompensa _obtenerListadoRecompensa;
        public TiendaController(IObtenerListadoRecompensa obtenerListadoRecompensa)
        {
            _obtenerListadoRecompensa = obtenerListadoRecompensa;
        }
        /// <summary>
        /// Este endpoint permite a un profesor Obtener una Lista de  recompensa de una tienda.
        /// </summary>
        /// <returns>
        /// 201 Created: Si devuelve la lista de recompensas correctamente.
        /// 400 Bad Request: Si faltan datos o alguno es inválido.
        /// 401 Unauthorized: Si quien lo intenta hacer no es una persona autorizada (alguien que no sea un profesor).
        /// 500 Internal Server Error: Si ocurre un error inesperado durante el procesamiento.
        /// </returns>
        [HttpGet("{tiendaId}/recompensas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerRecompensas([FromRoute] int tiendaId)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized(new Error("Error.Unauthorized", "No tienes permisos para realizar esta acción."));

            var resultado = await _obtenerListadoRecompensa.EjecutarAsync(tiendaId);
            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);
            return Ok(resultado.Valor);
        }
    }
}
