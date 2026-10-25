using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaAplicacion.InterfacesCasosUsos.Jwt;
using LogicaAplicacion.InterfacesCasosUsos.Login;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Identity;

namespace LogicaAplicacion.ImplementacionCasosUsos.Login;

public class LoginUsuario: ILoginUsuario
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IManejadorJwt _manejadorJwt;

    public LoginUsuario(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IManejadorJwt manejadorJwt)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _manejadorJwt = manejadorJwt;
    }

    public async Task<Resultado<LoginRespuestaDto>> EjecutarAsync(LoginSolicitudDto loginDto)
    {
        var result = await _signInManager.PasswordSignInAsync(
            loginDto.NombreUsuario,
            loginDto.Contrasenia,
            isPersistent: false,
            lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            return Resultado<LoginRespuestaDto>.Falla(new Error("Error.Validation", "Cuenta bloqueada. Intente más tarde."));
        }

        if (!result.Succeeded)
        {
            return Resultado<LoginRespuestaDto>.Falla(new Error("Error.Validation", "Nombre de usuario o contraseña incorrectos."));
        }

        var usuario = await _userManager.FindByNameAsync(loginDto.NombreUsuario);
        if (usuario == null)
        {
            return Resultado<LoginRespuestaDto>.Falla(new Error("Error.NotFound", "No se pudieron obtener los detalles del usuario después del login."));
        }

        var roles = await _userManager.GetRolesAsync(usuario);
        string rolUnico = roles.FirstOrDefault();

        string token = _manejadorJwt.GenerarToken(usuario.Id, usuario.UserName, rolUnico);

        var respuesta = new LoginRespuestaDto
        {
            Token = token,
            Rol = rolUnico,
            NombreUsuario = usuario.UserName,
            Id = usuario.Id
        };

        return Resultado<LoginRespuestaDto>.Exitoso(respuesta);
    }
}