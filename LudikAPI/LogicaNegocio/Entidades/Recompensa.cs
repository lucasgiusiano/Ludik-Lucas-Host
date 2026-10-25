using LogicaNegocio.EntidadesAuxiliares;
using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogicaNegocio.Entidades
{
	public abstract class Recompensa : IEntity, IValidable
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre Recompensa  debe tener entre 3 y 50 caracteres.")]
        public string Nombre { get; set; }
        
        public abstract RepresentacionVisualBase Representacion { get; protected set; }
        public int Precio { get; set; }

        public virtual Resultado esValido()
        {
            var errores = new List<Error>();
            if (string.IsNullOrWhiteSpace(Nombre) || Nombre.Length < 3 || Nombre.Length > 50)
                errores.Add(new Error("Error.Validation", "El nombre de la recompensa debe tener entre 3 y 50 caracteres."));
            if (Precio < 0)
                errores.Add(new Error("Error.Validation", "El precio de la recompensa debe ser >= 0."));
            if (errores.Any())
                return Resultado.Falla(errores);
            return Resultado.Exitoso();
        }

        public abstract Resultado Otorgar(PerfilEstudiante perfil);

        public void pagar(int precio)
		{

		}

    }

}

