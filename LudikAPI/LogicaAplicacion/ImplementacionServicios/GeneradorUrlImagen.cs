using LogicaAplicacion.Servicios;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.ImplementacionServicios;

public class GeneradorUrlImagen : IGeneradorUrlImagen
{
    private readonly IRepositorioAlmacenamientoArchivos _repositorioArchivos;

    public GeneradorUrlImagen(IRepositorioAlmacenamientoArchivos repositorioArchivos)
    {
        _repositorioArchivos = repositorioArchivos;
    }

    public async Task<string?> GenerarUrlLecturaAsync(string? nombreBlob)
    {
        if (string.IsNullOrEmpty(nombreBlob))
        {
            return null;
        }

        var resultado = await _repositorioArchivos.ObtenerArchivoSasUrlAsync(nombreBlob);

        return resultado.EsExitoso ? resultado.Valor : null; 
    }
}