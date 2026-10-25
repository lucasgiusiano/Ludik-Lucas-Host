using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.PerfilEstudianteDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Profesor
{
    public interface IObtenerRecompensasReclamadasAlumnos
    {
        Task<Resultado<List<PerfilEstudianteRecompensaProfesorDto>>> EjecutarAsync(string grupoId);

    }
}
