using LogicaAplicacion.DTOs.PerfilEstudianteDTOs;
using LogicaNegocio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace LogicaAplicacion.DTOsMappers.PerfilEstudianteMappers
{
    public static class PerfilEstudianteRecompensaProfesorMapper
    {
        public static List<PerfilEstudianteRecompensaProfesorDto> ToDtoList(
            IEnumerable<PerfilEstudiante> alumnos,
            HashSet<int> recompensasProfesorIds)
        {
            if (alumnos == null || !alumnos.Any() || recompensasProfesorIds == null || !recompensasProfesorIds.Any())
            {
                return new List<PerfilEstudianteRecompensaProfesorDto>();
            }

            var dtoList = alumnos
                .SelectMany(perfil => perfil.InventarioRecompensas, (perfil, per) => new
                {
                    Perfil = perfil,
                    RelacionRecompensa = per
                })
                .Where(data => recompensasProfesorIds.Contains(data.RelacionRecompensa.RecompensaId))
                .Select(data =>
                {
                    var nombreVO = data.Perfil.Estudiante?.NombreCompleto;

                    return new PerfilEstudianteRecompensaProfesorDto
                    {
                        EstudianteId = data.Perfil.EstudianteId,

                        NombreEstudiante = nombreVO != null
                                           ? $"{nombreVO.Nombre} {nombreVO.Apellido}"
                                           : "Estudiante Desconocido",

                        RecompensaId = data.RelacionRecompensa.RecompensaId.ToString(),
                        Nombre = data.RelacionRecompensa.Recompensa?.Nombre
                    };
                })
                .ToList();

            return dtoList;
        }
    }
}
