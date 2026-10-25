using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.AsignacionMedalla;

public interface IQuitarMedalla
{
    public Task<Resultado> EjecutarAsync(string idProfesor, int idPerfilEstudiante, int idMedalla);
}