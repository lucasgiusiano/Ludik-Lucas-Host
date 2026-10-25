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
    public class ReinicioLogrosDeUnGrupo:IReinicioLogrosDeUnGrupo
    {
        private readonly IRepositorioGrupos _repoGrupos;
        private readonly IRepositorioRendimientoPeriodos _repoRendimientos;
        private readonly IRepositorioPerfilEstudianteGrupo _repoPerfiles;

        public ReinicioLogrosDeUnGrupo(
            IRepositorioGrupos repoGrupos,
            IRepositorioRendimientoPeriodos repoRendimientos,
            IRepositorioPerfilEstudianteGrupo repoPerfiles)
        {
            _repoGrupos = repoGrupos;
            _repoRendimientos = repoRendimientos;
            _repoPerfiles = repoPerfiles;
        }
        public async Task<Resultado> EjecutarAsync(int grupoId, string profesorId)
        {
            var grupoRes = await _repoGrupos.GetByIdAsync(grupoId);
            if (grupoRes.EsFallo) return Resultado.Falla(grupoRes.Errores);

            var grupo = grupoRes.Valor;
            if (grupo.ProfesorId != profesorId)
                return Resultado.Falla(new Error("Error.Validation", "Sin permiso."));

            var ahora = DateTime.Now;
            var desde = grupo.FechaUltimoReinicio ?? grupo.FCreacion;

            var rendimientos = grupo.ReiniciarMedallasEstudiantes(desde, ahora);
            foreach (var r in rendimientos)
            {
                var addRes = await _repoRendimientos.AddAsync(r);
                if (addRes.EsFallo) return Resultado.Falla(addRes.Errores);
            }

            grupo.FechaUltimoReinicio = ahora;
            var updRes = await _repoGrupos.UpdateAsync(grupo);
            if (updRes.EsFallo) return Resultado.Falla(updRes.Errores);

            var saveRes = await _repoPerfiles.SaveCambiosAsync();
            if (saveRes.EsFallo) return Resultado.Falla(saveRes.Errores);

            return Resultado.Exitoso();
        }
    }
}
