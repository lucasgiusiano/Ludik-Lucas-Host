using LogicaNegocio.InterfacesEntidades;

namespace LogicaNegocio.Entidades;

public class KudoOtorgado : IEntity
{
    public int Id { get; set; }

    public int PerfilEstudianteEmisorId { get; set; }
    public PerfilEstudiante Emisor { get; set; }

    public int PerfilEstudianteReceptorId { get; set; }
    public PerfilEstudiante Receptor { get; set; }

    public int TipoKudoId { get; set; }
    public TipoKudo TipoKudo { get; set; }

    public DateTime FechaOtorgamiento { get; set; }

    // Si este kudo fue usado para obtener una medalla, guardamos el ID de esa asignación
    // Es nulable porque un kudo recién otorgado aún no ha sido utilizado
    public int? PerfilEstudianteMedallaId { get; private set; }
    public PerfilEstudianteMedalla AsignacionMedalla { get; set; }
    private KudoOtorgado() { } 

    public KudoOtorgado(PerfilEstudiante emisor, PerfilEstudiante receptor, TipoKudo tipo, DateTime fecha)
    {
        Emisor = emisor;
        Receptor = receptor;
        TipoKudo = tipo;
        FechaOtorgamiento = fecha;
    }
    public void MarcarComoUsadoPara(PerfilEstudianteMedalla asignacion)
    {
        AsignacionMedalla = asignacion;
        PerfilEstudianteMedallaId = asignacion.Id;
    }
}