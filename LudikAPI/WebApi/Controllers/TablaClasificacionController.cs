using LogicaAplicacion.DTOs.TablaClasificacionDTOs;
using System.Security.Claims;
using LogicaAplicacion.InterfacesCasosUsos.TablaClasificacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
using LogicaNegocio.Resultados;
using LogicaAplicacion.ImplementacionCasosUsos.TablaClasificacion;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TablaClasificacionController : ControllerBase
	{
		private readonly IAltaTablaClasificacion _altaTablaClasificacion;
		private readonly IObtenerTablaClasificacion _obtenerTablaClasificacion;
		private readonly IObtenerTodasLasTablasClasificacionGrupo _obtenerTodasLasTablasClasificacionGrupo;
		private readonly IBajaTablaClasificacion _bajaTablaClasificacion;
        public TablaClasificacionController(
            IAltaTablaClasificacion altaTablaClasificacion, 
            IObtenerTablaClasificacion obtenerTablaClasificacion, 
            IBajaTablaClasificacion bajaTablaClasificacion,
            IObtenerTodasLasTablasClasificacionGrupo obtenerTodasLasTablasClasificacionGrupo)
		{
			_altaTablaClasificacion = altaTablaClasificacion;
			_obtenerTablaClasificacion = obtenerTablaClasificacion;
			_bajaTablaClasificacion = bajaTablaClasificacion;
            _obtenerTodasLasTablasClasificacionGrupo = obtenerTodasLasTablasClasificacionGrupo;

        }
		/// <summary>
		/// Crea una nueva tabla de clasificación dentro de un grupo.
		/// </summary>
		/// <param name="grupoId">ID del grupo al que pertenece la tabla.</param>
		/// <param name="dto">Datos de la tabla a crear (nombre + medalla asociada).</param>
		/// <response code="201">Tabla creada con éxito.</response>
		/// <response code="400">Validaciones de negocio fallaron.</response>
		/// <response code="401">Usuario no autenticado.</response>
		/// <response code="403">El usuario no es profesor o no es dueño del grupo.</response>
		[Authorize(Policy = "EsProfesor")]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> CrearTablaClasificacion(
		[FromQuery] int grupoId,
		[FromBody] TablaClasificacionAltaDto dto)
		{
			var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(profesorId))
				return Unauthorized(new Error("Error.Unauthorized", "No se pudo identificar al profesor del token."));

			var resultado = await _altaTablaClasificacion.EjecutarAsync(profesorId, grupoId, dto);

			if (resultado.EsFallo)
				return this.ManejarFallo(resultado);

			return StatusCode(
				StatusCodes.Status201Created,
				new { Mensaje = "Tabla de clasificación creada exitosamente." }
			);
		}
		/// <summary>
		/// Obtiene la información de una tabla de clasificación (nombre y participantes ordenados) por su ID.
		/// </summary>
		/// <param name="tablaId">ID de la tabla de clasificación a consultar.</param>
		/// <response code="200">Devuelve el DTO con el nombre y la lista de participantes.</response>
		/// <response code="400">La consulta falló (p. ej. tabla no existe).</response>
		/// <response code="401">Usuario no autenticado.</response>
		/// <response code="403">El usuario no tiene rol de Profesor o no puede acceder a esta tabla.</response>
		[HttpGet("{tablaId}")]
		[Authorize(Policy = "EsProfesorOEstudiante")]
		[ProducesResponseType(typeof(TablaClasificacionInfoDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> ObtenerTablaClasificacion(int tablaId)
		{
			var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(profesorId))
				return Unauthorized(new Error("Error.Unauthorized", "No se pudo identificar al profesor del token."));

			var resultado = await _obtenerTablaClasificacion.EjecutarAsync(tablaId);

			if (resultado.EsFallo)
				return this.ManejarFallo(resultado);

			return Ok(resultado.Valor);
		}

        /// <summary>
        /// Obtiene todas las tablas de clasificación de un grupo concreto
        /// (cada una con sus participantes ordenados).
        /// </summary>
        [Authorize(Policy = "EsProfesorOEstudiante")]
        [HttpGet("grupo/{grupoId:int}")]
        [ProducesResponseType(typeof(IEnumerable<TablaClasificacionInfoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObtenerTodasLasTablasClasificacionGrupo([FromRoute] int grupoId)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized(new Error("Error.Unauthorized", "No estás autorizado para realizar esta acción."));

            var resultado = await _obtenerTodasLasTablasClasificacionGrupo
                .EjecutarAsync(grupoId, usuarioId);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);

            return Ok(resultado.Valor);
        }
        /// <summary>
        /// Elimina una tabla de clasificación por su ID.
        /// </summary>
        /// <param name="tablaId">ID de la tabla a eliminar.</param>
        /// <response code="204">Eliminación exitosa.</response>
        /// <response code="400">Falló la operación.</response>
        /// <response code="401">Usuario no autenticado.</response>
        /// <response code="403">Usuario no autorizado.</response>
        [HttpDelete("{tablaId}")]
		[Authorize(Policy = "EsProfesor")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> EliminarTablaClasificacion(int tablaId)
		{
			var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(profesorId))
				return Unauthorized(new Error("Error.Unauthorized", "No estás autorizado para realizar esta acción."));

			var resultado = await _bajaTablaClasificacion.EjecutarAsync(tablaId, profesorId);
			if (resultado.EsFallo)
				return this.ManejarFallo(resultado);

			return NoContent();
		}
	}
}
