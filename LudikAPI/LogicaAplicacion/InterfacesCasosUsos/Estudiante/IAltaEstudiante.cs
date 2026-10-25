using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Estudiante
{
    public interface IAltaEstudiante
    {
        Task<Resultado> EjecutarAsync(EstudianteAltaDto estudianteAltaDto);
    }
}
