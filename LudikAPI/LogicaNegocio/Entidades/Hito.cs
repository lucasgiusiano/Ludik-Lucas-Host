using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogicaNegocio.Entidades
{
	public class Hito : IEntity, IValidable
    {
        public int Id { get; set; }

        public int CantMedallasRequeridas { get; set; }

        public int RecompensaId { get; set; }

        [ForeignKey("RecompensaId")]
        public Recompensa Recompensa { get; set; }

        public bool Cumple(int totalMedallas) => totalMedallas >= CantMedallasRequeridas;

        public Resultado esValido()
        {
            throw new NotImplementedException();
        }
    }

}

