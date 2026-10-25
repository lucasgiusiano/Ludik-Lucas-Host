using System.Collections.Generic;
using System.Linq;
using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.PerfilEstudianteDTO;

namespace LogicaAplicacion.DTOsMappers.MedallaMappers
{
    public static class MedallaCantidadMapper
    {
        public static List<MedallaAgrupadaDto> AgruparMedallas(IEnumerable<Medalla> medallas)
        {
            if (medallas == null)
                return new List<MedallaAgrupadaDto>();

            var agrupadas = medallas
                .GroupBy(m => new
                {
                    m.Id,
                    m.Nombre,
                    Icono = m.NombreIcono,
                    m.Descripcion,
                    m.MonedasOtorgadas
                })
                .Select(g => new MedallaAgrupadaDto
                {
                    MedallaId = g.Key.Id,
                    Nombre = g.Key.Nombre,
                    Icono = g.Key.Icono,
                    Descripcion = g.Key.Descripcion,
                    MonedasOtorgadas = g.Key.MonedasOtorgadas,
                    Cantidad = g.Count()
                })
                .ToList();

            return agrupadas;
        }
    }
}
