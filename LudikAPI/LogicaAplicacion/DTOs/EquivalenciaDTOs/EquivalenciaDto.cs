using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.MedallaDTOs;

namespace LogicaAplicacion.DTOs.EquivalenciaDTOs
{
    public class EquivalenciaDto
    {
        public int Id { get; set; }
        public int Nota { get; set; }
        public List<MedallaDto> MedallasNecesarias { get; set; }
    }
}
