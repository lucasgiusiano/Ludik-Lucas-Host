using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF;

public class RepositorioAvataresEF: IRepositorioAvatares
{
    private readonly ContextoDb _db;
    public RepositorioAvataresEF(ContextoDb db)
    {
        _db = db;
    }

    public Task<Resultado> AddAsync(Avatar unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(Avatar unObjeto)
    {
        throw new NotImplementedException();
    }

    public async Task<Resultado> UpdateAsync(Avatar unObjeto)
    {
        if (unObjeto == null)
            return Resultado.Falla(new Error("Error.Validation", "El objeto a actualizar no puede ser nulo."));

        try
        {
            _db.Avatares.Update(unObjeto);
            await _db.SaveChangesAsync();
            return Resultado.Exitoso();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Resultado.Falla(Error.Conflict);
        }
        catch (DbUpdateException dbEx)
        {
            var detalle = dbEx.InnerException?.Message ?? dbEx.Message;
            return Resultado.Falla(new Error("Error.Unexpected", $"Error al actualizar en la BD: {detalle}"));
        }
        catch (Exception ex)
        {
            return Resultado.Falla(new Error("Error.Unexpected", ex.Message));
        }
        
    }

    public Task<Resultado<Avatar>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado<IEnumerable<Avatar>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }


    public async Task<Resultado<Avatar>> GetByPerfilIdAsync(int idPerfilEstudiante)
    {
        try
        {
            Avatar avatarDelPerfil = await _db.Avatares.Include(p=>p.AtributosSeleccionados).FirstOrDefaultAsync(a => a.PerfilEstudianteId == idPerfilEstudiante);
            return Resultado<Avatar>.Exitoso(avatarDelPerfil);
        }
        catch (Exception e)
        {
            return Resultado<Avatar>.Falla(new Error("Error.Unexpected", e.Message));
        }
    }
}