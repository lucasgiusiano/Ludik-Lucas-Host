using System;
using System.Collections.Generic;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaAplicacion.DTOsMappers.MedallaMappers;
using LogicaAplicacion.InterfacesCasosUsos.Medalla;
using LogicaNegocio.Resultados;


namespace LogicaAplicacion.ImplementacionCasosUsos.Medallas
{
    public class AltaMedalla: IAltaMedalla
    {
        private readonly IRepositorioMedallas _repositorioMedallas;

        public AltaMedalla(IRepositorioMedallas repositorioMedallas)
        {
            _repositorioMedallas = repositorioMedallas;
        }
        //Pre: Recibe un DTO con los datos de la medalla a crear
        //Pos: Se crea una medalla en la base de datos
        public async Task<Resultado> EjecutarAsync(MedallaAltaDto medallaAltaDto, string profesorId)
        {

            if (medallaAltaDto == null)
                return Resultado.Falla(new Error("Error.Validation", "Los datos para crear la medalla no pueden ser nulos."));

            Medalla medallaNueva = MedallaAltaMapper.fromDto(medallaAltaDto);

            medallaNueva.ProfesorId = profesorId;

            var resultadoValidacion = medallaNueva.esValido();
            if (resultadoValidacion.EsFallo)
                return resultadoValidacion;

            Resultado resultadoCreacion = await _repositorioMedallas.AddAsync(medallaNueva);

            return resultadoCreacion;
        }
    }
}
