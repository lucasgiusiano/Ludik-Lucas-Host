using LogicaAplicacion.DTOs.KudoDTOs;
using LogicaAplicacion.DTOs.PerfilEstudianteDTO;
using LogicaAplicacion.InterfacesCasosUsos.Kudo;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Helpers;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EsProfesorOEstudiante")]
    public class KudoController : ControllerBase
    {

        private readonly IAsignarKudo _asignarKudo;
        private readonly IObtenerKudos _obtenerKudos;

        public KudoController(
            IAsignarKudo asignarKudo, IObtenerKudos obtenerKudos)
        {
            _asignarKudo = asignarKudo;
            _obtenerKudos = obtenerKudos;
        }

        /// <summary>
        /// Permite asignarle un kudo a un estudiante.
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AsignarKudo([FromBody] AsignarKudoDto kudoDto)
        {
            var estudianteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(estudianteId))
                return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

            var resultado = await _asignarKudo.EjecutarAsync(estudianteId,kudoDto);

            return resultado.EsExitoso ? NoContent() : this.ManejarFallo(resultado);

            
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerKudos()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
                return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

            Resultado<IEnumerable<TipoKudoDto>> resultado = await _obtenerKudos.EjecutarAsync();

            return resultado.EsExitoso ? Ok(resultado.Valor) : this.ManejarFallo(resultado);


        }

    }
}
