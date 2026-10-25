using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Estudiante
{
    public interface ICanjearRecompensa
    {
        Task<Resultado> EjecutarAsync(int recompensaId,int perfilEstudianteId,string estudianteId);
    }
}
