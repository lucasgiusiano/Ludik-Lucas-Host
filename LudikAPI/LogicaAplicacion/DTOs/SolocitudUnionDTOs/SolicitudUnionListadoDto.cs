using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.SolocitudUnionDTOs
{
    public class SolicitudUnionListadoDto
    {
        public int IdSolicitud { get; set; }
        public string IdEstudiante { get; set; }
        public string NombreEstudiante { get; set; }
        public DateOnly fecha { get; set; }
    }
}
