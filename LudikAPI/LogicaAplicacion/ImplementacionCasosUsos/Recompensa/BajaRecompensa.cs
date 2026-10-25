using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.InterfacesCasosUsos.Recompensa;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Recompensa
{
    public class BajaRecompensa : IBajaRecompensa
    {
        private readonly IRepositorioRecompensas _repositorioRecompensas;
        private readonly IRepositorioPerfilEstudianteRecompensa _repositorioPerfilEstudianteRecompensa;
        public BajaRecompensa(IRepositorioRecompensas repositorioRecompensas,IRepositorioPerfilEstudianteRecompensa repositorioPerfilEstudianteRecompensa)
        {
            _repositorioRecompensas = repositorioRecompensas;
            _repositorioPerfilEstudianteRecompensa = repositorioPerfilEstudianteRecompensa;

        }
        public async Task<Resultado> EjecutarAsync(string recompensaIdString, string profesorId)
        {
            if (!int.TryParse(recompensaIdString, out int recompensaId))
                return Resultado.Falla(new Error("Error.InvalidId", $"ID de recompensa inválido: '{recompensaIdString}'"));

            // 1) Verificar si la recompensa fue canjeada
            var fueCanjeada = await _repositorioPerfilEstudianteRecompensa.FueCanjeadaAsync(recompensaId);
            if (fueCanjeada)
                return Resultado.Falla(new Error("Error.Validation", "No se puede eliminar la recompensa porque ya fue canjeada por al menos un estudiante."));

            var resultadoRecuperar = await _repositorioRecompensas.GetByIdAsync(recompensaId);
            if (resultadoRecuperar.EsFallo)
                return Resultado.Falla(new Error("Error.NotFound", "No se encontró la recompensa especificada."));

            var recompensa = resultadoRecuperar.Valor!;
            
            //deberia validar que la recompensa pertenezca a una tienda de un grupo del profesor , el profesor asi no podria borrar recompensas con ID que no esten en su grupo
            
            var resultadoEliminar = await _repositorioRecompensas.RemoveAsync(recompensaId);
            if (resultadoEliminar.EsFallo)
                return Resultado.Falla(new Error("Error.Unexpected", "No se pudo eliminar la recompensa. " ));

            return Resultado.Exitoso();
        }
    }
}
