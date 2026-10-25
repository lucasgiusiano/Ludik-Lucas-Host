using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Recompensa
{
    public interface IEditarRecompensa
    {
        Task<Resultado> EjecutarAsync(string recompensaId, RecompensaSimpleEditarDto dto, string profesorId);
    }
}
