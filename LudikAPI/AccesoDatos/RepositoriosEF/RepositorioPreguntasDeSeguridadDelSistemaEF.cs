using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF;

public class RepositorioPreguntasDeSeguridadDelSistemaEF: IRepositorioPreguntasDeSeguridadDelSistema
{
    private readonly ContextoDb _db;
    public RepositorioPreguntasDeSeguridadDelSistemaEF(ContextoDb db)
    {
        _db = db;
    }

    public Task<Resultado> AddAsync(PreguntaDeSeguridad unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(PreguntaDeSeguridad unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> UpdateAsync(PreguntaDeSeguridad unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado<PreguntaDeSeguridad>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Resultado<IEnumerable<PreguntaDeSeguridad>>> GetAllAsync()
    {
        try
        {
            var preguntas = await _db.PreguntasDeSeguridad
                .AsNoTracking()
                .ToListAsync();

            return Resultado<IEnumerable<PreguntaDeSeguridad>>.Exitoso(preguntas);
        }
        catch (Exception e)
        {
            return Resultado<IEnumerable<PreguntaDeSeguridad>>.Falla(new Error ("Error.Unexpected", e.Message));
        }
    }
}