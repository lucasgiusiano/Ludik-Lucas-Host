using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.TablaClasificacion
{
    public interface IBajaTablaClasificacion
    {
        Task<Resultado> EjecutarAsync(int tablaId,string profesorId);
    }
}
