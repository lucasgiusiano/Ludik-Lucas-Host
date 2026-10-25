using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.Entidades;

public class UmbralParaMedallaPorKudos: IEntity
{
    public int Id { get; set; }

    public int CantidadKudos { get; set; }

    public int MedallaId { get; set; }
    public Medalla Medalla { get; set; }

    public int TipoKudoId { get; set; }
    public TipoKudo TipoKudo { get; set; }

    public int GrupoId { get; set; }
    public Grupo Grupo { get; set; }

    public Resultado esValido()
    {
        if (CantidadKudos <= 0)
        {
            return Resultado.Falla(new Error("Error.Validation", "La cantidad de kudos debe ser un número entero mayor que cero."));
        }
        return Resultado.Exitoso();
    }
}