using InterfacesRepositorio;
using LogicaNegocio.Entidades;
using System.Collections.Generic;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace InterfacesRepositorio
{
	public interface IRepositorioGrupos : IRepositorio<Grupo>
	{

        Task<Resultado<IEnumerable<Grupo>>> obtenerGruposPorProfesorAsync(string idProfesor);
        Task<Resultado<TablaEquivalencia>> GetTablaEquivalenciaPorPerfilEstudianteAsync(int perfilEstudianteId);

        Task<Grupo> ObtenerPorEnlaceAsync(string codigoBase);

        Task<List<Grupo>> ObtenerGruposPorEstudianteId(string idEstudiante);
        Task<List<Grupo>> ObtenerGruposPorProfesorId(string idProfesor);

        Task<Resultado<List<Grupo>>> ObtenerGruposPorIdsYProfesor(List<int> gruposIds, string profesorId);
        Task<bool> EstudiantePerteneceAlGrupoAsync(int grupoId, string estudianteId);

        Task<Resultado<bool>> GrupoPerteneceProfesorAsync(int grupoId, string profesorId);

        Task<Resultado<IEnumerable<Medalla>>> GetMedallasDelProfesorPorGrupoAsync(int grupoId);
        Task<Resultado<Grupo>> GetGrupoConPerfilesYRecompensasAsync(string grupoId);

    }

}

