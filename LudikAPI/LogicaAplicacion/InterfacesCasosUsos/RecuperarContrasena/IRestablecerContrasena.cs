using LogicaAplicacion.DTOs.RestablecerContrasenaDTO;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.RecuperarContrasena;

public interface IRestablecerContrasena
{
    Task<Resultado> EjecutarAsync(InformacionParaRestablecerContrasenaDto dto);
}