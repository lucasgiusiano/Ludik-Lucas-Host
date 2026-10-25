using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Login;

public interface ILoginUsuario
{
    Task<Resultado<LoginRespuestaDto>> EjecutarAsync(LoginSolicitudDto loginDto);
}