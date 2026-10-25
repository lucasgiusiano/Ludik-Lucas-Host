using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class PerfilEstudianteRecompensa
    {
        public int Id { get; set; }

        public int PerfilEstudianteId { get; set; }
        public PerfilEstudiante PerfilEstudiante { get; set; }

        public int RecompensaId { get; set; }
        public Recompensa Recompensa { get; set; }
    }
}
