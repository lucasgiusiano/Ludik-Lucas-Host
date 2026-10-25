namespace LogicaAplicacion.DTOs.PerfilEstudianteDTO
{
    public class PerfilConMedallasDto
    {
        public int Id { get; set; }
        public string? EnlaceAvatar { get; set; }
        public int MetaCalificacion { get; set; }
        public string EstudianteId { get; set; }
        public string NombreEstudiante { get; set; }
        public int Monedas { get; set; }
        public int GrupoId { get; set; }
        public string NombreGrupo { get; set; }
        public List<MedallaAgrupadaDto> Medallas { get; set; }
        public double? MultiplicadorPotenciador { get; set; }
        public TimeSpan? TiempoRestantePotenciador { get; set; }
    }
}