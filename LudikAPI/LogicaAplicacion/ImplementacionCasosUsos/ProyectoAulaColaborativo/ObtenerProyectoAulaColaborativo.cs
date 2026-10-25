using InterfacesRepositorio;
using LogicaAplicacion.DTOs.ProyectoAulaColaborativoDTOs;
using LogicaAplicacion.DTOsMappers.ProyectoAulaColaborativoMappers;
using LogicaAplicacion.InterfacesCasosUsos.ProyectoAulaColaborativo;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorio;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicaAplicacion.ImplementacionCasosUsos.ProyectoAulaColaborativo
{
    public class ObtenerProyectoAulaColaborativo : IObtenerProyectoAulaColaborativo
    {
        private readonly IRepositorioProyectoAulaColaborativo _repoPac;
        private readonly IRepositorioGrupos _repositorioGrupos;


        public ObtenerProyectoAulaColaborativo(
            IRepositorioProyectoAulaColaborativo repoPac, 
            IRepositorioGrupos repositorioGrupos)
        {
            _repoPac = repoPac;
            _repositorioGrupos = repositorioGrupos;
        }

        public async Task<Resultado<ProyectoAulaColaborativoDto>> EjecutarAsync(int grupoId)
        {

            var resultadoPac = await _repoPac.GetByGrupoAsync(grupoId);
            
            if (resultadoPac.EsFallo)
                return Resultado<ProyectoAulaColaborativoDto>.Falla(resultadoPac.Errores);
            
            LogicaNegocio.Entidades.ProyectoAulaColaborativo pac = resultadoPac.Valor.FirstOrDefault(p => p.Estado == EstadoPAC.Activo);
            if (pac == null)
                return Resultado<ProyectoAulaColaborativoDto>.Exitoso(new ProyectoAulaColaborativoDto());

            var resultadoGrupos = await _repositorioGrupos.GetByIdAsync(grupoId);
            
            if (resultadoGrupos.EsFallo)
                return Resultado<ProyectoAulaColaborativoDto>.Falla(resultadoGrupos.Errores);
            
            var grupo = resultadoGrupos.Valor;
            int cantMedallasObtenidas = grupo.ContarMedallasEnPeriodo(pac.FechaInicio, pac.FechaFin);

            var dto = pac.ToDto();
            dto.TotalContribuciones = cantMedallasObtenidas;

            return Resultado<ProyectoAulaColaborativoDto>.Exitoso(dto);
        }
    }
}
