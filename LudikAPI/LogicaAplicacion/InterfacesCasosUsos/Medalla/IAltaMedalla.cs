using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Medalla
{
    public interface IAltaMedalla
    {
        Task<Resultado> EjecutarAsync(MedallaAltaDto medallaAltaDto, string profesorId);


    }
}
