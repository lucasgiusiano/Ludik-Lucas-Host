using LogicaAplicacion.DTOs.AtributoAvatarDTOs;

namespace LogicaAplicacion.DTOs.AvatarDTOs;

public class AvatarDto
{
    public int Id { get; set; }

    // Atributos Generales
    public string ColorFondo { get; set; }
    public bool Voltear { get; set; }
    public int Rotacion { get; set; }
    public int Zoom { get; set; }

    // Ejemplo: { "Pelo": { Id: 1, Nombre: "Pelo Punk", ... }, "Ropa": { Id: 25, ... } }
    public Dictionary<string, AtributoAvatarDto> AtributosSeleccionados { get; set; }
}