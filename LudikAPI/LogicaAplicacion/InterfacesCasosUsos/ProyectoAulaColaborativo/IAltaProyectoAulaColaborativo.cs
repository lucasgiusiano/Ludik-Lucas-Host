using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.ProyectoAulaColaborativoDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.ProyectoAulaColaborativo
{
    public interface IAltaProyectoAulaColaborativo
    {
        Task<Resultado> EjecutarAsync(int grupoId, AltaProyectoAulaColaborativoDto dto,string profesorId);
    }
}
