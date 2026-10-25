using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.TablaClasificacionDTOs
{
    public class ParticipanteTablaDto
    {
        public int PerfilEstudianteId { get; set; }
        public string NombreEstudiante { get; set; }  // o la propiedad que uses
        public int CantidadMedallas { get; set; }
    }
}
