using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LogicaAplicacion.DTOs.ImagenDto;
using LogicaAplicacion.DTOs.ImagenPerfilDtos;
using LogicaAplicacion.InterfacesCasosUsos.Imagenes;
using Microsoft.AspNetCore.Authorization;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EsProfesorOEstudiante")]
    public class ImagenController : ControllerBase
    {
        private readonly IServicioGestionImagen _servicioGestionImagen;

        public ImagenController(IServicioGestionImagen servicioGestionImagen)
        {
            _servicioGestionImagen = servicioGestionImagen;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SubirImagen([FromQuery] string proposito, IFormFile imagen, [FromQuery] int? entidadId)
        {
            if (imagen == null || imagen.Length == 0)
            {
                return BadRequest(new { Codigo = "Error.Validation", Mensaje = "No se ha proporcionado una imagen válida." });
            }
            if (string.IsNullOrWhiteSpace(proposito))
            {
                return BadRequest(new { Codigo = "Error.Validation", Mensaje = "El parámetro 'proposito' es requerido." });
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await using var streamImagen = imagen.OpenReadStream();

            var request = new SubirImagenDto { ImagenStream = streamImagen, IdUsuarioAutenticado=userIdString, Proposito=proposito, EntidadAsociadaId= entidadId };
            var resultado = await _servicioGestionImagen.SubirImagenAsync(request);

            return resultado.EsExitoso
                ? Ok()
                : this.ManejarFallo(resultado);
        }

        [HttpGet("{idPerfilEstudiante}")]
        [ProducesResponseType(typeof(ImagenPerfilDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerUrlImagen([FromRoute] int idPerfilEstudiante)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var resultadoDto = await _servicioGestionImagen.ObtenerUrlImagenPerfilAsync(idPerfilEstudiante, userIdString);

            return resultadoDto.EsExitoso
                ? Ok(resultadoDto.Valor)
                : this.ManejarFallo(resultadoDto);
        }
    }
}
