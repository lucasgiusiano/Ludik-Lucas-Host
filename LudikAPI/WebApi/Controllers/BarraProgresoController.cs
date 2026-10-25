using System.Security.Claims;
using LogicaAplicacion.InterfacesCasosUsos.BarraProgreso;
using LogicaNegocio.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EsEstudiante")]
    public class BarraProgresoController : ControllerBase
    {

        private readonly IObtenerContenidoBarraProgreso _obtenerContenidoBarraProgreso;

        public BarraProgresoController(IObtenerContenidoBarraProgreso obtenerContenidoBarraProgreso)
        {
            _obtenerContenidoBarraProgreso = obtenerContenidoBarraProgreso;
        }

        // <summary>
        /// Obtiene todos los datos para generar la barra de progreso para un perfil de estudiante.
        /// </summary>
        /// <param name="idPerfilEstudiante">El ID del perfil del estudiante.</param>
        /// <returns>Un resultado de la operación.</returns>
        [HttpGet("{idPerfilEstudiante}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerDatosParaGenerarBarraProgreso(int idPerfilEstudiante)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var resultado = await _obtenerContenidoBarraProgreso.EjecutarAsync(idPerfilEstudiante, usuarioId);

            return resultado.EsExitoso
                ? Ok(resultado.Valor)
                : this.ManejarFallo(resultado);
        }

    }
}
