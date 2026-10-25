using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaAplicacion.DTOsMappers.RecompensaMappers;
using LogicaAplicacion.InterfacesCasosUsos.Recompensa;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Recompensa
{
    public class AltaRecompensa:IAltaRecompensa
    {
        private readonly IRepositorioProfesores _repositorioProfesores;

        public AltaRecompensa(IRepositorioProfesores repositorioProfesores)
        {
            _repositorioProfesores = repositorioProfesores;
        }

        public async Task<Resultado> EjecutarAsync(RecompensaSimpleAltaDto recompensaDto, string profesorId)
        {

            if (recompensaDto == null)
                return Resultado.Falla(new Error("Error.Validation", "No hay información para poder dar de alta la recompensa."));
            

            var recompensa = RecompensaSimpleAltaMapper.FromDto(recompensaDto);
            var resultadoValidacion = recompensa.esValido();

            if (resultadoValidacion.EsFallo)
                return resultadoValidacion;

            var resultadoProfesor = await _repositorioProfesores.GetByStringIdAsync(profesorId);

            if (resultadoProfesor.EsFallo)
                return Resultado.Falla(Error.NotFound);
            
            var profesor = resultadoProfesor.Valor;
            var resultadoAsignarRecompensa = profesor.CrearRecompensa(recompensa);

            if (resultadoAsignarRecompensa.EsFallo)
                return Resultado.Falla(resultadoAsignarRecompensa.Errores);
            
            
            return await _repositorioProfesores.UpdateAsync(profesor);
        }
    }
}
