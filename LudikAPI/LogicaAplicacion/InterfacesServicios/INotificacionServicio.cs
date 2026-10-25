namespace LogicaAplicacion.Servicios;

public interface INotificacionServicio
{
    Task NotificarObtencionMedallaAsync(string estudianteId, string nombreMedalla, int monedasGanadas);
}