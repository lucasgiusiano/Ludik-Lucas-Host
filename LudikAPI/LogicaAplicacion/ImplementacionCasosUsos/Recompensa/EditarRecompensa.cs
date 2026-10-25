using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaAplicacion.DTOsMappers.RecompensaMappers;
using LogicaAplicacion.InterfacesCasosUsos.Recompensa;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Recompensa
{
    public class EditarRecompensa:IEditarRecompensa
    {
        private readonly IRepositorioRecompensas _repositorioRecompensas;
        private readonly IRepositorioTiendas _repositorioTiendas;
        private readonly IRepositorioProfesores _repositorioProfesores;
        public EditarRecompensa(IRepositorioRecompensas repositorioRecompensas, IRepositorioTiendas repositorioTiendas, IRepositorioProfesores repositorioProfesores)
        {
            _repositorioRecompensas = repositorioRecompensas;
            _repositorioTiendas = repositorioTiendas;
            _repositorioProfesores = repositorioProfesores;
        }
        public async Task<Resultado> EjecutarAsync(string recompensaIdString, RecompensaSimpleEditarDto dto, string profesorId)
        {
           

            if (!int.TryParse(recompensaIdString, out int recompensaId))
                return Resultado.Falla(new Error("Error.InvalidId", $"ID de recompensa inválido: '{recompensaIdString}'."));

            var esDeProfesor = await _repositorioProfesores.EsRecompensaDeAsync(profesorId, recompensaId);
            if (!esDeProfesor)
                return Resultado.Falla(new Error("Error.Validation", "La recompensa no pertenece al profesor logueado."));

            var resultadoRecuperar = await _repositorioRecompensas.GetByIdAsync(recompensaId);
            if (resultadoRecuperar.EsFallo)
                return Resultado.Falla(new Error("Error.NotFound", "No se encontró la recompensa especificada."));
            RecompensaSimple recompensa = (RecompensaSimple)resultadoRecuperar.Valor;

            RecompensaEditarMapper.Update(recompensa, dto);

            var validacion = recompensa.esValido();
            if (validacion.EsFallo)
                return validacion;

            var resultadoUpdate = await _repositorioRecompensas.UpdateAsync(recompensa);
            if (resultadoUpdate.EsFallo)
                return Resultado.Falla(new Error("Error.Unexpected", "No se pudo actualizar la recompensa. "));

            return Resultado.Exitoso();
        }
    }
}

