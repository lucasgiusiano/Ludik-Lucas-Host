using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Medalla
{
    public interface IObtenerMedallasProfesorPorGrupo
    {
        Task<Resultado<IEnumerable<MedallaDto>>> EjecutarAsync(int grupoId);
    }
}
