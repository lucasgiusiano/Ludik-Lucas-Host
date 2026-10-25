using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.AsignacionMedalla;

public interface IAsignarMedalla
{
    public Task<Resultado> EjecutarAsync(string profesorId, int idPerfilEstudiante, int idMedalla, int cantidadAOtorgar);
}