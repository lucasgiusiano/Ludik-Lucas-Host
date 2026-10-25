using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.DTOs.PerfilEstudianteDTOs;
using LogicaAplicacion.DTOsMappers.GrupoMappers;
using LogicaAplicacion.DTOsMappers.PerfilEstudianteMappers;
using LogicaAplicacion.InterfacesCasosUsos.Profesor;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Profesores
{
    public class ObtenerRecompensasReclamadasAlumnos : IObtenerRecompensasReclamadasAlumnos
    {
        private readonly IRepositorioGrupos _repositorioGrupos;
        private readonly IRepositorioProfesores _repositorioProfesores;

        public ObtenerRecompensasReclamadasAlumnos(IRepositorioGrupos repositorioGrupos, IRepositorioProfesores repositorioProfesores)
        {
            _repositorioGrupos = repositorioGrupos;
            _repositorioProfesores = repositorioProfesores;
        }
        public async Task<Resultado<List<PerfilEstudianteRecompensaProfesorDto>>> EjecutarAsync(string grupoId)
        {
            var resultadoGrupo = await _repositorioGrupos.GetGrupoConPerfilesYRecompensasAsync(grupoId);

            if (!resultadoGrupo.EsExitoso)
            {
                return Resultado<List<PerfilEstudianteRecompensaProfesorDto>>.Falla(resultadoGrupo.Errores);
            }

            var grupo = resultadoGrupo.Valor;
            if (grupo?.Alumnos == null || !grupo.Alumnos.Any())
            {
                return Resultado<List<PerfilEstudianteRecompensaProfesorDto>>.Exitoso(new List<PerfilEstudianteRecompensaProfesorDto>());
            }

            string profesorId = grupo.ProfesorId;

            var resultadoProfesor = await _repositorioProfesores.ObtenerRecompensasPorProfesorIdAsync(profesorId);
            if (!resultadoProfesor.EsExitoso)
            {
                return Resultado<List<PerfilEstudianteRecompensaProfesorDto>>.Falla(resultadoProfesor.Errores);
            }

            var recompensasProfesorIds = resultadoProfesor.Valor.RecompensasCreadas
                .Select(rp => rp.RecompensaId)
                .ToHashSet();

            var dtoList = PerfilEstudianteRecompensaProfesorMapper.ToDtoList(
                grupo.Alumnos,
                recompensasProfesorIds
            );

            return Resultado<List<PerfilEstudianteRecompensaProfesorDto>>.Exitoso(dtoList);
        }

    }
}
