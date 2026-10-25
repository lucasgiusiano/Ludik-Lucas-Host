using InterfacesRepositorio;
using LogicaAplicacion.DTOs.TablaClasificacionDTOs;
using LogicaAplicacion.DTOsMappers.TablaClasificacionMappers;
using LogicaAplicacion.InterfacesCasosUsos.TablaClasificacion;
using Entidades = LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.ImplementacionCasosUsos.TablaClasificacion
{
    public class ObtenerTablaClasificacion : IObtenerTablaClasificacion
    {
        private readonly IRepositorioTablasClasificacion _repoTablas;

        public ObtenerTablaClasificacion(IRepositorioTablasClasificacion repoTablas)
        {
            _repoTablas = repoTablas;
        }

        public async Task<Resultado<TablaClasificacionInfoDto>> EjecutarAsync(int tablaId)
        {
            var res = await _repoTablas.GetByIdAsync(tablaId);
            if (res.EsFallo)
                return Resultado<TablaClasificacionInfoDto>.Falla(res.Errores);

            var tabla = res.Valor;

            var participantes = tabla.Grupo?.Alumnos ?? new List<Entidades.PerfilEstudiante>();

            var participantesOrdenados = participantes
                .OrderByDescending(p =>
                    p.MedallasObtenidas?.Count(pm => pm.MedallaId == tabla.MedallaAsociadaId) ?? 0
                )
                .ToList();

            var dto = TablaClasificacionInfoMapper.Map(tabla, participantesOrdenados);

            return Resultado<TablaClasificacionInfoDto>.Exitoso(dto);
        }
    }
}
