using AccesoDatos.RepositoriosEF;
using AccesoDatos.Servicios;
using Azure.Identity;
using Azure.Storage.Blobs;
using Hangfire;
using Hangfire.SqlServer;
using InterfacesRepositorio;
using LogicaAplicacion.ImplementacionCasosUsos.AsignarMedalla;
using LogicaAplicacion.ImplementacionCasosUsos.Avatar;
using LogicaAplicacion.ImplementacionCasosUsos.BarraProgreso;
using LogicaAplicacion.ImplementacionCasosUsos.Estudiantes;
using LogicaAplicacion.ImplementacionCasosUsos.Grupos;
using LogicaAplicacion.ImplementacionCasosUsos.Imagenes;
using LogicaAplicacion.ImplementacionCasosUsos.Imagenes.Estrategias;
using LogicaAplicacion.ImplementacionCasosUsos.Kudo;
using LogicaAplicacion.ImplementacionCasosUsos.Login;
using LogicaAplicacion.ImplementacionCasosUsos.Medallas;
using LogicaAplicacion.ImplementacionCasosUsos.PerfilEstudiante;
using LogicaAplicacion.ImplementacionCasosUsos.Profesores;
using LogicaAplicacion.ImplementacionCasosUsos.Recompensa;
using LogicaAplicacion.ImplementacionCasosUsos.RecuperarContrasena;
using LogicaAplicacion.ImplementacionCasosUsos.SolicitudUnion;
using LogicaAplicacion.ImplementacionCasosUsos.TablaClasificacion;
using LogicaAplicacion.ImplementacionCasosUsos.TablaEquivalencia;
using LogicaAplicacion.ImplementacionCasosUsos.Tienda;
using LogicaAplicacion.ImplementacionCasosUsos.UmbralParaObtenerMedallaPorKudos;
using LogicaAplicacion.ImplementacionServicios;
using LogicaAplicacion.InterfacesCasosUsos.AsignacionMedalla;
using LogicaAplicacion.InterfacesCasosUsos.Avatar;
using LogicaAplicacion.InterfacesCasosUsos.BarraProgreso;
using LogicaAplicacion.InterfacesCasosUsos.Estudiante;
using LogicaAplicacion.InterfacesCasosUsos.Grupo;
using LogicaAplicacion.InterfacesCasosUsos.Imagenes;
using LogicaAplicacion.InterfacesCasosUsos.Kudo;
using LogicaAplicacion.InterfacesCasosUsos.Login;
using LogicaAplicacion.InterfacesCasosUsos.Medalla;
using LogicaAplicacion.InterfacesCasosUsos.PerfilEstudiante;
using LogicaAplicacion.InterfacesCasosUsos.Profesor;
using LogicaAplicacion.InterfacesCasosUsos.Recompensa;
using LogicaAplicacion.InterfacesCasosUsos.RecuperarContrasena;
using LogicaAplicacion.InterfacesCasosUsos.ServicioPrecargaArchivos;
using LogicaAplicacion.InterfacesCasosUsos.SolicitudUnion;
using LogicaAplicacion.InterfacesCasosUsos.TablaClasificacion;
using LogicaAplicacion.InterfacesCasosUsos.TablaEquivalencia;
using LogicaAplicacion.InterfacesCasosUsos.Tienda;
using LogicaAplicacion.InterfacesCasosUsos.UmbralParaObtenerMedallaPorKudos;
using LogicaAplicacion.Servicios;
using LogicaNegocio.ConstantesAplicacion;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi.Helpers;
using WebApi.Jwt;
using WebApi.Servicios;
using IManejadorJwt = LogicaAplicacion.InterfacesCasosUsos.Jwt.IManejadorJwt;
using LogicaAplicacion.InterfacesCasosUsos.TablaClasificacion;
using LogicaAplicacion.ImplementacionCasosUsos.TablaClasificacion;
using LogicaAplicacion.InterfacesCasosUsos.BarraProgreso;
using LogicaAplicacion.InterfacesCasosUsos.RecuperarContrasena;
using LogicaAplicacion.InterfacesCasosUsos.SolicitudPerfilMedalla;
using LogicaAplicacion.ImplementacionCasosUsos.SolicitudPerfilMedalla;
using LogicaAplicacion.InterfacesCasosUsos.ProyectoAulaColaborativo;
using LogicaAplicacion.ImplementacionCasosUsos.ProyectoAulaColaborativo;
using LogicaAplicacion.InterfacesCasosUsos.Usuario;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaNegocio.ValueObject;

