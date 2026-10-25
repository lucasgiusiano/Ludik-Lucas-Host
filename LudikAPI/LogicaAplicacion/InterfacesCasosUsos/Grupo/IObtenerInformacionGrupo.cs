using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Grupo
{
    public interface IObtenerInformacionGrupo
    {
        Task<Resultado<GrupoInformacionDto>> EjecutarAsync(int grupoId,string profesor);
    }
}
