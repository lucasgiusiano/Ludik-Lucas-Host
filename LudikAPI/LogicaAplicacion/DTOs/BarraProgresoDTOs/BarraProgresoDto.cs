using LogicaAplicacion.DTOs.MedallaDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.BarraProgresoDTOs
{
    public class BarraProgresoDto
    {
        public int CalificacionActual { get; set; }
        public int CalificacionMaxima { get; set; }
        public int CalificacionMinima { get; set; }
        public List<MedallaBasicaDto> MedallasNecesariasParaSiguienteNota { get; set; }
    }
}
