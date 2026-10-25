using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.InterfacesRepositorios;

public interface IRepositorioAtributosAvatar: IRepositorio<AtributoAvatar>
{
    Task<Resultado<IEnumerable<AtributoAvatar>>> GetByIdsAsync(IEnumerable<int> ids);
}