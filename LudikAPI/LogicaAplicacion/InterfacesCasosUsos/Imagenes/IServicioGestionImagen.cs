using LogicaAplicacion.DTOs.ImagenDto;
using LogicaAplicacion.DTOs.ImagenPerfilDtos;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Http;

namespace LogicaAplicacion.InterfacesCasosUsos.Imagenes;

public interface IServicioGestionImagen
{
    Task<Resultado> SubirImagenAsync(SubirImagenDto dto);

    Task<Resultado<ImagenPerfilDto>> ObtenerUrlImagenPerfilAsync(int idPerfilEstudiante, string idUsuarioAutenticado);
}