using LogicaAplicacion.Servicios;

namespace WebApi.Servicios
{
    /// <summary>
    /// Implementación temporal que no realiza ninguna operación.
    /// Se usa mientras se decide el mecanismo real de notificación.
    /// </summary>
    public class NotificacionServicioFalso : INotificacionServicio
    {
        private readonly ILogger<NotificacionServicioFalso> _logger;

        public NotificacionServicioFalso(ILogger<NotificacionServicioFalso> logger)
        {
            _logger = logger;
        }

        public Task NotificarObtencionMedallaAsync(string usuarioId, string nombreMedalla, int monedasGanadas)
        {
            _logger.LogInformation($"[FAKE NOTIFICATION] Usuario: {usuarioId}, Medalla: {nombreMedalla}, Monedas: {monedasGanadas}. (No se envió notificación real).");

            return Task.CompletedTask;
        }
    }
}
