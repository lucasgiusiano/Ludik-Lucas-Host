using LogicaNegocio.InterfacesEntidades;

namespace LogicaNegocio.Entidades;

public class TipoKudo: IEntity
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string Descripcion { get; set; }
    public string NombreIcono { get; set; }

}