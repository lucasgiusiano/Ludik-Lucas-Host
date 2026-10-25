using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.InterfacesCasosUsos.Estudiante;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Estudiantes
{
    public class CanjearRecompensa:ICanjearRecompensa
    {
        private readonly IRepositorioRecompensas _repositorioRecompensas;
        private readonly IRepositorioPerfilEstudianteGrupo _repositorioPerfilEstudianteGrupo;
        private readonly IRepositorioPerfilEstudianteRecompensa _repositorioPerfilEstudianteRecompensa;

        public CanjearRecompensa(
            IRepositorioRecompensas repositorioRecompensas,
            IRepositorioPerfilEstudianteGrupo repositorioPerfilEstudianteGrupo,IRepositorioPerfilEstudianteRecompensa repositorioPerfilEstudianteRecompensa)
        {
            _repositorioRecompensas = repositorioRecompensas;
            _repositorioPerfilEstudianteGrupo = repositorioPerfilEstudianteGrupo;
            _repositorioPerfilEstudianteRecompensa = repositorioPerfilEstudianteRecompensa;
        }

        public async Task<Resultado> EjecutarAsync(int recompensaId, int perfilEstudianteID,string estudianteId)
        {
            var resultadoRecuperarRecompensa = await _repositorioRecompensas.GetByIdAsync(recompensaId);
            if (resultadoRecuperarRecompensa.EsFallo)
                return Resultado.Falla(new Error("Error.NotFound", "No se encontró la recompensa especificada."));
            var recompensa = resultadoRecuperarRecompensa.Valor!;

            var resultadoRecuperarPerfil = await _repositorioPerfilEstudianteGrupo.GetByIdAsync(perfilEstudianteID);
            if (resultadoRecuperarPerfil.EsFallo)
                return Resultado.Falla(new Error("Error.NotFound", "No se encontró el perfil del estudiante especificado."));
            var perfil = resultadoRecuperarPerfil.Valor!;

            if (perfil.Monedas < recompensa.Precio)
                return Resultado.Falla(new Error("Error.Validation", "El estudiante no tiene suficientes puntos."));

            if(perfil.EstudianteId != estudianteId)
                return Resultado.Falla(new Error("Error.Forbidden", "No tienes permiso para canjear recompensas en este perfil."));

            perfil.Monedas -= recompensa.Precio;

            var perfilYRecompensa = new PerfilEstudianteRecompensa
            {
                PerfilEstudianteId = perfil.Id,
                RecompensaId = recompensa.Id
            };

            var resultadoAdd =
                await _repositorioPerfilEstudianteRecompensa
                      .AddAsync(perfilYRecompensa);
            if (resultadoAdd.EsFallo)
                return Resultado.Falla(
                    new Error("Error.Database",
                              "No se pudo agregar la recompensa al perfil."));

            var resultadoUpd =
                await _repositorioPerfilEstudianteGrupo
                      .UpdateAsync(perfil);
            if (resultadoUpd.EsFallo)
                return Resultado.Falla(
                    new Error("Error.Database",
                              "No se pudo actualizar el perfil del estudiante."));

            return Resultado.Exitoso();
        }
    }
}
