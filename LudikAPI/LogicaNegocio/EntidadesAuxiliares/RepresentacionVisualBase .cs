using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.ValueObject;

namespace LogicaNegocio.EntidadesAuxiliares;
/// <summary>
/// Clase base abstracta para la jerarquía de Representación Visual.
/// Esto facilita la configuración de la herencia TPH en Entity Framework.
/// </summary>
public abstract class RepresentacionVisualBase: IRepresentacionVisual
{
    public abstract InformacionVisual GetInformacionVisual();
}