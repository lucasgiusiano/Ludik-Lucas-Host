using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.Eventos;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.ValueObject;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LogicaAplicacion.ManejadoresDeEventos
{
    public class PacEventoHandler : INotificationHandler<AsignacionMedallaCompletadaEvento>
    {
        private readonly IRepositorioProyectoAulaColaborativo _repoPac;
        private readonly IRepositorioGrupos _repoGrupos;
        private readonly IRepositorioPerfilEstudianteRecompensa _repoRecompensa;
        private readonly ILogger<PacEventoHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public PacEventoHandler(
            IRepositorioProyectoAulaColaborativo repoPac,
            IRepositorioGrupos repoGrupos,
            IRepositorioPerfilEstudianteRecompensa repoRecompensa,
            ILogger<PacEventoHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _repoPac = repoPac;
            _repoGrupos = repoGrupos;
            _repoRecompensa = repoRecompensa;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            AsignacionMedallaCompletadaEvento notification,
            CancellationToken cancellationToken)
        {
            try
            {
                var perfil = notification.Estudiante
                                   .Perfiles
                                   .FirstOrDefault(p => p.Id == notification.PerfilEstudianteId);
                if (perfil == null)
                    return;

                var grupoId = perfil.GrupoId;

                var grupoRes = await _repoGrupos.GetByIdAsync(grupoId);
                
                if (grupoRes.EsFallo)
                    return;
                
                var grupo = grupoRes.Valor!;

                var pacsRes = await _repoPac.GetByGrupoAsync(grupoId);
                
                if (pacsRes.EsFallo)
                    return;
                var pac = pacsRes.Valor.FirstOrDefault(p => p.Estado == EstadoPAC.Activo);
                
                if (pac == null)
                    return;
                
                int totalMedallasObtenidasEnPeriodo = grupo.ContarMedallasEnPeriodo(pac.FechaInicio, pac.FechaFin);
                
                bool estabaCompletado = pac.Estado == EstadoPAC.Completado;

                if (totalMedallasObtenidasEnPeriodo >= pac.CantidadMedallasNecesarias)
                    pac.Estado = EstadoPAC.Completado;

                var updPac = await _repoPac.UpdateAsync(pac);
                if (updPac.EsFallo)
                {
                    _logger.LogError("[PacEventoHandler] No se pudo actualizar PAC {PacId}: {Errores}", pac.Id, updPac.Errores);
                    return;
                }

                if (!estabaCompletado && pac.Estado == EstadoPAC.Completado)
                {
                    foreach (var alumnoPerfil in grupo.Alumnos)
                    {
                        var otorgarRes = pac.RecompensaClase.Otorgar(alumnoPerfil);
                        if (otorgarRes.EsFallo)
                        {
                            _logger.LogError(
                                "[PacEventoHandler] Falló al otorgar recompensa {RecompensaId} " +
                                "a perfil {PerfilId}: {Errores}",
                                pac.RecompensaClase.Id,
                                alumnoPerfil.Id,
                                otorgarRes.Errores);
                        }
                        else
                        {
                            _logger.LogInformation(
                                "[PacEventoHandler] Recompensa {RecompensaId} aplicada " +
                                "a perfil {PerfilId}.",
                                pac.RecompensaClase.Id,
                                alumnoPerfil.Id);

                         
                        }
                    }
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"[PacEventoHandler] Excepción interna al procesar evento PAC.");
            }
        }
    }
}