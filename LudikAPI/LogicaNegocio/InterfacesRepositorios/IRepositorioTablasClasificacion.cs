using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace InterfacesRepositorio
{
	public interface IRepositorioTablasClasificacion : IRepositorio<TablaClasificacion>
	{
        Task<Resultado<IEnumerable<TablaClasificacion>>> GetAllByAsync(int grupoId);

        Task<bool> ExisteTablaClasificacionConMedallaAsync(int medallaId);


    }

}

