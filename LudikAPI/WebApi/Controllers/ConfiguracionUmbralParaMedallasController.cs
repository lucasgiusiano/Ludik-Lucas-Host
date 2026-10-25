using LogicaAplicacion.DTOs.UmbralParaMedallaPorKudosDTOs;
using LogicaAplicacion.InterfacesCasosUsos.UmbralParaObtenerMedallaPorKudos;
using LogicaNegocio.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Helpers;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracionUmbralParaMedallasController : ControllerBase
    {
        private readonly IAltaUmbralParaMedallaPorKudos _altaUmbral;
        private readonly IObtenerUmbralesParaMedallasPorKudos _obtenerUmbrales;
        private readonly IActualizarUmbralParaMedallaPorKudos _actualizarUmbral;
        private readonly IEliminarUmbralParaMedallaPorKudos _eliminarUmbral;

        public ConfiguracionUmbralParaMedallasController(IAltaUmbralParaMedallaPorKudos altaUmbral, 
            IObtenerUmbralesParaMedallasPorKudos obtenerUmbrales, 
            IActualizarUmbralParaMedallaPorKudos actualizarUmbral, 
            IEliminarUmbralParaMedallaPorKudos eliminarUmbral)
        {
            _altaUmbral = altaUmbral;
            _obtenerUmbrales = obtenerUmbrales;
            _actualizarUmbral = actualizarUmbral;
            _eliminarUmbral = eliminarUmbral;
        }

        /// <summary>
        /// Crea una nueva configuración de umbral para obtener una medalla por kudos.
        /// </summary>
        /// <param name="dto">Datos del nuevo umbral.</param>
        [HttpPost]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(typeof(UmbralParaMedallaPorKudos), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearUmbral([FromBody] AltaUmbralParaMedallaPorKudosDto dto)
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(profesorId))
                return Unauthorized(); 
            

            var resultado = await _altaUmbral.EjecutarAsync(profesorId, dto);

             return resultado.EsExitoso ? Created() : this.ManejarFallo(resultado);
        }
        /// <summary>
        /// Obtiene la lista de configuraciones de umbrales para un grupo específico.
        /// </summary>
        /// <param name="grupoId">El ID del grupo a consultar.</param>
        /// <returns>Una lista de configuraciones de umbral con detalles.</returns>
        [HttpGet]
        [Authorize(Policy = "EsProfesorOEstudiante")]
        [ProducesResponseType(typeof(IEnumerable<UmbralParaMedallaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerUmbralesParaMedallasDeUnGrupo([FromQuery] int grupoId)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized();
            var resultado = await _obtenerUmbrales.EjecutarAsync(grupoId);
            return resultado.EsExitoso ? Ok(resultado.Valor) 
                : this.ManejarFallo(resultado);
        }


        /// <summary>
        /// Actualiza la cantidad de kudos de una configuración de umbral existente.
        /// </summary>
        /// <param name="id">El ID del umbral a actualizar.</param>
        /// <param name="dto">Los datos para la actualización.</param>
        [HttpPut]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActualizarUmbralParaMedalla([FromBody] UmbralParaMedallaDto dto)
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(profesorId))
                Unauthorized();
            var resultado = await _actualizarUmbral.EjecutarAsync( dto, profesorId);
            return resultado.EsExitoso ? NoContent() : this.ManejarFallo(resultado);
        }

        /// <summary>
        /// Elimina una configuración de umbral para una medalla por su ID.
        /// </summary>
        /// <param name="id">El ID del umbral a eliminar.</param>
        [HttpDelete("{id}")]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarUmbralParaMedalla(int id)
        {
            var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(profesorId))
                return Unauthorized();
            
            var resultado = await _eliminarUmbral.EjecutarAsync(id, profesorId);

            return resultado.EsExitoso? NoContent(): this.ManejarFallo(resultado);
        }
    }
}
