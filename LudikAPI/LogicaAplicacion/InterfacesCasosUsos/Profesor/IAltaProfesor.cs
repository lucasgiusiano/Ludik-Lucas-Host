using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.ProfesorDTOs;
using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Identity;

namespace LogicaAplicacion.InterfacesCasosUsos.Profesor
{
    public interface IAltaProfesor
    {
        Task<Resultado> EjecutarAsync(ProfesorAltaDto profesorAltaDto);
    }
}
