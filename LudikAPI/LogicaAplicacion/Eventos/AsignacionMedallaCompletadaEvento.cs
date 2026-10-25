using LogicaNegocio.Entidades;
using MediatR;

namespace LogicaAplicacion.Eventos;

public class AsignacionMedallaCompletadaEvento : INotification
{
    public int PerfilEstudianteId { get; }
    public int MedallaId { get; }
    public Estudiante Estudiante { get; }

    public AsignacionMedallaCompletadaEvento(int perfilEstudianteId, int medallaId, Estudiante estudiante)
    {
        PerfilEstudianteId = perfilEstudianteId;
        MedallaId = medallaId;
        Estudiante = estudiante;
    }
}