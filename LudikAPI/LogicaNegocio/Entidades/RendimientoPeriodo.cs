using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;

namespace LogicaNegocio.Entidades
{
	public class RendimientoPeriodo : IEntity, IValidable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public RangoFechas Rangofecha { get; set; }

        public int NotaObtenida { get; set; }

        public List<RendimientoPeriodoMedalla> RendimientoMedallas { get; set; } = new();

        [ForeignKey(nameof(PerfilEstudiante))]
        public int PerfilEstudianteId { get; set; } // Clave for�nea


        public Resultado esValido()
        {
            throw new NotImplementedException();
        }
    }

}