var builder = WebApplication.CreateBuilder(args);

var cadenaDeConexionBD = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ContextoDb>(options => options.UseSqlServer(cadenaDeConexionBD));
// -------------------------------
// Configura Identity y sus servicios
// -------------------------------
builder.Services.AddAuthorization();


builder.Services.AddIdentity<Usuario, IdentityRole>(opciones =>
{
	// ===== Validaciones de Contraseña =====
	opciones.Password.RequireDigit = true;                   // Al menos un dígito [0-9]
	opciones.Password.RequireLowercase = true;               // Al menos una minúscula [a-z]
	opciones.Password.RequireUppercase = true;               // Al menos una mayúscula [A-Z]
	opciones.Password.RequireNonAlphanumeric = true;         // Al menos un carácter no alfanumérico (por ejemplo, !, @, #)
	opciones.Password.RequiredLength = 8;                    // Longitud mínima de 8 caracteres

	// ===== Validaciones de Usuario =====
	opciones.User.RequireUniqueEmail = false;                  // SI el email debe ser único
	opciones.User.AllowedUserNameCharacters =
		"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

	// ===== Bloqueo de usuario =====
	opciones.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	opciones.Lockout.MaxFailedAccessAttempts = 5;
	opciones.Lockout.AllowedForNewUsers = true;

})
	.AddEntityFrameworkStores<ContextoDb>()
	.AddDefaultTokenProviders();

//--------------------------
// Configuración de Hangfire
//--------------------------
builder.Services.AddHangfire(config => config
	.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
	.UseSimpleAssemblyNameTypeSerializer()
	.UseRecommendedSerializerSettings()
	.UseSqlServerStorage(cadenaDeConexionBD, new SqlServerStorageOptions
	{
		CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
		SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
		QueuePollInterval = TimeSpan.Zero,
		UseRecommendedIsolationLevel = true,
		DisableGlobalLocks = true // Mejora el rendimiento en SQL Server
	}));

//procesador de trabajos de Hangfire
builder.Services.AddHangfireServer();


// -------------------------------
// Configura JWT Authentication
// -------------------------------
builder.Services.AddSingleton<IManejadorJwt, ManejadorJwt>();

// Lee configuración JWT desde appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var claveDificil = jwtSettings.GetValue<string>("Key");
var issuer = jwtSettings.GetValue<string>("Issuer");
var audience = jwtSettings.GetValue<string>("Audience");

var claveDificilEncriptada = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveDificil));
builder.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = issuer,
			ValidAudience = audience,
			IssuerSigningKey = claveDificilEncriptada,
			ClockSkew = TimeSpan.Zero
		};
	});

// -------------------------------
// Configura Authorization (políticas/roles)
// -------------------------------

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("EsAdministrador", policy => policy.RequireRole("Administrador"));
	options.AddPolicy("EsProfesor", policy => policy.RequireRole("Profesor"));
	options.AddPolicy("EsEstudiante", policy => policy.RequireRole("Estudiante"));
	options.AddPolicy("EsProfesorOEstudiante", policy => policy.RequireRole("Profesor", "Estudiante"));
});

// Registrar MediatR
builder.Services.AddMediatR(cfg =>
	cfg.RegisterServicesFromAssembly(typeof(LogicaAplicacion.AssemblyReference).Assembly));

builder.Services.AddScoped<INotificacionServicio, NotificacionServicioFalso>();


