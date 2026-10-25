using LogicaAplicacion.DTOs.AtributoAvatarDTOs;
using LogicaAplicacion.DTOs.AvatarDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Avatar;

public interface IObtenerAtributosAvatarDisponiblesParaPerfil
{
    public Task<Resultado<IEnumerable<AtributoAvatarDto>>> EjecutarAsync(int idPerfilEstudiante, string idUsuario);
}