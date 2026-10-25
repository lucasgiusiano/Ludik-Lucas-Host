using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.DTOsMappers.GrupoMappers;
using LogicaAplicacion.InterfacesCasosUsos.Profesor;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Profesores
{
    public class ObtenerGruposDeProfesor: IObtenerGruposDeProfesor
    {
        private readonly IRepositorioGrupos _repositorioGrupos;

        public ObtenerGruposDeProfesor(IRepositorioGrupos repositorioGrupos)
        {
            _repositorioGrupos = repositorioGrupos;
        }


        public async Task<Resultado<List<GrupoDto>>> EjecutarAsync(string idProfesor)
        {
            if (string.IsNullOrWhiteSpace(idProfesor))
            {
                return Resultado<List<GrupoDto>>.Falla(
                    new Error("Error.Validation", "El ID del profesor no puede ser nulo o vacío.")
                );
            }

            List<Grupo> gruposEntidad = await _repositorioGrupos.ObtenerGruposPorProfesorId(idProfesor);
            List<GrupoDto> gruposDto = gruposEntidad.Select(g => GrupoMapper.toDto(g)).ToList();


            return Resultado<List<GrupoDto>>.Exitoso(gruposDto);
        }
    }
}
