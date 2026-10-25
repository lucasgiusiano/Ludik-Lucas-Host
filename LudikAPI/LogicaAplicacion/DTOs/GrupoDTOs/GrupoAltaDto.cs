using System.Text.Json.Serialization;

namespace LogicaAplicacion.DTOs.GrupoDTOs
{
    public class GrupoAltaDto
    {

        public string Nombre { get; set; }
        public int TablaEquivalenciaId { get; set; }
        public string ProfesorId { get; set; }
        public string? Institucion { get; set; }        
        public string? Materia { get; set; }

        public string? CodigoEnlace { get; set; }
        public string? UrlCompleta { get; set; }
    }
}