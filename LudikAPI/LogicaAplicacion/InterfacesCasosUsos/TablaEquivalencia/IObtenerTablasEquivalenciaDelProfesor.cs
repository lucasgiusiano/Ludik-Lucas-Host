using LogicaAplicacion.DTOs.TablaEquivalenciaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.TablaEquivalencia;

public interface IObtenerTablasEquivalenciaDelProfesor
{
    public Task<Resultado<IEnumerable<TablaEquivalenciaDto>>> EjecutarAsync(string profesorId);
}