using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace InterfacesRepositorio
{
	public interface IRepositorioProfesores : IRepositorio<Profesor>
	{
        Task<Resultado<Profesor>> GetByStringIdAsync(string id);
        Task<Resultado<bool>> PerteneceGrupoAsync(string profesorId, int grupoId);
        Task<Resultado<bool>> PoseeMedallaAsync(string profesorId, int medallaId);
        Task<Resultado<Profesor>> ObtenerRecompensasPorProfesorIdAsync(string profesorId);

        Task<bool> EsRecompensaDeAsync(string profesorId, int recompensaId);

    }

}

