using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.PerfilEstudianteDTO;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.PerfilEstudiante
{
    public interface IObtenerPerfilesPorGrupo
    {
        public Task<Resultado<List<PerfilEstudianteInformacionDto>>> EjecutarAsync(int grupoId,string profesorId);
    }
}
