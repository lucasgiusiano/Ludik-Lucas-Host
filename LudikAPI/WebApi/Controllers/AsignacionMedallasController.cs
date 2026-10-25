using LogicaAplicacion.InterfacesCasosUsos.AsignacionMedalla;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LogicaAplicacion.DTOs.MedallaDTOs;
using WebApi.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EsProfesor")]
    public class AsignacionMedallasController : ControllerBase
    {
        private readonly IAsignarMedalla _asignarMedalla;
        private readonly IQuitarMedalla _quitarMedalla;

        public AsignacionMedallasController(IAsignarMedalla asignarMedalla, IQuitarMedalla quitarMedalla)
        {
            _asignarMedalla = asignarMedalla;
            _quitarMedalla = quitarMedalla;
        }

        /// <summary>
        /// Asigna una medalla existente a un perfil de estudiante.
        /// </summary>
        /// <param name="idPerfilEstudiante">El ID del perfil del estudiante que recibirá la medalla.</param>
        /// <param name="idMedalla">El ID de la medalla a asignar.</param>
        /// <returns>Un resultado de la operación.</returns>
        [HttpPost("perfil-estudiante/{idPerfilEstudiante}/medalla/{idMedalla}/cantidad/{cantidadMedallasOtorgar}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AsignarMedalla([FromRoute] int idPerfilEstudiante, [FromRoute]  int idMedalla, [FromRoute] int cantidadMedallasOtorgar)
        {

            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(profesorId))
                return Unauthorized();
            
            var resultado = await _asignarMedalla.EjecutarAsync(profesorId, idPerfilEstudiante, idMedalla, cantidadMedallasOtorgar);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);
            

            return Ok(new { Mensaje = "Medalla asignada exitosamente." });
        }
        /// <summary>
        /// Quita una medalla a un perfil de estudiante.
        /// </summary>
        /// <param name="idPerfilEstudiante">El ID del perfil del estudiante al cual se le quitara la medalla.</param>
        /// <param name="medallaDto">DTO que contiene el ID de la medalla a quitar.</param>
        /// <returns>Un resultado de la operación.</returns>
        [HttpDelete("perfil-estudiante/{idPerfilEstudiante}/medalla/{idMedalla}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> QuitarMedalla([FromRoute] int idPerfilEstudiante, [FromRoute] int idMedalla)
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(profesorId))
                return Unauthorized();

            var resultado = await _quitarMedalla.EjecutarAsync(profesorId, idPerfilEstudiante, idMedalla);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return NoContent();
        }
    }
}
