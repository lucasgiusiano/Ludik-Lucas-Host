namespace LogicaNegocio.Entidades
{
    public class PerfilEstudianteMedalla
    {
        public int Id { get; set; }
        public int PerfilEstudianteId { get; set; }
        public PerfilEstudiante PerfilEstudiante { get; set; }
        public int MedallaId { get; set; }
        public Medalla Medalla { get; set; } 

        public DateTime FechaObtencion { get; set; }

        public PerfilEstudianteMedalla()
        {
            FechaObtencion = DateTime.UtcNow;
        }

        public PerfilEstudianteMedalla(PerfilEstudiante perfilEstudiante, Medalla medalla, DateTime fecha)
        {
            PerfilEstudiante = perfilEstudiante;
            Medalla = medalla;
            FechaObtencion = fecha;
        }

    }
}
