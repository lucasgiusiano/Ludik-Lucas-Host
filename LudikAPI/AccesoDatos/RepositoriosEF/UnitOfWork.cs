using LogicaNegocio.InterfacesRepositorios;

namespace AccesoDatos.RepositoriosEF;

public class UnitOfWork : IUnitOfWork
{
    private readonly ContextoDb _context;

    public UnitOfWork(ContextoDb context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
  
}