using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.Entidades
{
	public class Tienda : IEntity, IValidable
    {
        public int Id { get; set; }

        public List<Recompensa> Recompesas { get; set; } = new();

        [ForeignKey(nameof(Grupo))]
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }


        public Resultado esValido()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Agrega una recompensa a la tienda, evitando duplicados.
        /// </summary>
        /// <param name="recompensa">La recompensa a agregar.</param>
        /// <returns>Resultado que indica éxito o fallo si la recompensa ya existe.</returns>
        public Resultado AgregarRecompensa(Recompensa recompensa)
        {

            if (Recompesas.Any(r => r.Id == recompensa.Id))
                return Resultado.Falla(new Error("Error.Conflict", $"La recompensa '{recompensa.Nombre}' ya existe en esta tienda."));
            
            Recompesas.Add(recompensa);
            return Resultado.Exitoso();
        }

        /// <summary>
        /// Agrega una recompensa precargada a la tienda, no comprueba duplicados.
        /// </summary>
        /// <param name="recompensa">La recompensa a agregar.</param>
        /// <returns>Resultado que indica éxito o fallo si la recompensa ya existe.</returns>
        public Resultado AgregarRecompensaPrecargada(Recompensa recompensa)
        {
            Recompesas.Add(recompensa);
            return Resultado.Exitoso();
        }
}

}