// Inyeccion de dependencias repositorios
builder.Services.AddScoped<IRepositorioAvatares, RepositorioAvataresEF>();
builder.Services.AddScoped<IRepositorioAtributosAvatar, RepositorioAtributosAvatarEF>();
builder.Services.AddScoped<IRepositorioEstudiantes, RepositorioEstudiantesEF>();
builder.Services.AddScoped<IRepositorioEnlacesUnionGrupo, RepositorioEnlacesUnionGrupoEF>();
builder.Services.AddScoped<IRepositorioUsuarios, RepositorioUsuariosEF>();
builder.Services.AddScoped<IRepositorioGrupos, RepositorioGruposEF>();
builder.Services.AddScoped<IRepositorioMedallas, RepositorioMedallasEF>();
builder.Services.AddScoped<IRepositorioSolicitudesUnion, RepositorioSolocitudesUnionEF>();
builder.Services.AddScoped<IRepositorioTiposKudo, RepositorioTiposKudoEF>();
builder.Services.AddScoped<IRepositorioTablasEquivalencia, RepositorioTablasEquivalenciaEF>();
builder.Services.AddScoped<IRepositorioTiendas, RepositorioTiendasEF>();
builder.Services.AddScoped<IRepositorioTablasClasificacion, RepositorioTablasClasificacionEF>();
builder.Services.AddScoped<IRepositorioRecompensas, RepositorioRecompensasEF>();
builder.Services.AddScoped<IRepositorioRendimientoPeriodos, RepositorioRendimientoPeriodosEF>();
builder.Services.AddScoped<IRepositorioPerfilEstudianteGrupo, RepositorioPerfilEstudianteGrupoEF>();
builder.Services.AddScoped<IRepositorioPerfilEstudianteMedalla, RepositorioPerfilEstudianteMedallaEF>();
builder.Services.AddScoped<IRepositorioProfesores, RepositorioProfesoresEF>();
builder.Services.AddScoped<IRepositorioPerfilEstudianteRecompensa, RepositorioPerfilEstudianteRecompensaEF>();
builder.Services.AddScoped<IRepositorioPreguntasSeguridad, RepositorioPreguntasSeguridadEF>();
builder.Services.AddScoped<IRepositorioPreguntasDeSeguridadDelSistema, RepositorioPreguntasDeSeguridadDelSistemaEF>();
builder.Services.AddScoped<IRepositorioHitos, RepositorioHitosEF>();
builder.Services.AddScoped<IRepositorioKudosOtorgados, RepositorioKudosOtorgadosEF>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepositorioUmbralesParaMedallasPorKudos, RepositorioUmbralesParaMedallasesPorKudosEF>();
builder.Services.AddScoped<IRepositorioSolicitudPerfilMedalla, RepositorioSolicitudPerfilMedallaEF>();
builder.Services.AddScoped<IRepositorioProyectoAulaColaborativo, RepositorioProyectoAulaColaborativoEF>();
builder.Services.AddScoped<IRepositorioRecompensasDeProfesores, RepositorioRecompensasDeProfesoresEF>();


builder.Services.AddAzureClients(clientBuilder =>
{

	if (!builder.Environment.IsDevelopment())
	{
		var connectionString = builder.Configuration.GetConnectionString("AzureStorage");
		clientBuilder.AddBlobServiceClient(connectionString);
	}
	else
	{
		// Si ESTÁS en desarrollo, usa la cadena de conexión de Azurite.
		var connectionString = builder.Configuration.GetConnectionString("StorageConnection");
		clientBuilder.AddBlobServiceClient(connectionString);
	}
});
builder.Services.AddScoped<IRepositorioAlmacenamientoArchivos, RepositorioAzureBlobsStorage>();


//Inyeccion de dependencias casos de uso

