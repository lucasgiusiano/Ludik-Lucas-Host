using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.TablaEquivalenciaDTOs;
using LogicaAplicacion.InterfacesCasosUsos.TablaEquivalencia;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.TablaEquivalencia
{
    public class EditarTablaEquivalencia: IEditarTablaEquivalencia
    {
        private readonly IRepositorioTablasEquivalencia _repositorioTablasEquivalencia;
        private readonly IRepositorioMedallas _repositorioMedallas;

        public EditarTablaEquivalencia(
            IRepositorioTablasEquivalencia repositorioTablasEquivalencia,
            IRepositorioMedallas repositorioMedallas)
        {
            _repositorioTablasEquivalencia = repositorioTablasEquivalencia;
            _repositorioMedallas = repositorioMedallas;
        }

        public async Task<Resultado> EjecutarAsync(TablaEquivalenciaDto tablaDto, string profesorId)
        {
            var resultadoTabla = await _repositorioTablasEquivalencia.GetByIdAsync(tablaDto.Id);
            if (resultadoTabla.EsFallo)
                return Resultado.Falla(Error.NotFound);
            
            var tablaExistente = resultadoTabla.Valor;

            if (tablaExistente.ProfesorId != profesorId)
                return Resultado.Falla(Error.Forbidden); 

            var idsMedallasDto = tablaDto.Equivalencias
                .SelectMany(e => e.MedallasNecesarias.Select(m => m.Id))
                .Distinct().ToList();

            var resultadoMedallas = await _repositorioMedallas.FindByIdsAsync(idsMedallasDto);

            if (resultadoMedallas.EsFallo || resultadoMedallas.Valor.Count() != idsMedallasDto.Count)
                return Resultado.Falla(new Error("Error.NotFound", "Una o más medallas especificadas no existen."));
            
            var medallasMap = resultadoMedallas.Valor.ToDictionary(m => m.Id);

            var nuevasEquivalencias = tablaDto.Equivalencias
                .Select(dto => new Equivalencia(
                    dto.Nota,
                    dto.MedallasNecesarias.Select(mDto => medallasMap[mDto.Id]).ToList()
                )).ToList();

            tablaExistente.Actualizar(tablaDto.Nombre, nuevasEquivalencias);

            var resultadoValidacion = tablaExistente.esValido();
            if (resultadoValidacion.EsFallo)
                return resultadoValidacion;
            
            return await _repositorioTablasEquivalencia.UpdateAsync(tablaExistente);
        }
    }
}
