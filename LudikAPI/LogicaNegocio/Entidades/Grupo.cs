using System;
using System.Collections.Generic;
using LogicaNegocio.InterfacesEntidades;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObject;

namespace LogicaNegocio.Entidades
{
	public class Grupo: IEntity, IValidable
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre del grupo debe tener entre 3 y 30 caracteres.")]
        public String Nombre { get; set; }

        public String Institucion { get; set; }

        public String Materia { get; set; }

        public DateTime FCreacion { get; set; }
        [ForeignKey(nameof(TablaEquivalencia))]
        public int TablaEquivalenciaId { get; set; }
        [Required]
        public TablaEquivalencia TablaEquivalencia { get; set; }

        public Tienda Tienda { get; set; }

        public List<TablaClasificacion> TablasClasificacion { get; set; } = new();

        public List<PerfilEstudiante> Alumnos { get; set; } = new();

        public List<SolicitudUnion> Solicitudes { get; set; }
        [ForeignKey(nameof(EnlaceUnion))]
        public int EnlaceUnionId { get; set; }

        public EnlaceUnion EnlaceUnion { get; set; }

        public List<SolicitudPerfilMedalla> SolicitudesPerfilMedalla { get; set; } = new();

        [ForeignKey(nameof(Profesor))]
        public string ProfesorId { get; set; }
        public Profesor Profesor { get; set; }
        public DateTime? FechaUltimoReinicio { get; set; }

        public ProyectoAulaColaborativo Pac { get; set; }

		public bool estudiantePertenece(PerfilEstudiante pEstudiante)
		{
			return true;
		}

        public Resultado esValido()
        {
            var errores = new List<Error>();

            if (string.IsNullOrWhiteSpace(Nombre) || Nombre.Length < 3 || Nombre.Length > 30)
                errores.Add(new Error("Error.Validation", "El nombre del grupo debe tener entre 3 y 30 caracteres."));

            if (TablaEquivalencia == null)
                errores.Add(new Error("Error.Validation", "La tabla de equivalencia es obligatoria."));

            if (string.IsNullOrWhiteSpace(ProfesorId))
                errores.Add(new Error("Error.Validation", "El identificador del profesor es obligatorio."));

            if (errores.Any())
                return Resultado.Falla(errores);

            return Resultado.Exitoso();
        }

        public int CalcularNotaDeEstudiante(IEnumerable<Medalla> medallasObtenidas)
        {
            if (TablaEquivalencia == null)
                return 0;

            return TablaEquivalencia.MaximaCalificacionSegun(medallasObtenidas);
        }
        public List<RendimientoPeriodo> ReiniciarMedallasEstudiantes(DateTime desde, DateTime hasta)
        {
            var rendimientos = new List<RendimientoPeriodo>();

            foreach (var perfil in Alumnos)
            {
                var rendimiento = new RendimientoPeriodo
                {
                    PerfilEstudianteId = perfil.Id,
                    NotaObtenida = perfil.CalcularNotaActual(),
                    Rangofecha = new RangoFechas(desde, hasta),

                    RendimientoMedallas = perfil.Medallas
                        .Select(m => new RendimientoPeriodoMedalla
                        {
                            MedallaId = m.Id,
                            Medalla = m,          
                                                  
                        })
                        .ToList()
                };

                rendimientos.Add(rendimiento);

                perfil.MedallasObtenidas.Clear();
            }

            return rendimientos;
        }
        public void AgregarSolicitudPerfilMedalla(SolicitudPerfilMedalla solicitud)
        {
            SolicitudesPerfilMedalla.Add(solicitud);

        }
        public int ContarMedallasEnPeriodo(DateTime fIni, DateTime fFin)
        {
            return Alumnos.Sum(p => p.MedallasObtenidas.Count(
                    m => m.FechaObtencion >= fIni && m.FechaObtencion <= fFin
                )
            );
        }
    }

}

