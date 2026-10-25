using LogicaAplicacion.DTOs.AvatarDTOs;
using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaNegocio.Entidades;
using LogicaNegocio.ValueObjects;

namespace LogicaAplicacion.DTOsMappers;

public class AvatarMapper
{
    /// <summary>
    /// Convierte una entidad de dominio Avatar a un AvatarDto.
    /// Transforma la colección de atributos seleccionados en un diccionario.
    /// </summary>
    public static AvatarDto toDto(Avatar avatar)
    {
        if (avatar == null) return null;

        return new AvatarDto
        {
            Id = avatar.Id,
            // Mapeo de atributos generales
            ColorFondo = avatar.ColorFondo,
            Voltear = avatar.Voltear,
            Rotacion = avatar.Rotacion,
            Zoom = avatar.Zoom,

            // Usamos LINQ para transformar la lista de entidades en un diccionario de DTOs.
            // Es eficiente y declarativo.
            AtributosSeleccionados = avatar.AtributosSeleccionados
                .ToDictionary(
                    attr => attr.Tipo.ToString(), // La clave es el nombre del tipo de atributo (ej. "Pelo")
                    attr => AtributoAvatarMapper.toDto(attr)          
                )
        };
    }
}