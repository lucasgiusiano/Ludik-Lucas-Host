using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogicaNegocio.Entidades
{
	public class BarraProgreso : IEntity, IValidable
    {
        public int Id { get; set; }

        public int ValorMin { get; set; }

        public int ValorMax { get; set; }

        public TablaEquivalencia TablaEquivalencia { get; set; }

        [ForeignKey(nameof(PerfilEstudiante))]
        public int PerfilEstudianteId { get; set; }

        public Resultado esValido()
        {
            throw new NotImplementedException();
        }

       

	}

}

