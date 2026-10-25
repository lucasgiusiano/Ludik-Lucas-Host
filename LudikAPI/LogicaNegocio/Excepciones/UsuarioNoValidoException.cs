using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Excepciones
{
    public class UsuarioNoValidoException : Exception
    {
        public UsuarioNoValidoException() { }
        public UsuarioNoValidoException(string mensaje) : base(mensaje) { }
        public UsuarioNoValidoException(string mensaje, Exception ex) : base(mensaje, ex) { }
    }
}
