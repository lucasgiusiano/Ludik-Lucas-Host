using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Recompensa
{
    public interface IAltaRecompensa
    {
        Task<Resultado> EjecutarAsync(RecompensaSimpleAltaDto recompensaRequestDto, string profesorId);
    }
}
