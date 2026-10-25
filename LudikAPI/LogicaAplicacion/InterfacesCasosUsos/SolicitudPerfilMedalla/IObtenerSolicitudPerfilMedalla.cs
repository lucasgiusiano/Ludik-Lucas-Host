using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.SolicitudPerfilMedallaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.SolicitudPerfilMedalla
{
    public interface IObtenerSolicitudPerfilMedalla
    {
        Task<Resultado<List<SolicitudPerfilMedallaDto>>> EjecutarAsync(int grupoId,string profesorId);
    }
}
