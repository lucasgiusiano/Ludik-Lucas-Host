using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.GrupoDTOs
{
    public class GrupoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ProfesorId { get; set; }
        public string Institucion { get; set; }
        public string Materia { get; set; }
        public int CantAlumnos { get; set; }
    }
}
