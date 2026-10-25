using LogicaAplicacion.DTOs.UmbralParaMedallaPorKudosDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.UmbralParaObtenerMedallaPorKudos;

public interface IObtenerUmbralesParaMedallasPorKudos
{
    Task<Resultado<IEnumerable<UmbralParaMedallaDto>>> EjecutarAsync(int grupoId);
}