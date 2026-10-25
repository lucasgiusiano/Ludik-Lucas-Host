using LogicaNegocio.Entidades;
using LogicaNegocio.EntidadesAuxiliares;
using LogicaNegocio.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.RepositoriosEF
{
	public static class SembradorDeDatos
	{

		// IDs compartidos a nivel de clase para evitar pasarlos como parámetros constantemente
		private const string RolProfesorId = "2c5e174e-3b0e-446f-86af-483d56fd7210";
		private const string RolEstudianteId = "3d5e174e-3b0e-446f-86af-483d56fd7211";
		private const string Profesor1Id = "8e445865-a24d-4543-a6c6-9443d048cdb9";
		private const string Profesor2Id = "9e445865-a24d-4543-a6c6-9443d048cdb0";
		private const string Estudiante1Id = "a1445865-a24d-4543-a6c6-9443d048cdb1";
		private const string Estudiante2Id = "b2445865-a24d-4543-a6c6-9443d048cdb2";
		private const string Estudiante3Id = "c3445865-a24d-4543-a6c6-9443d048cdb3";
		private const string Estudiante4Id = "d4445865-a24d-4543-a6c6-9443d048cdb4";
		private const string Estudiante5Id = "e5445865-a24d-4543-a6c6-9443d048cdb5";
		private const string Estudiante6Id = "f6445865-a24d-4543-a6c6-9443d048cdb6";
		private const string Estudiante7Id = "g7445865-a24d-4543-a6c6-9443d048cdb7";
		private const string Estudiante8Id = "h8445865-a24d-4543-a6c6-9443d048cdb8";
		private const string Estudiante9Id = "i9445865-a24d-4543-a6c6-9443d048cdb9";
		private const string Estudiante10Id = "jA445865-a24d-4543-a6c6-9443d048cdbA";
		private const string Estudiante11Id = "kB445865-a24d-4543-a6c6-9443d048cdbB";
		private const string Estudiante12Id = "lC445865-a24d-4543-a6c6-9443d048cdbC";
		private const string Estudiante13Id = "mD445865-a24d-4543-a6c6-9443d048cdbD";
		private const string Estudiante14Id = "nE445865-a24d-4543-a6c6-9443d048cdbE";
		private const string Estudiante15Id = "oF445865-a24d-4543-a6c6-9443d048cdbF";
		private const string Estudiante16Id = "p0445865-a24d-4543-a6c6-9443d048cdc0";
		private const string Estudiante17Id = "q1445865-a24d-4543-a6c6-9443d048cdc1";
		private const string Estudiante18Id = "r2445865-a24d-4543-a6c6-9443d048cdc2";
		private const string Estudiante19Id = "s3445865-a24d-4543-a6c6-9443d048cdc3";
		private const string Estudiante20Id = "t4445865-a24d-4543-a6c6-9443d048cdc4";
		private const string Estudiante21Id = "u5445865-a24d-4543-a6c6-9443d048cdc5";
		private const string Estudiante22Id = "v6445865-a24d-4543-a6c6-9443d048cdc6";
		private const string Estudiante23Id = "w7445865-a24d-4543-a6c6-9443d048cdc7";
		private const string Estudiante24Id = "x8445865-a24d-4543-a6c6-9443d048cdc8";
		private const string Estudiante25Id = "y9445865-a24d-4543-a6c6-9443d048cdcA";
		private const string Estudiante26Id = "zA445865-a24d-4543-a6c6-9443d048cdcB";
		private const string Estudiante27Id = "aB445865-a24d-4543-a6c6-9443d048cdcC";
		private const string Estudiante28Id = "bC445865-a24d-4543-a6c6-9443d048cdcD";
		private const string Estudiante29Id = "cD445865-a24d-4543-a6c6-9443d048cdcE";
		private const string Estudiante30Id = "dE445865-a24d-4543-a6c6-9443d048cdcF";
		private const string Estudiante31Id = "eF445865-a24d-4543-a6c6-9443d048cdd0";
		private const string Estudiante32Id = "f0445865-a24d-4543-a6c6-9443d048cdd1";
		private const string Estudiante33Id = "g1445865-a24d-4543-a6c6-9443d048cdd2";
		private const string Estudiante34Id = "h2445865-a24d-4543-a6c6-9443d048cdd3";
		private const string Estudiante35Id = "i3445865-a24d-4543-a6c6-9443d048cdd4";

		// Contraseñas comunes para simplificar la precarga.
		private const string CommonPasswordHash = "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==";

		/// <summary>
		/// Método principal que orquesta toda la siembra de datos.
		/// </summary>
		public static void Semilla(this ModelBuilder modelBuilder)
		{
			// --- Roles y Usuarios ---
			PrecargarRoles(modelBuilder);
			PrecargarProfesores(modelBuilder);
			PrecargarEstudiantes(modelBuilder);
			PrecargarNombresDeUsuarios(modelBuilder);
			AsignarRolesAUsuarios(modelBuilder);

			// --- Gamificación y Grupos ---
			var medallas = PrecargaDeMedallas(modelBuilder);
			var tablasEquivalencia = PrecargarTablasDeEquivalencia(modelBuilder);
			var equivalencias = PrecargarEquivalencias(modelBuilder, tablasEquivalencia);
			VincularEquivalenciasYMedallas(modelBuilder, equivalencias, medallas);
			var enlaces = PrecargarEnlacesDeUnion(modelBuilder);
			var grupos = PrecargarGrupos(modelBuilder, tablasEquivalencia, enlaces);
			var perfiles = PrecargarPerfilesDeEstudiante(modelBuilder, grupos);
			PrecargarTiendasYRecompensasSimples(modelBuilder, grupos);

			// --- Avatares ---
			PrecargarAvatares(modelBuilder, perfiles);


			// ====================================================
			// --- INICIO DE LA PRECARGA DE ATRIBUTOS DE AVATAR ---
			// ====================================================

			var atributos = PrecargarAtributosAvatar(modelBuilder);


			// ===========================================
			// --- PRECARGA DE PREGUNTAS DE SEGURIDAD ---
			// ===========================================
			PrecargarPreguntasDeSeguridad(modelBuilder);

			// ===========================================
			// --- PRECARGA DE RESPUESTAS DE SEGURIDAD ---
			// ===========================================
			PrecargarRespuestasDeSeguridad(modelBuilder);

			// =================================
			// --- PRECARGA DE TIPOS DE KUDO ---
			// =================================
			PrecargarTiposDeKudos(modelBuilder);

			// ==========================
			// --- PRECARGA DE HITOS ---
			// ==========================
			PrecargarHitos(modelBuilder);
		}
		private static void PrecargarRoles(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<IdentityRole>().HasData(
				new IdentityRole { Id = RolProfesorId, Name = "Profesor", NormalizedName = "PROFESOR" },
				new IdentityRole { Id = RolEstudianteId, Name = "Estudiante", NormalizedName = "ESTUDIANTE" }
			);
		}

		private static void PrecargarProfesores(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Profesor>().HasData(
				new Profesor
				{
					Id = Profesor1Id,
					UserName = "cecilia",
					NormalizedUserName = "CECILIA",
					Email = "cecilia@gmail.com",
					NormalizedEmail = "CECILIA@GMAIL.COM",
					EmailConfirmed = true,
					PasswordHash = CommonPasswordHash,
					SecurityStamp = "STATIC_SECURITY_STAMP_1",
					ConcurrencyStamp = "b0c8b6a8-8e6b-4e6a-9e1e-2e0b166a9c76"
				},
				new Profesor
				{
					Id = Profesor2Id,
					UserName = "laura",
					NormalizedUserName = "LAURA.FERNANDEZ",
					Email = "laura.fernandez@ludik.edu.uy",
					NormalizedEmail = "LAURA.FERNANDEZ@LUDIK.EDU.UY",
					EmailConfirmed = true,
					PasswordHash = CommonPasswordHash,
					SecurityStamp = "STATIC_SECURITY_STAMP_2",
					ConcurrencyStamp = "a1d3b5e7-9f2d-4b8c-8a1e-3f0e2d5b4a6b"
				}
			);
		}

		private static void PrecargarEstudiantes(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Estudiante>().HasData(
				new Estudiante { Id = Estudiante1Id, UserName = "usuario1", NormalizedUserName = "USUARIO1", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_3", ConcurrencyStamp = "c4b6e8a0-1d3f-4e9a-9c8e-5d2a4f6b8c0d" },
				new Estudiante { Id = Estudiante2Id, UserName = "usuario2", NormalizedUserName = "USUARIO2", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_4", ConcurrencyStamp = "d5c7f9b1-2e4g-5f0b-a0d9-6e3b5g7c9d1e" },
				new Estudiante { Id = Estudiante3Id, UserName = "usuario3", NormalizedUserName = "USUARIO3", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_5", ConcurrencyStamp = "e6d80ac2-3f5h-6g1c-b1e0-7f4c6h8d0e2f" },
				new Estudiante { Id = Estudiante4Id, UserName = "usuario4", NormalizedUserName = "USUARIO4", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_6", ConcurrencyStamp = "f7e91bd3-4g6i-7h2d-c2f1-8g5d7i9e1f3g" },
				new Estudiante { Id = Estudiante5Id, UserName = "usuario5", NormalizedUserName = "USUARIO5", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_7", ConcurrencyStamp = "g8f02ce4-5h7j-8i3e-d3g2-9h6e8j0f2g4h" },

				new Estudiante { Id = Estudiante6Id, UserName = "usuario6", NormalizedUserName = "USUARIO6", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_8", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c06" },
				new Estudiante { Id = Estudiante7Id, UserName = "usuario7", NormalizedUserName = "USUARIO7", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_9", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c07" },
				new Estudiante { Id = Estudiante8Id, UserName = "usuario8", NormalizedUserName = "USUARIO8", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_10", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c08" },
				new Estudiante { Id = Estudiante9Id, UserName = "usuario9", NormalizedUserName = "USUARIO9", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_11", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c09" },
				new Estudiante { Id = Estudiante10Id, UserName = "usuario10", NormalizedUserName = "USUARIO10", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_12", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c0a" },
				new Estudiante { Id = Estudiante11Id, UserName = "usuario11", NormalizedUserName = "USUARIO11", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_13", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c0b" },
				new Estudiante { Id = Estudiante12Id, UserName = "usuario12", NormalizedUserName = "USUARIO12", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_14", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c0c" },
				new Estudiante { Id = Estudiante13Id, UserName = "usuario13", NormalizedUserName = "USUARIO13", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_15", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c0d" },
				new Estudiante { Id = Estudiante14Id, UserName = "usuario14", NormalizedUserName = "USUARIO14", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_16", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c0e" },
				new Estudiante { Id = Estudiante15Id, UserName = "usuario15", NormalizedUserName = "USUARIO15", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_17", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c0f" },
				new Estudiante { Id = Estudiante16Id, UserName = "usuario16", NormalizedUserName = "USUARIO16", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_18", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c10" },
				new Estudiante { Id = Estudiante17Id, UserName = "usuario17", NormalizedUserName = "USUARIO17", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_19", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c11" },
				new Estudiante { Id = Estudiante18Id, UserName = "usuario18", NormalizedUserName = "USUARIO18", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_20", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c12" },
				new Estudiante { Id = Estudiante19Id, UserName = "usuario19", NormalizedUserName = "USUARIO19", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_21", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c13" },
				new Estudiante { Id = Estudiante20Id, UserName = "usuario20", NormalizedUserName = "USUARIO20", PasswordHash = CommonPasswordHash, SecurityStamp = "STATIC_SECURITY_STAMP_22", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c14" },
				new Estudiante { Id = Estudiante21Id, UserName = "manuel", NormalizedUserName = "MANUEL", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_23", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c15" },
				new Estudiante { Id = Estudiante22Id, UserName = "ana", NormalizedUserName = "ANA", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_24", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c16" },
				new Estudiante { Id = Estudiante23Id, UserName = "facundo", NormalizedUserName = "FACUNDO", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_25", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c17" },
				new Estudiante { Id = Estudiante24Id, UserName = "daniela", NormalizedUserName = "DANIELA", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_26", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c18" },
				new Estudiante { Id = Estudiante25Id, UserName = "ignacio", NormalizedUserName = "IGNACIO", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_27", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c19" },
				new Estudiante { Id = Estudiante26Id, UserName = "romina", NormalizedUserName = "ROMINA", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_28", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c1a" },
				new Estudiante { Id = Estudiante27Id, UserName = "alejandro", NormalizedUserName = "ALEJANDRO", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_29", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c1b" },
				new Estudiante { Id = Estudiante28Id, UserName = "carolina", NormalizedUserName = "CAROLINA", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_30", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c1c" },
				new Estudiante { Id = Estudiante29Id, UserName = "bruno", NormalizedUserName = "BRUNO", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_31", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c1d" },
				new Estudiante { Id = Estudiante30Id, UserName = "gabriela", NormalizedUserName = "GABRIELA", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_32", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c1e" },
				new Estudiante { Id = Estudiante31Id, UserName = "leandro", NormalizedUserName = "LEANDRO", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_33", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c1f" },
				new Estudiante { Id = Estudiante32Id, UserName = "andrea", NormalizedUserName = "ANDREA", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_34", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c20" },
				new Estudiante { Id = Estudiante33Id, UserName = "guillermo", NormalizedUserName = "GUILLERMO", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_35", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c21" },
				new Estudiante { Id = Estudiante34Id, UserName = "jimena", NormalizedUserName = "JIMENA", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_36", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c22" },
				new Estudiante { Id = Estudiante35Id, UserName = "mateo", NormalizedUserName = "MATEO", PasswordHash = "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", SecurityStamp = "STATIC_SECURITY_STAMP_37", ConcurrencyStamp = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c23" }

			);
		}

		private static void PrecargarNombresDeUsuarios(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Usuario>().OwnsOne(u => u.NombreCompleto).HasData(
				// Profesores
				new { UsuarioId = Profesor1Id, Nombre = "Carlos", Apellido = "Rodríguez" },
				new { UsuarioId = Profesor2Id, Nombre = "Laura", Apellido = "Fernández" },
				// Estudiantes
				new { UsuarioId = Estudiante1Id, Nombre = "Santiago", Apellido = "Pérez" },
				new { UsuarioId = Estudiante2Id, Nombre = "Valentina", Apellido = "Gómez" },
				new { UsuarioId = Estudiante3Id, Nombre = "Matías", Apellido = "González" },
				new { UsuarioId = Estudiante4Id, Nombre = "Camila", Apellido = "Martínez" },
				new { UsuarioId = Estudiante5Id, Nombre = "Lucas", Apellido = "Silva" },
				new { UsuarioId = Estudiante6Id, Nombre = "Sofía", Apellido = "Rodríguez" },
				new { UsuarioId = Estudiante7Id, Nombre = "Juan", Apellido = "García" },
				new { UsuarioId = Estudiante8Id, Nombre = "Lucía", Apellido = "Sánchez" },
				new { UsuarioId = Estudiante9Id, Nombre = "Diego", Apellido = "López" },
				new { UsuarioId = Estudiante10Id, Nombre = "Martina", Apellido = "Díaz" },
				new { UsuarioId = Estudiante11Id, Nombre = "Agustín", Apellido = "Torres" },
				new { UsuarioId = Estudiante12Id, Nombre = "María", Apellido = "Romero" },
				new { UsuarioId = Estudiante13Id, Nombre = "Nicolás", Apellido = "Álvarez" },
				new { UsuarioId = Estudiante14Id, Nombre = "Paula", Apellido = "Ruiz" },
				new { UsuarioId = Estudiante15Id, Nombre = "Federico", Apellido = "Vázquez" },
				new { UsuarioId = Estudiante16Id, Nombre = "Florencia", Apellido = "Sosa" },
				new { UsuarioId = Estudiante17Id, Nombre = "Sebastián", Apellido = "Castro" },
				new { UsuarioId = Estudiante18Id, Nombre = "Victoria", Apellido = "Giménez" },
				new { UsuarioId = Estudiante19Id, Nombre = "Joaquín", Apellido = "Acosta" },
				new { UsuarioId = Estudiante20Id, Nombre = "Julieta", Apellido = "Ramos" },
				new { UsuarioId = Estudiante21Id, Nombre = "Manuel", Apellido = "Herrera" },
				new { UsuarioId = Estudiante22Id, Nombre = "Ana", Apellido = "Medina" },
				new { UsuarioId = Estudiante23Id, Nombre = "Facundo", Apellido = "Morales" },
				new { UsuarioId = Estudiante24Id, Nombre = "Daniela", Apellido = "Núñez" },
				new { UsuarioId = Estudiante25Id, Nombre = "Ignacio", Apellido = "Flores" },
				new { UsuarioId = Estudiante26Id, Nombre = "Romina", Apellido = "Ríos" },
				new { UsuarioId = Estudiante27Id, Nombre = "Alejandro", Apellido = "Pereyra" },
				new { UsuarioId = Estudiante28Id, Nombre = "Carolina", Apellido = "Cabrera" },
				new { UsuarioId = Estudiante29Id, Nombre = "Bruno", Apellido = "Castillo" },
				new { UsuarioId = Estudiante30Id, Nombre = "Gabriela", Apellido = "Paz" },
				new { UsuarioId = Estudiante31Id, Nombre = "Leandro", Apellido = "Molina" },
				new { UsuarioId = Estudiante32Id, Nombre = "Andrea", Apellido = "Vega" },
				new { UsuarioId = Estudiante33Id, Nombre = "Guillermo", Apellido = "Rojas" },
				new { UsuarioId = Estudiante34Id, Nombre = "Jimena", Apellido = "Ortiz" },
				new { UsuarioId = Estudiante35Id, Nombre = "Mateo", Apellido = "Benítez" }
			);
		}

		private static void AsignarRolesAUsuarios(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(
				new IdentityUserRole<string> { UserId = Profesor1Id, RoleId = RolProfesorId },
				new IdentityUserRole<string> { UserId = Profesor2Id, RoleId = RolProfesorId },
				new IdentityUserRole<string> { UserId = Estudiante1Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante2Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante3Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante4Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante5Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante6Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante7Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante8Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante9Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante10Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante11Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante12Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante13Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante14Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante15Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante16Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante17Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante18Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante19Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante20Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante21Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante22Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante23Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante24Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante25Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante26Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante27Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante28Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante29Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante30Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante31Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante32Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante33Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante34Id, RoleId = RolEstudianteId },
				new IdentityUserRole<string> { UserId = Estudiante35Id, RoleId = RolEstudianteId }

			);
		}
		private static IEnumerable<Grupo> PrecargarGrupos(ModelBuilder modelBuilder, IEnumerable<TablaEquivalencia> tablas, IEnumerable<EnlaceUnion> enlaces)
		{
			var fechaCreacion = new DateTime(2025, 6, 19, 10, 30, 0, DateTimeKind.Utc);
			var grupos = new List<Grupo>
			{
				new() { Id = 1, Nombre = "Matemática 1A - 2025", Institucion = "Liceo N°5", Materia = "Matemática", FCreacion = fechaCreacion, ProfesorId = Profesor1Id, TablaEquivalenciaId = 1, EnlaceUnionId = 1 },
				new() { Id = 2, Nombre = "Historia Universal - 2025", Institucion = "Liceo N°5", Materia = "Historia", FCreacion = fechaCreacion, ProfesorId = Profesor2Id, TablaEquivalenciaId = 2, EnlaceUnionId = 2 },
				new() { Id = 3, Nombre = "Matemática 2B - 2025", Institucion = "Liceo N°6", Materia = "Matemática", FCreacion = fechaCreacion, ProfesorId = Profesor1Id, TablaEquivalenciaId = 1, EnlaceUnionId = 3 },
				new() { Id = 4, Nombre = "Matemática Cientifico A - 2025", Institucion = "Liceo N°6", Materia = "Matemática", FCreacion = fechaCreacion, ProfesorId = Profesor1Id, TablaEquivalenciaId = 1, EnlaceUnionId = 4 }
			};
			modelBuilder.Entity<Grupo>().HasData(grupos);
			return grupos;
		}

		private static IEnumerable<PerfilEstudiante> PrecargarPerfilesDeEstudiante(ModelBuilder modelBuilder, IEnumerable<Grupo> grupos)
		{
			var perfiles = new List<PerfilEstudiante>
			{

				new PerfilEstudiante { Id = 4, Monedas = 200, MetaCalificacion = 10, EstudianteId = Estudiante4Id, GrupoId = 2, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 5, Monedas = 180, MetaCalificacion = 8, EstudianteId = Estudiante5Id, GrupoId = 2, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg", KudosDisponiblesParaOtorgar = 3 },
                
                // --- Perfiles para el Grupo 1 (total 20) ---
                new PerfilEstudiante { Id = 1, Monedas = 120, MetaCalificacion = 8, EstudianteId = Estudiante1Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 2, Monedas = 150, MetaCalificacion = 9, EstudianteId = Estudiante2Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 3, Monedas = 95, MetaCalificacion = 7, EstudianteId = Estudiante3Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 6, Monedas = 110, MetaCalificacion = 7, EstudianteId = Estudiante6Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 7, Monedas = 210, MetaCalificacion = 9, EstudianteId = Estudiante7Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 8, Monedas = 80, MetaCalificacion = 6, EstudianteId = Estudiante8Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 9, Monedas = 300, MetaCalificacion = 10, EstudianteId = Estudiante9Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 10, Monedas = 125, MetaCalificacion = 8, EstudianteId = Estudiante10Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 11, Monedas = 145, MetaCalificacion = 8, EstudianteId = Estudiante11Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 12, Monedas = 160, MetaCalificacion = 9, EstudianteId = Estudiante12Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 13, Monedas = 70, MetaCalificacion = 6, EstudianteId = Estudiante13Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 14, Monedas = 190, MetaCalificacion = 9, EstudianteId = Estudiante14Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 15, Monedas = 250, MetaCalificacion = 10, EstudianteId = Estudiante15Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 16, Monedas = 130, MetaCalificacion = 8, EstudianteId = Estudiante16Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 17, Monedas = 115, MetaCalificacion = 7, EstudianteId = Estudiante17Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 18, Monedas = 90, MetaCalificacion = 7, EstudianteId = Estudiante18Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 19, Monedas = 220, MetaCalificacion = 9, EstudianteId = Estudiante19Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 20, Monedas = 170, MetaCalificacion = 8, EstudianteId = Estudiante20Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 21, Monedas = 155, MetaCalificacion = 8, EstudianteId = Estudiante21Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 22, Monedas = 105, MetaCalificacion = 7, EstudianteId = Estudiante22Id, GrupoId = 1, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},

                // ---Perfiles para el Grupo 3 ---
                new PerfilEstudiante { Id = 23, Monedas = 100, MetaCalificacion = 7, EstudianteId = Estudiante23Id, GrupoId = 3, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 24, Monedas = 120, MetaCalificacion = 8, EstudianteId = Estudiante24Id, GrupoId = 3, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 25, Monedas = 250, MetaCalificacion = 10, EstudianteId = Estudiante25Id, GrupoId = 3, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3 },
				new PerfilEstudiante { Id = 26, Monedas = 130, MetaCalificacion = 8, EstudianteId = Estudiante26Id, GrupoId = 3, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 27, Monedas = 90, MetaCalificacion = 6, EstudianteId = Estudiante27Id, GrupoId = 3, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 28, Monedas = 160, MetaCalificacion = 9, EstudianteId = Estudiante28Id, GrupoId = 3, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3 },
				new PerfilEstudiante { Id = 29, Monedas = 175, MetaCalificacion = 9, EstudianteId = Estudiante29Id, GrupoId = 3, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 30, Monedas = 140, MetaCalificacion = 8, EstudianteId = Estudiante30Id, GrupoId = 3, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},

                // ---Perfiles para el Grupo 4 ---
                new PerfilEstudiante { Id = 31, Monedas = 110, MetaCalificacion = 7, EstudianteId = Estudiante31Id, GrupoId = 4, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 32, Monedas = 200, MetaCalificacion = 9, EstudianteId = Estudiante32Id, GrupoId = 4, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 33, Monedas = 150, MetaCalificacion = 8, EstudianteId = Estudiante33Id, GrupoId = 4, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 34, Monedas = 180, MetaCalificacion = 9, EstudianteId = Estudiante34Id, GrupoId = 4, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3},
				new PerfilEstudiante { Id = 35, Monedas = 220, MetaCalificacion = 10, EstudianteId = Estudiante35Id, GrupoId = 4, NombreImagenCompleta = "default/avatar_full.jpg", NombreImagenMiniatura = "default/avatar_thumb.jpg" , KudosDisponiblesParaOtorgar = 3}
			};
			modelBuilder.Entity<PerfilEstudiante>().HasData(perfiles);
			return perfiles;
		}

		private static IEnumerable<EnlaceUnion> PrecargarEnlacesDeUnion(ModelBuilder modelBuilder)
		{
			var fechaCreacion = new DateTime(2025, 6, 19, 10, 30, 0, DateTimeKind.Utc);
			var enlaces = new List<EnlaceUnion>
			{
				new() { Id = 1, CodigoUnico = "MAT1A25", UrlCompleta = "https://www.ludik.app/unirse/MAT1A25", Expiracion = fechaCreacion.AddDays(300) },
				new() { Id = 2, CodigoUnico = "HISTU25", UrlCompleta = "https://www.ludik.app/unirse/HISTU25", Expiracion = fechaCreacion.AddDays(300) },
				new() { Id = 3, CodigoUnico = "MAT2B", UrlCompleta = "https://www.ludik.app/unirse/FIS2B25", Expiracion = fechaCreacion.AddDays(300) },
				new() { Id = 4, CodigoUnico = "MATCIENA", UrlCompleta = "https://www.ludik.app/unirse/QUIgen25", Expiracion = fechaCreacion.AddDays(300) }
			};
			modelBuilder.Entity<EnlaceUnion>().HasData(enlaces);
			return enlaces;
		}
		private static IEnumerable<TablaEquivalencia> PrecargarTablasDeEquivalencia(ModelBuilder modelBuilder)
		{
			var tablas = new List<TablaEquivalencia>
			{
				new() { Id = 1, Nombre = "Calificaciones Estándar", ProfesorId = Profesor1Id },
				new() { Id = 2, Nombre = "Evaluación Continua", ProfesorId = Profesor2Id },

			};
			modelBuilder.Entity<TablaEquivalencia>().HasData(tablas);
			return tablas;
		}

		private static IEnumerable<Equivalencia> PrecargarEquivalencias(ModelBuilder modelBuilder, IEnumerable<TablaEquivalencia> tablas)
		{
			var equivalencias = new List<Equivalencia>
			{
				new() { Id = 1, Nota = 1, TablaEquivalenciaId = 1 },
				new() { Id = 2, Nota = 2, TablaEquivalenciaId = 1 },
				new() { Id = 3, Nota = 3, TablaEquivalenciaId = 1 },
				new() { Id = 4, Nota = 4, TablaEquivalenciaId = 1 },
				new() { Id = 5, Nota = 5, TablaEquivalenciaId = 1 },
				new() { Id = 6, Nota = 6, TablaEquivalenciaId = 1 },
				new() { Id = 7, Nota = 7, TablaEquivalenciaId = 1 },
				new() { Id = 8, Nota = 8, TablaEquivalenciaId = 1 },
				new() { Id = 9, Nota = 9, TablaEquivalenciaId = 1 },
				new() { Id = 10, Nota = 10, TablaEquivalenciaId = 1 },
				new() { Id = 11, Nota = 1, TablaEquivalenciaId = 2 },
			};
			modelBuilder.Entity<Equivalencia>().HasData(equivalencias);
			return equivalencias;
		}

		private static void VincularEquivalenciasYMedallas(ModelBuilder modelBuilder, IEnumerable<Equivalencia> equivalencias, IEnumerable<Medalla> medallas)
		{
			modelBuilder.Entity("EquivalenciaMedallas").HasData(
				// Medalla para la Tabla 2 (se mantiene igual)
				new { EquivalenciaId = 3, MedallaId = 13 },

				// --- Vinculaciones para Tabla 1 (Calificaciones Estándar) ---

				// Nota 1: Requiere 1 medalla
				new { EquivalenciaId = 1, MedallaId = 1 },

				// Nota 2: Requiere medallas de Nota 1 + 1 nueva
				new { EquivalenciaId = 2, MedallaId = 1 },
				new { EquivalenciaId = 2, MedallaId = 4 },

				// Nota 3: Requiere medallas de Nota 2 + 1 nueva
				new { EquivalenciaId = 4, MedallaId = 1 },
				new { EquivalenciaId = 4, MedallaId = 4 },
				new { EquivalenciaId = 4, MedallaId = 6 },

				// Nota 4: Requiere medallas de Nota 3 + 1 nueva
				new { EquivalenciaId = 5, MedallaId = 1 },
				new { EquivalenciaId = 5, MedallaId = 4 },
				new { EquivalenciaId = 5, MedallaId = 6 },
				new { EquivalenciaId = 5, MedallaId = 8 },

				// Nota 5: Requiere medallas de Nota 4 + 1 nueva
				new { EquivalenciaId = 6, MedallaId = 1 },
				new { EquivalenciaId = 6, MedallaId = 4 },
				new { EquivalenciaId = 6, MedallaId = 6 },
				new { EquivalenciaId = 6, MedallaId = 8 },
				new { EquivalenciaId = 6, MedallaId = 10 },

				// Nota 6: Requiere medallas de Nota 5 + 1 nueva
				new { EquivalenciaId = 7, MedallaId = 1 },
				new { EquivalenciaId = 7, MedallaId = 4 },
				new { EquivalenciaId = 7, MedallaId = 6 },
				new { EquivalenciaId = 7, MedallaId = 8 },
				new { EquivalenciaId = 7, MedallaId = 10 },
				new { EquivalenciaId = 7, MedallaId = 12 },

				// Nota 7: Requiere medallas de Nota 6 + 1 nueva
				new { EquivalenciaId = 8, MedallaId = 1 },
				new { EquivalenciaId = 8, MedallaId = 4 },
				new { EquivalenciaId = 8, MedallaId = 6 },
				new { EquivalenciaId = 8, MedallaId = 8 },
				new { EquivalenciaId = 8, MedallaId = 10 },
				new { EquivalenciaId = 8, MedallaId = 12 },
				new { EquivalenciaId = 8, MedallaId = 2 },

				// Nota 8: Requiere medallas de Nota 7 + 1 nueva
				new { EquivalenciaId = 9, MedallaId = 1 },
				new { EquivalenciaId = 9, MedallaId = 4 },
				new { EquivalenciaId = 9, MedallaId = 6 },
				new { EquivalenciaId = 9, MedallaId = 8 },
				new { EquivalenciaId = 9, MedallaId = 10 },
				new { EquivalenciaId = 9, MedallaId = 12 },
				new { EquivalenciaId = 9, MedallaId = 2 },
				new { EquivalenciaId = 9, MedallaId = 3 },

				// Nota 9: Requiere medallas de Nota 8 + 1 nueva
				new { EquivalenciaId = 10, MedallaId = 1 },
				new { EquivalenciaId = 10, MedallaId = 4 },
				new { EquivalenciaId = 10, MedallaId = 6 },
				new { EquivalenciaId = 10, MedallaId = 8 },
				new { EquivalenciaId = 10, MedallaId = 10 },
				new { EquivalenciaId = 10, MedallaId = 12 },
				new { EquivalenciaId = 10, MedallaId = 2 },
				new { EquivalenciaId = 10, MedallaId = 3 },
				new { EquivalenciaId = 10, MedallaId = 5 },

				// Nota 10: Requiere medallas de Nota 9 + 1 nueva
				new { EquivalenciaId = 11, MedallaId = 1 },
				new { EquivalenciaId = 11, MedallaId = 4 },
				new { EquivalenciaId = 11, MedallaId = 6 },
				new { EquivalenciaId = 11, MedallaId = 8 },
				new { EquivalenciaId = 11, MedallaId = 10 },
				new { EquivalenciaId = 11, MedallaId = 12 },
				new { EquivalenciaId = 11, MedallaId = 2 },
				new { EquivalenciaId = 11, MedallaId = 3 },
				new { EquivalenciaId = 11, MedallaId = 5 },
				new { EquivalenciaId = 11, MedallaId = 7 }
			);
		}

		private static IEnumerable<Medalla> PrecargaDeMedallas(ModelBuilder modelBuilder)
		{
			var profesor1Id = "8e445865-a24d-4543-a6c6-9443d048cdb9";
			var profesor2Id = "9e445865-a24d-4543-a6c6-9443d048cdb0";
			var medallas = new List<Medalla>
	        {
                // Medallas de tu ejemplo original
                new Medalla { Id = 1, Nombre = "Participación Perfecta", Descripcion = "Asistencia y participación en todas las clases del mes.", NombreIcono = "graduation-cap", MonedasOtorgadas = 30, ProfesorId = profesor1Id },
		        new Medalla { Id = 2, Nombre = "Maestro de la Colaboración", Descripcion = "Ayuda destacada a compañeros en proyectos grupales.", NombreIcono = "people-group", MonedasOtorgadas = 25, ProfesorId = profesor1Id },
		        new Medalla { Id = 3, Nombre = "Mente Curiosa", Descripcion = "Realización de preguntas perspicaces que enriquecen la clase.", NombreIcono = "magnifying-glass", MonedasOtorgadas = 15, ProfesorId = profesor1Id },

                // --- INICIO DE MEDALLAS ASOCIADAS A KUDOS ---

                // Medalla por Kudo "Gracias por la Ayuda"
                new Medalla { Id = 4, Nombre = "Compañerismo", Descripcion = "Se otorga por ser un pilar de apoyo para tus compañeros. Demuestra que estás siempre dispuesto a ofrecer tu ayuda cuando alguien la necesita.", NombreIcono = "handshake", MonedasOtorgadas = 20, ProfesorId = profesor1Id },

                // Medalla por Kudo "Esa Pregunta Suma"
                new Medalla { Id = 5, Nombre = "Curiosidad Insaciable", Descripcion = "Premia a las mentes que nunca dejan de preguntar. Se consigue al realizar preguntas que desafían al grupo y enriquecen el aprendizaje de todos.", NombreIcono = "lightbulb", MonedasOtorgadas = 15, ProfesorId = profesor1Id },

                // Medalla por Kudo "Inspirador"
                new Medalla { Id = 6, Nombre = "Faro del Grupo", Descripcion = "Reconoce a quienes inspiran con su ejemplo. Se obtiene al demostrar una actitud y un esfuerzo que motivan a todo el grupo a superarse.", NombreIcono = "star", MonedasOtorgadas = 25, ProfesorId = profesor1Id },

                // Medalla por Kudo "Conectando Ideas"
                new Medalla { Id = 7, Nombre = "Arquitecto de Ideas", Descripcion = "Para aquellos que no solo tienen buenas ideas, sino que construyen sobre las de los demás para crear algo aún mejor.", NombreIcono = "brain", MonedasOtorgadas = 20, ProfesorId = profesor1Id },

                // Medalla por Kudo "Líder de Equipo"
                new Medalla { Id = 8, Nombre = "Capitán de Equipo", Descripcion = "Se otorga por demostrar liderazgo natural, guiando y organizando al equipo para alcanzar metas comunes de forma efectiva.", NombreIcono = "crown", MonedasOtorgadas = 25, ProfesorId = profesor1Id },

                // Medalla por Kudo "Recurso Valioso"
                new Medalla { Id = 9, Nombre = "Cazador de Tesoros", Descripcion = "Premia la iniciativa de buscar y compartir recursos valiosos (videos, artículos, herramientas) que benefician a toda la clase.", NombreIcono = "gem", MonedasOtorgadas = 15, ProfesorId = profesor1Id },

                // Medalla por Kudo "Codo a Codo"
                new Medalla { Id = 10, Nombre = "Espíritu de Equipo", Descripcion = "Se consigue al fomentar activamente un ambiente de respeto e inclusión, asegurando que cada miembro del grupo se sienta valorado.", NombreIcono = "hands-clapping", MonedasOtorgadas = 20, ProfesorId = profesor1Id },

                // Medalla por Kudo "Crítica que Construye"
                new Medalla { Id = 11, Nombre = "Pulidor de Diamantes", Descripcion = "Reconoce la habilidad de dar críticas constructivas que ayudan a los compañeros a mejorar su trabajo de forma positiva y amable.", NombreIcono = "diamond", MonedasOtorgadas = 15, ProfesorId = profesor1Id },

                // Medalla por Kudo "Chispa Creativa"
                new Medalla { Id = 12, Nombre = "Mente Innovadora", Descripcion = "Se otorga por aportar ideas creativas y soluciones originales que sacan al grupo de la rutina y abren nuevas posibilidades.", NombreIcono = "wand-magic-sparkles", MonedasOtorgadas = 20, ProfesorId = profesor1Id },

                // Medalla por Kudo "Einstein"
                new Medalla { Id = 13, Nombre = "El Explicador", Descripcion = "Premia la increíble habilidad de tomar un tema complejo y explicarlo de una manera tan clara y sencilla que todos puedan entenderlo.", NombreIcono = "chalkboard", MonedasOtorgadas = 25, ProfesorId = profesor2Id }
	        };
			modelBuilder.Entity<Medalla>().HasData(medallas);
			return medallas;
		}



		private static void PrecargarTiendasYRecompensasSimples(ModelBuilder modelBuilder, IEnumerable<Grupo> grupos)
		{
			var tiendas = new List<object>
			{
				new { Id = 1, GrupoId = 1 },
				new { Id = 2, GrupoId = 2 },
				new { Id = 3, GrupoId = 3 },
				new { Id = 4, GrupoId = 4 }
			};
			modelBuilder.Entity<Tienda>().HasData(tiendas);



			// 1. Crear una lista de las entidades RecompensaSimple
			var recompensasSimples = new List<RecompensaSimple>
			{
                // Tienda 1
                new RecompensaSimple { Id = 1, Nombre = "Estrella Mágica", Precio = 50 },
				new RecompensaSimple { Id = 2, Nombre = "Regalo Sorpresa", Precio = 30 },
				new RecompensaSimple { Id = 3, Nombre = "Corazón Brillante", Precio = 20 },
				new RecompensaSimple { Id = 4, Nombre = "Medalla de Oro", Precio = 80 },
				new RecompensaSimple { Id = 5, Nombre = "Montón de Monedas", Precio = 100 },
                // Tienda 2
                new RecompensaSimple { Id = 6, Nombre = "Trofeo Brillante", Precio = 70 },
				new RecompensaSimple { Id = 7, Nombre = "Llama de Fuego", Precio = 40 },
				new RecompensaSimple { Id = 8, Nombre = "Corona Real", Precio = 90 },
				new RecompensaSimple { Id = 9, Nombre = "Cohete Espacial", Precio = 60 },
				new RecompensaSimple { Id = 10, Nombre = "Robot Amistoso", Precio = 55 }
			};

			// 2. Asignar los datos específicos a la propiedad Representacion
			//    El constructor de RecompensaSimple ya creó el objeto RepresentacionIcono, solo necesitamos poblarlo.
			((RepresentacionIcono)recompensasSimples[0].Representacion).NombreIcono = "star";
			((RepresentacionIcono)recompensasSimples[1].Representacion).NombreIcono = "gift";
			((RepresentacionIcono)recompensasSimples[2].Representacion).NombreIcono = "heart";
			((RepresentacionIcono)recompensasSimples[3].Representacion).NombreIcono = "medal";
			((RepresentacionIcono)recompensasSimples[4].Representacion).NombreIcono = "coins";
			((RepresentacionIcono)recompensasSimples[5].Representacion).NombreIcono = "trophy";
			((RepresentacionIcono)recompensasSimples[6].Representacion).NombreIcono = "fire";
			((RepresentacionIcono)recompensasSimples[7].Representacion).NombreIcono = "crown";
			((RepresentacionIcono)recompensasSimples[8].Representacion).NombreIcono = "rocket";
			((RepresentacionIcono)recompensasSimples[9].Representacion).NombreIcono = "robot";

			// 3. Usar la lista de entidades reales en HasData
			modelBuilder.Entity<RecompensaSimple>().HasData(recompensasSimples);
		}

		private static void PrecargarAvatares(ModelBuilder modelBuilder, IEnumerable<PerfilEstudiante> perfiles)
		{
			var avatares = new List<Avatar>
			{
				new() { Id = 1, PerfilEstudianteId = 1, ColorFondo = "#b1e2ff" },
				new() { Id = 2, PerfilEstudianteId = 2, ColorFondo = "#a7ffc4" },
				new() { Id = 3, PerfilEstudianteId = 3, ColorFondo = "#ffafb9" },
				new() { Id = 4, PerfilEstudianteId = 4, ColorFondo = "#ffffb1" },
				new() { Id = 5, PerfilEstudianteId = 5, ColorFondo = "#e6e6e6" },
				new() { Id = 6, PerfilEstudianteId = 6, ColorFondo = "#f8d7da" },
				new() { Id = 7, PerfilEstudianteId = 7, ColorFondo = "#d4edda" },
				new() { Id = 8, PerfilEstudianteId = 8, ColorFondo = "#fff3cd" },
				new() { Id = 9, PerfilEstudianteId = 9, ColorFondo = "#d1ecf1" },
				new() { Id = 10, PerfilEstudianteId = 10, ColorFondo = "#e2d9f3" },
				new() { Id = 11, PerfilEstudianteId = 11, ColorFondo = "#fce3d4" },
				new() { Id = 12, PerfilEstudianteId = 12, ColorFondo = "#c3e6cb" },
				new() { Id = 13, PerfilEstudianteId = 13, ColorFondo = "#f5c6cb" },
				new() { Id = 14, PerfilEstudianteId = 14, ColorFondo = "#bee5eb" },
				new() { Id = 15, PerfilEstudianteId = 15, ColorFondo = "#ffeeba" },
				new() { Id = 16, PerfilEstudianteId = 16, ColorFondo = "#d6d8f5" },
				new() { Id = 17, PerfilEstudianteId = 17, ColorFondo = "#fde2e2" },
				new() { Id = 18, PerfilEstudianteId = 18, ColorFondo = "#d1e7dd" },
				new() { Id = 19, PerfilEstudianteId = 19, ColorFondo = "#cce7ff" },
				new() { Id = 20, PerfilEstudianteId = 20, ColorFondo = "#fbf8cc" },
				new() { Id = 21, PerfilEstudianteId = 21, ColorFondo = "#f1e0ff" },
				new() { Id = 22, PerfilEstudianteId = 22, ColorFondo = "#e0f7fa" },
				new() { Id = 23, PerfilEstudianteId = 23, ColorFondo = "#ffe0e0" },
				new() { Id = 24, PerfilEstudianteId = 24, ColorFondo = "#e0ffe0" },
				new() { Id = 25, PerfilEstudianteId = 25, ColorFondo = "#e0e0ff" },
				new() { Id = 26, PerfilEstudianteId = 26, ColorFondo = "#fff0e0" },
				new() { Id = 27, PerfilEstudianteId = 27, ColorFondo = "#f0fff0" },
				new() { Id = 28, PerfilEstudianteId = 28, ColorFondo = "#f0f0ff" },
				new() { Id = 29, PerfilEstudianteId = 29, ColorFondo = "#e0fff8" },
				new() { Id = 30, PerfilEstudianteId = 30, ColorFondo = "#f8e0ff" },
				new() { Id = 31, PerfilEstudianteId = 31, ColorFondo = "#eaf5ff" },
				new() { Id = 32, PerfilEstudianteId = 32, ColorFondo = "#fff5e6" },
				new() { Id = 33, PerfilEstudianteId = 33, ColorFondo = "#f2f2f2" },
				new() { Id = 34, PerfilEstudianteId = 34, ColorFondo = "#e6f7ff" },
				new() { Id = 35, PerfilEstudianteId = 35, ColorFondo = "#fae6ff" }
			};
			modelBuilder.Entity<Avatar>().HasData(avatares);
		}


		private static void PrecargarRespuestasDeSeguridad(ModelBuilder modelBuilder)
		{
			// --- IDs de los estudiantes a los que asignaremos respuestas ---
			var estudiante1Id = "a1445865-a24d-4543-a6c6-9443d048cdb1"; // santiago
			var estudiante2Id = "b2445865-a24d-4543-a6c6-9443d048cdb2"; // valentina

			// --- Hashes Pre-generados para las respuestas ---
			// Respuestas para Santiago: "Cecilia1." y "Cecilia1."
			var hashPrimaria = "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==";
			var hashMascota = "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==";

			// Respuestas para Valentina: "Cecilia1." y "Cecilia1."
			var hashAbuela = "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==";
			var hashPersonaje = "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==";

			modelBuilder.Entity<PreguntaRespuestaSeguridad>().HasData(
				// --- Respuestas para Santiago ---
				new PreguntaRespuestaSeguridad
				{
					Id = 1, // PK de esta tabla
					PreguntaDeSeguridadId = 1, // FK a "¿Cuál era el nombre de tu escuela primaria?"
					Respuesta = hashPrimaria,
					EstudianteId = estudiante1Id
				},
				new PreguntaRespuestaSeguridad
				{
					Id = 2,
					PreguntaDeSeguridadId = 3, // FK a "¿Cuál era el nombre de tu primera mascota?"
					Respuesta = hashMascota,
					EstudianteId = estudiante1Id
				},

				// --- Respuestas para Valentina ---
				new PreguntaRespuestaSeguridad
				{
					Id = 3,
					PreguntaDeSeguridadId = 2, // FK a "¿Cuál es el primer nombre de tu abuela materna?"
					Respuesta = hashAbuela,
					EstudianteId = estudiante2Id
				},
				new PreguntaRespuestaSeguridad
				{
					Id = 4,
					PreguntaDeSeguridadId = 5, // FK a "¿Cuál es el nombre de tu personaje de ficción favorito...?"
					Respuesta = hashPersonaje,
					EstudianteId = estudiante2Id
				}
			);
		}

		private static void PrecargarPreguntasDeSeguridad(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PreguntaDeSeguridad>().HasData(
				new PreguntaDeSeguridad { Id = 1, Texto = "¿Cuál era el nombre de tu escuela primaria?" },
				new PreguntaDeSeguridad { Id = 2, Texto = "¿Cuál es el primer nombre de tu abuela materna?" },
				new PreguntaDeSeguridad { Id = 3, Texto = "¿Cuál era el nombre de tu primera mascota?" },
				new PreguntaDeSeguridad { Id = 4, Texto = "¿Cuál era el apodo que te decía tu familia en la infancia?" },
				new PreguntaDeSeguridad { Id = 5, Texto = "¿Cuál es el nombre de tu personaje de ficción favorito (de un libro, serie o videojuego)?" },
				new PreguntaDeSeguridad { Id = 6, Texto = "¿Cuál fue el primer videojuego que lograste completar?" },
				new PreguntaDeSeguridad { Id = 7, Texto = "Si pudieras tener un superpoder, ¿cuál sería?" },
				new PreguntaDeSeguridad { Id = 8, Texto = "¿Cuál es el apellido del primer amigo o amiga que hiciste al empezar el liceo?" },
				new PreguntaDeSeguridad { Id = 9, Texto = "¿Cuál es el nombre del hospital donde naciste?" }



			);
		}

		private static List<AtributoAvatar> PrecargarAtributosAvatar(ModelBuilder modelBuilder)
		{
			int idCounter = 1;
			var atributos = new List<AtributoAvatar>();
			Func<string, string> capitalizar = s => char.ToUpper(s[0]) + s.Substring(1);
			var mapeoPrefijos = new Dictionary<TipoAtributo, string>
			{
				{ TipoAtributo.Pelo, "top-" },
				{ TipoAtributo.Cejas, "eyebrows-" },
				{ TipoAtributo.Ojos, "eyes-" },
				{ TipoAtributo.Boca, "mouth-" },
				{ TipoAtributo.Barba, "beard-" },
				{ TipoAtributo.Gafas, "accessories-" },
				{ TipoAtributo.Ropa, "clothing-" },
				{ TipoAtributo.ColorPiel, "skinColor-" },
				{ TipoAtributo.ColorPelo, "hairColor-" },
				{ TipoAtributo.ColorBarba, "beardColor-" },
				{ TipoAtributo.ColorRopa, "clothesColor-" },
				{ TipoAtributo.ColorGafas, "accessoriesColor-" }
			};

			var datos = new Dictionary<TipoAtributo, string[]>
			{
				{ TipoAtributo.Pelo, new[] { "curly", "curvy", "dreads", "dreads01", "dreads02", "frida", "frizzle", "fro", "froBand", "longButNotTooLong" } },
				{ TipoAtributo.Cejas, new[] { "angryNatural", "defaultNatural", "flatNatural", "frownNatural", "raisedExcitedNatural", "sadConcernedNatural", "unibrowNatural", "upDownNatural" } },
				{ TipoAtributo.Ojos, new[] { "closed", "cry", "default", "happy", "hearts", "side", "squint", "surprised", "wink", "winkWacky", "xDizzy" } },
				{ TipoAtributo.Boca, new[] { "concerned", "default", "disbelief", "eating", "grimace", "sad", "screamOpen" } },
				{ TipoAtributo.Barba, new string[] { } },
				{ TipoAtributo.Gafas, new[] { "eyepatch", "kurt", "none", "prescription01", "prescription02", "round", "sunglasses", "wayfarers" } },
				{ TipoAtributo.Ropa, new[] { "blazerAndShirt", "blazerAndSweater", "collarAndSweater", "hoodie", "overall", "shirtCrewNeck", "shirtScoopNeck", "shirtVNeck" } },
				{ TipoAtributo.ColorPiel, new[] { "#614335", "#ae5d29", "#d08b5b", "#edb98a", "#f8d25c", "#fd9841", "#ffdbb4" } },
				{ TipoAtributo.ColorPelo, new[] { "#2c1b18", "#4a312c", "#724133", "#a55728", "#b58143", "#c93305", "#d6b370", "#e8e1e1", "#ecdcbf", "#f59797" } },
				{ TipoAtributo.ColorBarba, new[] { "#2c1b18", "#4a312c", "#724133", "#a55728", "#b58143", "#c93305", "#d6b370", "#e8e1e1", "#ecdcbf", "#f59797" } },
				{ TipoAtributo.ColorRopa, new[] { "#3c4f5c", "#65c9ff", "#262e33", "#5199e4", "#25557c", "#929598", "#a7ffc4", "#b1e2ff", "#e6e6e6", "#ff5c5c", "#ff488e", "#ffafb9", "#ffffb1", "#ffffff" } },
				{ TipoAtributo.ColorGafas, new[] { "#3c4f5c", "#65c9ff", "#262e33", "#5199e4", "#25557c", "#929598", "#a7ffc4", "#b1e2ff", "#e6e6e6", "#ff5c5c", "#ff488e", "#ffafb9", "#ffdeb5", "#ffffb1", "#ffffff" } }
			};

			foreach (var kvp in datos)
			{

				var prefijo = mapeoPrefijos[kvp.Key];
				var tipoAtributo = kvp.Key;

				foreach (var codigo in kvp.Value)
				{
					atributos.Add(new AtributoAvatar
					{
						Id = idCounter++,
						Tipo = tipoAtributo,
						Nombre = tipoAtributo.ToString().Contains("Color") ? codigo : capitalizar(codigo),
						CodigoUnico = codigo,
						NombreImagenRecurso = $"{prefijo}{codigo}.png"
					});
				}
			}

			modelBuilder.Entity<AtributoAvatar>().HasData(atributos);
			return atributos;
		}


		private static void PrecargarHitos(ModelBuilder modelBuilder)
		{
			// 1. CREAR LAS RECOMPENSAS (POTENCIADORES)
			// Se ajusta el rango de multiplicadores y se utiliza TimeSpan para la duración.
			var potenciadores = new List<Potenciador>
			{
				new Potenciador { Id = 101, Nombre = "Bono x1.5 (24h)", Multiplicador = 1.5, DuracionHoras = 24 },
				new Potenciador { Id = 102, Nombre = "Bono x1.6 (24h)", Multiplicador = 1.6, DuracionHoras = 24 },
				new Potenciador { Id = 103, Nombre = "Bono x1.7 (48h)", Multiplicador = 1.7, DuracionHoras = 48 },
				new Potenciador { Id = 104, Nombre = "Bono x1.8 (48h)", Multiplicador = 1.8, DuracionHoras = 48 },
				new Potenciador { Id = 105, Nombre = "Bono x1.9 (72h)", Multiplicador = 1.9, DuracionHoras = 72 },
				new Potenciador { Id = 106, Nombre = "¡Doble Moneda! (72h)", Multiplicador = 2.0, DuracionHoras = 72 },
				new Potenciador { Id = 107, Nombre = "Bono x2.1 (96h)", Multiplicador = 2.1, DuracionHoras = 96 },
				new Potenciador { Id = 108, Nombre = "Bono x2.2 (96h)", Multiplicador = 2.2, DuracionHoras = 96 },
				new Potenciador { Id = 109, Nombre = "Bono x2.3 (120h)", Multiplicador = 2.3, DuracionHoras = 120 },
				new Potenciador { Id = 110, Nombre = "¡Super Bono x2.5! (168h)", Multiplicador = 2.5, DuracionHoras = 168 }
			};

			modelBuilder.Entity<Potenciador>().HasData(potenciadores);

			// 2. CREAR LOS HITOS Y ASOCIARLOS A LAS RECOMPENSAS (esta parte no cambia)
			modelBuilder.Entity<Hito>().HasData(
				new Hito { Id = 1, CantMedallasRequeridas = 5, RecompensaId = 101 },
				new Hito { Id = 2, CantMedallasRequeridas = 10, RecompensaId = 102 },
				new Hito { Id = 3, CantMedallasRequeridas = 20, RecompensaId = 103 },
				new Hito { Id = 4, CantMedallasRequeridas = 35, RecompensaId = 104 },
				new Hito { Id = 5, CantMedallasRequeridas = 50, RecompensaId = 105 },
				new Hito { Id = 6, CantMedallasRequeridas = 75, RecompensaId = 106 },
				new Hito { Id = 7, CantMedallasRequeridas = 100, RecompensaId = 107 },
				new Hito { Id = 8, CantMedallasRequeridas = 150, RecompensaId = 108 },
				new Hito { Id = 9, CantMedallasRequeridas = 200, RecompensaId = 109 },
				new Hito { Id = 10, CantMedallasRequeridas = 250, RecompensaId = 110 }
			);
		}
		private static void PrecargarTiposDeKudos(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TipoKudo>().HasData(
			new TipoKudo
			{
				Id = 1,
				Nombre = "Gracias por la Ayuda",
				Descripcion = "Considera dar este kudo cuando un compañero te dedica tiempo para explicarte algo que no entendías o te ayuda a completar una tarea.",
				NombreIcono = "thumbs-up"
			},
			new TipoKudo
			{
				Id = 2,
				Nombre = "Esa Pregunta Suma",
				Descripcion = "Considera dar este kudo cuando la pregunta de un compañero aclara una duda para todo el grupo o genera un debate que enriquece la clase.",
				NombreIcono = "hands-clapping"
			},
			new TipoKudo
			{
				Id = 3,
				Nombre = "Inspirador",
				Descripcion = "Considera dar este kudo cuando el esfuerzo, la perseverancia o la actitud positiva de un compañero te motiven a superarte.",
				NombreIcono = "running"
			},
			new TipoKudo
			{
				Id = 4,
				Nombre = "Conectando Ideas",
				Descripcion = "Considera dar este kudo cuando un compañero toma tu idea o la de alguien más y la mejora, aportando un punto de vista que hace el trabajo más fuerte.",
				NombreIcono = "lightbulb"
			},
			new TipoKudo
			{
				Id = 5,
				Nombre = "Líder de Equipo",
				Descripcion = "Considera dar este kudo cuando un compañero organiza el trabajo en equipo, se asegura de que todos participen o guía al grupo para cumplir el objetivo.",
				NombreIcono = "star"
			},
			new TipoKudo
			{
				Id = 6,
				Nombre = "Bibliotecario",
				Descripcion = "Considera dar este kudo cuando un compañero comparte un enlace, video, apunte o cualquier material que te resultó muy útil para estudiar o hacer una tarea.",
				NombreIcono = "book"
			},
			new TipoKudo
			{
				Id = 7,
				Nombre = "Codo a Codo",
				Descripcion = "Considera dar este kudo cuando notes que un compañero se esfuerza por integrar a otros, asegurándose de que nadie se quede atrás y todos se sientan parte del equipo.",
				NombreIcono = "face-smile"
			},
			new TipoKudo
			{
				Id = 8,
				Nombre = "Crítica que Construye",
				Descripcion = "Considera dar este kudo cuando un compañero te da una sugerencia para mejorar tu trabajo de forma respetuosa y con la intención real de ayudar.",
				NombreIcono = "hammer"
			},
			new TipoKudo
			{
				Id = 9,
				Nombre = "Chispa Creativa",
				Descripcion = "Considera dar este kudo cuando un compañero propone una solución original a un problema o una idea innovadora para un proyecto que sorprende al grupo.",
				NombreIcono = "fire"
			},
			new TipoKudo
			{
				Id = 10,
				Nombre = "Einstein",
				Descripcion = "Considera dar este kudo cuando la explicación de un compañero sobre un tema muy difícil hace que, finalmente, lo entiendas con total claridad.",
				NombreIcono = "brain"
			}
			);
		}
	}
}