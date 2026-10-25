using LogicaNegocio.Resultados;

namespace LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorio<T> where T : class
	{
        Task<Resultado> AddAsync(T unObjeto);
        Task<Resultado> RemoveAsync(int id);
        Task<Resultado> RemoveAsync(T unObjeto);
        Task<Resultado> UpdateAsync(T unObjeto);
        Task<Resultado<T>> GetByIdAsync(int id);
        Task<Resultado<IEnumerable<T>>> GetAllAsync();

    }

}

