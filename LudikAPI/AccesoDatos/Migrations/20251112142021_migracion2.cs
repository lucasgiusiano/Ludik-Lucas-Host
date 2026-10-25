using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class migracion2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TablaClasificacionParticipantes");

            migrationBuilder.AddColumn<int>(
                name: "PerfilEstudianteId",
                table: "TablasClasificacion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TablasClasificacion_PerfilEstudianteId",
                table: "TablasClasificacion",
                column: "PerfilEstudianteId");

            migrationBuilder.AddForeignKey(
                name: "FK_TablasClasificacion_PerfilesEstudiantes_PerfilEstudianteId",
                table: "TablasClasificacion",
                column: "PerfilEstudianteId",
                principalTable: "PerfilesEstudiantes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TablasClasificacion_PerfilesEstudiantes_PerfilEstudianteId",
                table: "TablasClasificacion");

            migrationBuilder.DropIndex(
                name: "IX_TablasClasificacion_PerfilEstudianteId",
                table: "TablasClasificacion");

            migrationBuilder.DropColumn(
                name: "PerfilEstudianteId",
                table: "TablasClasificacion");

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

            migrationBuilder.CreateIndex(
                name: "IX_TablaClasificacionParticipantes_PerfilEstudianteId",
                table: "TablaClasificacionParticipantes",
                column: "PerfilEstudianteId");
        }
    }
}
