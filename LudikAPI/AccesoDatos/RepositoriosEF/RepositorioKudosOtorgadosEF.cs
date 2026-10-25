using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace AccesoDatos.RepositoriosEF;

public class RepositorioKudosOtorgadosEF: IRepositorioKudosOtorgados
{
    public Task<Resultado> AddAsync(KudoOtorgado unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(KudoOtorgado unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> UpdateAsync(KudoOtorgado unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado<KudoOtorgado>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado<IEnumerable<KudoOtorgado>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}