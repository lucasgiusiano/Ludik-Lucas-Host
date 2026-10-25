using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF;

public class RepositorioTiposKudoEF: IRepositorioTiposKudo
{
    private readonly ContextoDb _db;
    public RepositorioTiposKudoEF(ContextoDb db)
    {
        _db = db;
    }
    public Task<Resultado> AddAsync(TipoKudo unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(TipoKudo unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> UpdateAsync(TipoKudo unObjeto)
    {
        throw new NotImplementedException();
    }

    public async Task<Resultado<TipoKudo>> GetByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
                return Resultado<TipoKudo>.Falla(new Error("Error.Validation", $"El id no es valido. Id: {id}"));
            TipoKudo tipoKudo = await _db.TiposKudo.FindAsync(id);
            return Resultado<TipoKudo>.Exitoso(tipoKudo);
        }
        catch (Exception e)
        {
            return Resultado<TipoKudo>.Falla(new Error("Error.Unexpected", "Ha ocurrido un error inesperado. Error: "+ e.Message));
        }
    }

    public async Task<Resultado<IEnumerable<TipoKudo>>> GetAllAsync()
    {
        try
        {
            var tiposKudo = await _db.TiposKudo.AsNoTracking().ToListAsync();

            return Resultado<IEnumerable<TipoKudo>>.Exitoso(tiposKudo);
        }
        catch (Exception e)
        {
            return Resultado<IEnumerable<TipoKudo>>.Falla(Error.Unexpected);
        }
    }
}