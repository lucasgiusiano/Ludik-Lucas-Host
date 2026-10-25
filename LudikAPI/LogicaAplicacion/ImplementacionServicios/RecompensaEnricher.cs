using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Entidades;
using LogicaNegocio.ValueObject;

namespace LogicaAplicacion.ImplementacionServicios;

public class RecompensaEnricher: IRecompensaEnricher
{
    private readonly IGeneradorUrlImagen _generadorUrl;

    public RecompensaEnricher(IGeneradorUrlImagen generadorUrl)
    {
        _generadorUrl = generadorUrl;
    }

    public async Task<List<RecompensaClienteDto>> EnrichAsync(IEnumerable<Recompensa> recompensas)
    {
        var listaClienteDto = new List<RecompensaClienteDto>();

        foreach (var recompensa in recompensas)
        {
            var infoVisual = recompensa.Representacion.GetInformacionVisual();
            RespuestaVisual datosRespuesta;

            switch (infoVisual.Datos)
            {
                case DatosImagen datosImagen:
                    var urlMiniaturaTask = _generadorUrl.GenerarUrlLecturaAsync(datosImagen.NombreMiniatura);
                    var urlCompletaTask = _generadorUrl.GenerarUrlLecturaAsync(datosImagen.NombreCompleta);
                    await Task.WhenAll(urlMiniaturaTask, urlCompletaTask);
                    datosRespuesta = new RespuestaImagenDto(urlMiniaturaTask.Result, urlCompletaTask.Result);
                    break;

                case DatosIcono datosIcono:
                    datosRespuesta = new RespuestaIconoDto(datosIcono.NombreIcono);
                    break;

                default:
                    datosRespuesta = null;
                    break;
            }

            if (datosRespuesta != null)
            {
                listaClienteDto.Add(new RecompensaClienteDto
                {
                    Id = recompensa.Id,
                    Nombre = recompensa.Nombre,
                    Precio = recompensa.Precio,
                    Tipo = infoVisual.Tipo,
                    Datos = datosRespuesta
                });
            }
        }
        return listaClienteDto;
    }
}