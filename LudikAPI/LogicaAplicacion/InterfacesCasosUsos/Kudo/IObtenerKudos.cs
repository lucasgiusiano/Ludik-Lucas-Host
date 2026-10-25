using LogicaAplicacion.DTOs.KudoDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Kudo;

public interface IObtenerKudos
{
    Task<Resultado<IEnumerable<TipoKudoDto>>> EjecutarAsync();
}