namespace LogicaNegocio.InterfacesRepositorios;

// IDisposable es importante para asegurar que el DbContext se libere correctamente.
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Guarda todos los cambios realizados en el contexto de la base de datos de forma asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>El número de objetos de estado escritos en la base de datos.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}