using System;
using System;
using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.GrupoDTOs;

namespace LogicaAplicacion.DTOsMappers.GrupoMappers
{
    public class GrupoAltaMapper
    {
        public static GrupoAltaDto toDto(Grupo grupo)
        {
            return new GrupoAltaDto
            {
                Nombre = grupo.Nombre,
                TablaEquivalenciaId = grupo.TablaEquivalencia.Id,
                ProfesorId =grupo.ProfesorId,
                Institucion = grupo.Institucion,
                Materia = grupo.Materia
            };
        }
        public static Grupo fromDto(GrupoAltaDto dto, TablaEquivalencia tabla)
        {
            var grupo = new Grupo
            {
                Nombre = dto.Nombre,
                TablaEquivalencia = tabla,
                ProfesorId = dto.ProfesorId,
                Institucion = dto.Institucion ?? "",
                Materia = dto.Materia ?? "",
                FCreacion = DateTime.Now,
                Tienda = new Tienda()
            };

            // Si los datos del enlace están disponibles, creamos el EnlaceUnion
            if (!string.IsNullOrEmpty(dto.CodigoEnlace) && !string.IsNullOrEmpty(dto.UrlCompleta))
            {
                grupo.EnlaceUnion = new EnlaceUnion
                {
                    CodigoUnico = dto.CodigoEnlace,
                    UrlCompleta = dto.UrlCompleta,
                    Expiracion = DateTime.UtcNow.AddDays(300)
                };
            }

            return grupo;
        }
    }
}
