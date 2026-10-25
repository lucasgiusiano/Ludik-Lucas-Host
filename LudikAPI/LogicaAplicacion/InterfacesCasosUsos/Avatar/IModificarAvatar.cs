using LogicaAplicacion.DTOs.AvatarDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Avatar;

public interface IModificarAvatar
{
    public Task<Resultado> EjecutarAsync(int idPerfilEstudiante, string idUsuario, ActualizarAvatarDto actualizarAvatarDto, Stream streamImagen);
}