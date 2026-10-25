using System.Collections.Generic;
using System;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Identity;

namespace LogicaNegocio.Entidades
{
	public class Estudiante: Usuario
	{
        public List<PerfilEstudiante> Perfiles { get; set; }

        public List<Hito> Hitos { get; set; }

        public List<PreguntaRespuestaSeguridad> PreguntasSeguridad { get; set; }

        public EstudiantePotenciador EstPotenciador { get; set; }

        public bool CoincidenLasRespuestas(List<PreguntaRespuestaSeguridad> respuestasIngresadas, IPasswordHasher<Usuario> hasher)
        {
            if (respuestasIngresadas == null || this.PreguntasSeguridad == null) return false;

            if (respuestasIngresadas.Count != this.PreguntasSeguridad.Count) return false;

            foreach (var respuestaIngresada in respuestasIngresadas)
            {
                var preguntaAlmacenada = this.PreguntasSeguridad.FirstOrDefault(p => p.Id == respuestaIngresada.Id);

                if (preguntaAlmacenada == null) return false;

                var resultadoVerificacion = hasher.VerifyHashedPassword(this, preguntaAlmacenada.Respuesta, respuestaIngresada.Respuesta);

                if (resultadoVerificacion == PasswordVerificationResult.Failed)
                    return false;
                
            }

            return true;
        }

        public int ContarCantidadMedallasTotales()
        {
            if (Perfiles == null || !Perfiles.Any())
                return 0;

            return Perfiles.Sum(perfil => perfil.MedallasObtenidas?.Count ?? 0);
        }

        public Resultado<PerfilEstudiante> ObtenerPerfilEstudiantePor(int id)
        {
            if (Perfiles == null || !Perfiles.Any())
                return Resultado<PerfilEstudiante>.Falla(new Error("Error.NotFound", "No se encontraron perfiles asociados al estudiante."));
            var perfil = Perfiles.FirstOrDefault(p => p.Id == id);
            if (perfil == null)
                return Resultado<PerfilEstudiante>.Falla(new Error("Error.NotFound", $"No se encontró el perfil con ID {id}."));
            return Resultado<PerfilEstudiante>.Exitoso(perfil);
        }
        public void ActivarPotenciador(Potenciador plantilla)
        {
            if(EstPotenciador != null)
            {
                LimpiarPotenciadorExpirado();
            }
            if (EstPotenciador == null)
                EstPotenciador = new EstudiantePotenciador
                {
                    Est = this,
                    EstudianteId = this.Id,
                    FechaActivacion = DateTime.UtcNow,
                    PotenciadorId = plantilla.Id,
                    Potenciador = plantilla
                };
        }
        public void LimpiarPotenciadorExpirado()
        {
            if (EstPotenciador != null )
                EstPotenciador = null;
        }
        public double ObtenerMultiplicadorMonedas()
        {
           
            if (EstPotenciador != null && EstPotenciador.EstaActivo)
                return EstPotenciador.Potenciador.Multiplicador;
            return 1.0;
        }
    }

}

