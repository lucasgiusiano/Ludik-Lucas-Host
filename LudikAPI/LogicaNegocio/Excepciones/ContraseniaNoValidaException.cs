using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Excepciones
{
    public class ContraseniaNoValidaException: Exception
    {
        public ContraseniaNoValidaException() { }
        public ContraseniaNoValidaException(string mensaje) : base(mensaje) { }
        public ContraseniaNoValidaException(string mensaje, Exception ex) : base(mensaje, ex) { }
    }
}
