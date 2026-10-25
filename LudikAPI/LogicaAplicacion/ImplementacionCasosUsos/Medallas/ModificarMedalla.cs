using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaAplicacion.DTOsMappers.MedallaMappers;
using LogicaAplicacion.InterfacesCasosUsos.Medalla;
using LogicaNegocio.Resultados;
using System.Threading.Tasks;

namespace LogicaAplicacion.ImplementacionCasosUsos.Medallas
{
    public class ModificarMedalla : IModificarMedalla
    {
        private readonly IRepositorioMedallas _repositorioMedallas;

        public ModificarMedalla(IRepositorioMedallas repositorioMedallas)
        {
            _repositorioMedallas = repositorioMedallas;
        }

        public async Task<Resultado> EjecutarAsync(int id, MedallaEditarDto dto, string profesorId)
        {
            if (id <= 0)
                return Resultado.Falla(new Error("Error.Validation", "El ID de la medalla debe ser un entero positivo."));
            if (dto == null)
                return Resultado.Falla(new Error("Error.Validation", "Los datos para editar la medalla no pueden ser nulos."));

            Resultado<Medalla> resultadoObtener = await _repositorioMedallas.GetByIdAsync(id);
            if (resultadoObtener.EsFallo)
            {
                    return Resultado.Falla(new Error("Error.NotFound", $"No se encontró ninguna medalla con ID {id}."));
            }
            Medalla existente = resultadoObtener.Valor;
            MedallaEditarMapper.actualizarMedalla(existente, dto);

            if(existente.ProfesorId != profesorId)
                return Resultado.Falla(new Error("Error.Forbidden", "No tienes permiso para modificar esta medalla."));

            Resultado resultadoValidacion = existente.esValido();
            if (resultadoValidacion.EsFallo)
                return resultadoValidacion;

            Resultado resultadoUpdate = await _repositorioMedallas.UpdateAsync(existente);
            return resultadoUpdate;
        }
    }
}