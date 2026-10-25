using InterfacesRepositorio;
using System.Collections.Generic;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace InterfacesRepositorio
{
	public interface IRepositorioPerfilEstudianteGrupo : IRepositorio<PerfilEstudiante>
	{
        public List<Medalla> verMedallasAlumno(int idAlumno, int idGrupo);
        public Task<Resultado<List<PerfilEstudiante>>> ObtenerPorGrupoIdAsync(int grupoId);

        Task<Resultado<PerfilEstudiante>> GetPerfilEstudianteAsync(string estudianteId, int grupoId);
        public Task<Resultado<IEnumerable<RecompensaPersonalizacionAvatar>>> ObtenerItemsAvatarAdquiridosAsync(int idPerfilEstudiante);
        public Task<Resultado> SaveCambiosAsync();

        Task<Resultado<PerfilEstudiante>> GetParaAsignacionMedallaAsync(int id);

        Task<Resultado<IEnumerable<int>>> GetIdsPorEstudianteAsync(string estudianteId);

        Task<Resultado<List<PerfilEstudiante>>> GetPerfilesPorEstudianteAsync(string estudianteId);

        Task<Resultado<IEnumerable<Recompensa>>> ObtenerRecompensasInventarioAsync(int idPerfilEstudiante);

    }

}

