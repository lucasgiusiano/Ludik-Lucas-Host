using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.SolicitudPerfilMedalla
{
    public interface IRechazarSolicitudPerfilMedalla
    {
        Task<Resultado> EjecutarAsync(int idSolicitudPerfil, string profesorId);
    }
}
