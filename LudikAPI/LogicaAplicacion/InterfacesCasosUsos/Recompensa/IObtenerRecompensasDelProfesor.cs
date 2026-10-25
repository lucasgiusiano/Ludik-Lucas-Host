using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Recompensa;

public interface IObtenerRecompensasDelProfesor
{
    Task<Resultado<IEnumerable<RecompensaDto>>> EjecutarAsync(string profesorId);
}