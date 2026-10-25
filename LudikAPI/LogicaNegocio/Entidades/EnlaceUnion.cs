using System;
using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.Entidades{

    public class EnlaceUnion : IEntity, IValidable
    {
        public int Id { get; set; }

        public String CodigoUnico { get; set; }

        public DateTime Expiracion { get; set; }
        public string UrlCompleta { get; set; } // Este ser� el enlace listo para usar

        public EnlaceUnion() { }

        public EnlaceUnion(string urlCompleta, string codigoUnico)
        {
            this.UrlCompleta = urlCompleta;
            this.CodigoUnico = codigoUnico;
            this.Expiracion = DateTime.UtcNow.AddDays(300); 
        }




        public Resultado esValido()
        {
            var errores = new List<Error>();

            if (string.IsNullOrWhiteSpace(CodigoUnico))
                errores.Add(new Error("Error.Validation", "El código del enlace no puede ser vacío."));

            if (Expiracion < DateTime.UtcNow)
                errores.Add(new Error("Error.Validation", "El enlace está expirado."));

            if (errores.Any())
                return Resultado.Falla(errores);

            return Resultado.Exitoso();
        }


    }

}

