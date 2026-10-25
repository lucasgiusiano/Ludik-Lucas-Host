using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.SolicitudPerfilMedallaDTOs
{
    public class SolicitudPerfilMedallaDto
    {
        public int Id { get; set; }
        public int PerfilEstudianteId { get; set; }
        public int MedallaId { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
    }
}
