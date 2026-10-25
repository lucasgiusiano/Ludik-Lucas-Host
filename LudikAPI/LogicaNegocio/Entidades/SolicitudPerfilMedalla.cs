using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;

namespace LogicaNegocio.Entidades
{
    public class SolicitudPerfilMedalla:IValidable, IEntity
    {
        public int Id { get; set; }

        public int PerfilEstudianteId { get; set; }
        public int MedallaId { get; set; }
        public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        public Resultado esValido()
        {
            var errores = new List<Error>();

            if (PerfilEstudianteId <= 0)
                errores.Add(new Error("Error.Validation", "El PerfilEstudianteId debe ser un entero positivo."));

            if (MedallaId <= 0)
                errores.Add(new Error("Error.Validation", "El MedallaId debe ser un entero positivo."));

            if (GrupoId <= 0)
                errores.Add(new Error("Error.Validation", "El GrupoId debe ser un entero positivo."));

            if (string.IsNullOrWhiteSpace(Descripcion))
                errores.Add(new Error("Error.Validation", "La descripción es obligatoria."));
            else if (Descripcion.Length > 500)
                errores.Add(new Error("Error.Validation", "La descripción no puede exceder 500 caracteres."));

            return errores.Any()
                ? Resultado.Falla(errores)
                : Resultado.Exitoso();
        }
    }
}
