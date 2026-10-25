using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.PerfilEstudianteDTO;
using LogicaAplicacion.DTOsMappers.MedallaMappers;

public static class PerfilMapper
{
    public static PerfilConMedallasDto ToDtoConMedallas(PerfilEstudiante perfil)
    {
        if (perfil == null) return null;

        var dto = new PerfilConMedallasDto
        {
            Id = perfil.Id,
            EnlaceAvatar = perfil.NombreImagenMiniatura,
            MetaCalificacion = perfil.MetaCalificacion,
            EstudianteId = perfil.EstudianteId,
            NombreEstudiante = perfil.Estudiante?.NombreCompleto?.Nombre,
            Monedas = perfil.Monedas,
            GrupoId = perfil.GrupoId,
            NombreGrupo = perfil.Grupo?.Nombre,
            Medallas = MedallaCantidadMapper
                                     .AgruparMedallas(perfil.MedallasObtenidas?
                                                           .Select(pm => pm.Medalla)
                                                           .ToList())
        };

        // Ahora perfil.PotenciadorActivo es PerfilEstudiantePotenciador
        var activo = perfil.Estudiante.EstPotenciador;
        
        if (activo != null )
        {
            dto.MultiplicadorPotenciador = activo.Potenciador.Multiplicador;
            var fin = activo.FechaActivacion + activo.Potenciador.Duracion;
            dto.TiempoRestantePotenciador = fin - DateTime.UtcNow;
        }
        else
        {
            dto.MultiplicadorPotenciador = null;
            dto.TiempoRestantePotenciador = null;
        }

        return dto;
    }

}
