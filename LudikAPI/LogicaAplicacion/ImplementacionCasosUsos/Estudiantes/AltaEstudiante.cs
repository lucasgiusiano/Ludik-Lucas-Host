using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaAplicacion.DTOsMappers.EstudianteMappers;
using LogicaAplicacion.InterfacesCasosUsos.Estudiante;
using LogicaNegocio.Excepciones;
using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObjects;
using Microsoft.AspNetCore.Identity;
using LogicaAplicacion.InterfacesCasosUsos.Login;

namespace LogicaAplicacion.ImplementacionCasosUsos.Estudiantes
{
    public class AltaEstudiante : IAltaEstudiante
    {
        private readonly UserManager<Usuario> _userManager; 
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public AltaEstudiante(UserManager<Usuario> userManager,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;

        }

        /// <summary>
        /// Crea un nuevo usuario de tipo Estudiante. No lanza excepciones por fallos de validación o negocio,
        /// en su lugar, retorna un objeto Resultado que encapsula el éxito o el fallo de la operación.
        /// </summary>
        /// <param name="estudianteAltaDto">DTO con los datos para el alta.</param>
        /// <returns>Un objeto Resultado que indica el éxito o contiene los errores de la operación.</returns>
        public async Task<Resultado> EjecutarAsync(EstudianteAltaDto estudianteAltaDto)
        {

            if (estudianteAltaDto == null)
                return Resultado.Falla(new Error("Error.Validation", "Los datos del estudiante no pueden ser nulos."));

            if (estudianteAltaDto.PreguntasDeSeguridad == null || estudianteAltaDto.PreguntasDeSeguridad.Count != 2)
                return Resultado.Falla(new Error("Error.Validation", "Se deben proporcionar exactamente 2 preguntas de seguridad."));

            if (estudianteAltaDto.PreguntasDeSeguridad[0].PreguntaId == estudianteAltaDto.PreguntasDeSeguridad[1].PreguntaId)
                return Resultado.Falla(new Error("Error.Validation", "Debe seleccionar dos preguntas de seguridad diferentes."));

            var resultadoNombre = NombreCompleto.Crear(estudianteAltaDto.Nombre, estudianteAltaDto.Apellido);
            if (resultadoNombre.EsFallo)
                return Resultado.Falla(new Error("Error.Validation", "El nombre o apellido no cumple el formato 3-20 caracteres"));

            var estudianteNuevo = EstudianteAltaMapper.fromDto(estudianteAltaDto, resultadoNombre.Valor);

            var existeNombre = await _userManager.FindByNameAsync(estudianteAltaDto.NombreUsuario);
            if (existeNombre != null)
                return Resultado.Falla(new Error("Error.Conflict", "El nombre de usuario ya está en uso."));
            
            estudianteNuevo.PreguntasSeguridad = new List<PreguntaRespuestaSeguridad>();
            foreach (var preguntaDto in estudianteAltaDto.PreguntasDeSeguridad)
            {
                var respuestaHasheada = _passwordHasher.HashPassword(estudianteNuevo, preguntaDto.Respuesta);

                var preguntaRespuestaEntidad = new PreguntaRespuestaSeguridad
                {
                    PreguntaDeSeguridadId = preguntaDto.PreguntaId,
                    Respuesta = respuestaHasheada
                };
                estudianteNuevo.PreguntasSeguridad.Add(preguntaRespuestaEntidad);
            }


            var resultadoCreacion = await _userManager.CreateAsync(estudianteNuevo, estudianteAltaDto.Contrasenia);
            if (!resultadoCreacion.Succeeded)
            {
                var errores = resultadoCreacion.Errors
                    .Select(e => new Error("Error.Validation", e.Description));
                return Resultado.Falla(errores);
            }

            var rolAsignado = await _userManager.AddToRoleAsync(estudianteNuevo, "Estudiante");
            if (!rolAsignado.Succeeded)
            {

                await _userManager.DeleteAsync(estudianteNuevo);

                return Resultado.Falla(new Error ("Error.Unexpected", "Ocurrió un error crítico al procesar el registro. No se pudo asignar el rol."));
            }

            return Resultado.Exitoso();
        }


    }
}
