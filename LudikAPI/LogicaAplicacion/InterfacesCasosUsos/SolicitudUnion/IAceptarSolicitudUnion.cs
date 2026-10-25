using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.SolocitudUnionDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.SolicitudUnion
{
    public interface IAceptarSolicitudUnion
    {
        Task<Resultado> EjecutarAsync(int idSolicitud,string profesorId);
    }
}
