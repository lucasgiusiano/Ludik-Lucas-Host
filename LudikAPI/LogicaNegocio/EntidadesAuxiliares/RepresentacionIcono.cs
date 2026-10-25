using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.ValueObject;

namespace LogicaNegocio.EntidadesAuxiliares;

public class RepresentacionIcono: RepresentacionVisualBase
{
    public string NombreIcono { get; set; }
    public override InformacionVisual GetInformacionVisual()
    {
       var datos = new DatosIcono(
            NombreIcono: NombreIcono
        );
        return new InformacionVisual(datos, TipoVisual.Icono);
    }
}