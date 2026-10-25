using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using LogicaAplicacion.DTOs.EstablecerMetaCalificacionDto;
using LogicaAplicacion.InterfacesCasosUsos.PerfilEstudiante;
using LogicaNegocio.Resultados;
using LogicaAplicacion.DTOs.PerfilEstudianteDTO;
using WebApi.Helpers;
using LogicaAplicacion.DTOs.UsuarioDTOs;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EsEstudiante")]
    public class PerfilEstudianteController : ControllerBase
    {
        private readonly IObtenerPerfilConMedallas _uc;
        private readonly IEstablecerMetaCalificacion _establecerMetaCalificacion;
        private readonly IObtenerPerfilesPorGrupoSinLogueado _ucSinLogueado;
        public PerfilEstudianteController(IObtenerPerfilConMedallas uc, IEstablecerMetaCalificacion establecerMetaCalificacion, IObtenerPerfilesPorGrupoSinLogueado ucSinLogueado)
        {
            _uc = uc;
            _establecerMetaCalificacion = establecerMetaCalificacion;
            _ucSinLogueado = ucSinLogueado;
        }

        /// <summary>
        /// Obtiene el perfil del estudiante autenticado en el grupo indicado, incluyendo lista de medallas agrupadas.
        /// </summary>
        /// <param name="grupoId">ID del grupo al que pertenece el perfil.</param>
        /// <returns>
        /// 200 OK con PerfilConMedallasDto si existe y pertenece;
        /// 401 Unauthorized si no hay estudiante autenticado;
        /// 404 NotFound si no se encuentra el perfil en el grupo;
        /// 400 BadRequest para otros errores de validación o negocio.
        /// </returns>
        [HttpGet("mi-perfil/grupo/{grupoId}/medallas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PerfilConMedallasDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMiPerfilConMedallas(int grupoId)
        {
            
            var estudianteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(estudianteId))
                return this.ManejarFallo(Resultado.Falla(Error.Unauthorized)); 

            var resultado = await _uc.EjecutarAsync(estudianteId, grupoId);

            if (!resultado.EsExitoso)
            {
                return this.ManejarFallo(resultado);
            }

            return Ok(resultado.Valor);
        }
        /// <summary>
        /// Establece la meta de calificacion para el perfil de estudiante.
        /// </summary>
        /// <param name="id">El ID de la tabla de equivalencia a modificar.</param>
        /// <param name="dto">Objeto con los datos necesarios para establecer la meta de calificacion.</param>
        /// <response code="200">**OK.** Se establecio la meta de calificacion exitosamente.</response>
        /// <response code="400">**Solicitud Incorrecta.** Los datos proporcionados son inválidos.</response>
        /// <response code="403">**Prohibido.** El usuario no posee el perfil del estudiante al cual desea establecerle la meta de calificacion.</response>
        /// <response code="404">**No Encontrado.** No se encontró un perfil de estudiante o grupo con los IDs brindados.</response>
        [HttpPost("definir-meta-califiacion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DefinirMetaCalificacion([FromBody] EstablecerMetaCalificacionDto dto)
        {
            if (dto == null)
                return BadRequest(new Error("Error.Validation", "El DTO de meta de calificación no puede ser nulo."));
            var estudianteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resultado = await _establecerMetaCalificacion.EjecutarAsync(dto, estudianteId);
            return resultado.EsExitoso
                ? Ok(new { message = "Meta de calificación establecida correctamente." })
                : this.ManejarFallo(resultado);
        }
        /// <summary>
        /// Obtiene los perfiles de los compañeros de grupo, excluyendo el propio del estudiante logueado.
        /// </summary>
        [HttpGet("grupo/{grupoId}/companeros")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PerfilEstudianteInformacionDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCompanerosPorGrupo(int grupoId)
        {
            var estudianteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(estudianteId))
                return Unauthorized(new Error("Error.Unauthorized", "No se pudo identificar al estudiante."));

            var resultado = await _ucSinLogueado.EjecutarAsync(grupoId, estudianteId);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return Ok(resultado.Valor);
        }

    }
}
