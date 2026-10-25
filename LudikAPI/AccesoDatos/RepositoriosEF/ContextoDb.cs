using AccesoDatos.RepositoriosEF.Configuraciones;
using LogicaNegocio.Entidades;
using LogicaNegocio.EntidadesAuxiliares;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.RepositoriosEF
{
    public class ContextoDb : IdentityDbContext<Usuario>
    {
        public ContextoDb(DbContextOptions<ContextoDb> options) : base(options)
        {
        }


        //Tablas que hay en la base de datos
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Recompensa> Recompensas { get; set; }
        public DbSet<RecompensaProfesor> RecompensasDeProfesores { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Tienda> Tiendas { get; set; }
        public DbSet<Medalla> Medallas { get; set; }
        public DbSet<PerfilEstudiante> PerfilesEstudiantes { get; set; }
        public DbSet<PerfilEstudianteMedalla> PerfilEstudianteMedallas { get; set; }
        public DbSet<TablaClasificacion> TablasClasificacion { get; set; }
        public DbSet<TablaEquivalencia> TablasEquivalencia { get; set; }
        public DbSet<SolicitudUnion> SolicitudesUnion { get; set; }
        public DbSet<SolicitudPerfilMedalla> SolicitudesPerfilMedalla { get; set; }
        public DbSet<RendimientoPeriodo> RendimientosPeriodos { get; set; }
        public DbSet<RendimientoPeriodoMedalla> RendimientosPeriodosMedallas { get; set; }
        public DbSet<PreguntaDeSeguridad> PreguntasDeSeguridad { get; set; }
        public DbSet<PreguntaRespuestaSeguridad> PreguntasRespuestasSeguridad { get; set; }
        public DbSet<Potenciador> Potenciadores { get; set; }
        public DbSet<Hito> Hitos { get; set; }
        public DbSet<Equivalencia> Equivalencias { get; set; }
        public DbSet<EnlaceUnion> EnlacesUnion { get; set; }
        public DbSet<BarraProgreso> BarrasProgreso { get; set; }
        public DbSet<Avatar> Avatares { get; set; }
        public DbSet<AtributoAvatar> AtributosAvatar { get; set; }
        public DbSet<PerfilEstudianteRecompensa> PerfilEstudianteRecompensas { get; set; }
        public DbSet<KudoOtorgado> KudosOtorgados { get; set; }
        public DbSet<TipoKudo> TiposKudo { get; set; }
        public DbSet<UmbralParaMedallaPorKudos> UmbralesParaMedallasPorKudos { get; set; }
        public DbSet<ProyectoAulaColaborativo> ProyectosAulaColaborativos { get; set; }
        public DbSet<EstudiantePotenciador> EstudiantePotenciadores { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().UseTptMappingStrategy();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextoDb).Assembly);

            // Llamada al método de extensión para las tablas de Identity
            modelBuilder.ConfigurarTablasIdentity();
                

            // ----PLANTAR DATOS-----
            modelBuilder.Semilla();

        }
    }
}
