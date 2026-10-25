using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositoriosEF;

public class RepositorioRecompensasDeProfesoresEF: IRepositorioRecompensasDeProfesores
{
    private readonly ContextoDb _db;
    public RepositorioRecompensasDeProfesoresEF(ContextoDb db)
    {
        _db = db;
    }
    public Task<Resultado> AddAsync(RecompensaProfesor unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> RemoveAsync(RecompensaProfesor unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado> UpdateAsync(RecompensaProfesor unObjeto)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado<RecompensaProfesor>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Resultado<IEnumerable<RecompensaProfesor>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Resultado<IEnumerable<RecompensaProfesor>>> GetByProfesorIdAsync(string profesorId)
    {
        try
        {
            var listaRecompensasDelProfesor = await _db.RecompensasDeProfesores
                .Include(pr => pr.Recompensa) 
                .Where(pr => pr.ProfesorId == profesorId)
                .ToListAsync();

            return Resultado<IEnumerable<RecompensaProfesor>>.Exitoso(listaRecompensasDelProfesor);
        }
        catch (Exception e)
        {
            return Resultado<IEnumerable<RecompensaProfesor>>.Falla(new Error("Error.Unexpected",
                "Ha ocurrido un error en la base de datos. Error: " + e.Message));
        }
    }
}