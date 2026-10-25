using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Estudiante
{
    public interface IObtenerRecompensasInventarioPerfil
    {
        Task<Resultado<List<RecompensaClienteDto>>> EjecutarAsync(int idPerfil,string idEstudiante);



    }
}
