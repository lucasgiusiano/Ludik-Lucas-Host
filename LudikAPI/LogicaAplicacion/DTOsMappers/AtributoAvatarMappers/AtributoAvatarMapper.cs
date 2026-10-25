using LogicaAplicacion.DTOs.AtributoAvatarDTOs;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.DTOsMappers;

public class AtributoAvatarMapper
{
    /// <summary>
    /// Convierte una entidad de dominio AtributoAvatar a un AtributoAvatarDto.
    /// </summary>
    public static AtributoAvatarDto toDto(AtributoAvatar atributo)
    {
        if (atributo == null) return null;

        return new AtributoAvatarDto
        {
            Id = atributo.Id,
            Nombre = atributo.Nombre,
            Tipo = atributo.Tipo.ToString(), // Convierte el enum a string para el DTO
            EnlaceImagen = atributo.NombreImagenRecurso,
            CodigoUnico = atributo.CodigoUnico
        };
    }
}