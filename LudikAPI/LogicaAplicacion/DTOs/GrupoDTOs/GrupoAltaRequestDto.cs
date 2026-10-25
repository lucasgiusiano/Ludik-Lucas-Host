using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.GrupoDTOs
{
    public class GrupoAltaRequestDto
    {
        public string Nombre { get; set; }
        public int TablaEquivalenciaId { get; set; }
        public string? Institucion { get; set; }
        public string? Materia { get; set; }
    }
}
