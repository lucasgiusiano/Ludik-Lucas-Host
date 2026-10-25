using LogicaNegocio.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.Entidades
{
	public class Profesor: Usuario
	{

        public List<Medalla> Medallas { get; set; }

        public List<TablaEquivalencia> TablasEquivalencia { get; set; }

        public List<Grupo> Grupos { get; set; }

        public ICollection<RecompensaProfesor> RecompensasCreadas { get; private set; } = new HashSet<RecompensaProfesor>();



        public void asignarMedalla(Medalla medalla, Grupo grupo, PerfilEstudiante pEstudiante)
		{

		}

        /// <summary>
        /// Asocia una nueva recompensa a este profesor, encapsulando la lógica de creación.
        /// </summary>
        /// <param name="recompensa">La recompensa a ser creada por el profesor.</param>
        /// <returns>Retorna un resultado indicando si la operación se completo con éxito o fallo.</returns>
        public Resultado CrearRecompensa(Recompensa recompensa)
        {

            if (RecompensasCreadas.Any(pr => pr.RecompensaId == recompensa.Id))
            {
                return Resultado.Falla(Error.Conflict);
            }

            var nuevaCreacion = new RecompensaProfesor
            {
                Profesor = this,
                ProfesorId = this.Id, 
                Recompensa = recompensa,
                RecompensaId = recompensa.Id,
                FechaCreacion = DateTime.UtcNow
            };

            RecompensasCreadas.Add(nuevaCreacion);

            return Resultado.Exitoso();
        }

        public bool TieneRecompensa(int recompensaId)
        {
            foreach (var recompensaProfesor in RecompensasCreadas)
            {
                if (recompensaProfesor.Recompensa.Id == recompensaId)
                    return true;
            }

            return false;
        }
    }

}

