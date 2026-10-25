using LogicaNegocio.InterfacesEntidades;
using System.Security.Principal;

namespace LogicaNegocio.Entidades;

public class RecompensaProfesor : IEntity
{
    public int Id { get; set; }

    public string ProfesorId { get; set; }
    public Profesor Profesor { get; set; }

    public int RecompensaId { get; set; }
    public Recompensa Recompensa { get; set; }

    public DateTime FechaCreacion { get; set; }

}