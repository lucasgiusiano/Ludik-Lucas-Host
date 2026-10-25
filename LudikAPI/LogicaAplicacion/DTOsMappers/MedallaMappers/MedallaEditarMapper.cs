using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.MedallaDTOs;

namespace LogicaAplicacion.DTOsMappers.MedallaMappers
{
    public static class MedallaEditarMapper
    {
        public static void actualizarMedalla(Medalla entidad, MedallaEditarDto dto)
        {
            entidad.NombreIcono = dto.NombreIcono;
            entidad.Nombre = dto.Nombre;
            entidad.Descripcion = dto.Descripcion;
            entidad.MonedasOtorgadas = dto.CantidadMonedasBrinda;
        }
    }
}
