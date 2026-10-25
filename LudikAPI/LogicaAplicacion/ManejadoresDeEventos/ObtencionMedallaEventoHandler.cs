using InterfacesRepositorio;
using LogicaAplicacion.Eventos;
using LogicaAplicacion.Servicios;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LogicaAplicacion.ManejadoresDeEventos;

public class ObtencionMedallaEventoHandler : INotificationHandler<AsignacionMedallaCompletadaEvento>
{
    private readonly ILogger<ObtencionMedallaEventoHandler> _logger;
    private readonly INotificacionServicio _notificacionServicio;
    private readonly IRepositorioMedallas _repoMedallas;

    public ObtencionMedallaEventoHandler(
        ILogger<ObtencionMedallaEventoHandler> logger,
        INotificacionServicio notificacionServicio,
        IRepositorioMedallas repoMedallas)
    {
        _logger = logger;
        _notificacionServicio = notificacionServicio;
        _repoMedallas = repoMedallas;
    }

    public async Task Handle(AsignacionMedallaCompletadaEvento notification, CancellationToken cancellationToken)
    {
        // 1. Log para el servidor (la lógica original de PerfilObserver)
        _logger.LogInformation($"[Evento] Se asignó la medalla {notification.MedallaId} al perfil {notification.PerfilEstudianteId}.");

        // 2. Lógica para notificar al usuario final
        var medallaResult = await _repoMedallas.GetByIdAsync(notification.MedallaId);
        if (medallaResult.EsExitoso && notification.Estudiante != null)
        {
            var medalla = medallaResult.Valor;

            string usuarioId = notification.Estudiante.Id;

            await _notificacionServicio.NotificarObtencionMedallaAsync(
                usuarioId,
                medalla.Nombre,
                medalla.MonedasOtorgadas);
        }
        else
        {
            _logger.LogWarning($"No se pudo enviar notificación para la medalla {notification.MedallaId} al perfil {notification.PerfilEstudianteId} porque no se encontró la medalla o el estudiante.");
        }
    }
}
