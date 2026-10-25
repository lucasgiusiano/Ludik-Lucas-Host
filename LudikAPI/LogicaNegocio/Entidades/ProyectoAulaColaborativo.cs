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
    public class ProyectoAulaColaborativo : IEntity, IValidable
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; } = default!;
        [Required]
        public string Nombre { get; set; }
        public int CantidadMedallasNecesarias { get; set; }
        public int RecompensaClaseId { get; set; }
        public Recompensa RecompensaClase { get; set; } = default!;
        public EstadoPAC Estado { get; set; } = EstadoPAC.Activo;

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }



        public Resultado esValido()
        {
            var errores = new List<Error>();
            if (string.IsNullOrWhiteSpace(Nombre) || Nombre.Length < 3 || Nombre.Length > 50){
                errores.Add(new Error("Error.Validation","El nombre del proyecto debe tener entre 3 y 50 caracteres."));
            }
            if (CantidadMedallasNecesarias <= 0){
                errores.Add(new Error("Error.Validation","La cantidad de medallas necesarias debe ser un número entero positivo."));
            }
            if (RecompensaClase is null){
                errores.Add(new Error("Error.Validation","Debe especificar la recompensa de la clase."));
            }
            else{
                var resultadoRecompensa = RecompensaClase.esValido();
                if (resultadoRecompensa.EsFallo)
                    errores.AddRange(resultadoRecompensa.Errores!);
            }

            if (errores.Any())
                return Resultado.Falla(errores);

            return Resultado.Exitoso();
        }
    }
}
