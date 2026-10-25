using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.SolocitudUnionDTOs;
using LogicaAplicacion.DTOsMappers.SolicitudMappers;
using LogicaAplicacion.InterfacesCasosUsos.Profesor;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Profesores
{
    public class ObtenerSolicitudesUnionDelGrupo : IObtenerSolicitudesUnionDelGrupo
    {
        private readonly IRepositorioGrupos _repositorioGrupos;
        private readonly IRepositorioSolicitudesUnion _repositorioSolicitudesUnion;
        private readonly IRepositorioProfesores _repositorioProfesores;
        public ObtenerSolicitudesUnionDelGrupo(
            IRepositorioGrupos repositorioGrupos,
            IRepositorioSolicitudesUnion repositorioSolicitudesUnion,
            IRepositorioProfesores repositorioProfesores)
        {
            _repositorioGrupos = repositorioGrupos;
            _repositorioSolicitudesUnion = repositorioSolicitudesUnion;
            _repositorioProfesores = repositorioProfesores;
        }
        public async Task<Resultado<List<SolicitudUnionListadoDto>>> EjecutarAsync(int grupoId, string profesorId)
        {
            var grupoResultado = await _repositorioGrupos.GetByIdAsync(grupoId);
            if (!grupoResultado.EsExitoso)
                return Resultado<List<SolicitudUnionListadoDto>>.Falla(Error.NotFound);

            var grupo = grupoResultado.Valor;

            if (grupo.ProfesorId != profesorId)
                return Resultado<List<SolicitudUnionListadoDto>>.Falla(new Error("Error.Validation", "El grupo no pertenece al profesor."));

            var solicitudes = await _repositorioSolicitudesUnion.ObtenerSolicitudesPendientesPorGrupoAsync(grupoId);

            var solicitudesDto = SolicitudUnionListadoMapper.MapearLista(solicitudes);

            return Resultado<List<SolicitudUnionListadoDto>>.Exitoso(solicitudesDto);
        }
    }
}
