namespace LogicaNegocio.Resultados;

/// <summary>
/// Representa el resultado de una operación que devuelve un valor de tipo <typeparamref name="TValor"/>.
/// Encapsula el estado de éxito o fracaso, el valor (si es exitoso) y los errores asociados (si falló).
/// </summary>
/// <typeparam name="TValor">El tipo del valor devuelto por la operación exitosa.</typeparam>
public class Resultado<TValor>: Resultado
{
    private readonly TValor _valor;

    /// <summary>
    /// Obtiene el valor del resultado si la operación fue exitosa.
    /// Acceder a esta propiedad si EsFallo es true puede resultar en una excepción o valor default.
    /// Es recomendable verificar EsExitoso antes de acceder a Valor.
    /// </summary>
    public TValor Valor
    {
        get
        {
            return _valor;
        }
    }

    protected Resultado(TValor valor, bool esExitoso, IEnumerable<Error> errores)
        : base(esExitoso, errores)
    {
        _valor = valor;
    }

    /// <summary>
    /// Crea un resultado exitoso con un valor.
    /// </summary>
    /// <param name="valor">El valor resultante de la operación exitosa.</param>
    /// <returns>Una instancia de Resultado<TValor> indicando éxito con el valor especificado.</returns>
    public static Resultado<TValor> Exitoso(TValor valor) => new(valor, true, Enumerable.Empty<Error>());

    /// <summary>
    /// Crea un resultado fallido con un único error.
    /// El valor será el default para TValor.
    /// </summary>
    /// <param name="error">El error que causó el fallo.</param>
    /// <returns>Una instancia de Resultado<TValor> indicando fallo con el error especificado.</returns>
    public new static Resultado<TValor> Falla(Error error) => new(default, false, new List<Error> { error });
    // Usamos 'new' para ocultar el Falla de la clase base si se accede directamente a través de Resultado<T>
    // y proporcionar uno que establezca TValor a default.

    /// <summary>
    /// Crea un resultado fallido con una colección de errores.
    /// El valor será el default para TValor.
    /// </summary>
    /// <param name="errores">La colección de errores que causaron el fallo.</param>
    /// <returns>Una instancia de Resultado<TValor> indicando fallo con los errores especificados.</returns>
    public new static Resultado<TValor> Falla(IEnumerable<Error> errores) => new(default, false, errores);

} 
