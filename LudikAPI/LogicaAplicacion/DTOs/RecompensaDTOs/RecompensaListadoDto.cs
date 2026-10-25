using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.RecompensaDTOs
{
    public class RecompensaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Precio { get; set; }
        public DatosVisuales Representacion { get; set; }
        public TipoVisual Tipo { get; set; }
    }
}
