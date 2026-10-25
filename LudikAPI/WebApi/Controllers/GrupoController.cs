using LogicaAplicacion.DTOs.ProfesorDTOs;
using System.ComponentModel.DataAnnotations;
using LogicaAplicacion.InterfacesCasosUsos.Grupo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaNegocio.Excepciones;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using LogicaNegocio.Resultados;
using System.Security.Claims;
using WebApi.Helpers;
using LogicaAplicacion.ImplementacionCasosUsos.Grupos;
using LogicaAplicacion.InterfacesCasosUsos.PerfilEstudiante;
using LogicaAplicacion.DTOs.PerfilEstudianteDTO;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize] // Solo autenticación, sin restringir aún por rol
	public class GrupoController : ControllerBase
	{
		private readonly IAltaGrupo _altaGrupo;
		private readonly IEditarGrupo _editarGrupo;
		private readonly IBajaGrupo _bajaGrupo;
		private readonly IObtenerInformacionGrupo _obtenerInformacionGrupo;
		private readonly IObtenerPerfilesPorGrupo _obtenerPerfilesPorGrupo;

		public GrupoController(
			IAltaGrupo altaGrupo,
			IEditarGrupo editarGrupo,
			IBajaGrupo bajaGrupo,
			IObtenerInformacionGrupo obtenerInformacionGrupo,
			IObtenerPerfilesPorGrupo obtenerPerfilesPorGrupo)
		{
			_altaGrupo = altaGrupo;
			_editarGrupo = editarGrupo;
			_bajaGrupo = bajaGrupo;
			_obtenerInformacionGrupo = obtenerInformacionGrupo;
			_obtenerPerfilesPorGrupo = obtenerPerfilesPorGrupo;
		}

		[HttpPost("alta")]
		[Authorize(Policy = "EsProfesor")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> AltaGrupo([FromBody] GrupoAltaRequestDto grupoRequest)
		{
			var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(profesorId))
				return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

			var resultado = await _altaGrupo.EjecutarAsync(grupoRequest, profesorId);

			if (resultado.EsFallo)
				return this.ManejarFallo(resultado);

			return StatusCode(StatusCodes.Status201Created, "El grupo ha sido creado correctamente.");
		}

		[HttpPut("editar/{id:int}")]
		[Authorize(Policy = "EsProfesor")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> EditarGrupo([FromBody] GrupoEditarDto grupoDto)
		{
			var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(profesorId))
				return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

			var resultado = await _editarGrupo.EjecutarAsync(grupoDto, profesorId);

			if (resultado.EsFallo)
				return this.ManejarFallo(resultado);

			return StatusCode(StatusCodes.Status200OK, "El grupo fue editado con éxito.");
		}

		[HttpDelete("eliminar/{id:int}")]
		[Authorize(Policy = "EsProfesor")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> EliminarGrupo(int id)
		{
			var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(profesorId))
				return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

			var resultado = await _bajaGrupo.EjecutarAsync(id, profesorId);

			if (resultado.EsFallo)
				return this.ManejarFallo(resultado);

			return StatusCode(StatusCodes.Status200OK, "El grupo se ha eliminado de forma exitosa.");
		}

		[HttpGet("info/{grupoId:int}")]
		[Authorize] // Estudiantes y profesores autenticados
		[ProducesResponseType(typeof(GrupoInformacionDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ObtenerInformacionGrupo(int grupoId)
		{
			try
			{
                var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(profesorId))
                    return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

                var resultado = await _obtenerInformacionGrupo.EjecutarAsync(grupoId,profesorId);

				if (resultado.EsFallo)
					return this.ManejarFallo(resultado);

				return Ok(resultado.Valor);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new
				{
					Mensaje = "Ocurrió un error inesperado al obtener la información del grupo. " + ex.Message
				});
			}
		}

		[HttpGet("{grupoId:int}/perfiles")]
		[Authorize] // Estudiantes y profesores autenticados
		[ProducesResponseType(typeof(List<PerfilEstudianteInformacionDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ObtenerPerfilesPorGrupo(int grupoId)
		{
			try
			{
                var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(profesorId))
                    return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));

                var resultado = await _obtenerPerfilesPorGrupo.EjecutarAsync(grupoId, profesorId);

				if (resultado.EsFallo)
					return this.ManejarFallo(resultado);

				return Ok(resultado.Valor);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new
				{
					Mensaje = "Ocurrió un error inesperado al obtener los perfiles del grupo. " + ex.Message
				});
			}
		}
	}
}