builder.Services.AddScoped<IAltaEstudiante, AltaEstudiante>();
builder.Services.AddScoped<IAltaProfesor, AltaProfesor>();
builder.Services.AddScoped<IAltaGrupo, AltaGrupo>();
builder.Services.AddScoped<IAltaTablaEquivalencia, AltaTablaEquivalencia>();
builder.Services.AddScoped<IAltaMedalla, AltaMedalla>();
builder.Services.AddScoped<IAceptarSolicitudUnion, AceptarSolicitudUnion>();
builder.Services.AddScoped<IAsignarMedalla, AsignarMedalla>();
builder.Services.AddKeyedScoped<IActualizadorRutaImagen, ActualizadorImagenPerfilEstudiante>(Constantes.PropositoImagen.PerfilEstudiante);
builder.Services.AddKeyedScoped<IActualizadorRutaImagen, ActualizadorImagenPerfilProfesor>(Constantes.PropositoImagen.PerfilProfesor);
builder.Services.AddScoped<IAltaRecompensa, AltaRecompensa>();
builder.Services.AddScoped<IAltaTablaClasificacion, AltaTablaClasificacion>();
builder.Services.AddScoped<IAsignarKudo, AsignarKudo>();
builder.Services.AddScoped<IAltaUmbralParaMedallaPorKudos, AltaUmbralParaMedallaPorKudos>();
builder.Services.AddScoped<IAceptarSolicitudPerfilMedalla, AceptarSolicitudPerfilMedalla>();
builder.Services.AddScoped<IAltaSolicitudPerfilMedalla, AltaSolicitudPerfilMedalla>();
builder.Services.AddScoped<IAltaProyectoAulaColaborativo, AltaProyectoAulaColaborativo>();
builder.Services.AddScoped<IActualizarUmbralParaMedallaPorKudos, ActualizarUmbralParaMedallaPorKudos>();
builder.Services.AddScoped<IAsignarRecompensaTiendas, AsignarRecompensaTiendas>();

builder.Services.AddScoped<IBajaMedalla, BajaMedalla>();
builder.Services.AddScoped<IBajaGrupo, BajaGrupo>();
builder.Services.AddScoped<IBajaRecompensa, BajaRecompensa>();
builder.Services.AddScoped<IBajaTablaClasificacion, BajaTablaClasificacion>();

builder.Services.AddScoped<ICanjearRecompensa, CanjearRecompensa>();
builder.Services.AddScoped<ICrearSolicitudUnion, CrearSolicitudUnion>();

builder.Services.AddScoped<IEditarGrupo, EditarGrupo>();
builder.Services.AddScoped<IEditarTablaEquivalencia, EditarTablaEquivalencia>();
builder.Services.AddScoped<IEstablecerMetaCalificacion, EstablecerMetaCalificacion>();
builder.Services.AddScoped<IEditarRecompensa, EditarRecompensa>();
builder.Services.AddScoped<IEliminarUmbralParaMedallaPorKudos, EliminarUmbralParaMedallaPorKudos>();


