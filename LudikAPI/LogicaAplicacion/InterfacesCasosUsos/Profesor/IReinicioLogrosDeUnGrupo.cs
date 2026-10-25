using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Profesor
{
    public interface IReinicioLogrosDeUnGrupo
    {
        Task<Resultado> EjecutarAsync(int grupoId,string idProfesor);
    }
}
