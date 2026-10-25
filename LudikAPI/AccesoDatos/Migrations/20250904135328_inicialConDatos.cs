using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class inicialConDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AtributosAvatar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    NombreImagenRecurso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoUnico = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtributosAvatar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnlacesUnion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UrlCompleta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnlacesUnion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreguntasDeSeguridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreguntasDeSeguridad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RolId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NombreRol = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NombreRolNormalizado = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EstampaConcurrencia = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RolId);
                });

            migrationBuilder.CreateTable(
                name: "TiposKudo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreIcono = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposKudo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImagenPerfil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NombreUsuarioNormalizado = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CorreoNormalizado = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CorreoConfirmado = table.Column<bool>(type: "bit", nullable: false),
                    ContraseniaHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstampaSeguridad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstampaConcurrencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelefonoConfirmado = table.Column<bool>(type: "bit", nullable: false),
                    AutenticacionDosFactores = table.Column<bool>(type: "bit", nullable: false),
                    FinBloqueo = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    BloqueoHabilitado = table.Column<bool>(type: "bit", nullable: false),
                    IntentosFallidos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioId);
                });

            migrationBuilder.CreateTable(
                name: "Recompensas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Representacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<int>(type: "int", nullable: false),
                    TipoRecompensa = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    DuracionHoras = table.Column<int>(type: "int", nullable: true),
                    Multiplicador = table.Column<double>(type: "float", nullable: true),
                    AtributoAvatarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recompensas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recompensas_AtributosAvatar_AtributoAvatarId",
                        column: x => x.AtributoAvatarId,
                        principalTable: "AtributosAvatar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReclamacionesRoles",
                columns: table => new
                {
                    ReclamacionRolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TipoReclamacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorReclamacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReclamacionesRoles", x => x.ReclamacionRolId);
                    table.ForeignKey(
                        name: "FK_ReclamacionesRoles_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_Estudiantes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IniciosSesionUsuario",
                columns: table => new
                {
                    Proveedor = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaveProveedor = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NombreProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IniciosSesionUsuario", x => new { x.Proveedor, x.ClaveProveedor });
                    table.ForeignKey(
                        name: "FK_IniciosSesionUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profesores",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesores", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_Profesores_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReclamacionesUsuario",
                columns: table => new
                {
                    ReclamacionUsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TipoReclamacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorReclamacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReclamacionesUsuario", x => x.ReclamacionUsuarioId);
                    table.ForeignKey(
                        name: "FK_ReclamacionesUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TokensUsuario",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Proveedor = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NombreToken = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ValorToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokensUsuario", x => new { x.UsuarioId, x.Proveedor, x.NombreToken });
                    table.ForeignKey(
                        name: "FK_TokensUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosRoles",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RolId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosRoles", x => new { x.UsuarioId, x.RolId });
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hitos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CantMedallasRequeridas = table.Column<int>(type: "int", nullable: false),
                    RecompensaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hitos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hitos_Recompensas_RecompensaId",
                        column: x => x.RecompensaId,
                        principalTable: "Recompensas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstudiantePotenciadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstudianteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PotenciadorId = table.Column<int>(type: "int", nullable: false),
                    FechaActivacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstudiantePotenciadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstudiantePotenciadores_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiantes",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstudiantePotenciadores_Recompensas_PotenciadorId",
                        column: x => x.PotenciadorId,
                        principalTable: "Recompensas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreguntasRespuestasSeguridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreguntaDeSeguridadId = table.Column<int>(type: "int", nullable: false),
                    Respuesta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstudianteId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreguntasRespuestasSeguridad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreguntasRespuestasSeguridad_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiantes",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreguntasRespuestasSeguridad_PreguntasDeSeguridad_PreguntaDeSeguridadId",
                        column: x => x.PreguntaDeSeguridadId,
                        principalTable: "PreguntasDeSeguridad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medallas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NombreIcono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonedasOtorgadas = table.Column<int>(type: "int", nullable: false),
                    ProfesorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medallas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medallas_Profesores_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesores",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecompensasDeProfesores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfesorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecompensaId = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecompensasDeProfesores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecompensasDeProfesores_Profesores_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesores",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecompensasDeProfesores_Recompensas_RecompensaId",
                        column: x => x.RecompensaId,
                        principalTable: "Recompensas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TablasEquivalencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProfesorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablasEquivalencia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TablasEquivalencia_Profesores_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesores",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstudianteHitos",
                columns: table => new
                {
                    EstudianteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HitoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstudianteHitos", x => new { x.EstudianteId, x.HitoId });
                    table.ForeignKey(
                        name: "FK_EstudianteHitos_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiantes",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstudianteHitos_Hitos_HitoId",
                        column: x => x.HitoId,
                        principalTable: "Hitos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equivalencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    TablaEquivalenciaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equivalencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equivalencias_TablasEquivalencia_TablaEquivalenciaId",
                        column: x => x.TablaEquivalenciaId,
                        principalTable: "TablasEquivalencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Institucion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Materia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TablaEquivalenciaId = table.Column<int>(type: "int", nullable: false),
                    EnlaceUnionId = table.Column<int>(type: "int", nullable: false),
                    ProfesorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaUltimoReinicio = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grupos_EnlacesUnion_EnlaceUnionId",
                        column: x => x.EnlaceUnionId,
                        principalTable: "EnlacesUnion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grupos_Profesores_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesores",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grupos_TablasEquivalencia_TablaEquivalenciaId",
                        column: x => x.TablaEquivalenciaId,
                        principalTable: "TablasEquivalencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquivalenciaMedallas",
                columns: table => new
                {
                    EquivalenciaId = table.Column<int>(type: "int", nullable: false),
                    MedallaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquivalenciaMedallas", x => new { x.EquivalenciaId, x.MedallaId });
                    table.ForeignKey(
                        name: "FK_EquivalenciaMedallas_Equivalencias_EquivalenciaId",
                        column: x => x.EquivalenciaId,
                        principalTable: "Equivalencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquivalenciaMedallas_Medallas_MedallaId",
                        column: x => x.MedallaId,
                        principalTable: "Medallas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PerfilesEstudiantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetaCalificacion = table.Column<int>(type: "int", nullable: false),
                    EstudianteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Monedas = table.Column<int>(type: "int", nullable: false),
                    NombreImagenCompleta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreImagenMiniatura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KudosDisponiblesParaOtorgar = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilesEstudiantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilesEstudiantes_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiantes",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerfilesEstudiantes_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProyectosAulaColaborativo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CantidadMedallasNecesarias = table.Column<int>(type: "int", nullable: false),
                    RecompensaClaseId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProyectosAulaColaborativo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProyectosAulaColaborativo_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProyectosAulaColaborativo_Recompensas_RecompensaClaseId",
                        column: x => x.RecompensaClaseId,
                        principalTable: "Recompensas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudesUnion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstudianteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesUnion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudesUnion_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiantes",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudesUnion_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TablasClasificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MedallaAsociadaId = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablasClasificacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TablasClasificacion_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TablasClasificacion_Medallas_MedallaAsociadaId",
                        column: x => x.MedallaAsociadaId,
                        principalTable: "Medallas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tiendas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrupoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tiendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tiendas_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UmbralesParaMedallasPorKudos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CantidadKudos = table.Column<int>(type: "int", nullable: false),
                    MedallaId = table.Column<int>(type: "int", nullable: false),
                    TipoKudoId = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UmbralesParaMedallasPorKudos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UmbralesParaMedallasPorKudos_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UmbralesParaMedallasPorKudos_Medallas_MedallaId",
                        column: x => x.MedallaId,
                        principalTable: "Medallas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UmbralesParaMedallasPorKudos_TiposKudo_TipoKudoId",
                        column: x => x.TipoKudoId,
                        principalTable: "TiposKudo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Avatares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorFondo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voltear = table.Column<bool>(type: "bit", nullable: false),
                    Rotacion = table.Column<int>(type: "int", nullable: false),
                    Zoom = table.Column<int>(type: "int", nullable: false),
                    PerfilEstudianteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avatares_PerfilesEstudiantes_PerfilEstudianteId",
                        column: x => x.PerfilEstudianteId,
                        principalTable: "PerfilesEstudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BarrasProgreso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValorMin = table.Column<int>(type: "int", nullable: false),
                    ValorMax = table.Column<int>(type: "int", nullable: false),
                    TablaEquivalenciaId = table.Column<int>(type: "int", nullable: false),
                    PerfilEstudianteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarrasProgreso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BarrasProgreso_PerfilesEstudiantes_PerfilEstudianteId",
                        column: x => x.PerfilEstudianteId,
                        principalTable: "PerfilesEstudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BarrasProgreso_TablasEquivalencia_TablaEquivalenciaId",
                        column: x => x.TablaEquivalenciaId,
                        principalTable: "TablasEquivalencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PerfilEstudianteMedallas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerfilEstudianteId = table.Column<int>(type: "int", nullable: false),
                    MedallaId = table.Column<int>(type: "int", nullable: false),
                    FechaObtencion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilEstudianteMedallas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilEstudianteMedallas_Medallas_MedallaId",
                        column: x => x.MedallaId,
                        principalTable: "Medallas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerfilEstudianteMedallas_PerfilesEstudiantes_PerfilEstudianteId",
                        column: x => x.PerfilEstudianteId,
                        principalTable: "PerfilesEstudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerfilEstudianteRecompensas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerfilEstudianteId = table.Column<int>(type: "int", nullable: false),
                    RecompensaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilEstudianteRecompensas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilEstudianteRecompensas_PerfilesEstudiantes_PerfilEstudianteId",
                        column: x => x.PerfilEstudianteId,
                        principalTable: "PerfilesEstudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerfilEstudianteRecompensas_Recompensas_RecompensaId",
                        column: x => x.RecompensaId,
                        principalTable: "Recompensas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RendimientosPeriodos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NotaObtenida = table.Column<int>(type: "int", nullable: false),
                    PerfilEstudianteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RendimientosPeriodos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RendimientosPeriodos_PerfilesEstudiantes_PerfilEstudianteId",
                        column: x => x.PerfilEstudianteId,
                        principalTable: "PerfilesEstudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudesPerfilMedalla",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerfilEstudianteId = table.Column<int>(type: "int", nullable: false),
                    MedallaId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesPerfilMedalla", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudesPerfilMedalla_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitudesPerfilMedalla_Medallas_MedallaId",
                        column: x => x.MedallaId,
                        principalTable: "Medallas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudesPerfilMedalla_PerfilesEstudiantes_PerfilEstudianteId",
                        column: x => x.PerfilEstudianteId,
                        principalTable: "PerfilesEstudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TablaClasificacionParticipantes",
                columns: table => new
                {
                    TablaClasificacionId = table.Column<int>(type: "int", nullable: false),
                    PerfilEstudianteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablaClasificacionParticipantes", x => new { x.TablaClasificacionId, x.PerfilEstudianteId });
                    table.ForeignKey(
                        name: "FK_TablaClasificacionParticipantes_PerfilesEstudiantes_PerfilEstudianteId",
                        column: x => x.PerfilEstudianteId,
                        principalTable: "PerfilesEstudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TablaClasificacionParticipantes_TablasClasificacion_TablaClasificacionId",
                        column: x => x.TablaClasificacionId,
                        principalTable: "TablasClasificacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecompensaTienda",
                columns: table => new
                {
                    RecompesasId = table.Column<int>(type: "int", nullable: false),
                    TiendaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecompensaTienda", x => new { x.RecompesasId, x.TiendaId });
                    table.ForeignKey(
                        name: "FK_RecompensaTienda_Recompensas_RecompesasId",
                        column: x => x.RecompesasId,
                        principalTable: "Recompensas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecompensaTienda_Tiendas_TiendaId",
                        column: x => x.TiendaId,
                        principalTable: "Tiendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AvatarAtributos",
                columns: table => new
                {
                    AvatarId = table.Column<int>(type: "int", nullable: false),
                    AtributoSeleccionadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvatarAtributos", x => new { x.AvatarId, x.AtributoSeleccionadoId });
                    table.ForeignKey(
                        name: "FK_AvatarAtributos_AtributosAvatar_AtributoSeleccionadoId",
                        column: x => x.AtributoSeleccionadoId,
                        principalTable: "AtributosAvatar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvatarAtributos_Avatares_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Avatares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KudosOtorgados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerfilEstudianteEmisorId = table.Column<int>(type: "int", nullable: false),
                    PerfilEstudianteReceptorId = table.Column<int>(type: "int", nullable: false),
                    TipoKudoId = table.Column<int>(type: "int", nullable: false),
                    FechaOtorgamiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PerfilEstudianteMedallaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KudosOtorgados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KudosOtorgados_PerfilEstudianteMedallas_PerfilEstudianteMedallaId",
                        column: x => x.PerfilEstudianteMedallaId,
                        principalTable: "PerfilEstudianteMedallas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KudosOtorgados_PerfilesEstudiantes_PerfilEstudianteEmisorId",
                        column: x => x.PerfilEstudianteEmisorId,
                        principalTable: "PerfilesEstudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KudosOtorgados_PerfilesEstudiantes_PerfilEstudianteReceptorId",
                        column: x => x.PerfilEstudianteReceptorId,
                        principalTable: "PerfilesEstudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KudosOtorgados_TiposKudo_TipoKudoId",
                        column: x => x.TipoKudoId,
                        principalTable: "TiposKudo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RendimientoPeriodoMedallas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RendimientoPeriodoId = table.Column<int>(type: "int", nullable: false),
                    MedallaId = table.Column<int>(type: "int", nullable: false),
                    FechaOtorgada = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RendimientoPeriodoMedallas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RendimientoPeriodoMedallas_Medallas_MedallaId",
                        column: x => x.MedallaId,
                        principalTable: "Medallas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RendimientoPeriodoMedallas_RendimientosPeriodos_RendimientoPeriodoId",
                        column: x => x.RendimientoPeriodoId,
                        principalTable: "RendimientosPeriodos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AtributosAvatar",
                columns: new[] { "Id", "CodigoUnico", "Nombre", "NombreImagenRecurso", "Tipo" },
                values: new object[,]
                {
                    { 1, "curly", "Curly", "top-curly.png", 0 },
                    { 2, "curvy", "Curvy", "top-curvy.png", 0 },
                    { 3, "dreads", "Dreads", "top-dreads.png", 0 },
                    { 4, "dreads01", "Dreads01", "top-dreads01.png", 0 },
                    { 5, "dreads02", "Dreads02", "top-dreads02.png", 0 },
                    { 6, "frida", "Frida", "top-frida.png", 0 },
                    { 7, "frizzle", "Frizzle", "top-frizzle.png", 0 },
                    { 8, "fro", "Fro", "top-fro.png", 0 },
                    { 9, "froBand", "FroBand", "top-froBand.png", 0 },
                    { 10, "longButNotTooLong", "LongButNotTooLong", "top-longButNotTooLong.png", 0 },
                    { 11, "angryNatural", "AngryNatural", "eyebrows-angryNatural.png", 1 },
                    { 12, "defaultNatural", "DefaultNatural", "eyebrows-defaultNatural.png", 1 },
                    { 13, "flatNatural", "FlatNatural", "eyebrows-flatNatural.png", 1 },
                    { 14, "frownNatural", "FrownNatural", "eyebrows-frownNatural.png", 1 },
                    { 15, "raisedExcitedNatural", "RaisedExcitedNatural", "eyebrows-raisedExcitedNatural.png", 1 },
                    { 16, "sadConcernedNatural", "SadConcernedNatural", "eyebrows-sadConcernedNatural.png", 1 },
                    { 17, "unibrowNatural", "UnibrowNatural", "eyebrows-unibrowNatural.png", 1 },
                    { 18, "upDownNatural", "UpDownNatural", "eyebrows-upDownNatural.png", 1 },
                    { 19, "closed", "Closed", "eyes-closed.png", 2 },
                    { 20, "cry", "Cry", "eyes-cry.png", 2 },
                    { 21, "default", "Default", "eyes-default.png", 2 },
                    { 22, "happy", "Happy", "eyes-happy.png", 2 },
                    { 23, "hearts", "Hearts", "eyes-hearts.png", 2 },
                    { 24, "side", "Side", "eyes-side.png", 2 },
                    { 25, "squint", "Squint", "eyes-squint.png", 2 },
                    { 26, "surprised", "Surprised", "eyes-surprised.png", 2 },
                    { 27, "wink", "Wink", "eyes-wink.png", 2 },
                    { 28, "winkWacky", "WinkWacky", "eyes-winkWacky.png", 2 },
                    { 29, "xDizzy", "XDizzy", "eyes-xDizzy.png", 2 },
                    { 30, "concerned", "Concerned", "mouth-concerned.png", 3 },
                    { 31, "default", "Default", "mouth-default.png", 3 },
                    { 32, "disbelief", "Disbelief", "mouth-disbelief.png", 3 },
                    { 33, "eating", "Eating", "mouth-eating.png", 3 },
                    { 34, "grimace", "Grimace", "mouth-grimace.png", 3 },
                    { 35, "sad", "Sad", "mouth-sad.png", 3 },
                    { 36, "screamOpen", "ScreamOpen", "mouth-screamOpen.png", 3 },
                    { 37, "eyepatch", "Eyepatch", "accessories-eyepatch.png", 5 },
                    { 38, "kurt", "Kurt", "accessories-kurt.png", 5 },
                    { 39, "none", "None", "accessories-none.png", 5 },
                    { 40, "prescription01", "Prescription01", "accessories-prescription01.png", 5 },
                    { 41, "prescription02", "Prescription02", "accessories-prescription02.png", 5 },
                    { 42, "round", "Round", "accessories-round.png", 5 },
                    { 43, "sunglasses", "Sunglasses", "accessories-sunglasses.png", 5 },
                    { 44, "wayfarers", "Wayfarers", "accessories-wayfarers.png", 5 },
                    { 45, "blazerAndShirt", "BlazerAndShirt", "clothing-blazerAndShirt.png", 6 },
                    { 46, "blazerAndSweater", "BlazerAndSweater", "clothing-blazerAndSweater.png", 6 },
                    { 47, "collarAndSweater", "CollarAndSweater", "clothing-collarAndSweater.png", 6 },
                    { 48, "hoodie", "Hoodie", "clothing-hoodie.png", 6 },
                    { 49, "overall", "Overall", "clothing-overall.png", 6 },
                    { 50, "shirtCrewNeck", "ShirtCrewNeck", "clothing-shirtCrewNeck.png", 6 },
                    { 51, "shirtScoopNeck", "ShirtScoopNeck", "clothing-shirtScoopNeck.png", 6 },
                    { 52, "shirtVNeck", "ShirtVNeck", "clothing-shirtVNeck.png", 6 },
                    { 53, "#614335", "#614335", "skinColor-#614335.png", 7 },
                    { 54, "#ae5d29", "#ae5d29", "skinColor-#ae5d29.png", 7 },
                    { 55, "#d08b5b", "#d08b5b", "skinColor-#d08b5b.png", 7 },
                    { 56, "#edb98a", "#edb98a", "skinColor-#edb98a.png", 7 },
                    { 57, "#f8d25c", "#f8d25c", "skinColor-#f8d25c.png", 7 },
                    { 58, "#fd9841", "#fd9841", "skinColor-#fd9841.png", 7 },
                    { 59, "#ffdbb4", "#ffdbb4", "skinColor-#ffdbb4.png", 7 },
                    { 60, "#2c1b18", "#2c1b18", "hairColor-#2c1b18.png", 8 },
                    { 61, "#4a312c", "#4a312c", "hairColor-#4a312c.png", 8 },
                    { 62, "#724133", "#724133", "hairColor-#724133.png", 8 },
                    { 63, "#a55728", "#a55728", "hairColor-#a55728.png", 8 },
                    { 64, "#b58143", "#b58143", "hairColor-#b58143.png", 8 },
                    { 65, "#c93305", "#c93305", "hairColor-#c93305.png", 8 },
                    { 66, "#d6b370", "#d6b370", "hairColor-#d6b370.png", 8 },
                    { 67, "#e8e1e1", "#e8e1e1", "hairColor-#e8e1e1.png", 8 },
                    { 68, "#ecdcbf", "#ecdcbf", "hairColor-#ecdcbf.png", 8 },
                    { 69, "#f59797", "#f59797", "hairColor-#f59797.png", 8 },
                    { 70, "#2c1b18", "#2c1b18", "beardColor-#2c1b18.png", 9 },
                    { 71, "#4a312c", "#4a312c", "beardColor-#4a312c.png", 9 },
                    { 72, "#724133", "#724133", "beardColor-#724133.png", 9 },
                    { 73, "#a55728", "#a55728", "beardColor-#a55728.png", 9 },
                    { 74, "#b58143", "#b58143", "beardColor-#b58143.png", 9 },
                    { 75, "#c93305", "#c93305", "beardColor-#c93305.png", 9 },
                    { 76, "#d6b370", "#d6b370", "beardColor-#d6b370.png", 9 },
                    { 77, "#e8e1e1", "#e8e1e1", "beardColor-#e8e1e1.png", 9 },
                    { 78, "#ecdcbf", "#ecdcbf", "beardColor-#ecdcbf.png", 9 },
                    { 79, "#f59797", "#f59797", "beardColor-#f59797.png", 9 },
                    { 80, "#3c4f5c", "#3c4f5c", "clothesColor-#3c4f5c.png", 10 },
                    { 81, "#65c9ff", "#65c9ff", "clothesColor-#65c9ff.png", 10 },
                    { 82, "#262e33", "#262e33", "clothesColor-#262e33.png", 10 },
                    { 83, "#5199e4", "#5199e4", "clothesColor-#5199e4.png", 10 },
                    { 84, "#25557c", "#25557c", "clothesColor-#25557c.png", 10 },
                    { 85, "#929598", "#929598", "clothesColor-#929598.png", 10 },
                    { 86, "#a7ffc4", "#a7ffc4", "clothesColor-#a7ffc4.png", 10 },
                    { 87, "#b1e2ff", "#b1e2ff", "clothesColor-#b1e2ff.png", 10 },
                    { 88, "#e6e6e6", "#e6e6e6", "clothesColor-#e6e6e6.png", 10 },
                    { 89, "#ff5c5c", "#ff5c5c", "clothesColor-#ff5c5c.png", 10 },
                    { 90, "#ff488e", "#ff488e", "clothesColor-#ff488e.png", 10 },
                    { 91, "#ffafb9", "#ffafb9", "clothesColor-#ffafb9.png", 10 },
                    { 92, "#ffffb1", "#ffffb1", "clothesColor-#ffffb1.png", 10 },
                    { 93, "#ffffff", "#ffffff", "clothesColor-#ffffff.png", 10 },
                    { 94, "#3c4f5c", "#3c4f5c", "accessoriesColor-#3c4f5c.png", 11 },
                    { 95, "#65c9ff", "#65c9ff", "accessoriesColor-#65c9ff.png", 11 },
                    { 96, "#262e33", "#262e33", "accessoriesColor-#262e33.png", 11 },
                    { 97, "#5199e4", "#5199e4", "accessoriesColor-#5199e4.png", 11 },
                    { 98, "#25557c", "#25557c", "accessoriesColor-#25557c.png", 11 },
                    { 99, "#929598", "#929598", "accessoriesColor-#929598.png", 11 },
                    { 100, "#a7ffc4", "#a7ffc4", "accessoriesColor-#a7ffc4.png", 11 },
                    { 101, "#b1e2ff", "#b1e2ff", "accessoriesColor-#b1e2ff.png", 11 },
                    { 102, "#e6e6e6", "#e6e6e6", "accessoriesColor-#e6e6e6.png", 11 },
                    { 103, "#ff5c5c", "#ff5c5c", "accessoriesColor-#ff5c5c.png", 11 },
                    { 104, "#ff488e", "#ff488e", "accessoriesColor-#ff488e.png", 11 },
                    { 105, "#ffafb9", "#ffafb9", "accessoriesColor-#ffafb9.png", 11 },
                    { 106, "#ffdeb5", "#ffdeb5", "accessoriesColor-#ffdeb5.png", 11 },
                    { 107, "#ffffb1", "#ffffb1", "accessoriesColor-#ffffb1.png", 11 },
                    { 108, "#ffffff", "#ffffff", "accessoriesColor-#ffffff.png", 11 }
                });

            migrationBuilder.InsertData(
                table: "EnlacesUnion",
                columns: new[] { "Id", "CodigoUnico", "Expiracion", "UrlCompleta" },
                values: new object[,]
                {
                    { 1, "MAT1A25", new DateTime(2026, 4, 15, 10, 30, 0, 0, DateTimeKind.Utc), "https://www.ludik.app/unirse/MAT1A25" },
                    { 2, "HISTU25", new DateTime(2026, 4, 15, 10, 30, 0, 0, DateTimeKind.Utc), "https://www.ludik.app/unirse/HISTU25" },
                    { 3, "MAT2B", new DateTime(2026, 4, 15, 10, 30, 0, 0, DateTimeKind.Utc), "https://www.ludik.app/unirse/FIS2B25" },
                    { 4, "MATCIENA", new DateTime(2026, 4, 15, 10, 30, 0, 0, DateTimeKind.Utc), "https://www.ludik.app/unirse/QUIgen25" }
                });

            migrationBuilder.InsertData(
                table: "PreguntasDeSeguridad",
                columns: new[] { "Id", "Texto" },
                values: new object[,]
                {
                    { 1, "¿Cuál era el nombre de tu escuela primaria?" },
                    { 2, "¿Cuál es el primer nombre de tu abuela materna?" },
                    { 3, "¿Cuál era el nombre de tu primera mascota?" },
                    { 4, "¿Cuál era el apodo que te decía tu familia en la infancia?" },
                    { 5, "¿Cuál es el nombre de tu personaje de ficción favorito (de un libro, serie o videojuego)?" },
                    { 6, "¿Cuál fue el primer videojuego que lograste completar?" },
                    { 7, "Si pudieras tener un superpoder, ¿cuál sería?" },
                    { 8, "¿Cuál es el apellido del primer amigo o amiga que hiciste al empezar el liceo?" },
                    { 9, "¿Cuál es el nombre del hospital donde naciste?" }
                });

            migrationBuilder.InsertData(
                table: "Recompensas",
                columns: new[] { "Id", "Nombre", "Precio", "Representacion", "TipoRecompensa" },
                values: new object[,]
                {
                    { 1, "Estrella Mágica", 50, "{\"Type\":\"RepresentacionIcono\",\"NombreIcono\":\"star\"}", "Recompensa_Simple" },
                    { 2, "Regalo Sorpresa", 30, "{\"Type\":\"RepresentacionIcono\",\"NombreIcono\":\"gift\"}", "Recompensa_Simple" },
                    { 3, "Corazón Brillante", 20, "{\"Type\":\"RepresentacionIcono\",\"NombreIcono\":\"heart\"}", "Recompensa_Simple" },
                    { 4, "Medalla de Oro", 80, "{\"Type\":\"RepresentacionIcono\",\"NombreIcono\":\"medal\"}", "Recompensa_Simple" },
                    { 5, "Montón de Monedas", 100, "{\"Type\":\"RepresentacionIcono\",\"NombreIcono\":\"coins\"}", "Recompensa_Simple" },
                    { 6, "Trofeo Brillante", 70, "{\"Type\":\"RepresentacionIcono\",\"NombreIcono\":\"trophy\"}", "Recompensa_Simple" },
                    { 7, "Llama de Fuego", 40, "{\"Type\":\"RepresentacionIcono\",\"NombreIcono\":\"fire\"}", "Recompensa_Simple" },
                    { 8, "Corona Real", 90, "{\"Type\":\"RepresentacionIcono\",\"NombreIcono\":\"crown\"}", "Recompensa_Simple" },
                    { 9, "Cohete Espacial", 60, "{\"Type\":\"RepresentacionIcono\",\"NombreIcono\":\"rocket\"}", "Recompensa_Simple" },
                    { 10, "Robot Amistoso", 55, "{\"Type\":\"RepresentacionIcono\",\"NombreIcono\":\"robot\"}", "Recompensa_Simple" }
                });

            migrationBuilder.InsertData(
                table: "Recompensas",
                columns: new[] { "Id", "DuracionHoras", "Multiplicador", "Nombre", "Precio", "Representacion", "TipoRecompensa" },
                values: new object[,]
                {
                    { 101, 24, 1.5, "Bono x1.5 (24h)", 0, "{\"Type\":\"RepresentacionImagen\",\"NombreImagenCompleta\":null,\"NombreImagenMiniatura\":null}", "Recompensa_Potenciador" },
                    { 102, 24, 1.6000000000000001, "Bono x1.6 (24h)", 0, "{\"Type\":\"RepresentacionImagen\",\"NombreImagenCompleta\":null,\"NombreImagenMiniatura\":null}", "Recompensa_Potenciador" },
                    { 103, 48, 1.7, "Bono x1.7 (48h)", 0, "{\"Type\":\"RepresentacionImagen\",\"NombreImagenCompleta\":null,\"NombreImagenMiniatura\":null}", "Recompensa_Potenciador" },
                    { 104, 48, 1.8, "Bono x1.8 (48h)", 0, "{\"Type\":\"RepresentacionImagen\",\"NombreImagenCompleta\":null,\"NombreImagenMiniatura\":null}", "Recompensa_Potenciador" },
                    { 105, 72, 1.8999999999999999, "Bono x1.9 (72h)", 0, "{\"Type\":\"RepresentacionImagen\",\"NombreImagenCompleta\":null,\"NombreImagenMiniatura\":null}", "Recompensa_Potenciador" },
                    { 106, 72, 2.0, "¡Doble Moneda! (72h)", 0, "{\"Type\":\"RepresentacionImagen\",\"NombreImagenCompleta\":null,\"NombreImagenMiniatura\":null}", "Recompensa_Potenciador" },
                    { 107, 96, 2.1000000000000001, "Bono x2.1 (96h)", 0, "{\"Type\":\"RepresentacionImagen\",\"NombreImagenCompleta\":null,\"NombreImagenMiniatura\":null}", "Recompensa_Potenciador" },
                    { 108, 96, 2.2000000000000002, "Bono x2.2 (96h)", 0, "{\"Type\":\"RepresentacionImagen\",\"NombreImagenCompleta\":null,\"NombreImagenMiniatura\":null}", "Recompensa_Potenciador" },
                    { 109, 120, 2.2999999999999998, "Bono x2.3 (120h)", 0, "{\"Type\":\"RepresentacionImagen\",\"NombreImagenCompleta\":null,\"NombreImagenMiniatura\":null}", "Recompensa_Potenciador" },
                    { 110, 168, 2.5, "¡Super Bono x2.5! (168h)", 0, "{\"Type\":\"RepresentacionImagen\",\"NombreImagenCompleta\":null,\"NombreImagenMiniatura\":null}", "Recompensa_Potenciador" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RolId", "EstampaConcurrencia", "NombreRol", "NombreRolNormalizado" },
                values: new object[,]
                {
                    { "2c5e174e-3b0e-446f-86af-483d56fd7210", null, "Profesor", "PROFESOR" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", null, "Estudiante", "ESTUDIANTE" }
                });

            migrationBuilder.InsertData(
                table: "TiposKudo",
                columns: new[] { "Id", "Descripcion", "Nombre", "NombreIcono" },
                values: new object[,]
                {
                    { 1, "Considera dar este kudo cuando un compañero te dedica tiempo para explicarte algo que no entendías o te ayuda a completar una tarea.", "Gracias por la Ayuda", "thumbs-up" },
                    { 2, "Considera dar este kudo cuando la pregunta de un compañero aclara una duda para todo el grupo o genera un debate que enriquece la clase.", "Esa Pregunta Suma", "hands-clapping" },
                    { 3, "Considera dar este kudo cuando el esfuerzo, la perseverancia o la actitud positiva de un compañero te motiven a superarte.", "Inspirador", "running" },
                    { 4, "Considera dar este kudo cuando un compañero toma tu idea o la de alguien más y la mejora, aportando un punto de vista que hace el trabajo más fuerte.", "Conectando Ideas", "lightbulb" },
                    { 5, "Considera dar este kudo cuando un compañero organiza el trabajo en equipo, se asegura de que todos participen o guía al grupo para cumplir el objetivo.", "Líder de Equipo", "star" },
                    { 6, "Considera dar este kudo cuando un compañero comparte un enlace, video, apunte o cualquier material que te resultó muy útil para estudiar o hacer una tarea.", "Bibliotecario", "book" },
                    { 7, "Considera dar este kudo cuando notes que un compañero se esfuerza por integrar a otros, asegurándose de que nadie se quede atrás y todos se sientan parte del equipo.", "Codo a Codo", "face-smile" },
                    { 8, "Considera dar este kudo cuando un compañero te da una sugerencia para mejorar tu trabajo de forma respetuosa y con la intención real de ayudar.", "Crítica que Construye", "hammer" },
                    { 9, "Considera dar este kudo cuando un compañero propone una solución original a un problema o una idea innovadora para un proyecto que sorprende al grupo.", "Chispa Creativa", "fire" },
                    { 10, "Considera dar este kudo cuando la explicación de un compañero sobre un tema muy difícil hace que, finalmente, lo entiendas con total claridad.", "Einstein", "brain" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "UsuarioId", "IntentosFallidos", "EstampaConcurrencia", "Correo", "CorreoConfirmado", "ImagenPerfil", "BloqueoHabilitado", "FinBloqueo", "CorreoNormalizado", "NombreUsuarioNormalizado", "ContraseniaHash", "Telefono", "TelefonoConfirmado", "EstampaSeguridad", "AutenticacionDosFactores", "NombreUsuario", "Apellido", "Nombre" },
                values: new object[,]
                {
                    { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, "b0c8b6a8-8e6b-4e6a-9e1e-2e0b166a9c76", "cecilia@gmail.com", true, null, false, null, "CECILIA@GMAIL.COM", "CECILIA", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_1", false, "cecilia", "Rodríguez", "Carlos" },
                    { "9e445865-a24d-4543-a6c6-9443d048cdb0", 0, "a1d3b5e7-9f2d-4b8c-8a1e-3f0e2d5b4a6b", "laura.fernandez@ludik.edu.uy", true, null, false, null, "LAURA.FERNANDEZ@LUDIK.EDU.UY", "LAURA.FERNANDEZ", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_2", false, "laura", "Fernández", "Laura" },
                    { "a1445865-a24d-4543-a6c6-9443d048cdb1", 0, "c4b6e8a0-1d3f-4e9a-9c8e-5d2a4f6b8c0d", null, false, null, false, null, null, "USUARIO1", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_3", false, "usuario1", "Pérez", "Santiago" },
                    { "aB445865-a24d-4543-a6c6-9443d048cdcC", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c1b", null, false, null, false, null, null, "ALEJANDRO", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_29", false, "alejandro", "Pereyra", "Alejandro" },
                    { "b2445865-a24d-4543-a6c6-9443d048cdb2", 0, "d5c7f9b1-2e4g-5f0b-a0d9-6e3b5g7c9d1e", null, false, null, false, null, null, "USUARIO2", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_4", false, "usuario2", "Gómez", "Valentina" },
                    { "bC445865-a24d-4543-a6c6-9443d048cdcD", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c1c", null, false, null, false, null, null, "CAROLINA", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_30", false, "carolina", "Cabrera", "Carolina" },
                    { "c3445865-a24d-4543-a6c6-9443d048cdb3", 0, "e6d80ac2-3f5h-6g1c-b1e0-7f4c6h8d0e2f", null, false, null, false, null, null, "USUARIO3", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_5", false, "usuario3", "González", "Matías" },
                    { "cD445865-a24d-4543-a6c6-9443d048cdcE", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c1d", null, false, null, false, null, null, "BRUNO", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_31", false, "bruno", "Castillo", "Bruno" },
                    { "d4445865-a24d-4543-a6c6-9443d048cdb4", 0, "f7e91bd3-4g6i-7h2d-c2f1-8g5d7i9e1f3g", null, false, null, false, null, null, "USUARIO4", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_6", false, "usuario4", "Martínez", "Camila" },
                    { "dE445865-a24d-4543-a6c6-9443d048cdcF", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c1e", null, false, null, false, null, null, "GABRIELA", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_32", false, "gabriela", "Paz", "Gabriela" },
                    { "e5445865-a24d-4543-a6c6-9443d048cdb5", 0, "g8f02ce4-5h7j-8i3e-d3g2-9h6e8j0f2g4h", null, false, null, false, null, null, "USUARIO5", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_7", false, "usuario5", "Silva", "Lucas" },
                    { "eF445865-a24d-4543-a6c6-9443d048cdd0", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c1f", null, false, null, false, null, null, "LEANDRO", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_33", false, "leandro", "Molina", "Leandro" },
                    { "f0445865-a24d-4543-a6c6-9443d048cdd1", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c20", null, false, null, false, null, null, "ANDREA", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_34", false, "andrea", "Vega", "Andrea" },
                    { "f6445865-a24d-4543-a6c6-9443d048cdb6", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c06", null, false, null, false, null, null, "USUARIO6", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_8", false, "usuario6", "Rodríguez", "Sofía" },
                    { "g1445865-a24d-4543-a6c6-9443d048cdd2", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c21", null, false, null, false, null, null, "GUILLERMO", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_35", false, "guillermo", "Rojas", "Guillermo" },
                    { "g7445865-a24d-4543-a6c6-9443d048cdb7", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c07", null, false, null, false, null, null, "USUARIO7", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_9", false, "usuario7", "García", "Juan" },
                    { "h2445865-a24d-4543-a6c6-9443d048cdd3", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c22", null, false, null, false, null, null, "JIMENA", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_36", false, "jimena", "Ortiz", "Jimena" },
                    { "h8445865-a24d-4543-a6c6-9443d048cdb8", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c08", null, false, null, false, null, null, "USUARIO8", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_10", false, "usuario8", "Sánchez", "Lucía" },
                    { "i3445865-a24d-4543-a6c6-9443d048cdd4", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c23", null, false, null, false, null, null, "MATEO", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_37", false, "mateo", "Benítez", "Mateo" },
                    { "i9445865-a24d-4543-a6c6-9443d048cdb9", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c09", null, false, null, false, null, null, "USUARIO9", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_11", false, "usuario9", "López", "Diego" },
                    { "jA445865-a24d-4543-a6c6-9443d048cdbA", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c0a", null, false, null, false, null, null, "USUARIO10", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_12", false, "usuario10", "Díaz", "Martina" },
                    { "kB445865-a24d-4543-a6c6-9443d048cdbB", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c0b", null, false, null, false, null, null, "USUARIO11", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_13", false, "usuario11", "Torres", "Agustín" },
                    { "lC445865-a24d-4543-a6c6-9443d048cdbC", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c0c", null, false, null, false, null, null, "USUARIO12", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_14", false, "usuario12", "Romero", "María" },
                    { "mD445865-a24d-4543-a6c6-9443d048cdbD", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c0d", null, false, null, false, null, null, "USUARIO13", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_15", false, "usuario13", "Álvarez", "Nicolás" },
                    { "nE445865-a24d-4543-a6c6-9443d048cdbE", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c0e", null, false, null, false, null, null, "USUARIO14", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_16", false, "usuario14", "Ruiz", "Paula" },
                    { "oF445865-a24d-4543-a6c6-9443d048cdbF", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c0f", null, false, null, false, null, null, "USUARIO15", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_17", false, "usuario15", "Vázquez", "Federico" },
                    { "p0445865-a24d-4543-a6c6-9443d048cdc0", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c10", null, false, null, false, null, null, "USUARIO16", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_18", false, "usuario16", "Sosa", "Florencia" },
                    { "q1445865-a24d-4543-a6c6-9443d048cdc1", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c11", null, false, null, false, null, null, "USUARIO17", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_19", false, "usuario17", "Castro", "Sebastián" },
                    { "r2445865-a24d-4543-a6c6-9443d048cdc2", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c12", null, false, null, false, null, null, "USUARIO18", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_20", false, "usuario18", "Giménez", "Victoria" },
                    { "s3445865-a24d-4543-a6c6-9443d048cdc3", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c13", null, false, null, false, null, null, "USUARIO19", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_21", false, "usuario19", "Acosta", "Joaquín" },
                    { "t4445865-a24d-4543-a6c6-9443d048cdc4", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c14", null, false, null, false, null, null, "USUARIO20", "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==", null, false, "STATIC_SECURITY_STAMP_22", false, "usuario20", "Ramos", "Julieta" },
                    { "u5445865-a24d-4543-a6c6-9443d048cdc5", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c15", null, false, null, false, null, null, "MANUEL", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_23", false, "manuel", "Herrera", "Manuel" },
                    { "v6445865-a24d-4543-a6c6-9443d048cdc6", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c16", null, false, null, false, null, null, "ANA", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_24", false, "ana", "Medina", "Ana" },
                    { "w7445865-a24d-4543-a6c6-9443d048cdc7", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c17", null, false, null, false, null, null, "FACUNDO", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_25", false, "facundo", "Morales", "Facundo" },
                    { "x8445865-a24d-4543-a6c6-9443d048cdc8", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c18", null, false, null, false, null, null, "DANIELA", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_26", false, "daniela", "Núñez", "Daniela" },
                    { "y9445865-a24d-4543-a6c6-9443d048cdcA", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c19", null, false, null, false, null, null, "IGNACIO", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_27", false, "ignacio", "Flores", "Ignacio" },
                    { "zA445865-a24d-4543-a6c6-9443d048cdcB", 0, "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c1a", null, false, null, false, null, null, "ROMINA", "AQAAAAIAAYagAAAAENuS3fE5d1k/aN2zV8mY9wR8cI7qU5kY4tL6wP9eO3bF0dG1sS5nC2vX3jJ4oP7eWw==", null, false, "STATIC_SECURITY_STAMP_28", false, "romina", "Ríos", "Romina" }
                });

            migrationBuilder.InsertData(
                table: "Estudiantes",
                column: "UsuarioId",
                values: new object[]
                {
                    "a1445865-a24d-4543-a6c6-9443d048cdb1",
                    "aB445865-a24d-4543-a6c6-9443d048cdcC",
                    "b2445865-a24d-4543-a6c6-9443d048cdb2",
                    "bC445865-a24d-4543-a6c6-9443d048cdcD",
                    "c3445865-a24d-4543-a6c6-9443d048cdb3",
                    "cD445865-a24d-4543-a6c6-9443d048cdcE",
                    "d4445865-a24d-4543-a6c6-9443d048cdb4",
                    "dE445865-a24d-4543-a6c6-9443d048cdcF",
                    "e5445865-a24d-4543-a6c6-9443d048cdb5",
                    "eF445865-a24d-4543-a6c6-9443d048cdd0",
                    "f0445865-a24d-4543-a6c6-9443d048cdd1",
                    "f6445865-a24d-4543-a6c6-9443d048cdb6",
                    "g1445865-a24d-4543-a6c6-9443d048cdd2",
                    "g7445865-a24d-4543-a6c6-9443d048cdb7",
                    "h2445865-a24d-4543-a6c6-9443d048cdd3",
                    "h8445865-a24d-4543-a6c6-9443d048cdb8",
                    "i3445865-a24d-4543-a6c6-9443d048cdd4",
                    "i9445865-a24d-4543-a6c6-9443d048cdb9",
                    "jA445865-a24d-4543-a6c6-9443d048cdbA",
                    "kB445865-a24d-4543-a6c6-9443d048cdbB",
                    "lC445865-a24d-4543-a6c6-9443d048cdbC",
                    "mD445865-a24d-4543-a6c6-9443d048cdbD",
                    "nE445865-a24d-4543-a6c6-9443d048cdbE",
                    "oF445865-a24d-4543-a6c6-9443d048cdbF",
                    "p0445865-a24d-4543-a6c6-9443d048cdc0",
                    "q1445865-a24d-4543-a6c6-9443d048cdc1",
                    "r2445865-a24d-4543-a6c6-9443d048cdc2",
                    "s3445865-a24d-4543-a6c6-9443d048cdc3",
                    "t4445865-a24d-4543-a6c6-9443d048cdc4",
                    "u5445865-a24d-4543-a6c6-9443d048cdc5",
                    "v6445865-a24d-4543-a6c6-9443d048cdc6",
                    "w7445865-a24d-4543-a6c6-9443d048cdc7",
                    "x8445865-a24d-4543-a6c6-9443d048cdc8",
                    "y9445865-a24d-4543-a6c6-9443d048cdcA",
                    "zA445865-a24d-4543-a6c6-9443d048cdcB"
                });

            migrationBuilder.InsertData(
                table: "Hitos",
                columns: new[] { "Id", "CantMedallasRequeridas", "RecompensaId" },
                values: new object[,]
                {
                    { 1, 5, 101 },
                    { 2, 10, 102 },
                    { 3, 20, 103 },
                    { 4, 35, 104 },
                    { 5, 50, 105 },
                    { 6, 75, 106 },
                    { 7, 100, 107 },
                    { 8, 150, 108 },
                    { 9, 200, 109 },
                    { 10, 250, 110 }
                });

            migrationBuilder.InsertData(
                table: "Profesores",
                column: "UsuarioId",
                values: new object[]
                {
                    "8e445865-a24d-4543-a6c6-9443d048cdb9",
                    "9e445865-a24d-4543-a6c6-9443d048cdb0"
                });

            migrationBuilder.InsertData(
                table: "UsuariosRoles",
                columns: new[] { "RolId", "UsuarioId" },
                values: new object[,]
                {
                    { "2c5e174e-3b0e-446f-86af-483d56fd7210", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { "2c5e174e-3b0e-446f-86af-483d56fd7210", "9e445865-a24d-4543-a6c6-9443d048cdb0" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "a1445865-a24d-4543-a6c6-9443d048cdb1" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "aB445865-a24d-4543-a6c6-9443d048cdcC" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "b2445865-a24d-4543-a6c6-9443d048cdb2" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "bC445865-a24d-4543-a6c6-9443d048cdcD" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "c3445865-a24d-4543-a6c6-9443d048cdb3" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "cD445865-a24d-4543-a6c6-9443d048cdcE" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "d4445865-a24d-4543-a6c6-9443d048cdb4" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "dE445865-a24d-4543-a6c6-9443d048cdcF" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "e5445865-a24d-4543-a6c6-9443d048cdb5" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "eF445865-a24d-4543-a6c6-9443d048cdd0" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "f0445865-a24d-4543-a6c6-9443d048cdd1" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "f6445865-a24d-4543-a6c6-9443d048cdb6" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "g1445865-a24d-4543-a6c6-9443d048cdd2" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "g7445865-a24d-4543-a6c6-9443d048cdb7" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "h2445865-a24d-4543-a6c6-9443d048cdd3" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "h8445865-a24d-4543-a6c6-9443d048cdb8" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "i3445865-a24d-4543-a6c6-9443d048cdd4" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "i9445865-a24d-4543-a6c6-9443d048cdb9" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "jA445865-a24d-4543-a6c6-9443d048cdbA" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "kB445865-a24d-4543-a6c6-9443d048cdbB" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "lC445865-a24d-4543-a6c6-9443d048cdbC" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "mD445865-a24d-4543-a6c6-9443d048cdbD" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "nE445865-a24d-4543-a6c6-9443d048cdbE" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "oF445865-a24d-4543-a6c6-9443d048cdbF" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "p0445865-a24d-4543-a6c6-9443d048cdc0" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "q1445865-a24d-4543-a6c6-9443d048cdc1" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "r2445865-a24d-4543-a6c6-9443d048cdc2" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "s3445865-a24d-4543-a6c6-9443d048cdc3" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "t4445865-a24d-4543-a6c6-9443d048cdc4" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "u5445865-a24d-4543-a6c6-9443d048cdc5" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "v6445865-a24d-4543-a6c6-9443d048cdc6" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "w7445865-a24d-4543-a6c6-9443d048cdc7" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "x8445865-a24d-4543-a6c6-9443d048cdc8" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "y9445865-a24d-4543-a6c6-9443d048cdcA" },
                    { "3d5e174e-3b0e-446f-86af-483d56fd7211", "zA445865-a24d-4543-a6c6-9443d048cdcB" }
                });

            migrationBuilder.InsertData(
                table: "Medallas",
                columns: new[] { "Id", "Descripcion", "MonedasOtorgadas", "Nombre", "NombreIcono", "ProfesorId" },
                values: new object[,]
                {
                    { 1, "Asistencia y participación en todas las clases del mes.", 30, "Participación Perfecta", "graduation-cap", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 2, "Ayuda destacada a compañeros en proyectos grupales.", 25, "Maestro de la Colaboración", "people-group", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 3, "Realización de preguntas perspicaces que enriquecen la clase.", 15, "Mente Curiosa", "magnifying-glass", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 4, "Se otorga por ser un pilar de apoyo para tus compañeros. Demuestra que estás siempre dispuesto a ofrecer tu ayuda cuando alguien la necesita.", 20, "Compañerismo", "handshake", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 5, "Premia a las mentes que nunca dejan de preguntar. Se consigue al realizar preguntas que desafían al grupo y enriquecen el aprendizaje de todos.", 15, "Curiosidad Insaciable", "lightbulb", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 6, "Reconoce a quienes inspiran con su ejemplo. Se obtiene al demostrar una actitud y un esfuerzo que motivan a todo el grupo a superarse.", 25, "Faro del Grupo", "star", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 7, "Para aquellos que no solo tienen buenas ideas, sino que construyen sobre las de los demás para crear algo aún mejor.", 20, "Arquitecto de Ideas", "brain", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 8, "Se otorga por demostrar liderazgo natural, guiando y organizando al equipo para alcanzar metas comunes de forma efectiva.", 25, "Capitán de Equipo", "crown", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 9, "Premia la iniciativa de buscar y compartir recursos valiosos (videos, artículos, herramientas) que benefician a toda la clase.", 15, "Cazador de Tesoros", "gem", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 10, "Se consigue al fomentar activamente un ambiente de respeto e inclusión, asegurando que cada miembro del grupo se sienta valorado.", 20, "Espíritu de Equipo", "hands-clapping", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 11, "Reconoce la habilidad de dar críticas constructivas que ayudan a los compañeros a mejorar su trabajo de forma positiva y amable.", 15, "Pulidor de Diamantes", "diamond", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 12, "Se otorga por aportar ideas creativas y soluciones originales que sacan al grupo de la rutina y abren nuevas posibilidades.", 20, "Mente Innovadora", "wand-magic-sparkles", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 13, "Premia la increíble habilidad de tomar un tema complejo y explicarlo de una manera tan clara y sencilla que todos puedan entenderlo.", 25, "El Explicador", "chalkboard", "9e445865-a24d-4543-a6c6-9443d048cdb0" }
                });

            migrationBuilder.InsertData(
                table: "PreguntasRespuestasSeguridad",
                columns: new[] { "Id", "EstudianteId", "PreguntaDeSeguridadId", "Respuesta" },
                values: new object[,]
                {
                    { 1, "a1445865-a24d-4543-a6c6-9443d048cdb1", 1, "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==" },
                    { 2, "a1445865-a24d-4543-a6c6-9443d048cdb1", 3, "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==" },
                    { 3, "b2445865-a24d-4543-a6c6-9443d048cdb2", 2, "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==" },
                    { 4, "b2445865-a24d-4543-a6c6-9443d048cdb2", 5, "AQAAAAIAAYagAAAAEICeFSdiFCtz68TPDuBQMzkt7RT8ecvoXx3nTwJev5fDDu098ITlRrx8fymingA8Mg==" }
                });

            migrationBuilder.InsertData(
                table: "TablasEquivalencia",
                columns: new[] { "Id", "Nombre", "ProfesorId" },
                values: new object[,]
                {
                    { 1, "Calificaciones Estándar", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { 2, "Evaluación Continua", "9e445865-a24d-4543-a6c6-9443d048cdb0" }
                });

            migrationBuilder.InsertData(
                table: "Equivalencias",
                columns: new[] { "Id", "Nota", "TablaEquivalenciaId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 1 },
                    { 4, 4, 1 },
                    { 5, 5, 1 },
                    { 6, 6, 1 },
                    { 7, 7, 1 },
                    { 8, 8, 1 },
                    { 9, 9, 1 },
                    { 10, 10, 1 },
                    { 11, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Grupos",
                columns: new[] { "Id", "EnlaceUnionId", "FCreacion", "FechaUltimoReinicio", "Institucion", "Materia", "Nombre", "ProfesorId", "TablaEquivalenciaId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 6, 19, 10, 30, 0, 0, DateTimeKind.Utc), null, "Liceo N°5", "Matemática", "Matemática 1A - 2025", "8e445865-a24d-4543-a6c6-9443d048cdb9", 1 },
                    { 2, 2, new DateTime(2025, 6, 19, 10, 30, 0, 0, DateTimeKind.Utc), null, "Liceo N°5", "Historia", "Historia Universal - 2025", "9e445865-a24d-4543-a6c6-9443d048cdb0", 2 },
                    { 3, 3, new DateTime(2025, 6, 19, 10, 30, 0, 0, DateTimeKind.Utc), null, "Liceo N°6", "Matemática", "Matemática 2B - 2025", "8e445865-a24d-4543-a6c6-9443d048cdb9", 1 },
                    { 4, 4, new DateTime(2025, 6, 19, 10, 30, 0, 0, DateTimeKind.Utc), null, "Liceo N°6", "Matemática", "Matemática Cientifico A - 2025", "8e445865-a24d-4543-a6c6-9443d048cdb9", 1 }
                });

            migrationBuilder.InsertData(
                table: "EquivalenciaMedallas",
                columns: new[] { "EquivalenciaId", "MedallaId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 4 },
                    { 3, 13 },
                    { 4, 1 },
                    { 4, 4 },
                    { 4, 6 },
                    { 5, 1 },
                    { 5, 4 },
                    { 5, 6 },
                    { 5, 8 },
                    { 6, 1 },
                    { 6, 4 },
                    { 6, 6 },
                    { 6, 8 },
                    { 6, 10 },
                    { 7, 1 },
                    { 7, 4 },
                    { 7, 6 },
                    { 7, 8 },
                    { 7, 10 },
                    { 7, 12 },
                    { 8, 1 },
                    { 8, 2 },
                    { 8, 4 },
                    { 8, 6 },
                    { 8, 8 },
                    { 8, 10 },
                    { 8, 12 },
                    { 9, 1 },
                    { 9, 2 },
                    { 9, 3 },
                    { 9, 4 },
                    { 9, 6 },
                    { 9, 8 },
                    { 9, 10 },
                    { 9, 12 },
                    { 10, 1 },
                    { 10, 2 },
                    { 10, 3 },
                    { 10, 4 },
                    { 10, 5 },
                    { 10, 6 },
                    { 10, 8 },
                    { 10, 10 },
                    { 10, 12 },
                    { 11, 1 },
                    { 11, 2 },
                    { 11, 3 },
                    { 11, 4 },
                    { 11, 5 },
                    { 11, 6 },
                    { 11, 7 },
                    { 11, 8 },
                    { 11, 10 },
                    { 11, 12 }
                });

            migrationBuilder.InsertData(
                table: "PerfilesEstudiantes",
                columns: new[] { "Id", "EstudianteId", "GrupoId", "KudosDisponiblesParaOtorgar", "MetaCalificacion", "Monedas", "NombreImagenCompleta", "NombreImagenMiniatura" },
                values: new object[,]
                {
                    { 1, "a1445865-a24d-4543-a6c6-9443d048cdb1", 1, 3, 8, 120, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 2, "b2445865-a24d-4543-a6c6-9443d048cdb2", 1, 3, 9, 150, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 3, "c3445865-a24d-4543-a6c6-9443d048cdb3", 1, 3, 7, 95, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 4, "d4445865-a24d-4543-a6c6-9443d048cdb4", 2, 3, 10, 200, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 5, "e5445865-a24d-4543-a6c6-9443d048cdb5", 2, 3, 8, 180, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 6, "f6445865-a24d-4543-a6c6-9443d048cdb6", 1, 3, 7, 110, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 7, "g7445865-a24d-4543-a6c6-9443d048cdb7", 1, 3, 9, 210, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 8, "h8445865-a24d-4543-a6c6-9443d048cdb8", 1, 3, 6, 80, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 9, "i9445865-a24d-4543-a6c6-9443d048cdb9", 1, 3, 10, 300, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 10, "jA445865-a24d-4543-a6c6-9443d048cdbA", 1, 3, 8, 125, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 11, "kB445865-a24d-4543-a6c6-9443d048cdbB", 1, 3, 8, 145, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 12, "lC445865-a24d-4543-a6c6-9443d048cdbC", 1, 3, 9, 160, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 13, "mD445865-a24d-4543-a6c6-9443d048cdbD", 1, 3, 6, 70, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 14, "nE445865-a24d-4543-a6c6-9443d048cdbE", 1, 3, 9, 190, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 15, "oF445865-a24d-4543-a6c6-9443d048cdbF", 1, 3, 10, 250, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 16, "p0445865-a24d-4543-a6c6-9443d048cdc0", 1, 3, 8, 130, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 17, "q1445865-a24d-4543-a6c6-9443d048cdc1", 1, 3, 7, 115, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 18, "r2445865-a24d-4543-a6c6-9443d048cdc2", 1, 3, 7, 90, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 19, "s3445865-a24d-4543-a6c6-9443d048cdc3", 1, 3, 9, 220, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 20, "t4445865-a24d-4543-a6c6-9443d048cdc4", 1, 3, 8, 170, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 21, "u5445865-a24d-4543-a6c6-9443d048cdc5", 1, 3, 8, 155, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 22, "v6445865-a24d-4543-a6c6-9443d048cdc6", 1, 3, 7, 105, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 23, "w7445865-a24d-4543-a6c6-9443d048cdc7", 3, 3, 7, 100, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 24, "x8445865-a24d-4543-a6c6-9443d048cdc8", 3, 3, 8, 120, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 25, "y9445865-a24d-4543-a6c6-9443d048cdcA", 3, 3, 10, 250, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 26, "zA445865-a24d-4543-a6c6-9443d048cdcB", 3, 3, 8, 130, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 27, "aB445865-a24d-4543-a6c6-9443d048cdcC", 3, 3, 6, 90, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 28, "bC445865-a24d-4543-a6c6-9443d048cdcD", 3, 3, 9, 160, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 29, "cD445865-a24d-4543-a6c6-9443d048cdcE", 3, 3, 9, 175, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 30, "dE445865-a24d-4543-a6c6-9443d048cdcF", 3, 3, 8, 140, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 31, "eF445865-a24d-4543-a6c6-9443d048cdd0", 4, 3, 7, 110, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 32, "f0445865-a24d-4543-a6c6-9443d048cdd1", 4, 3, 9, 200, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 33, "g1445865-a24d-4543-a6c6-9443d048cdd2", 4, 3, 8, 150, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 34, "h2445865-a24d-4543-a6c6-9443d048cdd3", 4, 3, 9, 180, "default/avatar_full.jpg", "default/avatar_thumb.jpg" },
                    { 35, "i3445865-a24d-4543-a6c6-9443d048cdd4", 4, 3, 10, 220, "default/avatar_full.jpg", "default/avatar_thumb.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Tiendas",
                columns: new[] { "Id", "GrupoId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "Avatares",
                columns: new[] { "Id", "ColorFondo", "PerfilEstudianteId", "Rotacion", "Voltear", "Zoom" },
                values: new object[,]
                {
                    { 1, "#b1e2ff", 1, 0, false, 0 },
                    { 2, "#a7ffc4", 2, 0, false, 0 },
                    { 3, "#ffafb9", 3, 0, false, 0 },
                    { 4, "#ffffb1", 4, 0, false, 0 },
                    { 5, "#e6e6e6", 5, 0, false, 0 },
                    { 6, "#f8d7da", 6, 0, false, 0 },
                    { 7, "#d4edda", 7, 0, false, 0 },
                    { 8, "#fff3cd", 8, 0, false, 0 },
                    { 9, "#d1ecf1", 9, 0, false, 0 },
                    { 10, "#e2d9f3", 10, 0, false, 0 },
                    { 11, "#fce3d4", 11, 0, false, 0 },
                    { 12, "#c3e6cb", 12, 0, false, 0 },
                    { 13, "#f5c6cb", 13, 0, false, 0 },
                    { 14, "#bee5eb", 14, 0, false, 0 },
                    { 15, "#ffeeba", 15, 0, false, 0 },
                    { 16, "#d6d8f5", 16, 0, false, 0 },
                    { 17, "#fde2e2", 17, 0, false, 0 },
                    { 18, "#d1e7dd", 18, 0, false, 0 },
                    { 19, "#cce7ff", 19, 0, false, 0 },
                    { 20, "#fbf8cc", 20, 0, false, 0 },
                    { 21, "#f1e0ff", 21, 0, false, 0 },
                    { 22, "#e0f7fa", 22, 0, false, 0 },
                    { 23, "#ffe0e0", 23, 0, false, 0 },
                    { 24, "#e0ffe0", 24, 0, false, 0 },
                    { 25, "#e0e0ff", 25, 0, false, 0 },
                    { 26, "#fff0e0", 26, 0, false, 0 },
                    { 27, "#f0fff0", 27, 0, false, 0 },
                    { 28, "#f0f0ff", 28, 0, false, 0 },
                    { 29, "#e0fff8", 29, 0, false, 0 },
                    { 30, "#f8e0ff", 30, 0, false, 0 },
                    { 31, "#eaf5ff", 31, 0, false, 0 },
                    { 32, "#fff5e6", 32, 0, false, 0 },
                    { 33, "#f2f2f2", 33, 0, false, 0 },
                    { 34, "#e6f7ff", 34, 0, false, 0 },
                    { 35, "#fae6ff", 35, 0, false, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvatarAtributos_AtributoSeleccionadoId",
                table: "AvatarAtributos",
                column: "AtributoSeleccionadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Avatares_PerfilEstudianteId",
                table: "Avatares",
                column: "PerfilEstudianteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BarrasProgreso_PerfilEstudianteId",
                table: "BarrasProgreso",
                column: "PerfilEstudianteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BarrasProgreso_TablaEquivalenciaId",
                table: "BarrasProgreso",
                column: "TablaEquivalenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_EquivalenciaMedallas_MedallaId",
                table: "EquivalenciaMedallas",
                column: "MedallaId");

            migrationBuilder.CreateIndex(
                name: "IX_Equivalencias_TablaEquivalenciaId",
                table: "Equivalencias",
                column: "TablaEquivalenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_EstudianteHitos_HitoId",
                table: "EstudianteHitos",
                column: "HitoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstudiantePotenciadores_EstudianteId",
                table: "EstudiantePotenciadores",
                column: "EstudianteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstudiantePotenciadores_PotenciadorId",
                table: "EstudiantePotenciadores",
                column: "PotenciadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupo_ProfesorId",
                table: "Grupos",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_EnlaceUnionId",
                table: "Grupos",
                column: "EnlaceUnionId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_Nombre",
                table: "Grupos",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_TablaEquivalenciaId",
                table: "Grupos",
                column: "TablaEquivalenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Hitos_RecompensaId",
                table: "Hitos",
                column: "RecompensaId");

            migrationBuilder.CreateIndex(
                name: "IX_IniciosSesionUsuario_UsuarioId",
                table: "IniciosSesionUsuario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_KudosOtorgados_PerfilEstudianteEmisorId",
                table: "KudosOtorgados",
                column: "PerfilEstudianteEmisorId");

            migrationBuilder.CreateIndex(
                name: "IX_KudosOtorgados_PerfilEstudianteMedallaId",
                table: "KudosOtorgados",
                column: "PerfilEstudianteMedallaId");

            migrationBuilder.CreateIndex(
                name: "IX_KudosOtorgados_PerfilEstudianteReceptorId",
                table: "KudosOtorgados",
                column: "PerfilEstudianteReceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_KudosOtorgados_TipoKudoId",
                table: "KudosOtorgados",
                column: "TipoKudoId");

            migrationBuilder.CreateIndex(
                name: "IX_Medallas_Nombre",
                table: "Medallas",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Medallas_ProfesorId",
                table: "Medallas",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilesEstudiantes_EstudianteId",
                table: "PerfilesEstudiantes",
                column: "EstudianteId");

            migrationBuilder.CreateIndex(
                name: "UX_PerfilEstudiante_GrupoId_EstudianteId",
                table: "PerfilesEstudiantes",
                columns: new[] { "GrupoId", "EstudianteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerfilEstudianteMedallas_MedallaId",
                table: "PerfilEstudianteMedallas",
                column: "MedallaId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilEstudianteMedallas_PerfilEstudianteId",
                table: "PerfilEstudianteMedallas",
                column: "PerfilEstudianteId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilEstudianteRecompensas_PerfilEstudianteId_RecompensaId",
                table: "PerfilEstudianteRecompensas",
                columns: new[] { "PerfilEstudianteId", "RecompensaId" });

            migrationBuilder.CreateIndex(
                name: "IX_PerfilEstudianteRecompensas_RecompensaId",
                table: "PerfilEstudianteRecompensas",
                column: "RecompensaId");

            migrationBuilder.CreateIndex(
                name: "IX_PreguntasRespuestasSeguridad_EstudianteId",
                table: "PreguntasRespuestasSeguridad",
                column: "EstudianteId");

            migrationBuilder.CreateIndex(
                name: "IX_PreguntasRespuestasSeguridad_PreguntaDeSeguridadId",
                table: "PreguntasRespuestasSeguridad",
                column: "PreguntaDeSeguridadId");

            migrationBuilder.CreateIndex(
                name: "IX_PAC_GrupoId",
                table: "ProyectosAulaColaborativo",
                column: "GrupoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProyectosAulaColaborativo_RecompensaClaseId",
                table: "ProyectosAulaColaborativo",
                column: "RecompensaClaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ReclamacionesRoles_RolId",
                table: "ReclamacionesRoles",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReclamacionesUsuario_UsuarioId",
                table: "ReclamacionesUsuario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Recompensas_AtributoAvatarId",
                table: "Recompensas",
                column: "AtributoAvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Recompensas_Nombre",
                table: "Recompensas",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_RecompensasDeProfesores_ProfesorId",
                table: "RecompensasDeProfesores",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_RecompensasDeProfesores_RecompensaId",
                table: "RecompensasDeProfesores",
                column: "RecompensaId");

            migrationBuilder.CreateIndex(
                name: "IX_RecompensaTienda_TiendaId",
                table: "RecompensaTienda",
                column: "TiendaId");

            migrationBuilder.CreateIndex(
                name: "IX_RendimientoPeriodoMedallas_MedallaId",
                table: "RendimientoPeriodoMedallas",
                column: "MedallaId");

            migrationBuilder.CreateIndex(
                name: "IX_RendimientoPeriodoMedallas_RendimientoPeriodoId_MedallaId",
                table: "RendimientoPeriodoMedallas",
                columns: new[] { "RendimientoPeriodoId", "MedallaId" });

            migrationBuilder.CreateIndex(
                name: "IX_RendimientosPeriodos_PerfilEstudianteId",
                table: "RendimientosPeriodos",
                column: "PerfilEstudianteId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NombreRolNormalizado",
                unique: true,
                filter: "[NombreRolNormalizado] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesPerfilMedalla_GrupoId",
                table: "SolicitudesPerfilMedalla",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesPerfilMedalla_MedallaId",
                table: "SolicitudesPerfilMedalla",
                column: "MedallaId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesPerfilMedalla_PerfilEstudianteId",
                table: "SolicitudesPerfilMedalla",
                column: "PerfilEstudianteId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesUnion_EstudianteId",
                table: "SolicitudesUnion",
                column: "EstudianteId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesUnion_GrupoId",
                table: "SolicitudesUnion",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_TablaClasificacionParticipantes_PerfilEstudianteId",
                table: "TablaClasificacionParticipantes",
                column: "PerfilEstudianteId");

            migrationBuilder.CreateIndex(
                name: "IX_TablasClasificacion_GrupoId",
                table: "TablasClasificacion",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_TablasClasificacion_MedallaAsociadaId",
                table: "TablasClasificacion",
                column: "MedallaAsociadaId");

            migrationBuilder.CreateIndex(
                name: "IX_TablasClasificacion_Nombre",
                table: "TablasClasificacion",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_TablasEquivalencia_Nombre",
                table: "TablasEquivalencia",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_TablasEquivalencia_ProfesorId",
                table: "TablasEquivalencia",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tiendas_GrupoId",
                table: "Tiendas",
                column: "GrupoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UmbralesParaMedallasPorKudos_GrupoId",
                table: "UmbralesParaMedallasPorKudos",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_UmbralesParaMedallasPorKudos_MedallaId",
                table: "UmbralesParaMedallasPorKudos",
                column: "MedallaId");

            migrationBuilder.CreateIndex(
                name: "IX_UmbralesParaMedallasPorKudos_TipoKudoId",
                table: "UmbralesParaMedallasPorKudos",
                column: "TipoKudoId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Usuarios",
                column: "CorreoNormalizado");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Usuarios",
                column: "NombreUsuarioNormalizado",
                unique: true,
                filter: "[NombreUsuarioNormalizado] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_RolId",
                table: "UsuariosRoles",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvatarAtributos");

            migrationBuilder.DropTable(
                name: "BarrasProgreso");

            migrationBuilder.DropTable(
                name: "EquivalenciaMedallas");

            migrationBuilder.DropTable(
                name: "EstudianteHitos");

            migrationBuilder.DropTable(
                name: "EstudiantePotenciadores");

            migrationBuilder.DropTable(
                name: "IniciosSesionUsuario");

            migrationBuilder.DropTable(
                name: "KudosOtorgados");

            migrationBuilder.DropTable(
                name: "PerfilEstudianteRecompensas");

            migrationBuilder.DropTable(
                name: "PreguntasRespuestasSeguridad");

            migrationBuilder.DropTable(
                name: "ProyectosAulaColaborativo");

            migrationBuilder.DropTable(
                name: "ReclamacionesRoles");

            migrationBuilder.DropTable(
                name: "ReclamacionesUsuario");

            migrationBuilder.DropTable(
                name: "RecompensasDeProfesores");

            migrationBuilder.DropTable(
                name: "RecompensaTienda");

            migrationBuilder.DropTable(
                name: "RendimientoPeriodoMedallas");

            migrationBuilder.DropTable(
                name: "SolicitudesPerfilMedalla");

            migrationBuilder.DropTable(
                name: "SolicitudesUnion");

            migrationBuilder.DropTable(
                name: "TablaClasificacionParticipantes");

            migrationBuilder.DropTable(
                name: "TokensUsuario");

            migrationBuilder.DropTable(
                name: "UmbralesParaMedallasPorKudos");

            migrationBuilder.DropTable(
                name: "UsuariosRoles");

            migrationBuilder.DropTable(
                name: "Avatares");

            migrationBuilder.DropTable(
                name: "Equivalencias");

            migrationBuilder.DropTable(
                name: "Hitos");

            migrationBuilder.DropTable(
                name: "PerfilEstudianteMedallas");

            migrationBuilder.DropTable(
                name: "PreguntasDeSeguridad");

            migrationBuilder.DropTable(
                name: "Tiendas");

            migrationBuilder.DropTable(
                name: "RendimientosPeriodos");

            migrationBuilder.DropTable(
                name: "TablasClasificacion");

            migrationBuilder.DropTable(
                name: "TiposKudo");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Recompensas");

            migrationBuilder.DropTable(
                name: "PerfilesEstudiantes");

            migrationBuilder.DropTable(
                name: "Medallas");

            migrationBuilder.DropTable(
                name: "AtributosAvatar");

            migrationBuilder.DropTable(
                name: "Estudiantes");

            migrationBuilder.DropTable(
                name: "Grupos");

            migrationBuilder.DropTable(
                name: "EnlacesUnion");

            migrationBuilder.DropTable(
                name: "TablasEquivalencia");

            migrationBuilder.DropTable(
                name: "Profesores");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
