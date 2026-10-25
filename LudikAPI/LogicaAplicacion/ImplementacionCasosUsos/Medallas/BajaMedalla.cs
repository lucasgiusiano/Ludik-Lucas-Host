using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.InterfacesCasosUsos.Medalla;
using LogicaNegocio.Resultados;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.ImplementacionCasosUsos.Medallas
{
    public class BajaMedalla : IBajaMedalla
    {
        private readonly IRepositorioMedallas _repoMedallas;
        private readonly IRepositorioProfesores _repoProfesores;
        private readonly IRepositorioTablasClasificacion _repoTablasClasificacion;
        private readonly IRepositorioTablasEquivalencia _repoTablaEquivalencia;
        private readonly IRepositorioPerfilEstudianteMedalla _repoPerfilEstudianteMedalla;
        private readonly IRepositorioRendimientoPeriodos _repositorioRendimientoPeriodos;
        public BajaMedalla(IRepositorioMedallas repoMedallas, IRepositorioProfesores repoProfesores, IRepositorioTablasClasificacion repoTablasClasificacion, IRepositorioTablasEquivalencia repoTablaEquivalencia,IRepositorioPerfilEstudianteMedalla repositorioPerfilEstudianteMedalla,IRepositorioRendimientoPeriodos repositorioRendimientoPeriodos)
        {
            _repoMedallas = repoMedallas;
            _repoProfesores = repoProfesores;
            _repoTablasClasificacion = repoTablasClasificacion;
            _repoTablaEquivalencia = repoTablaEquivalencia;
            _repoPerfilEstudianteMedalla = repositorioPerfilEstudianteMedalla;
            _repositorioRendimientoPeriodos = repositorioRendimientoPeriodos;

        }

        public async Task<Resultado> EjecutarAsync(int idMedalla, string profesorId)
        {
            var profesorBusqueda = await _repoProfesores.PoseeMedallaAsync(profesorId,idMedalla);
            if (profesorBusqueda.EsFallo)
            {
                return Resultado.Falla(new Error("Error.Validation", "No se puede eliminar la medalla porque no es del profesor logueado."));
            }

            if (idMedalla <= 0)
                return Resultado.Falla(new Error("Error.Validation", "El ID de la medalla debe ser un entero positivo."));

            Resultado<Medalla> resultadoObtener = await _repoMedallas.GetByIdAsync(idMedalla);
            if (resultadoObtener.EsFallo)
            {
                return Resultado.Falla(new Error("Error.NotFound", $"No se encontró ninguna medalla con ID {idMedalla}."));
            }

            var estaEnTabla = await _repoTablasClasificacion.ExisteTablaClasificacionConMedallaAsync(idMedalla);
            if (estaEnTabla)
            {
                return Resultado.Falla(new Error("Error.Validation", "No se puede eliminar la medalla porque está asociada a una tabla de clasificación."));
            }
            var estEnRendimiento = await _repositorioRendimientoPeriodos.ExisteEnRendimientoPeriodoAsync(idMedalla);
            if (estEnRendimiento)
            {
                return Resultado.Falla(new Error("Error.Validation", "No se puede eliminar la medalla porque está asociada a un rendimiento periodo"));
            }

            var estaEnTablaEquivalencia = await _repoTablaEquivalencia.ExisteTablaEquivalenciaConMedallaAsync(idMedalla);
            if (estaEnTablaEquivalencia)
            {
                return Resultado.Falla(new Error("Error.Validation", "No se puede eliminar la medalla porque está asociada a una tabla de equivalencia."));
            }
            var estaEnPerfilEstudianteMedalla = await _repoPerfilEstudianteMedalla.ExistePerfilEstudianteMedallaAsync(idMedalla);
            if (estaEnPerfilEstudianteMedalla)
            {
                return Resultado.Falla(new Error("Error.Validation", "No se puede eliminar la medalla porque está asociada a un perfil de estudiante."));
            }

            Medalla existente = resultadoObtener.Valor;
            if (existente == null)
            {
                return Resultado.Falla(new Error("Error.NotFound", $"No se encontró ninguna medalla con ID {idMedalla}."));
            }
            
            
            if (existente.ProfesorId!=profesorId)
            {
                return Resultado.Falla(new Error("Error.Validation", $"Esa Medalla No se encuentra dentro de la lista de medallas {idMedalla}."));
            }
            Resultado resultadoRemove = await _repoMedallas.RemoveAsync(existente);
            return resultadoRemove;
        }
    }
}
