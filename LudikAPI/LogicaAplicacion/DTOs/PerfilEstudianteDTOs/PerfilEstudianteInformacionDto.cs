using LogicaAplicacion.DTOs.MedallaDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.PerfilEstudianteDTO
{
    public class PerfilEstudianteInformacionDto
    {
        public int Id { get; set; }

        public int AvatarGrupoId { get; set; }

        public string? EnlaceAvatarCompleto { get; set; }
        public string? EnlaceAvatarMiniatura { get; set; }

        public int MetaCalificacion { get; set; }

        public string EstudianteId { get; set; }

        public string NombreEstudiante { get; set; }

        public int Monedas { get; set; }

        public int GrupoId { get; set; }

        public string NombreGrupo { get; set; }

        public int CalificacionActual { get; set; }

        public List<MedallaDto> Medallas { get; set; }
	}
}
