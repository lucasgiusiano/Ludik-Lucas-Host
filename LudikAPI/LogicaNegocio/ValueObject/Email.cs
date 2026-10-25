using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace LogicaNegocio.ValueObjects
{
    public record Email
    {
        [Required]
        [EmailAddress(ErrorMessage = "El formato del correo no es v�lido.")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
       ErrorMessage = "El correo contiene caracteres no permitidos o formato inv�lido.")]
        public string Valor { get; private set; }


        private Email() { }

        public Email(string Valor)
        {
            if (!EsValido(Valor))
                throw new ValidationException("El correo no cumple con los requisitos de seguridad.");

            this.Valor = Valor;
        }

        public static bool EsValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            return Regex.IsMatch(
                correo,
                @"^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
            );
        }

    }

}

