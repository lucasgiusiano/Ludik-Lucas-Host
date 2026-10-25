using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaAplicacion.InterfacesCasosUsos.Login;
using LogicaNegocio.Excepciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Jwt;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginUsuario _loginUsuario;

        public LoginController(ILoginUsuario loginUsuario)
        {
            _loginUsuario = loginUsuario;
        }

        /// <summary>
        /// Este endpoint permite que un usuario se autentifique en el sistema.
        /// </summary>
        /// <returns>
        /// 200 Ok: Si el usuario fue logueado correctamente devuelve un token y datos del usuario.
        /// 400 Bad Request: Si los datos enviados son inválidos o faltan, o si la cuenta requiere acciones adicionales.
        /// 401 Unauthorized: Si las credenciales son incorrectas o la cuenta está bloqueada.
        /// 500 Internal Server Error: Si ocurre un error inesperado durante el procesamiento.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginRespuestaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginSolicitudDto loginSolicitud)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _loginUsuario.EjecutarAsync(loginSolicitud);

            return resultado.EsExitoso
                ? Ok(resultado.Valor)
                : this.ManejarFallo(resultado);
        }
    }

}


