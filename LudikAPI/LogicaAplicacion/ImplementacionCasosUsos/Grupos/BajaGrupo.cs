using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.InterfacesCasosUsos.Grupo;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Grupos
{
    public class BajaGrupo: IBajaGrupo
    {
        private readonly IRepositorioGrupos _repositorioGrupo;
        public BajaGrupo(IRepositorioGrupos repo)
        {
            _repositorioGrupo = repo;
        }

        public async Task<Resultado> EjecutarAsync(int grupoId, string profesorId)
        {
            var resultado = await _repositorioGrupo.GetByIdAsync(grupoId);
            if (resultado.EsFallo)
               return Resultado.Falla(new Error("Error.Validation", "El grupo no existe."));

            if (resultado.Valor.ProfesorId != profesorId)
                return Resultado.Falla(new Error("Error.Validation", "No tiene permiso para eliminar este grupo."));

            await _repositorioGrupo.RemoveAsync(grupoId);
            return Resultado.Exitoso();
        }
    }
}
