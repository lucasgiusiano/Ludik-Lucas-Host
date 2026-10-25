using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.ProyectoAulaColaborativoDTOs;
using LogicaAplicacion.DTOsMappers.ProyectoAulaColaborativoMappers;
using LogicaAplicacion.InterfacesCasosUsos.ProyectoAulaColaborativo;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;

namespace LogicaAplicacion.ImplementacionCasosUsos.ProyectoAulaColaborativo
{
    public class AltaProyectoAulaColaborativo : IAltaProyectoAulaColaborativo
    {
        private readonly IRepositorioProyectoAulaColaborativo _repoPac;
        private readonly IRepositorioGrupos _repoGrupos;
        private readonly IRepositorioRecompensas _repoRecompensas;

        public AltaProyectoAulaColaborativo(IRepositorioProyectoAulaColaborativo repoPac,IRepositorioGrupos repoGrupos, IRepositorioRecompensas repoRecompensas)
        {
            _repoPac = repoPac;
            _repoGrupos = repoGrupos;
            _repoRecompensas = repoRecompensas;
        }
        public async Task<Resultado> EjecutarAsync(int grupoId, AltaProyectoAulaColaborativoDto dto,string profesorId)
        {
            if (dto == null)
                return Resultado.Falla(new Error("Error.Validation","No hay información para dar de alta el proyecto colaborativo."));

            var grupoRes = await _repoGrupos.GetByIdAsync(grupoId);
            if (grupoRes.EsFallo)
                return Resultado.Falla(new Error("Error.Validation",$"No se encontró el grupo con ID {grupoId}."));
            var gruposProfRes = await _repoGrupos.ObtenerGruposPorProfesorId(profesorId);
            if (gruposProfRes==null)
                return Resultado.Falla(new Error("Error.Validation", "Error al obtener los grupos del profesor."));
            if (!gruposProfRes!.Any(g => g.Id == grupoId))
                return Resultado.Falla(new Error("Error.Unauthorized", $"El profesor no tiene acceso al grupo con ID {grupoId}."));
            var pacs = await _repoPac.GetByGrupoAsync(grupoId);
            if (pacs.EsFallo)
                return Resultado.Falla(new Error("Error.Unexpected","Error verificando proyectos existentes."));
            if (pacs.Valor!.Any(p => p.Estado == EstadoPAC.Activo))
                return Resultado.Falla(new Error("Error.Validation","Ya existe un proyecto colaborativo activo para este grupo."));

            var pac = ProyectoAulaColaborativoMapper.ToDomain(dto, grupoId);

            var recRes = await _repoRecompensas.GetByIdAsync(dto.RecompensaClaseId);
            if (recRes.EsFallo)
                return Resultado.Falla(new Error("Error.Validation",$"No se encontró la recompensa con ID {dto.RecompensaClaseId}."));
            pac.RecompensaClase = recRes.Valor!;

            var valid = pac.esValido();
            if (valid.EsFallo)
                return valid;

            return await _repoPac.AddAsync(pac);
        }
    }
}
