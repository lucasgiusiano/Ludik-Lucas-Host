using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.DTOs.ProfesorDTOs;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Http;

namespace LogicaAplicacion.InterfacesCasosUsos.Grupo
{
    public interface IAltaGrupo
    {
        Task<Resultado> EjecutarAsync(GrupoAltaRequestDto grupoRequestDto, string profesorId);

    }
}
