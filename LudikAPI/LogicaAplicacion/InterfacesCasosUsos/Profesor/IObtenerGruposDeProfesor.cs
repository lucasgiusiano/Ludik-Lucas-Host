using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Profesor;

public interface IObtenerGruposDeProfesor
{
    Task<Resultado<List<GrupoDto>>> EjecutarAsync(string idProfesor);
}