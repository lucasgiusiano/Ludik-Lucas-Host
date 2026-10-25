using System;
using System.ComponentModel.DataAnnotations.Schema;
using LogicaNegocio.EntidadesAuxiliares;
using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.Entidades
{
	public class Potenciador : Recompensa
	{


        public int DuracionHoras { get; set; }

        public double Multiplicador { get; set; }

        [NotMapped]
        public TimeSpan Duracion => TimeSpan.FromHours(DuracionHoras);

        public override RepresentacionVisualBase Representacion { get; protected set; }
        
        public Potenciador()
        {
            Representacion = new RepresentacionImagen();
        }

        public override Resultado Otorgar(PerfilEstudiante estudiante)
        {
            return Resultado.Exitoso();
        }
        
    }
     
}

