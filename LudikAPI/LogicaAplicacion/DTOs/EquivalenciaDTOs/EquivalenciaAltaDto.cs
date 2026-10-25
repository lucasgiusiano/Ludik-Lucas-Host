using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.MedallaDTOs;

namespace LogicaAplicacion.DTOs.EquivalenciaDTOs
{
    public class EquivalenciaAltaDto
    {
        public int Nota { get; set; }
        public List<MedallaBasicaDto> MedallasNecesarias { get; set; }
    }
}
