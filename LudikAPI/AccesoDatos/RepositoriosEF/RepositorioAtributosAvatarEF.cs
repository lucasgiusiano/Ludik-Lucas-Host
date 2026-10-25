using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF;

public class RepositorioAtributosAvatarEF: IRepositorioAtributosAvatar
{
    private readonly ContextoDb _db;
    public RepositorioAtributosAvatarEF(ContextoDb db)
    {
        _db = db;
    }
    public Task<Resultado> AddAsync(AtributoAvatar unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(AtributoAvatar unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> UpdateAsync(AtributoAvatar unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado<AtributoAvatar>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Resultado<IEnumerable<AtributoAvatar>>> GetAllAsync()
    {
        try
        {
            var atributos = await _db.AtributosAvatar.ToListAsync();
            return Resultado<IEnumerable<AtributoAvatar>>.Exitoso(atributos);
        }
        catch (Exception e)
        {
            return Resultado<IEnumerable<AtributoAvatar>>.Falla(Error.Unexpected);
        }
    }

    public async Task<Resultado<IEnumerable<AtributoAvatar>>> GetByIdsAsync(IEnumerable<int> ids)
    {
        try
        {
            if (ids == null)
                return Resultado<IEnumerable<AtributoAvatar>>.Falla(new Error("Error.Validation", "No se han recibido atributos del avatar."));
            
            IEnumerable<AtributoAvatar> atributos = await _db.AtributosAvatar
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();
            return Resultado<IEnumerable<AtributoAvatar>>.Exitoso(atributos);

        }
        catch (Exception e)
        {
            return Resultado<IEnumerable<AtributoAvatar>>.Falla(new Error("Error.Unexpected", "Error: " + e.Message));
        }
    }
}