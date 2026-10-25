using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaAplicacion.InterfacesCasosUsos.Usuario;
using LogicaNegocio.Resultados;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IObtenerPerfilUsuarioLogueado _obtenerPerfil;

        public UsuarioController(IObtenerPerfilUsuarioLogueado obtenerPerfil)
        {
            _obtenerPerfil = obtenerPerfil;
        }

        /// <summary>
        /// Obtiene el perfil del usuario logueado (Estudiante o Profesor) con sus datos específicos.
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Me()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized(new Error("Error.Unauthorized", "No se pudo identificar al usuario."));

            var resultado = await _obtenerPerfil.EjecutarAsync(usuarioId);
            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return Ok(resultado.Valor);
        }
    }
}
