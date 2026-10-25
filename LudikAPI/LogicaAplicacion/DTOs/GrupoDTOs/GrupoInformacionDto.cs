using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.GrupoDTOs
{
    public class GrupoInformacionDto
    {
        public string Nombre { get; set; }
        public int TablaEquivalenciaId { get; set; }
		public int TablaEquivalenciaNotaMaxima { get; set; }
		public string ProfesorId { get; set; }
        public string? Institucion { get; set; }
        public string? Materia { get; set; }
        public DateTime fCreacion { get; set; }
        public string? UrlCompleta { get; set; }
        public int IdTienda { get; set; }
	}
}
