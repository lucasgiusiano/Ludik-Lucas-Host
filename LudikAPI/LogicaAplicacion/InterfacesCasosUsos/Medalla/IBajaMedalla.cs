using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Medalla;

public interface IBajaMedalla
{
    Task<Resultado> EjecutarAsync(int idMedalla, string profesorId);
}