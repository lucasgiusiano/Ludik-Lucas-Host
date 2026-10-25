using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.ValueObject;

namespace LogicaAplicacion.DTOs.ProyectoAulaColaborativoDTOs
{
    public class AltaProyectoAulaColaborativoDto
    {
        public string Nombre { get; set; } 
        public int CantidadMedallasNecesarias { get; set; }
        public int RecompensaClaseId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
