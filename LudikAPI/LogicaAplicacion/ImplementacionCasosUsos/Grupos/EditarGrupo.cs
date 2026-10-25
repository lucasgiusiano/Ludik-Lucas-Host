using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.DTOsMappers.GrupoMappers;
using LogicaAplicacion.InterfacesCasosUsos.Grupo;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Grupos
{
    public class EditarGrupo : IEditarGrupo
    {
        private readonly IRepositorioGrupos _repositorioGrupo;
        private readonly IRepositorioTablasEquivalencia _repositorioTablaEquivalencia;
        private readonly IRepositorioProfesores _repositorioProfesores;


        public EditarGrupo(IRepositorioGrupos repo, IRepositorioTablasEquivalencia repositorioTablaEquivalencia, IRepositorioProfesores repositorioProfesores)
        {
            _repositorioGrupo = repo;
            _repositorioTablaEquivalencia = repositorioTablaEquivalencia;
            _repositorioProfesores = repositorioProfesores;
        }
        public async Task<Resultado> EjecutarAsync(GrupoEditarDto grupoDto, string profesorId)
        {
            if (grupoDto == null)
                return Resultado.Falla(new Error("Error.Validation", "No hay informacion sobre el grupo."));

            var resultado = await _repositorioGrupo.GetByIdAsync(grupoDto.Id);
            if (resultado.Valor == null)
               return Resultado.Falla(new Error("Error.Validation", "No se encontró el grupo especificado."));

            if (resultado.Valor.ProfesorId != profesorId)
                throw new UnauthorizedAccessException("No tiene permiso para editar este grupo.");

            if (grupoDto.TablaEquivalenciaId == 0)
                return Resultado.Falla(new Error("Error.Validation", "Debe seleccionar una tabla de equivalencia válida."));

            var tablaEquivalencia = await _repositorioTablaEquivalencia.GetByIdAsync(grupoDto.TablaEquivalenciaId);
            if (tablaEquivalencia.EsFallo || tablaEquivalencia.Valor == null)
                return Resultado.Falla(new Error("Error.Validation", "La tabla de equivalencia especificada no existe."));

            var resultadoProfesor = await _repositorioProfesores.GetByStringIdAsync(profesorId);
            var profesor = resultadoProfesor.Valor;
            if (!profesor.TablasEquivalencia!.Any(g => g.Id == tablaEquivalencia.Valor.Id))
            {
                return Resultado.Falla(new Error("Error.Unauthorized", "La tabla no se encuentra dentro de las tablas del profesor logueado."));
            }

            GrupoEditarDtoMapper.UpdateFromDto(grupoDto, resultado.Valor);
            var resultadoValidacion = resultado.Valor.esValido();
            if (resultadoValidacion.EsFallo)
                return resultadoValidacion;
            await _repositorioGrupo.UpdateAsync(resultado.Valor);
            return Resultado.Exitoso();
        }
    }
}
