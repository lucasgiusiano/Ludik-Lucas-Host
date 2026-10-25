using System;
using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;

namespace LogicaNegocio.Entidades
{
	public class PreguntaRespuestaSeguridad : IEntity, IValidable
    {
        public int Id { get; set; }

        public int PreguntaDeSeguridadId { get; set; }
        public PreguntaDeSeguridad PreguntaDeSeguridad { get; set; } 

        public String Respuesta { get; set; }

        public String EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }


        public Resultado esValido()
        {
            throw new NotImplementedException();
        }
    }

}

