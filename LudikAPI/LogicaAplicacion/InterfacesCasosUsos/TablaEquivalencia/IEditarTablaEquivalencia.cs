using LogicaAplicacion.DTOs.TablaEquivalenciaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.TablaEquivalencia;

public interface IEditarTablaEquivalencia
{
    public Task<Resultado> EjecutarAsync(TablaEquivalenciaDto tablaDto, string profesorId);
}