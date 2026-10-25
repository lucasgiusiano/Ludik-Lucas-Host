using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LogicaNegocio.Excepciones;
using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.Entidades
{
	public class Medalla : IEntity, IValidable
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "El nombre de la medalla debe tener entre 1 y 30 caracteres.")]
        public String Nombre { get; set; }
        [StringLength(150, MinimumLength = 0, ErrorMessage = "la descripción de la medalla debe tener entre 3 y 150 caracteres.")]
        public String Descripcion { get; set; }
        
        public String NombreIcono { get; set; }
        [Required]
        public int MonedasOtorgadas { get; set; }

        public string ProfesorId { get; set; }
        [ForeignKey("ProfesorId")]
        public virtual Profesor Creador { get; set; }

        public Resultado esValido()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
                return Resultado.Falla(new Error("Error.Validation", "La medalla debe de tener un nombre."));
            if (Nombre.Length < 1)
                return Resultado.Falla(new Error("Error.Validation", "El nombre de la medalla debe de tener al menos 3 caracteres."));
            if (Nombre.Length>30)
                return Resultado.Falla(new Error("Error.Validation", "El nombre de la medalla debe de tener menos de 30 caracteres."));
            if (Descripcion.Length > 150)
                return Resultado.Falla(new Error("Error.Validation", "La descripción de la medalla no puede superar los 150 caracteres."));
            if (MonedasOtorgadas < 0)
                return Resultado.Falla(new Error("Error.Validation", "La cantidad de monedas otorgadas no puede ser menor a 0."));
            
            return Resultado.Exitoso();

        }

       
    }

}

