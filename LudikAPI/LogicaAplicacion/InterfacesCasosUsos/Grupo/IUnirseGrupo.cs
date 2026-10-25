using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.GrupoDTOs;

namespace LogicaAplicacion.InterfacesCasosUsos.Grupo
{
    public interface IUnirseGrupo
    {
        Task EjecutarAsync(UnirseGrupoDto dto);
    }
}
