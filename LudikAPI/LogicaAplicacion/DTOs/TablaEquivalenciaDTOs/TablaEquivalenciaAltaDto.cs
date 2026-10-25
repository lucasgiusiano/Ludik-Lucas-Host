using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.EquivalenciaDTOs;

namespace LogicaAplicacion.DTOs.TablaEquivalenciaDTOs
{
    public class TablaEquivalenciaAltaDto
    {
        public string Nombre { get; set; }
        public List<EquivalenciaAltaDto> Equivalencias { get; set; }
    }
}
