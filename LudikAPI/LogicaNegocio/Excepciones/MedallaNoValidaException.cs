using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Excepciones
{
    public class MedallaNoValidaException: Exception
    {
        public MedallaNoValidaException() { }
        public MedallaNoValidaException(string mensaje) : base(mensaje) { }
        public MedallaNoValidaException(string mensaje, Exception ex) : base(mensaje, ex) { }
    }
}
