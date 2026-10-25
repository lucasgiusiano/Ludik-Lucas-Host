using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.Entidades
{
	public class Equivalencia : IEntity, IValidable
    {
        public int Id { get; set; }

        public int Nota { get; set; }

        [ForeignKey(nameof(TablaEquivalencia))]
        public int TablaEquivalenciaId { get; set; }
        public TablaEquivalencia TablaEquivalencia { get; set; }

        public List<Medalla> MedallasNecesarias { get; set; }

        public Equivalencia()
        {
        }

        public Equivalencia(int nota, List<Medalla> medallas)
        {
            Nota = nota;
            MedallasNecesarias = medallas ?? new List<Medalla>();
        }

        public Resultado esValido()
        {
            var errores = new List<Error>();

            if (Nota <= 0)
                errores.Add(new Error("Error.Validation", "La nota debe ser un número positivo."));
            
            if (MedallasNecesarias == null || MedallasNecesarias.Count == 0)
                errores.Add(new Error("Error.Validation", "La equivalencia debe tener asociada al menos una medalla."));

           
            if (errores.Count > 0)
                return Resultado.Falla(errores);
            
            return Resultado.Exitoso();
        }

        public bool CumpleMedallasNecesarias(IEnumerable<Medalla> medallasObtenidas)
        {
            
            if (medallasObtenidas == null || MedallasNecesarias.Count() > medallasObtenidas.Count())
                return false;

            var conteoNecesario = MedallasNecesarias
                .GroupBy(m => m.Id)
                .ToDictionary(g => g.Key, g => g.Count());

            var conteoObtenido = medallasObtenidas
                .GroupBy(m => m.Id)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var requisito in conteoNecesario)
            {
                var idMedallaRequerida = requisito.Key;
                var cantidadRequerida = requisito.Value;

                if (!conteoObtenido.TryGetValue(idMedallaRequerida, out int cantidadObtenida) || cantidadObtenida < cantidadRequerida)
                    return false;
                
            }

            return true;


        }
    }

}

