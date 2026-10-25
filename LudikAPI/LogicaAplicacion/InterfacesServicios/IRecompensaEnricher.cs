using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.Servicios;

public interface IRecompensaEnricher
{
    Task<List<RecompensaClienteDto>> EnrichAsync(IEnumerable<Recompensa> recompensas);
}