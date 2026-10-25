using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.AvatarDTOs
{
    public class ActualizarAvatarDto
    {
        public string? ColorFondo { get; set; }
        public bool Voltear { get; set; }
        public int Rotacion { get; set; }
        public int Zoom { get; set; }

        public List<int> AtributosIds { get; set; } = new List<int>();
    }
}
