using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.SolocitudUnionDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.SolicitudUnion
{
    public interface ICrearSolicitudUnion
    {
        Task<Resultado> EjecutarAsync(SolicitudUnionDto dto);
    }
}