builder.Services.AddScoped<IObtenerMedallaPorId, ObtenerMedallaPorId>();
builder.Services.AddScoped<IObtenerTodasLasMedallas, ObtenerTodasLasMedallas>();
builder.Services.AddScoped<IObtenerGruposDeEstudiante, ObtenerGruposDeEstudiante>();
builder.Services.AddScoped<IObtenerGruposDeProfesor, ObtenerGruposDeProfesor>();
builder.Services.AddScoped<IObtenerInformacionGrupo, ObtenerInformacionGrupo>();
builder.Services.AddScoped<IObtenerPerfilesPorGrupo, ObtenerPerfilesPorGrupo>();
builder.Services.AddScoped<IObtenerSolicitudesUnionDelGrupo, ObtenerSolicitudesUnionDelGrupo>();
builder.Services.AddScoped<IObtenerTablasEquivalenciaDelProfesor, ObtenerTablasEquivalenciaDelProfesor>();
builder.Services.AddScoped<IObtenerPerfilConMedallas, ObtenerPerfilConMedallas>();
builder.Services.AddScoped<IObtenerListadoRecompensa, ObtenerListadoRecompensa>();
builder.Services.AddScoped<IObtenerRecompensasInventarioPerfil, ObtenerRecompensasInventarioPerfil>();
builder.Services.AddScoped<IObtenerTablaClasificacion, ObtenerTablaClasificacion>();
builder.Services.AddScoped<IObtenerContenidoBarraProgreso, ObtenerContenidoBarraProgreso>();
builder.Services.AddScoped<IObtenerPreguntasDeSegurididadPorNombreUsuario, ObtenerPreguntasDeSeguridadPorNombreUsuario>();
builder.Services.AddScoped<IObtenerPreguntasDeSeguridadDelSistema, ObtenerPreguntasDeSeguridadDelSistema>();
builder.Services.AddScoped<IObtenerAtributosAvatarDisponiblesParaPerfil, ObtenerAtributosAvatarDisponiblesParaPerfil>();
builder.Services.AddScoped<IObtenerPreguntasDeSegurididadPorNombreUsuario, ObtenerPreguntasDeSeguridadPorNombreUsuario>();
builder.Services.AddScoped<IObtenerPreguntasDeSeguridadDelSistema, ObtenerPreguntasDeSeguridadDelSistema>();
builder.Services.AddScoped<IObtenerSolicitudPerfilMedalla, ObtenerSolicitudesPerfilMedalla>();
builder.Services.AddScoped<IObtenerUmbralesParaMedallasPorKudos, ObtenerUmbralesParaMedallasPorKudos>();
builder.Services.AddScoped<IObtenerProyectoAulaColaborativo, ObtenerProyectoAulaColaborativo>();
builder.Services.AddScoped<IObtenerTodasLasTablasClasificacionGrupo, ObtenerTodasLasTablasClasificacionGrupo>();
builder.Services.AddScoped<IObtenerProyectoAulaColaborativo, ObtenerProyectoAulaColaborativo>();
builder.Services.AddScoped<IObtenerPerfilUsuarioLogueado, ObtenerPerfilUsuarioLogueado>();
builder.Services.AddScoped<IObtenerPerfilesPorGrupoSinLogueado, ObtenerPerfilesDeGrupoSinIncluirUsuarioLogueado>();
builder.Services.AddScoped<IObtenerKudos, ObtenerKudos>();
builder.Services.AddScoped<IObtenerRecompensasDelProfesor, ObtenerRecompensasDelProfesor>();
builder.Services.AddScoped<IObtenerMedallasProfesorPorGrupo, ObtenerMedallasProfesorPorGrupo>();
builder.Services.AddScoped<IObtenerRecompensasReclamadasAlumnos, ObtenerRecompensasReclamadasAlumnos>();
builder.Services.AddScoped<IConfirmarRecompensaReclamadaAlumno, ConfirmarRecompensaReclamadaAlumno>();



builder.Services.AddScoped<IModificarMedalla, ModificarMedalla>();
builder.Services.AddScoped<IModificarAvatar, ModificarAvatar>();
builder.Services.AddScoped<IGeneradorEnlaceGrupo, GeneradorEnlaceGrupo>();
builder.Services.AddScoped<IQuitarMedalla, QuitarMedalla>();

builder.Services.AddScoped<IRechazarSolicitudUnion, RechazarSolicitudUnion>();
builder.Services.AddScoped<IReinicioLogrosDeUnGrupo, ReinicioLogrosDeUnGrupo>();
builder.Services.AddScoped<IReinicioLogrosDeTodosLosGrupos, ReinicioLogrosDeTodosLosGrupos>();
builder.Services.AddScoped<IRestablecerContrasena, RestablecerContrasena>();
builder.Services.AddScoped<IRestablecerContrasena, RestablecerContrasena>();
builder.Services.AddScoped<IRechazarSolicitudPerfilMedalla, RechazarSolicitudPerfilMedalla>();
builder.Services.AddScoped<ILoginUsuario, LoginUsuario>();



// Inyeccion de dependencias para servicios
builder.Services.AddScoped<IServicioGestionImagen, ServicioGestionImagen>();
builder.Services.AddScoped<IServicioProcesamientoImagenes, ServicioImageSharp>();
builder.Services.AddScoped<ISeedServicio, SeedServicio>();
builder.Services.AddScoped<IServicioDeReinicioSemanal, ServicioDeReinicioSemanal>();
builder.Services.AddScoped<IGeneradorUrlImagen, GeneradorUrlImagen>();
builder.Services.AddScoped<IGeneradorUrlsParaColeccionesImagenes, GeneradorUrlsParaColeccionesImagenes>();
builder.Services.AddScoped<IServicioCrearObjetosParaProfesor, ServicioCrearObjetosParaProfesor>();
builder.Services.AddScoped<IRecompensaEnricher, RecompensaEnricher>();
builder.Services.AddScoped<ICrearTiendaPorDefecto, CrearTiendaPorDefecto>();




