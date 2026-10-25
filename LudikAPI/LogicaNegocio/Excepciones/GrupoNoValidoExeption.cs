using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Excepciones
{
    public class GrupoNoValidoExeption : Exception
    {
        public GrupoNoValidoExeption() { }
        public GrupoNoValidoExeption(string mensaje) : base(mensaje) { }
        public GrupoNoValidoExeption(string mensaje, Exception ex) : base(mensaje, ex) { }
    }
}
