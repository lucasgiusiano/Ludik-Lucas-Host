using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaAplicacion.DTOsMappers.MedallaMappers;
using LogicaAplicacion.InterfacesCasosUsos.Medalla;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Medallas
{
    public class ObtenerMedallasProfesorPorGrupo : IObtenerMedallasProfesorPorGrupo
    {
        private readonly IRepositorioGrupos _repositorioGrupos;

        public ObtenerMedallasProfesorPorGrupo(IRepositorioGrupos repositorioGrupos)
        {
            _repositorioGrupos = repositorioGrupos;
        }
        public async Task<Resultado<IEnumerable<MedallaDto>>> EjecutarAsync(int grupoId)
        {
            var resultadoRepo = await _repositorioGrupos.GetMedallasDelProfesorPorGrupoAsync(grupoId);

            if (resultadoRepo.EsFallo)
                return Resultado<IEnumerable<MedallaDto>>.Falla(resultadoRepo.Errores);

            var medallas = resultadoRepo.Valor ?? Enumerable.Empty<LogicaNegocio.Entidades.Medalla>();

            var medallasDtos = medallas.Select(m => MedallaMapper.toDto(m));

            return Resultado<IEnumerable<MedallaDto>>.Exitoso(medallasDtos);
        }
    }
}
