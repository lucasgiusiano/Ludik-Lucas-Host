using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class Avatar
    {
        public int Id { get; set; }

        // Atributos generales que no son "partes" intercambiables
        public string? ColorFondo { get; set; }
        public bool Voltear { get; set; }
        public int Rotacion { get; set; }
        public int Zoom { get; set; }
        public int PerfilEstudianteId { get; set; }
        public virtual ICollection<AtributoAvatar> AtributosSeleccionados { get; set; } = new List<AtributoAvatar>();

        public AtributoAvatar ObtenerAtributo(TipoAtributo tipo)
        {
            return AtributosSeleccionados.FirstOrDefault(a => a.Tipo == tipo);
        }
    }
}
