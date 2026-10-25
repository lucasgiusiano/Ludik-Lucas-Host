using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.InterfacesCasosUsos.AsignacionMedalla;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.AsignarMedalla
{
    public class QuitarMedalla: IQuitarMedalla
    {
        private readonly IRepositorioPerfilEstudianteGrupo _repositorioPerfilEstudiantes;
        private readonly IRepositorioProfesores _repositorioProfesores;
        private readonly IRepositorioPerfilEstudianteMedalla _repositorioPerfilEstudianteMedalla;
        public QuitarMedalla(
            IRepositorioPerfilEstudianteGrupo repositorioPerfilEstudiante,
            IRepositorioProfesores repositorioProfesor,IRepositorioPerfilEstudianteMedalla repositorioPerfilEstudianteMedalla)
        {
            _repositorioPerfilEstudiantes = repositorioPerfilEstudiante;
            _repositorioProfesores = repositorioProfesor;
            _repositorioPerfilEstudianteMedalla = repositorioPerfilEstudianteMedalla;
        }

        public async Task<Resultado> EjecutarAsync(string idProfesor, int idPerfilEstudiante, int idMedalla)
        {
            var profesorResultado = await _repositorioProfesores.GetByStringIdAsync(idProfesor);
            var perfilResultado = await _repositorioPerfilEstudiantes.GetByIdAsync(idPerfilEstudiante);
            if (profesorResultado.EsFallo || perfilResultado.EsFallo)
                return Resultado.Falla(Error.NotFound);

            var profesor = profesorResultado.Valor;
            var perfil = perfilResultado.Valor;
            if (!profesor.Grupos.Any(g => g.Id == perfil.GrupoId))
                return Resultado.Falla(Error.Forbidden);

            var existenteResultado = await _repositorioPerfilEstudianteMedalla
                .GetByPerfilYMedallaAsync(perfil.Id, idMedalla);


            if (existenteResultado.EsFallo)
            {
                return Resultado.Falla(new Error("Error.Validation", "El estudiante no posee la medalla que se intenta quitar."));
            }
            var entidadAsignacion = existenteResultado.Valor;
            var removeResultado = await _repositorioPerfilEstudianteMedalla.RemoveAsync(entidadAsignacion);
            return removeResultado;
        }
    }
}
