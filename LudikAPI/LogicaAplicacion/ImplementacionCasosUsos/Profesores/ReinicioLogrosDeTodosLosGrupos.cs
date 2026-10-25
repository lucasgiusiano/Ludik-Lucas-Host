using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.InterfacesCasosUsos.Profesor;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Profesores
{
    public class ReinicioLogrosDeTodosLosGrupos : IReinicioLogrosDeTodosLosGrupos
    {
        private readonly IRepositorioGrupos _repoGrupos;
        private readonly IRepositorioRendimientoPeriodos _repoRendimientos;
        private readonly IRepositorioPerfilEstudianteGrupo _repoPerfiles;

        public ReinicioLogrosDeTodosLosGrupos(
            IRepositorioGrupos repoGrupos,
            IRepositorioRendimientoPeriodos repoRendimientos,
            IRepositorioPerfilEstudianteGrupo repoPerfiles)
        {
            _repoGrupos = repoGrupos;
            _repoRendimientos = repoRendimientos;
            _repoPerfiles = repoPerfiles;
        }

        public async Task<Resultado> EjecutarAsync(string profesorId)
        {
            var gruposRes = await _repoGrupos.obtenerGruposPorProfesorAsync(profesorId);
            if (gruposRes.EsFallo)
                return Resultado.Falla(gruposRes.Errores);

            var grupos = gruposRes.Valor;
            if (!grupos.Any())
                return Resultado.Falla(new Error("Error.Validation",
                    $"El profesor {profesorId} no tiene grupos registrados."));

            foreach (var grupo in grupos)
            {
                var ahora = DateTime.Now;
                var desde = grupo.FechaUltimoReinicio ?? grupo.FCreacion;

                var rendimientos = grupo.ReiniciarMedallasEstudiantes(desde, ahora);

                foreach (var rendimiento in rendimientos)
                {
                    var addRes = await _repoRendimientos.AddAsync(rendimiento);
                    if (addRes.EsFallo)
                        return Resultado.Falla(addRes.Errores);
                }

                grupo.FechaUltimoReinicio = ahora;
                var updRes = await _repoGrupos.UpdateAsync(grupo);
                if (updRes.EsFallo)
                    return Resultado.Falla(updRes.Errores);
            }

            var saveRes = await _repoPerfiles.SaveCambiosAsync();
            if (saveRes.EsFallo)
                return Resultado.Falla(saveRes.Errores);

            return Resultado.Exitoso();
        }
    }
}
