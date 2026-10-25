using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.TablaClasificacionDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.TablaClasificacion
{
    public interface IAltaTablaClasificacion
    {
        public Task<Resultado> EjecutarAsync(string profesorId, int grupoId, TablaClasificacionAltaDto dto);
    }
}
