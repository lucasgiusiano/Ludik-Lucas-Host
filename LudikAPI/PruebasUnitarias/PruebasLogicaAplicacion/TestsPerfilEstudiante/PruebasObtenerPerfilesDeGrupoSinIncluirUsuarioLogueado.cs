using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using LogicaNegocio.Entidades;
using LogicaAplicacion.DTOs.PerfilEstudianteDTO;
using LogicaAplicacion.ImplementacionCasosUsos.PerfilEstudiante;
using InterfacesRepositorio;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObjects;

namespace PruebasUnitarias.PruebasLogicaAplicacion.PerfilEstudiante
{
    public class PruebasObtenerPerfilesSinLogueado
    {
        [Fact]
        public async Task EjecutarAsync_CaminoFeliz_RetornaPerfilesSinLogueado_Y_MedallasMapeadas()
        {
            // Arrange
            var estudianteLogueadoId = "estu123";

            // ValueObject NombreCompleto
            var nombreLogueado = NombreCompleto.Crear("Logueado", "Estudiante").Valor!;
            var nombreCompanero = NombreCompleto.Crear("Companero", "Uno").Valor!;

            // Entidad perfil del usuario logueado (sin medallas)
            var perfilLogueado = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = 1,
                EstudianteId = estudianteLogueadoId,
                GrupoId = 10,
                NombreImagenMiniatura = "mini1.png",
                NombreImagenCompleta = "full1.png",
                MetaCalificacion = 5,
                Monedas = 50,
                Estudiante = new LogicaNegocio.Entidades.Estudiante { NombreCompleto = nombreLogueado },
                Grupo = new LogicaNegocio.Entidades.Grupo { Nombre = "Grupo Test" },
                MedallasObtenidas = new List<PerfilEstudianteMedalla>()
            };

            // Entidad perfil de un compañero con una medalla
            var medallaPrueba = new LogicaNegocio.Entidades.Medalla
            {
                Id = 99,
                Nombre = "Super Medalla",
                NombreIcono = "http://img/test.png",
                Descripcion = "Descripción test",
                MonedasOtorgadas = 10,
            };
            var perfilCompanero = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = 2,
                EstudianteId = "estu456",
                GrupoId = 10,
                NombreImagenMiniatura = "mini2.png",
                NombreImagenCompleta = "full2.png",
                MetaCalificacion = 10,
                Monedas = 100,
                Estudiante = new LogicaNegocio.Entidades.Estudiante { NombreCompleto = nombreCompanero },
                Grupo = new LogicaNegocio.Entidades.Grupo { Nombre = "Grupo Test" },
                MedallasObtenidas = new List<PerfilEstudianteMedalla>
                {
                    new PerfilEstudianteMedalla
                    {
                        PerfilEstudianteId = 2,
                        Medalla = medallaPrueba,
                        FechaObtencion = System.DateTime.UtcNow
                    }
                }
            };

            // Mock del repositorio de perfiles de estudiantes en grupo
            var mockRepositorioPerfiles = new Mock<IRepositorioPerfilEstudianteGrupo>();
            mockRepositorioPerfiles.Setup(r => r.ObtenerPorGrupoIdAsync(10))
                .ReturnsAsync(Resultado<List<LogicaNegocio.Entidades.PerfilEstudiante>>.Exitoso(
                    new List<LogicaNegocio.Entidades.PerfilEstudiante> { perfilLogueado, perfilCompanero }
                ));

            // Mock del generador de URLs (no hace nada en este test)
            var mockGeneradorUrls = new Mock<IGeneradorUrlsParaColeccionesImagenes>();
            mockGeneradorUrls
                .Setup(g => g.EjecutarProcesarUrlsAsync(
                    It.IsAny<List<PerfilEstudianteInformacionDto>>(),
                    It.IsAny<(System.Func<PerfilEstudianteInformacionDto, string>, System.Action<PerfilEstudianteInformacionDto, string>)>()
                ))
                .Returns(Task.CompletedTask);

            // Mock del repositorio de grupos: importante -> indicar que el estudiante pertenece al grupo
            var mockRepositorioGrupos = new Mock<IRepositorioGrupos>();
            mockRepositorioGrupos
                .Setup(r => r.EstudiantePerteneceAlGrupoAsync(10, estudianteLogueadoId))
                .ReturnsAsync(true);

            // (si tienes otros setups de grupo no necesarios, puedes mantenerlos)
            mockRepositorioGrupos.Setup(r => r.GetByIdAsync(10))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Exitoso(
                    new LogicaNegocio.Entidades.Grupo { Id = 10, Nombre = "Grupo Test" }
                ));

            var casoUso = new ObtenerPerfilesDeGrupoSinIncluirUsuarioLogueado(
                mockRepositorioPerfiles.Object,
                mockGeneradorUrls.Object,
                mockRepositorioGrupos.Object
            );

            // Act
            var resultado = await casoUso.EjecutarAsync(10, estudianteLogueadoId);

            // Assert
            Assert.True(resultado.EsExitoso);
            // Solo queda el compañero
            Assert.Single(resultado.Valor);

            var dto = resultado.Valor[0];
            Assert.Equal("Companero", dto.NombreEstudiante);
            Assert.Equal("estu456", dto.EstudianteId);

            // Verificamos que la medalla se mapeó
            Assert.Single(dto.Medallas);
            var dtoMedalla = dto.Medallas[0];
            Assert.Equal(99, dtoMedalla.Id);
            Assert.Equal("Super Medalla", dtoMedalla.Nombre);
            Assert.Equal("http://img/test.png", dtoMedalla.NombreIcono);
            Assert.Equal("Descripción test", dtoMedalla.Descripcion);
            Assert.Equal(10, dtoMedalla.CantidadMedallasBrinda);

            // Verificaciones de llamadas a mocks
            mockRepositorioPerfiles.Verify(r => r.ObtenerPorGrupoIdAsync(10), Times.Once);
            mockGeneradorUrls.Verify(g => g.EjecutarProcesarUrlsAsync(
                It.IsAny<List<PerfilEstudianteInformacionDto>>(),
                It.IsAny<(System.Func<PerfilEstudianteInformacionDto, string>, System.Action<PerfilEstudianteInformacionDto, string>)>()
            ), Times.Once);
        }
    }
}