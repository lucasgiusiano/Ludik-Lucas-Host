using System.ComponentModel.DataAnnotations.Schema;
using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;

namespace LogicaNegocio.Entidades
{
	public class SolicitudUnion : IEntity, IValidable
    {
        public int Id { get; set; }

        public string EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }

        public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;

        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }
        public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        public SolicitudUnion()
        {
            
        }
        public Resultado esValido()
        {
            throw new NotImplementedException();
        }
    }

}

