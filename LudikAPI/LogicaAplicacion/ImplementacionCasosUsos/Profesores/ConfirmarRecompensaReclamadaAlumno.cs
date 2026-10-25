using System;
using System.Threading.Tasks;
using LogicaAplicacion.InterfacesCasosUsos.Profesor;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.ImplementacionCasosUsos.Profesores
{
    public class ConfirmarRecompensaReclamadaAlumno : IConfirmarRecompensaReclamadaAlumno
    {
        private readonly IRepositorioPerfilEstudianteRecompensa _repositorioPerfilRecompensa;

        public ConfirmarRecompensaReclamadaAlumno(
            IRepositorioPerfilEstudianteRecompensa repositorioPerfilRecompensa)
        {
            _repositorioPerfilRecompensa = repositorioPerfilRecompensa;
        }

        public async Task<Resultado> EjecutarAsync(string perfilEstudianteId, string recompensaId)
        {
            if (!int.TryParse(perfilEstudianteId, out int perfilId) || !int.TryParse(recompensaId, out int recompensaIdInt))
            {
                return Resultado.Falla(new Error("Error.Validation", "IDs de perfil o recompensa no válidos."));
            }

            var resultadoRelacion = await _repositorioPerfilRecompensa.GetByPerfilAndRecompensaIdAsync(perfilId, recompensaIdInt);

            if (resultadoRelacion.EsFallo)
            {
                return Resultado.Falla(resultadoRelacion.Errores);
            }

            if (resultadoRelacion == null)
            {
                return Resultado.Falla(new Error("Error.NotFound",
                    $"La recompensa {recompensaIdInt} no se encontró en el inventario del perfil {perfilId}."));
            }

            return await _repositorioPerfilRecompensa.RemoveAsync(resultadoRelacion.Valor);
        }
    }
}
