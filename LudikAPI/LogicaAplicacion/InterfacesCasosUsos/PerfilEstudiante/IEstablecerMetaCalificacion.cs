using LogicaAplicacion.DTOs.EstablecerMetaCalificacionDto;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.PerfilEstudiante;

public interface IEstablecerMetaCalificacion
{
    Task<Resultado> EjecutarAsync(EstablecerMetaCalificacionDto dto, string estudianteId);
}