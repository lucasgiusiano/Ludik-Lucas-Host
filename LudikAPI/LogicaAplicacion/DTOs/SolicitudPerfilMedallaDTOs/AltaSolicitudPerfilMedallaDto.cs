using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.DTOs.SolicitudPerfilMedallaDTOs
{
    public class AltaSolicitudPerfilMedallaDto
    {
        [Required]
        public int PerfilEstudianteId { get; set; }

        [Required]
        public int MedallaId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres.")]
        public string Descripcion { get; set; }
    }
}
