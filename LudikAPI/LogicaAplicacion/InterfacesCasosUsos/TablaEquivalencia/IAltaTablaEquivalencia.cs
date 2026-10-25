using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.TablaEquivalenciaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.TablaEquivalencia
{
    public interface IAltaTablaEquivalencia
    {
        public Task<Resultado> EjecutarAsync(TablaEquivalenciaAltaDto tablaEquivalencia, string profesorId);
    }
}
