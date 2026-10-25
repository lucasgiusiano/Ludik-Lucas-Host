using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.InterfacesRepositorio;

namespace InterfacesRepositorio
{
	public interface IRepositorioSolicitudesUnion : IRepositorio<SolicitudUnion>
	{
        Task<bool> ExisteSolicitudPendiente(string idEstudiante, int idGrupo);
        Task<List<SolicitudUnion>> ObtenerSolicitudesPendientesPorGrupoAsync(int grupoId);
        Task<SolicitudUnion> GetSolicitudConEstudianteYGrupoPorIdAsync(int id);
    }

}

