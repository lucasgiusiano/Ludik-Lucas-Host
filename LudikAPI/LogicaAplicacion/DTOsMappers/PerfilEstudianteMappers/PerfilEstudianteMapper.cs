using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.PerfilEstudianteDTO;
using LogicaAplicacion.DTOsMappers.MedallaMappers;

public static class PerfilEstudianteMapper
{
    public static PerfilEstudianteInformacionDto ToDto(PerfilEstudiante perfil, int calificacionActual)
    {
        if (perfil == null) return null;
        return new PerfilEstudianteInformacionDto
        {
            Id = perfil.Id,
            EnlaceAvatarMiniatura = perfil.NombreImagenMiniatura,
            EnlaceAvatarCompleto = perfil.NombreImagenCompleta,
            MetaCalificacion = perfil.MetaCalificacion,
            EstudianteId = perfil.EstudianteId,
            NombreEstudiante = perfil.Estudiante.NombreCompleto.Nombre,
            Monedas = perfil.Monedas,
            GrupoId = perfil.GrupoId,
            NombreGrupo = perfil.Grupo.Nombre,
            CalificacionActual = calificacionActual,
			    Medallas = perfil.MedallasObtenidas.Select(m =>MedallaMapper.toDto(m.Medalla)).ToList()
		};
    }
}
