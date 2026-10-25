using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Medalla;

public interface IObtenerTodasLasMedallas
{
    Task<Resultado<IEnumerable<MedallaDto>>> EjecutarAsync(string profesorId);
}