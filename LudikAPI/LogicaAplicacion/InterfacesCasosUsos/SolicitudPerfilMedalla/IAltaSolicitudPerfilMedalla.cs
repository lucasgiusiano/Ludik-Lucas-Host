using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.SolicitudPerfilMedallaDTOs;
using LogicaAplicacion.DTOs.SolocitudUnionDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.SolicitudPerfilMedalla
{
    public interface IAltaSolicitudPerfilMedalla
    {
        Task<Resultado> EjecutarAsync(AltaSolicitudPerfilMedallaDto dto, string estudianteLogueado);
    }
}
