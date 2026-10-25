using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.InterfacesEntidades;

namespace LogicaAplicacion.DTOs.RecompensaDTOs
{
    public class RecompensaSimpleAltaDto
    {
        public string Nombre { get; set; }
        public string NombreIcono { get; set; }
        public int Precio { get; set; }
    }
}
