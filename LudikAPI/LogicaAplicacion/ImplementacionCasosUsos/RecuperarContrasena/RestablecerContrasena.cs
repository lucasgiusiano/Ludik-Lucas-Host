using System.ComponentModel;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.RestablecerContrasenaDTO;
using LogicaAplicacion.InterfacesCasosUsos.RecuperarContrasena;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Identity;

namespace LogicaAplicacion.ImplementacionCasosUsos.RecuperarContrasena;

public class RestablecerContrasena: IRestablecerContrasena
{
    private readonly IRepositorioUsuarios _repositorioUsuarios;
    private readonly UserManager<Usuario> _userManager;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly IRepositorioPreguntasSeguridad _repositorioPreguntasSeguridad;

    public RestablecerContrasena(
        IRepositorioUsuarios repositorioUsuarios,
        UserManager<Usuario> userManager,
        IPasswordHasher<Usuario> passwordHasher,
        IRepositorioPreguntasSeguridad repositorioPreguntasSeguridad)
    {
        _repositorioUsuarios = repositorioUsuarios;
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _repositorioPreguntasSeguridad = repositorioPreguntasSeguridad;
    }

    public async Task<Resultado> EjecutarAsync(InformacionParaRestablecerContrasenaDto dto)
    {
        try
        {
            var resultadoUsuario = await _repositorioUsuarios.GetUsuarioPorNombreAsync(dto.NombreUsuario);
            if (resultadoUsuario.EsFallo)
                return Resultado.Falla(resultadoUsuario.Errores);
            

            var estudiante = resultadoUsuario.Valor as Estudiante;
            
            //Debo de cargarle al estudiante las preguntas de seguridad que ha respondido al momento de registrarse.
            await _repositorioPreguntasSeguridad.GetByNombreUsuarioAsync(dto.NombreUsuario);

            // Mapear dto a entidades para la verificación
            var respuestasIngresadas = dto.Respuestas.
                Select(r => new PreguntaRespuestaSeguridad { Id = r.PreguntaRespuestaSeguridadId ,PreguntaDeSeguridadId = r.PreguntaDeSeguridadId, Respuesta = r.Respuesta }).ToList();


            if (!estudiante.CoincidenLasRespuestas(respuestasIngresadas, _passwordHasher))
                return Resultado.Falla(Error.Unauthorized);
            

            var token = await _userManager.GeneratePasswordResetTokenAsync(estudiante);
            var resultadoIdentity = await _userManager.ResetPasswordAsync(estudiante, token, dto.NuevaContrasena);

            if (!resultadoIdentity.Succeeded)
            {
                var errores = resultadoIdentity.Errors.Select(e => new Error("Error.Validation", e.Description));
                return Resultado.Falla(errores);
            }

            return Resultado.Exitoso();
        }
        catch (UsuarioNoValidoException)
        {
            return Resultado.Falla(Error.NotFound);
        }
        catch (Exception ex)
        {
            return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
        }
    }

}