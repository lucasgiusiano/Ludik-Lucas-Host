using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class EstudiantePotenciador
    {
        public int Id { get; set; }                       
        public string EstudianteId { get; set; }           
        public int PotenciadorId { get; set; }
        public DateTime FechaActivacion { get; set; }

        public Estudiante Est { get; set; }               
        public Potenciador Potenciador { get; set; }

        [NotMapped]
        public bool EstaActivo =>
    DateTime.UtcNow >= FechaActivacion &&
    DateTime.UtcNow <= FechaActivacion + TimeSpan.FromHours(Potenciador.DuracionHoras);

    }
}
