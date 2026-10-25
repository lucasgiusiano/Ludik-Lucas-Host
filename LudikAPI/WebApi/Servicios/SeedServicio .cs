using Azure.Storage.Blobs;
using LogicaAplicacion.InterfacesCasosUsos.ServicioPrecargaArchivos;
using LogicaNegocio.InterfacesRepositorios;

namespace WebApi.Servicios;

public class SeedServicio: ISeedServicio
{
    private readonly IRepositorioAlmacenamientoArchivos _repositorioArchivos;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<SeedServicio> _logger;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public SeedServicio(
        IRepositorioAlmacenamientoArchivos repositorioArchivos,
        IWebHostEnvironment env,
        ILogger<SeedServicio> logger,
        IConfiguration configuration,
        BlobServiceClient blobServiceClient)
    {
        _repositorioArchivos = repositorioArchivos;
        _env = env;
        _logger = logger;
        _blobServiceClient = blobServiceClient;
        _containerName = configuration["StorageContainerName"];
    }

    public async Task PrecargarArchivosAsync()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        string assetsPath = Path.Combine(_env.ContentRootPath, "SeedArchivos", "ImagenesAtributosAvatar");

        if (!Directory.Exists(assetsPath))
        {
            _logger.LogWarning("La carpeta de activos de semilla 'SeedArchivos/ImagenesAtributosAvatar' no existe. Saltando la precarga de archivos.");
            return;
        }

        var archivos = Directory.GetFiles(assetsPath, "*.*", SearchOption.AllDirectories);
        _logger.LogInformation($"Se encontraron {archivos.Length} archivos de activos para la precarga.");

        foreach (var rutaArchivo in archivos)
        {
            // Convierte la ruta de archivo local a una ruta de blob relativa
            // Ej: "C:\...\WebApi\SeedAssets\imagenes\medallas\icono_asistencia.png" -> "medallas/icono_asistencia.png"
            string nombreBlob = rutaArchivo.Substring(assetsPath.Length + 1).Replace('\\', '/');

            var blobClient = containerClient.GetBlobClient(nombreBlob);

            if (await blobClient.ExistsAsync())
            {
                _logger.LogInformation($"El blob '{nombreBlob}' ya existe. Saltando.");
                continue;
            }

            try
            {
                await using var stream = File.OpenRead(rutaArchivo);
                var contentType = ObtenerContentType(rutaArchivo);
                await _repositorioArchivos.SubirArchivoAsync(stream, nombreBlob, contentType);
                _logger.LogInformation($"Archivo '{nombreBlob}' subido exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Falló la subida del archivo '{nombreBlob}'.");
            }
        }
    }

    private string ObtenerContentType(string rutaArchivo)
    {
        var extension = Path.GetExtension(rutaArchivo).ToLowerInvariant();
        return extension switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream",
        };
    }
}