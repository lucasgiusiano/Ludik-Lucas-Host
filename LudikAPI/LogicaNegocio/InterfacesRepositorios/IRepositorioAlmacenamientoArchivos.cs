using LogicaNegocio.Resultados;

namespace LogicaNegocio.InterfacesRepositorios;

public interface IRepositorioAlmacenamientoArchivos
{
    /// <summary>
    /// Sube un archivo al almacenamiento.
    /// </summary>
    /// <param name="streamArchivo">El contenido del archivo como un stream.</param>
    /// <param name="nombreArchivo">El nombre único del archivo en el almacenamiento.</param>
    /// <param name="tipoContenido">El tipo MIME del archivo (e.g., "image/jpeg").</param>
    /// <returns>La URL del archivo subido.</returns>
    Task<Resultado<string>> SubirArchivoAsync(Stream streamArchivo, string nombreArchivo, string tipoContenido);
        
    /// <summary>
    /// Genera una URL de acceso seguro y temporal (SAS Token) para un archivo.
    /// </summary>
    /// <param name="nombreArchivo">El nombre único del archivo.</param>
    /// <returns>La URL con el token SAS.</returns>
    Task<Resultado<string>> ObtenerArchivoSasUrlAsync(string nombreArchivo);
}