using InterfacesRepositorio;
using LogicaAplicacion.DTOs.ImagenPerfilDtos;
using LogicaAplicacion.InterfacesCasosUsos.Imagenes;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Entidad = LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.ImagenDto;
using Microsoft.Extensions.DependencyInjection;

namespace LogicaAplicacion.ImplementacionCasosUsos.Imagenes;

public class ServicioGestionImagen: IServicioGestionImagen
{
    private readonly IRepositorioAlmacenamientoArchivos _repositorioArchivos;
    private readonly IServicioProcesamientoImagenes _procesadorImagenes;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRepositorioPerfilEstudianteGrupo _repositorioPerfilesEstudiantes;

    public ServicioGestionImagen(
        IRepositorioAlmacenamientoArchivos fileStorage,
        IServicioProcesamientoImagenes procesadorImagenes,
        IServiceProvider serviceProvider,
        IRepositorioPerfilEstudianteGrupo repositorioPerfilesEstudiantes)
    {
        _repositorioArchivos = fileStorage;
        _procesadorImagenes = procesadorImagenes;
        _serviceProvider = serviceProvider;
        _repositorioPerfilesEstudiantes = repositorioPerfilesEstudiantes;
    }

    public async Task<Resultado> SubirImagenAsync(SubirImagenDto request)
    {
        var actualizador = _serviceProvider.GetKeyedService<IActualizadorRutaImagen>(request.Proposito);
        if (actualizador == null)
            return Resultado.Falla(new Error("Error.Validation", $"El propósito '{request.Proposito}' no es válido o no tiene una estrategia registrada."));
        

        var resultadoProcesamiento = await _procesadorImagenes.ProcesarImagenPerfilAsync(request.ImagenStream);
        if (resultadoProcesamiento.EsFallo)
            return Resultado.Falla(resultadoProcesamiento.Errores);
        

        var urls = new Dictionary<string, string>();
        var nombresArchivo = new Dictionary<string, string>();

        foreach (var imagenProcesada in resultadoProcesamiento.Valor)
        {
            var nombreArchivo = $"{request.Proposito}/{request.IdUsuarioAutenticado}/{imagenProcesada.Tipo}_{Guid.NewGuid()}.jpg";
            var resultadoUpload = await _repositorioArchivos.SubirArchivoAsync(imagenProcesada.Contenido, nombreArchivo, "image/jpeg");

            await imagenProcesada.Contenido.DisposeAsync();

            if (resultadoUpload.EsFallo) return Resultado.Falla(resultadoUpload.Errores);

            urls[imagenProcesada.Tipo] = resultadoUpload.Valor;
            nombresArchivo[imagenProcesada.Tipo] = nombreArchivo;
        }

        var resultadoActualizacion = await actualizador.ActualizarRutasAsync(request.IdUsuarioAutenticado, request.EntidadAsociadaId, nombresArchivo);
        if (resultadoActualizacion.EsFallo)
            return Resultado.Falla(resultadoActualizacion.Errores);
        

        return Resultado.Exitoso();
    }


    public async Task<Resultado<ImagenPerfilDto>> ObtenerUrlImagenPerfilAsync(int idPerfilEstudiante, string idUsuarioAutenticado)
    {
        var resultadoPerfil = await _repositorioPerfilesEstudiantes.GetByIdAsync(idPerfilEstudiante);
        if (resultadoPerfil.EsFallo)
            return Resultado<ImagenPerfilDto>.Falla(Error.NotFound);

        Entidad.PerfilEstudiante perfil = resultadoPerfil.Valor;
        if (perfil.EstudianteId != idUsuarioAutenticado)
        {
            // El usuario está autenticado, pero intenta modificar un recurso que no le pertenece.
            return Resultado<ImagenPerfilDto>.Falla(Error.Forbidden);
        }

        var nombreCompleta = resultadoPerfil.Valor.NombreImagenCompleta;
        var nombreMiniatura = resultadoPerfil.Valor.NombreImagenMiniatura;


        var resultadoUrlCompleta = await _repositorioArchivos.ObtenerArchivoSasUrlAsync(nombreCompleta);
        var resultadoUrlMiniatura = await _repositorioArchivos.ObtenerArchivoSasUrlAsync(nombreMiniatura);

        if (resultadoUrlCompleta.EsFallo || resultadoUrlMiniatura.EsFallo)
            return Resultado<ImagenPerfilDto>.Falla(new Error("Error.Unexpected", "Hubo un error al generar la url de la imagen del perfil."));
        

        var imagenPerfilDto = new ImagenPerfilDto
        {
            UrlCompleta = resultadoUrlCompleta.Valor,
            UrlMiniatura = resultadoUrlMiniatura.Valor
        };

        return Resultado<ImagenPerfilDto>.Exitoso(imagenPerfilDto);
    }
}