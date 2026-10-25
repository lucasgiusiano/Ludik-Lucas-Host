namespace LogicaAplicacion.Servicios;
/// <summary>
/// Define un servicio para procesar colecciones de objetos y generar URLs para sus propiedades de imagen.
/// </summary>
public interface IGeneradorUrlsParaColeccionesImagenes
{

    /// <summary>
    /// Procesa una colección de DTOs para generar y asignar URLs de lectura a sus propiedades de imagen.
    /// </summary>
    /// <typeparam name="T">El tipo del DTO a procesar.</typeparam>
    /// <param name="dtos">La colección de DTOs.</param>
    /// <param name="accesoresDePropiedad">Un array de tuplas que definen cómo obtener la ruta de la imagen y cómo establecer la URL generada en el DTO.</param>
    /// <returns>Una tarea que se completa cuando todos los DTOs han sido procesados.</returns>
    Task EjecutarProcesarUrlsAsync<T>(IEnumerable<T> dtos, params (Func<T, string> getPath, Action<T, string> setUrl)[] accesoresDePropiedad);
}