using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Estudiante;

public interface IObtenerGruposDeEstudiante
{
    Task<Resultado<List<GrupoDto>>> EjecutarAsync(string idEstudiante);

}