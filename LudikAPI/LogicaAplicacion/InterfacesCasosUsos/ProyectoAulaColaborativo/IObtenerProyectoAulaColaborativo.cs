using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.ProyectoAulaColaborativoDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.ProyectoAulaColaborativo
{
    public interface IObtenerProyectoAulaColaborativo
    {
        Task<Resultado<ProyectoAulaColaborativoDto>> EjecutarAsync(int grupoId);
    }
}
