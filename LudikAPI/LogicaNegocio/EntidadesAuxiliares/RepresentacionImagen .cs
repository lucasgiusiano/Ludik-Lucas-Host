using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.ValueObject;

namespace LogicaNegocio.EntidadesAuxiliares;

public class RepresentacionImagen: RepresentacionVisualBase
{
    public string NombreImagenCompleta { get; set; }
    public string NombreImagenMiniatura { get; set; }
    public override InformacionVisual GetInformacionVisual()
    {
        var datos = new DatosImagen(
            NombreMiniatura: NombreImagenMiniatura,
            NombreCompleta: NombreImagenCompleta
        );
        return new InformacionVisual(datos, TipoVisual.Imagen);
    }
}