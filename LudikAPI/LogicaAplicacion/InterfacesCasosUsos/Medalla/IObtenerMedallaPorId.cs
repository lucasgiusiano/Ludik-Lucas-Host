using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Medalla;

public interface IObtenerMedallaPorId
{
    Task<Resultado<MedallaDto>> EjecutarAsync(int idMedalla,string profesorId);
}