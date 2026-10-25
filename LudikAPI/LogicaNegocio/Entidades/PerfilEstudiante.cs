using System;
using System.Collections.Generic;
using LogicaNegocio.InterfacesEntidades;
using System.ComponentModel.DataAnnotations.Schema;
using LogicaNegocio.Resultados;
using LogicaNegocio.Entidades;
using System.Text.RegularExpressions;

namespace LogicaNegocio.Entidades
{
	public class PerfilEstudiante : IEntity, IValidable
    {
        public int Id { get; set; }

        public Avatar Avatar { get; private set; }

        public int MetaCalificacion { get; set; }

        public String EstudianteId { get; set; }

        [ForeignKey(nameof(EstudianteId))]
        public Estudiante Estudiante { get; set; }

        public int Monedas { get; set; }
        public string NombreImagenCompleta { get; set; }
        public string NombreImagenMiniatura { get; set; }
        public List<PerfilEstudianteMedalla> MedallasObtenidas { get; set; }
        public int KudosDisponiblesParaOtorgar { get; set; }
        public List<KudoOtorgado> KudosOtorgados { get; private set; }
        public List<KudoOtorgado> KudosRecibidos { get; private set; }

        [NotMapped]
        public IEnumerable<Medalla> Medallas
        {
            get => MedallasObtenidas.Select(pm => pm.Medalla);
        }

        public List<RendimientoPeriodo> HistorialRendimientoPeriodos { get; set; }

        public int GrupoId { get; set; }
        [ForeignKey(nameof(GrupoId))]
        public Grupo Grupo { get; set; }

        public List<PerfilEstudianteRecompensa> InventarioRecompensas { get; set; }

        public List<TablaClasificacion> TablasClasificacion { get; set; }

        [NotMapped]
        public IEnumerable<Recompensa> Inventario =>
            InventarioRecompensas.Select(x => x.Recompensa);

        public BarraProgreso BarraProgreso { get; set; }


        public PerfilEstudiante()
        {
            this.MedallasObtenidas = new List<PerfilEstudianteMedalla>();
            this.KudosOtorgados = new List<KudoOtorgado>();
            this.KudosRecibidos = new List<KudoOtorgado>();
            this.InventarioRecompensas = new List<PerfilEstudianteRecompensa>();
            this.TablasClasificacion = new List<TablaClasificacion>();
        }
        public PerfilEstudiante(string estudianteId,int grupoId)
        {
            this.MedallasObtenidas = new List<PerfilEstudianteMedalla>();
            this.KudosOtorgados = new List<KudoOtorgado>();
            this.KudosRecibidos = new List<KudoOtorgado>();
            this.InventarioRecompensas = new List<PerfilEstudianteRecompensa>();
            this.TablasClasificacion = new List<TablaClasificacion>();

            EstudianteId = estudianteId;
            NombreImagenCompleta = "eyebrows-defaultNatural.png";
            NombreImagenMiniatura = "eyebrows-defaultNatural.png";
            KudosDisponiblesParaOtorgar = 3;
            GrupoId = grupoId;
            Avatar = new Avatar();
        }

        public void RecibirMedallas(Medalla medalla, int cantidad)
        {
            double factorMultiplicador =Estudiante.ObtenerMultiplicadorMonedas();
            int monedasGanadas = (int)(medalla.MonedasOtorgadas * factorMultiplicador * cantidad);
            this.Monedas += monedasGanadas;

            for (int i = 0; i < cantidad; i++)
            {
                var nuevaAsignacion = new PerfilEstudianteMedalla
                {
                    PerfilEstudianteId = this.Id,
                    MedallaId = medalla.Id,
                    FechaObtencion = System.DateTime.UtcNow
                    // No es necesario asignar los objetos de navegación completos, 
                    // EF Core manejará las relaciones a través de las FK.
                };
                this.MedallasObtenidas.Add(nuevaAsignacion);
            }
        }
        
        public Resultado esValido()
        {
            throw new NotImplementedException();
        }
        public int CalcularNotaActual()
        {
            if (Grupo == null)
                return 0;
            return Grupo.CalcularNotaDeEstudiante(Medallas);
            
        }

        public List<RecompensaPersonalizacionAvatar> ObtenerItemsAvatarDisponibles()
        {
            return Inventario?.OfType<RecompensaPersonalizacionAvatar>().ToList() ?? new List<RecompensaPersonalizacionAvatar>();
        }

        public Resultado EstablecerMetaDeCalificacion(int nuevaMeta)
        {
            int notaMaxDeTablaEquivalencia = Grupo.TablaEquivalencia.ObtenerNotaMaxima();
            if (nuevaMeta < 0 || notaMaxDeTablaEquivalencia < nuevaMeta)
            {
                return Resultado.Falla(new Error("Error.Validation", "La meta de calificación debe estar entre 0 y "+notaMaxDeTablaEquivalencia+"."));
            }


            this.MetaCalificacion = nuevaMeta;

            return Resultado.Exitoso();
        }

        /// <summary>
        /// Encapsula la lógica de negocio para otorgar un kudo.
        /// Verifica si hay kudos disponibles y descuenta uno.
        /// </summary>
        /// <returns>Un resultado exitoso si se pudo otorgar, o de falla en caso contrario.</returns>
        public Resultado<KudoOtorgado> OtorgarKudo(TipoKudo tipoKudo, PerfilEstudiante perfilReceptor)
        {
            if (KudosDisponiblesParaOtorgar <= 0)
                return Resultado<KudoOtorgado>.Falla(new Error("Error.Validation", "No tienes Kudos disponibles esta semana. Recibirás más el próximo lunes."));
            

            KudosDisponiblesParaOtorgar--;
            var kudoOtorgado = new KudoOtorgado(this, perfilReceptor, tipoKudo, DateTime.UtcNow);
            
            return Resultado<KudoOtorgado>.Exitoso(kudoOtorgado);
        }
        public Resultado RecibirKudoYEvaluarMedalla(KudoOtorgado kudo, UmbralParaMedallaPorKudos umbral)
        {
            KudosRecibidos.Add(kudo);

            if (umbral == null)
                return Resultado.Exitoso();

            int kudosContabilizados = KudosRecibidos.Count(kr =>
                kr.TipoKudoId == umbral.TipoKudoId &&
                kr.PerfilEstudianteMedallaId == null);

            if (kudosContabilizados >= umbral.CantidadKudos)
            {
                var nuevaAsignacionMedalla = new PerfilEstudianteMedalla(this, umbral.Medalla, DateTime.UtcNow);
                MedallasObtenidas.Add(nuevaAsignacionMedalla);

                // Marcamos los kudos que se usaron para ganar esta medalla para que no se vuelvan a contar.
                var kudosParaMarcar = KudosRecibidos
                    .Where(kr => kr.TipoKudoId == umbral.TipoKudoId && kr.PerfilEstudianteMedallaId == null)
                    .Take(umbral.CantidadKudos);

                foreach (var k in kudosParaMarcar)
                {
                    k.MarcarComoUsadoPara(nuevaAsignacionMedalla);
                }
            }

            return Resultado.Exitoso();
        }
       
    }

}

