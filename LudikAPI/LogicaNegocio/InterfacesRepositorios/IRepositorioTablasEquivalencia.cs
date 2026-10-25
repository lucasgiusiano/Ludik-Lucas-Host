using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace InterfacesRepositorio
{
	public interface IRepositorioTablasEquivalencia : IRepositorio<TablaEquivalencia>
	{
        Task<Resultado<IEnumerable<TablaEquivalencia>>> GetByProfesorIdAsync(string profesorId);
        Task<bool> ExisteTablaEquivalenciaConMedallaAsync(int medallaId);
    }

}

