using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;
using LogicaAplicacion.DTOs.RestablecerContrasenaDTO;
using LogicaAplicacion.InterfacesCasosUsos.RecuperarContrasena;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
using WebApi.Jwt;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecuperarContrasenaController : ControllerBase
    {
        private readonly IObtenerPreguntasDeSegurididadPorNombreUsuario _obtenerPreguntasDeSegurididadPorNombreUsuario;
        private readonly IRestablecerContrasena _restablecerContrasena;
        private readonly IObtenerPreguntasDeSeguridadDelSistema _obtenerPreguntasDeSeguridadDelSistema;

        public RecuperarContrasenaController(
            IObtenerPreguntasDeSegurididadPorNombreUsuario obteeDeSegurididadPorNombreUsuario,
            IRestablecerContrasena restablecerContrasena,
            IObtenerPreguntasDeSeguridadDelSistema obtenerPreguntasDeSeguridadDelSistema)
        {
            _obtenerPreguntasDeSegurididadPorNombreUsuario = obteeDeSegurididadPorNombreUsuario;
            _restablecerContrasena = restablecerContrasena;
            _obtenerPreguntasDeSeguridadDelSistema = obtenerPreguntasDeSeguridadDelSistema;
        }


        /// <summary>
        /// Obtener las preguntas de seguridad que ha respondido el estudiante al momento de registrase.
        /// </summary>
        /// <response code="200">Devuelve la lista de preguntas.</response>
        /// <response code="404"></response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("preguntas/{nombreUsuario}")]
        [ProducesResponseType(typeof(PreguntasDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerPreguntasDeSeguridadPorNombreUsuario(string nombreUsuario)
        {
            var resultado = await _obtenerPreguntasDeSegurididadPorNombreUsuario.EjecutarAsync(nombreUsuario);


            return resultado.EsExitoso ? Ok(resultado.Valor) : this.ManejarFallo(resultado);
        }

        /// <summary>
        /// Obtener todas las preguntas de seguridad que hay cargadas en el sistema.
        /// </summary>
        /// <response code="200">Devuelve la lista de preguntas del sistema.</response>
        /// <response code="404"></response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("preguntas")]
        [ProducesResponseType(typeof(PreguntasDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerPreguntasDeSeguridadDelSistema()
        {
            var resultado = await _obtenerPreguntasDeSeguridadDelSistema.EjecutarAsync();


            return resultado.EsExitoso ? Ok(resultado.Valor) : this.ManejarFallo(resultado);
        }


        /// <summary>
        /// Verifica las respuestas de seguridad y restablece la contraseña de un estudiante.
        /// Nota: Se requiere el id de la pregunta de seguridad que hay en el sistema y el id de la respuesta de seguridad que se ha guardado en la base de datos al momento de registrarse el estudiante.
        /// </summary>
        /// <response code="204">La contraseña fue restablecida con éxito.</response>
        /// <response code="400">Datos inválidos o la contraseña no cumple las políticas.</response>
        /// <response code="401">Las respuestas de seguridad son incorrectas.</response>
        /// <response code="404">Usuario no encontrado.</response>
        [HttpPost("restablecer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestablecerContrasena([FromBody] InformacionParaRestablecerContrasenaDto dto)
        {
            var resultado = await _restablecerContrasena.EjecutarAsync(dto);

            if (resultado.EsFallo)
            {
                return this.ManejarFallo(resultado);
            }

            return NoContent();
        }


    }
}
