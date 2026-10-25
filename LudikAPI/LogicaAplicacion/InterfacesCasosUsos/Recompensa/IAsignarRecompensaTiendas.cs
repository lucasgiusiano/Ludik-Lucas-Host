using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Recompensa;

public interface IAsignarRecompensaTiendas
{
    Task<Resultado> EjecutarAsync(RecompensaYGruposDto recompensaYGrupos, string profesorId);
}