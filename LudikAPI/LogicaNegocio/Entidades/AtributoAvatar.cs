using LogicaNegocio.InterfacesEntidades;
using System.ComponentModel.DataAnnotations;

namespace LogicaNegocio.Entidades;

public enum TipoAtributo
{
    Pelo,
    Cejas,
    Ojos,
    Boca,
    Barba,
    Gafas,
    Ropa,
    ColorPiel,
    ColorPelo,
    ColorBarba,
    ColorRopa,
    ColorGafas

}

public class AtributoAvatar : IEntity 
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } // Ej: "Pelo Largo y Rubio", "Gafas de Sol Aviador"

    [Required]
    public TipoAtributo Tipo { get; set; }

    [Required]
    public string NombreImagenRecurso { get; set; }

    // Código único para ser referenciado por el sistema de renderizado de avatares.
    // Ej: "hair_long_blonde", "glasses_aviator"
    [Required]
    public string CodigoUnico { get; set; }
}