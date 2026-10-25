using LogicaAplicacion.DTOs.AvatarDTOs;
using LogicaAplicacion.InterfacesCasosUsos.Avatar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Helpers;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy="EsEstudiante")]
    public class AvatarController : ControllerBase
    {
        private readonly IModificarAvatar _modificarAvatar;
        private readonly IObtenerAtributosAvatarDisponiblesParaPerfil _obtenerAtributosAvatar;

        public AvatarController(IModificarAvatar modificarAvatar, IObtenerAtributosAvatarDisponiblesParaPerfil obtenerAtributosAvatar)
        {
            _modificarAvatar = modificarAvatar;
            _obtenerAtributosAvatar = obtenerAtributosAvatar;
        }

        /// <summary>
        /// Modifica el avatar del perfil de estudiante recibido.
        /// </summary>
        /// <param name="idPerfilEstudiante">El ID del perfil del estudiante al cual le pertenece el avatar.</param>
        /// <param name="avatarDto">DTO que contiene todos los atributos del avatar.</param>
        /// <param name="imagen">El nuevo archivo de imagen del avatar generado.</param>
        /// <returns>Un resultado de la operación. 204 No Content si es exitoso.</returns>
        [HttpPut("{idPerfilEstudiante}/personalizar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActualizarAvatarDePerfil(
            int idPerfilEstudiante,
            [FromForm] ActualizarAvatarDto avatarDto,
            IFormFile imagen)
        {
            if (imagen == null || imagen.Length == 0)
            {
                var error = new { Codigo = "Error.Validation", Mensaje = "No se ha proporcionado una imagen válida." };
                return BadRequest(error);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await using var streamImagen = imagen.OpenReadStream();

            var resultado = await _modificarAvatar.EjecutarAsync(idPerfilEstudiante, userId, avatarDto, streamImagen);

            return resultado.EsExitoso ? NoContent() : this.ManejarFallo(resultado);
        }

        /// <summary>
        /// Obtiene todos los atributos de avatar disponibles para el id del perfil de estudiante recibido.
        /// </summary>
        /// <param name="idPerfilEstudiante">El ID del perfil del estudiante.</param>
        /// <returns>Un resultado de la operación.</returns>
        [HttpGet("{idPerfilEstudiante}/inventario-avatar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerAtributosAvatarQuePoseePerfilEstudiante(int idPerfilEstudiante)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var resultado = await _obtenerAtributosAvatar.EjecutarAsync(idPerfilEstudiante, userId);
            return resultado.EsExitoso ? Ok(resultado.Valor) : this.ManejarFallo(resultado);
        }

    }
}
