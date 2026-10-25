using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{

    public record NombreUsuario
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 20 caracteres.")]
        public string Valor { get; set; }

        private NombreUsuario() { }
        public NombreUsuario(string Valor)
        {
            if (string.IsNullOrWhiteSpace(Valor) || Valor.Length < 3 || Valor.Length > 20)
                throw new ValidationException("Nombre de usuario invalido debe tener entre 3-20 caracteres");

            this.Valor = Valor;
        }

    }

}

