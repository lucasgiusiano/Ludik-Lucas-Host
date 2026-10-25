using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Recompensa
{
    public interface IBajaRecompensa
    {
        Task<Resultado> EjecutarAsync(string recompensaIdString, string profesorId);
    }
}
