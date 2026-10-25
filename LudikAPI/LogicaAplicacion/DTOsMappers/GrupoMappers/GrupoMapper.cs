using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.GrupoDTOs;

namespace LogicaAplicacion.DTOsMappers.GrupoMappers
{
    public class GrupoMapper
    {
        public static GrupoDto toDto(Grupo grupo)
        {
            return new GrupoDto
            {
                Id = grupo.Id,
                Nombre = grupo.Nombre,
                ProfesorId = grupo.ProfesorId,
                Institucion = grupo.Institucion,
                Materia = grupo.Materia,
				CantAlumnos = grupo.Alumnos.Count()
            };

        }
        /*
        public static Grupo fromDto(GrupoDto grupo)
        {
            return new Grupo
            {
               
            }
        }

        */

    }
}
