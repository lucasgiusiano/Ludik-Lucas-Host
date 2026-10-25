using LogicaAplicacion.DTOs.UsuarioDTOs;
using System.ComponentModel.DataAnnotations;
using LogicaAplicacion.InterfacesCasosUsos.Profesor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LogicaAplicacion.DTOs.ProfesorDTOs;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LogicaAplicacion.DTOs.SolocitudUnionDTOs;
using LogicaAplicacion.InterfacesCasosUsos.SolicitudUnion;
using WebApi.Helpers;
using LogicaAplicacion.ImplementacionCasosUsos.SolicitudUnion;
using LogicaAplicacion.InterfacesCasosUsos.Login;
using Microsoft.AspNetCore.Razor.TagHelpers;
using LogicaAplicacion.InterfacesCasosUsos.SolicitudPerfilMedalla;
using LogicaAplicacion.DTOs.SolicitudPerfilMedallaDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.SolicitudPerfilMedalla;
using LogicaAplicacion.InterfacesCasosUsos.ProyectoAulaColaborativo;
using LogicaAplicacion.DTOs.ProyectoAulaColaborativoDTOs;
using LogicaAplicacion.DTOs.PerfilEstudianteDTOs;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorController : ControllerBase
    {
        private readonly IAltaProfesor _altaProfesor;
        private readonly IObtenerGruposDeProfesor _obtenerGruposDeProfesor;
        private readonly IObtenerSolicitudesUnionDelGrupo _obtenerSolicitudesUnionDelGrupo;
        private readonly IAceptarSolicitudUnion _aceptarSolicitudUnion;
        private readonly IRechazarSolicitudUnion _rechazarSolicitudUnion;
        private readonly ILoginUsuario _loginUsuario;
        private readonly IReinicioLogrosDeUnGrupo _reinicioLogrosDeUnGrupo;
        private readonly IReinicioLogrosDeTodosLosGrupos _reinicioLogrosDeTodosLosGrupos;
        private readonly IObtenerSolicitudPerfilMedalla _obtenerSolicitudPerfilMedalla;
        private readonly IAceptarSolicitudPerfilMedalla _aceptarSolicitudPerfilMedalla;
        private readonly IRechazarSolicitudPerfilMedalla _rechazarSolicitudPerfilMedalla;
        private readonly IAltaProyectoAulaColaborativo _altaPac;
        private readonly IObtenerProyectoAulaColaborativo _obtenerProyectoAulaColaborativo;
        private readonly IObtenerRecompensasReclamadasAlumnos _obtenerRecompensasReclamadasAlumnos;
        private readonly IConfirmarRecompensaReclamadaAlumno _confirmarRecompensaReclamadaAlumno;
        public ProfesorController(IAltaProfesor altaProfesor, 
            IObtenerGruposDeProfesor obtenerGruposDeProfesor,
            IObtenerSolicitudesUnionDelGrupo obtenerSolicitudesUnionDelGrupo,
            IAceptarSolicitudUnion aceptarSolicitudUnion,
            IRechazarSolicitudUnion rechazarSolicitudUnion,
            ILoginUsuario loginUsuario, IReinicioLogrosDeUnGrupo reinicioLogrosDeUnGrupo, IReinicioLogrosDeTodosLosGrupos reinicioLogrosDeTodosLosGrupos, IObtenerSolicitudPerfilMedalla obtenerSolicitudPerfilMedalla, IAceptarSolicitudPerfilMedalla aceptarSolicitudPerfilMedalla, IRechazarSolicitudPerfilMedalla rechazarSolicitudPerfilMedalla, IAltaProyectoAulaColaborativo altaProyectoAulaColaborativo, IObtenerProyectoAulaColaborativo obtenerProyectoAulaColaborativo, IObtenerRecompensasReclamadasAlumnos obtenerRecompensasReclamadasAlumnos, IConfirmarRecompensaReclamadaAlumno confirmarRecompensaReclamadaAlumno)
        {
            _altaProfesor = altaProfesor;
            _obtenerGruposDeProfesor = obtenerGruposDeProfesor;
            _obtenerSolicitudesUnionDelGrupo = obtenerSolicitudesUnionDelGrupo;
            _aceptarSolicitudUnion = aceptarSolicitudUnion;
            _rechazarSolicitudUnion = rechazarSolicitudUnion;
            _loginUsuario = loginUsuario;
            _reinicioLogrosDeUnGrupo = reinicioLogrosDeUnGrupo;
            _reinicioLogrosDeTodosLosGrupos = reinicioLogrosDeTodosLosGrupos;
            _obtenerSolicitudPerfilMedalla = obtenerSolicitudPerfilMedalla;
            _aceptarSolicitudPerfilMedalla = aceptarSolicitudPerfilMedalla;
            _rechazarSolicitudPerfilMedalla = rechazarSolicitudPerfilMedalla;
            _altaPac = altaProyectoAulaColaborativo;
            _obtenerProyectoAulaColaborativo = obtenerProyectoAulaColaborativo;
            _obtenerRecompensasReclamadasAlumnos = obtenerRecompensasReclamadasAlumnos;
            _confirmarRecompensaReclamadaAlumno = confirmarRecompensaReclamadaAlumno;
        }
        /// <summary>
        /// Este endpoint permite registrar un nuevo Profesor en el sistema.
        /// </summary>
        /// <returns>
        /// 201 Created: Si el estudiante fue registrado correctamente.
        /// 400 Bad Request: Si los datos enviados son inválidos o faltan.
        /// 500 Internal Server Error: Si ocurre un error inesperado durante el procesamiento.
        /// </returns>

        [HttpPost("alta")]
        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AltaProfesor([FromBody] ProfesorAltaDto profesorDto)
        {
            Resultado resultado = await _altaProfesor.EjecutarAsync(profesorDto);

         
            if (resultado.EsFallo)
               return this.ManejarFallo(resultado);
           
           var loginDto = new LoginSolicitudDto
           {
               NombreUsuario = profesorDto.NombreUsuario,
               Contrasenia = profesorDto.Contrasenia
           };
           
           var resultadoLogin = await _loginUsuario.EjecutarAsync(loginDto);
           
           return resultadoLogin.EsExitoso
               ? CreatedAtAction(nameof(AltaProfesor), resultadoLogin.Valor)
               : this.ManejarFallo(resultadoLogin);

        }
        /// <summary>
        /// Obtiene todos los grupos que un profesor posee.
        /// </summary>
        /// <returns>
        /// 200 OK: Retorna la lista de grupos del profesor.
        /// 400 Bad Request: Si el ID del profesor es inválido.
        /// 404 Not Found: Si el profesor no existe.
        /// 500 Internal Server Error: Si ocurre un error inesperado.
        /// </returns>

        [HttpGet("mis-grupos")]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(typeof(List<GrupoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerGruposProfesor()
        {
            var idProfesorAutenticado = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(idProfesorAutenticado))
                return this.ManejarFallo(Resultado.Falla(Error.Unauthorized));
            

            Resultado<List<GrupoDto>> resultado = await _obtenerGruposDeProfesor.EjecutarAsync(idProfesorAutenticado);

            if (resultado.EsFallo)
                return this.ManejarFallo(resultado);
            

            return Ok(resultado.Valor);
        }
        /// <summary>
        /// Obtiene las solicitudes pendientes de unión a un grupo del profesor autenticado.
        /// </summary>
        /// <param name="grupoId">ID del grupo</param>
        /// <returns>
        /// 200 OK: Lista de solicitudes pendientes.
        /// 400 Bad Request: Grupo inválido o error de validación.
        /// 404 Not Found: Grupo no encontrado.
        /// 401 Unauthorized: Usuario no autenticado.
        /// 500 Internal Server Error: Error inesperado.
        /// </returns>
        [HttpGet("solicitudes-union")]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(typeof(List<SolicitudUnionListadoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerSolicitudesUnion([FromQuery][Required] int grupoId)
        {
            try
            {
                var idProfesorAutenticado = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(idProfesorAutenticado))
                    return Unauthorized(new { Mensaje = "No se pudo identificar al usuario autenticado." });

                var resultado = await _obtenerSolicitudesUnionDelGrupo.EjecutarAsync(grupoId, idProfesorAutenticado);

                if (resultado.EsFallo)
                {
                    if (resultado.Errores.Any(e => e.Codigo == Error.NotFound.Codigo))
                        return NotFound(resultado.Errores.ToList());

                    return BadRequest(resultado.Errores.ToList());
                }

                return Ok(resultado.Valor);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Mensaje = "Ocurrió un error inesperado al obtener las solicitudes. " + ex.Message });
            }
        }
        /// <summary>
        /// Acepta una solicitud de unión de un estudiante a un grupo del profesor autenticado.
        /// </summary>
        /// <param name="solicitudId">ID de la solicitud de unión</param>
        /// <returns>
        /// 200 OK: Solicitud aceptada correctamente.
        /// 400 Bad Request: Datos inválidos o solicitud ya procesada.
        /// 404 Not Found: La solicitud no existe.
        /// 401 Unauthorized: Usuario no autenticado.
        /// 500 Internal Server Error: Error inesperado.
        /// </returns>
        [HttpPost("aceptar-solicitud")]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AceptarSolicitud([FromQuery][Required] int solicitudId)
        {
            try
            {

                var idProfesor = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(idProfesor))
                    return Unauthorized(new { Mensaje = "No se pudo identificar al usuario autenticado." });

                var resultado = await _aceptarSolicitudUnion.EjecutarAsync(solicitudId,idProfesor);

                if (resultado.EsFallo)
                {
                    if (resultado.Errores.Any(e => e.Codigo == "NotFound" || e.Mensaje.Contains("no existe")))
                        return NotFound(resultado.Errores.ToList());

                    return BadRequest(resultado.Errores.ToList());
                }

                return Ok(new { Mensaje = "La solicitud fue aceptada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Mensaje = "Ocurrió un error inesperado al aceptar la solicitud. " + ex.Message
                });
            }
        }
        /// <summary>
        /// Rechaza una solicitud de unión de un estudiante a un grupo del profesor.
        /// </summary>
        /// <param name="solicitudId">ID de la solicitud de unión</param>
        /// <returns>
        /// 200 OK: Solicitud rechazada correctamente.
        /// 400 Bad Request: Datos inválidos o solicitud ya procesada.
        /// 404 Not Found: La solicitud no existe.
        /// 401 Unauthorized: Usuario no autenticado.
        /// 500 Internal Server Error: Error inesperado.
        /// </returns>
        [HttpPost("rechazar-solicitud")]
        [Authorize(Roles = "Profesor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RechazarSolicitud([FromQuery][Required] int solicitudId)
        {
            try
            {
                var idProfesor = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(idProfesor))
                    return Unauthorized(new { Mensaje = "No se pudo identificar al usuario autenticado." });

                var resultado = await _rechazarSolicitudUnion.EjecutarAsync(solicitudId, idProfesor);

                if (resultado.EsFallo)
                {
                    if (resultado.Errores.Any(e => e.Codigo == "NotFound" || e.Mensaje.Contains("no existe")))
                        return NotFound(resultado.Errores.ToList());

                    return BadRequest(resultado.Errores.ToList());
                }

                return Ok(new { Mensaje = "La solicitud fue rechazada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Mensaje = "Ocurrió un error inesperado al rechazar la solicitud. " + ex.Message
                });
            }
        }
        /// <summary>
        /// Reinicia los logros (medallas) de los perfiles de estudiante de un grupo,
        /// registrando previamente el rendimiento del período.
        /// </summary>
        /// <param name="grupoId">ID del grupo</param>
        /// <returns>
        /// 200 OK: Logros reiniciados exitosamente.
        /// 400 Bad Request: Si hay errores de validación.
        /// 404 Not Found: Si el grupo no existe.
        /// 401 Unauthorized: Si no está autenticado.
        /// 500 Internal Server Error: Si ocurre un error inesperado.
        /// </returns>
        [HttpPost("reiniciar-logros")]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReiniciarLogros([FromQuery][Required] int grupoId)
        {
            try
            {
                var idProfesorAutenticado = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(idProfesorAutenticado))
                    return Unauthorized(new { Mensaje = "No se pudo identificar al usuario autenticado." });


                var resultado = await _reinicioLogrosDeUnGrupo.EjecutarAsync(grupoId,idProfesorAutenticado);

                if (resultado.EsFallo)
                {
                    if (resultado.Errores.Any(e => e.Codigo == Error.NotFound.Codigo))
                        return NotFound(resultado.Errores.ToList());

                    return BadRequest(resultado.Errores.ToList());
                }

                return Ok(new { Mensaje = "Los logros del grupo fueron reiniciados correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Mensaje = "Ocurrió un error inesperado al reiniciar los logros. " + ex.Message
                });
            }
        }
        /// <summary>
        /// Reinicia los logros de **todos** los grupos del profesor autenticado,
        /// registrando previamente el historial en RendimientoPeriodo.
        /// </summary>
        /// <returns>
        /// 200 OK: Logros reiniciados en todos los grupos.
        /// 400 Bad Request: Errores de validación.
        /// 401 Unauthorized: Usuario no autenticado.
        /// 403 Forbidden: Si el usuario no es profesor.
        /// 500 Internal Server Error: Error inesperado.
        /// </returns>
        [HttpPost("reiniciar-todos-logros")]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReiniciarTodosLogros()
        {
            try
            {
                // 1) Obtengo el ID del profesor desde el token
                var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(profesorId))
                    return Unauthorized(new { Mensaje = "No se pudo identificar al profesor autenticado." });

                // 2) Ejecuto el caso de uso
                var resultado = await _reinicioLogrosDeTodosLosGrupos.EjecutarAsync(profesorId);

                // 3) Manejo errores
                if (resultado.EsFallo)
                {
                    // En este diseño no habrá NotFound individual, así que devolvemos BadRequest
                    return BadRequest(resultado.Errores.ToList());
                }

                // 4) Todo OK
                return Ok(new { Mensaje = "Los logros de todos los grupos fueron reiniciados correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Mensaje = "Error inesperado al reiniciar todos los logros. " + ex.Message });
            }
        }
        /// <summary>
        /// Obtiene las solicitudes de medalla para un grupo del profesor autenticado.
        /// </summary>
        /// <param name="grupoId">ID del grupo</param>
        /// <returns>
        /// 200 OK: Lista de solicitudes de medalla del grupo.
        /// 400 Bad Request: Grupo inválido o error de validación.
        /// 404 Not Found: Grupo no encontrado o sin solicitudes.
        /// 401 Unauthorized: Usuario no autenticado.
        /// 500 Internal Server Error: Error inesperado.
        /// </returns>
        [HttpGet("solicitudes-medallas")]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(typeof(List<SolicitudPerfilMedallaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerSolicitudesPerfilMedalla([FromQuery][Required] int grupoId)
        {
            try
            {
                var idProfesor = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(idProfesor))
                    return Unauthorized(new { Mensaje = "No se pudo identificar al usuario autenticado." });

                var resultado = await _obtenerSolicitudPerfilMedalla.EjecutarAsync(grupoId, idProfesor);

                if (resultado.EsFallo)
                {
                    if (resultado.Errores.Any(e => e.Codigo == Error.NotFound.Codigo))
                        return NotFound(resultado.Errores.ToList());

                    return BadRequest(resultado.Errores.ToList());
                }

                return Ok(resultado.Valor);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Mensaje = "Ocurrió un error inesperado al obtener las solicitudes de medalla. " + ex.Message });
            }
        }
        /// <summary>
        /// Acepta una solicitud de medalla enviada por un perfil de estudiante.
        /// </summary>
        /// <param name="solicitudId">ID de la solicitud de perfil medalla</param>
        /// <returns>
        /// 200 OK: Si la solicitud fue aceptada y la medalla asignada exitosamente.
        /// 400 Bad Request: Si la solicitud ya fue procesada o es inválida.
        /// 404 Not Found: Si la solicitud no existe.
        /// 401 Unauthorized: Si no está autenticado.
        /// 500 Internal Server Error: Si ocurre un error inesperado.
        /// </returns>
        [HttpPost("aceptar-solicitud-medalla")]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AceptarSolicitudMedalla([FromQuery][Required] int solicitudId)
        {
            try
            {
                var idProfesor = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(idProfesor))
                    return Unauthorized(new { Mensaje = "No se pudo identificar al profesor autenticado." });

                var resultado = await _aceptarSolicitudPerfilMedalla.EjecutarAsync(solicitudId, idProfesor);

                if (resultado.EsFallo)
                {
                    if (resultado.Errores.Any(e => e.Codigo == "Error.NotFound"))
                        return NotFound(resultado.Errores.ToList());

                    return BadRequest(resultado.Errores.ToList());
                }

                return Ok(new { Mensaje = "La solicitud fue aceptada y la medalla asignada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Mensaje = "Error inesperado al aceptar la solicitud de medalla. " + ex.Message });
            }
        }
        /// <summary>
        /// Rechaza una solicitud de medalla enviada por un perfil de estudiante.
        /// </summary>
        /// <param name="solicitudId">ID de la solicitud de perfil medalla</param>
        /// <returns>
        /// 200 OK: Si la solicitud fue rechazada correctamente.
        /// 400 Bad Request: Si la solicitud ya fue procesada o es inválida.
        /// 404 Not Found: Si la solicitud no existe.
        /// 401 Unauthorized: Si no está autenticado.
        /// 500 Internal Server Error: Si ocurre un error inesperado.
        /// </returns>
        [HttpPost("rechazar-solicitud-medalla")]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RechazarSolicitudMedalla([FromQuery][Required] int solicitudId)
        {
            try
            {
                var idProfesor = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(idProfesor))
                    return Unauthorized(new { Mensaje = "No se pudo identificar al profesor autenticado." });

                var resultado = await _rechazarSolicitudPerfilMedalla.EjecutarAsync(solicitudId,idProfesor);

                if (resultado.EsFallo)
                {
                    if (resultado.Errores.Any(e => e.Codigo == "Error.NotFound"))
                        return NotFound(resultado.Errores.ToList());

                    return BadRequest(resultado.Errores.ToList());
                }

                return Ok(new { Mensaje = "La solicitud fue rechazada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Mensaje = "Error inesperado al rechazar la solicitud de medalla. " + ex.Message });
            }
        }
        /// <summary>
        /// Crea un Proyecto de Aula Colaborativo (PAC) para un grupo.
        /// </summary>
        /// <param name="grupoId">ID del grupo</param>
        /// <param name="dto">Datos del PAC (nombre, meta, recompensa)</param>
        /// <returns>
        /// 200 OK: PAC creado exitosamente.
        /// 400 Bad Request: Datos inválidos o ya existe un PAC activo.
        /// 401 Unauthorized: No autenticado.
        /// 403 Forbidden: No tiene rol de profesor.
        /// 404 Not Found: Grupo no encontrado.
        /// 500 Internal Server Error: Error inesperado.
        /// </returns>
        [HttpPost("pac")]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AltaProyectoColaborativo([FromQuery][Required] int grupoId,[FromBody] AltaProyectoAulaColaborativoDto dto)
        {
            try
            {
                var profesorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(profesorId))
                    return Unauthorized(new { Mensaje = "No se pudo identificar al usuario autenticado." });

                var resultado = await _altaPac.EjecutarAsync(grupoId, dto,profesorId);
                if (resultado.EsFallo)
                {
                    if (resultado.Errores.Any(e => e.Codigo == Error.NotFound.Codigo))
                        return NotFound(resultado.Errores.ToList());
                    return BadRequest(resultado.Errores.ToList());
                }

                return Ok(new { Mensaje = "Proyecto colaborativo creado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Mensaje = "Ocurrió un error inesperado al crear el PAC. " + ex.Message });
            }
        }

        [HttpGet("pac")]
        [Authorize(Policy = "EsProfesorOEstudiante")]
        [ProducesResponseType(typeof(IEnumerable<ProyectoAulaColaborativoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerProyectoAulaColaborativo([FromQuery][Required] int grupoId)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized(new { Mensaje = "Debes de autenticarte para acceder a esta función." });

            var resultado = await _obtenerProyectoAulaColaborativo.EjecutarAsync(grupoId);
            if (resultado.EsFallo)
            {
                if (resultado.Errores.Any(e => e.Codigo == Error.NotFound.Codigo))
                    return NotFound(resultado.Errores.ToList());
                return BadRequest(resultado.Errores.ToList());
            }

            return Ok(resultado.Valor);
        }
        /// <summary>
        /// Obtiene las recompensas reclamadas por los estudiantes de un grupo,
        /// filtradas para incluir solo aquellas creadas por el profesor.
        /// </summary>
        /// <param name="grupoId">ID del grupo</param>
        /// <returns>
        /// 200 OK: Lista de recompensas reclamadas (filtradas) con el nombre del estudiante.
        /// 400 Bad Request: Si el ID del grupo es inválido o hay errores de validación.
        /// 401 Unauthorized: Si el usuario no está autenticado como profesor.
        /// 404 Not Found: Si el grupo o el profesor no existen.
        /// 500 Internal Server Error: Si ocurre un error inesperado.
        /// </returns>
        [HttpGet("recompensas-reclamadas")]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(typeof(List<PerfilEstudianteRecompensaProfesorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerRecompensasReclamadasAlumnos([FromQuery][Required] string grupoId)
        {
            try
            {
                // La validación del profesor autenticado y la autorización (EsProfesor)
                // ya están cubiertas por el [Authorize] y se manejan internamente en el Caso de Uso.
                // Sin embargo, por consistencia con otros endpoints, podemos verificar el ID.
                var idProfesorAutenticado = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(idProfesorAutenticado))
                    return Unauthorized(new { Mensaje = "No se pudo identificar al profesor autenticado." });


                Resultado<List<PerfilEstudianteRecompensaProfesorDto>> resultado =
                    await _obtenerRecompensasReclamadasAlumnos.EjecutarAsync(grupoId);

                if (resultado.EsFallo)
                {
                    // Usa el Helper `ManejarFallo` si lo tienes o usa la lógica de otros endpoints
                    if (resultado.Errores.Any(e => e.Codigo == Error.NotFound.Codigo))
                        return NotFound(resultado.Errores.ToList());

                    return BadRequest(resultado.Errores.ToList());
                }

                // Devolver la lista. Si la lista está vacía, devuelve 200 OK con un array vacío.
                return Ok(resultado.Valor);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Mensaje = "Ocurrió un error inesperado al obtener las recompensas reclamadas. " + ex.Message
                });
            }
        }
        /// <summary>
        /// Confirma la reclamación de una recompensa por parte de un estudiante, 
        /// eliminando la entrada de su inventario. Solo el profesor del grupo puede ejecutarlo.
        /// </summary>
        /// <param name="perfilEstudianteId">ID del PerfilEstudiante que reclamó la recompensa (string, pero representa int).</param>
        /// <param name="recompensaId">ID de la Recompensa que se reclamó (string, pero representa int).</param>
        /// <returns>
        /// 200 OK: Recompensa confirmada y eliminada del inventario.
        /// 400 Bad Request: IDs inválidos, o error de validación/lógica de negocio.
        /// 401 Unauthorized: Si el usuario no está autenticado como profesor.
        /// 403 Forbidden: Si el profesor no es el propietario del grupo.
        /// 404 Not Found: Si la relación PerfilEstudianteRecompensa no existe (no está en el inventario).
        /// 500 Internal Server Error: Si ocurre un error inesperado.
        /// </returns>
        [HttpPost("confirmar-recompensa")]
        [Authorize(Policy = "EsProfesor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmarRecompensaReclamada([FromQuery][Required] string perfilEstudianteId,[FromQuery][Required] string recompensaId)
        {
            try
            {
                var idProfesorAutenticado = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(idProfesorAutenticado))
                    return Unauthorized(new { Mensaje = "No se pudo identificar al profesor autenticado." });

                Resultado resultado = await _confirmarRecompensaReclamadaAlumno.EjecutarAsync(perfilEstudianteId, recompensaId);

                if (resultado.EsFallo)
                {
                    if (resultado.Errores.Any(e => e.Codigo == Error.NotFound.Codigo))
                        return NotFound(resultado.Errores.ToList());

                    return BadRequest(resultado.Errores.ToList());
                }

                return Ok(new { Mensaje = "Recompensa confirmada y eliminada del inventario del estudiante." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Mensaje = "Ocurrió un error inesperado al confirmar la recompensa reclamada. " + ex.Message
                });
            }
        }
    }
}