// -------------------------------
//      Swagger y CORS
// -------------------------------
builder.Services.AddControllers()
	.AddJsonOptions(opts =>
	{
		// Habilita polimorfismo para RespuestaVisual y DatosVisuales
		opts.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
		{
			Modifiers =
			{
				ti =>
				{
                    // Configuración para RespuestaVisual
                    if (ti.Type == typeof(RespuestaVisual))
					{
						ti.PolymorphismOptions = new JsonPolymorphismOptions
						{
							TypeDiscriminatorPropertyName = "$type",
							UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
							DerivedTypes =
							{
								new JsonDerivedType(typeof(RespuestaImagenDto), nameof(RespuestaImagenDto)),
								new JsonDerivedType(typeof(RespuestaIconoDto), nameof(RespuestaIconoDto))
							}
						};
					}

                    // Configuración para DatosVisuales
                    if (ti.Type == typeof(DatosVisuales))
					{
						ti.PolymorphismOptions = new JsonPolymorphismOptions
						{
							TypeDiscriminatorPropertyName = "$type",
							UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
							DerivedTypes =
							{
								new JsonDerivedType(typeof(DatosImagen), "Imagen"),
								new JsonDerivedType(typeof(DatosIcono), "Icono")
							}
						};
					}
				}
			}
		};
	});

builder.Services.AddEndpointsApiExplorer();

var ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebApi.xml");
builder.Services.AddSwaggerGen(opciones =>
{
	opciones.IncludeXmlComments(ruta);
	opciones.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "API de Ludik",
		Version = "v1",
		Description = "Bitácora digital de logros de aprendizaje.",
		Contact = new OpenApiContact { Email = "renatoriosx@gmail.com" }
	});

	opciones.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Introduce el token JWT con el prefijo 'Bearer ', por ejemplo: Bearer eyJhbGciOiJIUzI1NiIs..."
	});

	opciones.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		policy => policy.AllowAnyOrigin()
						.AllowAnyHeader()
						.AllowAnyMethod()
						);
});


var app = builder.Build();

// --- INICIO: Lógica para precargar archivos 
if (app.Environment.IsDevelopment())
{
	using (var scope = app.Services.CreateScope())
	{
		var services = scope.ServiceProvider;
		try
		{
			var seeder = services.GetRequiredService<ISeedServicio>();
			// Usamos .GetAwaiter().GetResult() para ejecutarlo de forma síncrona en el arranque.
			seeder.PrecargarArchivosAsync().GetAwaiter().GetResult();
		}
		catch (Exception ex)
		{
			var logger = services.GetRequiredService<ILogger<Program>>();
			logger.LogError(ex, "Ocurrió un error durante la precarga de archivos.");
		}
	}
}
// --- FIN: Lógica para precargar archivos ---

// -------------------------------
// Seed Roles al iniciar la app
// -------------------------------

try
{
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var roles = new[] { "Administrador", "Profesor", "Estudiante" };

        foreach (var rolNombre in roles)
        {
            var existe = await roleManager.RoleExistsAsync(rolNombre);
            if (!existe)
            {
                await roleManager.CreateAsync(new IdentityRole(rolNombre));
                logger.LogInformation("Rol '{RoleName}' creado exitosamente.", rolNombre);
            }
        }
    }
}
catch (Exception ex)
{
    // Captura cualquier excepción durante el sembrado, la registra, pero permite que la aplicación continúe su inicio.
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Ocurrió un error durante el sembrado de roles en el arranque.");
}


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
});

app.UseHttpsRedirection();

// CORS debe ir antes de autenticación/autorización
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Habilitar el Dashboard de Hangfire
app.UseHangfireDashboard();

// Programar el trabajo recurrente

RecurringJob.AddOrUpdate<IServicioDeReinicioSemanal>(
   "reinicio-semanal-kudos",
   servicio => servicio.ReiniciarKudosDeEstudiantesAsync(),
   "0 0 * * 1",
   TimeZoneInfo.Local);

app.Run();

public partial class Program { }