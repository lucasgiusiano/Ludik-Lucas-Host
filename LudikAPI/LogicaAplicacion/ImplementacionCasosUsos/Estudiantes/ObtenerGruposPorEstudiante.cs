using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.DTOsMappers.GrupoMappers;
using LogicaAplicacion.InterfacesCasosUsos.Estudiante;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Estudiantes
{
    public class ObtenerGruposDeEstudiante: IObtenerGruposDeEstudiante
    {
        private readonly IRepositorioGrupos _repositorioGrupos;

        public ObtenerGruposDeEstudiante(IRepositorioGrupos repositorioGrupos)
        {
            _repositorioGrupos = repositorioGrupos;
        }

        public async Task<Resultado<List<GrupoDto>>> EjecutarAsync(string idEstudiante)
        {
            if (string.IsNullOrWhiteSpace(idEstudiante))
            {
                return Resultado<List<GrupoDto>>.Falla(
                    new Error("Estudiante.IdInvalido", "El ID del estudiante no puede ser nulo o vacío.")
                );
            }

            List<Grupo> gruposEntidad = await _repositorioGrupos.ObtenerGruposPorEstudianteId(idEstudiante);
            List<GrupoDto> gruposDto = gruposEntidad.Select(g => GrupoMapper.toDto(g)).ToList();


            return Resultado<List<GrupoDto>>.Exitoso(gruposDto);
        }
    }
}
