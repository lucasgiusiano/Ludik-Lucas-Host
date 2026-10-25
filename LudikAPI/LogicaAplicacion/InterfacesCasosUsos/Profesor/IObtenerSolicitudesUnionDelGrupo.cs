using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.DTOs.SolocitudUnionDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Profesor
{
    public interface IObtenerSolicitudesUnionDelGrupo
    {
        Task<Resultado<List<SolicitudUnionListadoDto>>> EjecutarAsync(int grupoId, string profesorId);
    }
}
