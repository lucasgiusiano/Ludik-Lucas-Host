using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.PerfilEstudianteDTO;
using LogicaAplicacion.ImplementacionCasosUsos.PerfilEstudiante;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObjects;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.PerfilEstudiante
{
    public class PruebasObtenerPerfilesPorGrupo
    {
        private readonly Mock<IRepositorioPerfilEstudianteGrupo> _mockRepo;
        private readonly Mock<IGeneradorUrlsParaColeccionesImagenes> _mockUrlGen;
        private readonly Mock<IRepositorioGrupos> _mockRepoGrupos;
        private readonly ObtenerPerfilesPorGrupo _casoUso;

        public PruebasObtenerPerfilesPorGrupo()
        {
            _mockRepo = new Mock<IRepositorioPerfilEstudianteGrupo>();
            _mockUrlGen = new Mock<IGeneradorUrlsParaColeccionesImagenes>();
            _mockRepoGrupos = new Mock<IRepositorioGrupos>();
            _casoUso = new ObtenerPerfilesPorGrupo(_mockRepo.Object, _mockUrlGen.Object, _mockRepoGrupos.Object);
        }

        [Fact]
        public async Task EjecutarAsync_RepositorioFalla_RetornaFalloYSinProcesarUrls()
        {
            var error = new Error("ERR.TEST", "Fallo simulado");
            _mockRepo
                .Setup(r => r.ObtenerPorGrupoIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Resultado<List<LogicaNegocio.Entidades.PerfilEstudiante>>.Falla(error));

            var resultado = await _casoUso.EjecutarAsync(123, "profX");

            Assert.True(resultado.EsFallo);
            Assert.Contains(error, resultado.Errores);
            _mockUrlGen.Verify(
                g => g.EjecutarProcesarUrlsAsync<PerfilEstudianteInformacionDto>(
                    It.IsAny<IEnumerable<PerfilEstudianteInformacionDto>>(),
                    It.IsAny<(System.Func<PerfilEstudianteInformacionDto, string>, System.Action<PerfilEstudianteInformacionDto, string>)[]>()),
                Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_RepositorioRetornaPerfiles_MapeaYProcesaUrls()
        {
            // 1) Construyo una TablaEquivalencia que siempre entregue nota 85:
            var tabla = new TablaEquivalencia
            {
                Equivalencias = new List<Equivalencia>
                {
                    // Una única equivalencia de nota=85 y sin requisitos
                    new Equivalencia(85, new List<LogicaNegocio.Entidades.Medalla>())
                }
            };

            // 2) Creo el Grupo real usando esa tabla
            var grupoReal = new LogicaNegocio.Entidades.Grupo
            {
                Id = 7,
                Nombre = "Grupo Test",
                TablaEquivalencia = tabla,
                ProfesorId = "profX"
            };

            // 3) Creo el VO NombreCompleto
            var nombreRes = NombreCompleto.Crear("Juan", "Pérez");
            Assert.True(nombreRes.EsExitoso, "NombreCompleto inválido en test");
            var nombreVO = nombreRes.Valor;

            // 4) Creo el PerfilEstudiante apuntando a mi grupoReal
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = 42,
                NombreImagenMiniatura = "avatar-mini.png",
                NombreImagenCompleta = "avatar-full.png",
                MetaCalificacion = 100,
                EstudianteId = "est-001",
                Estudiante = new LogicaNegocio.Entidades.Estudiante { NombreCompleto = nombreVO },
                Monedas = 50,
                GrupoId = grupoReal.Id,
                Grupo = grupoReal,
                MedallasObtenidas = new List<PerfilEstudianteMedalla>
                {
                    new PerfilEstudianteMedalla {
                        Medalla = new LogicaNegocio.Entidades.Medalla {
                            Id                    = 10,
                            Nombre                = "Super Medalla",
                            Descripcion           = "Descripción test",
                            NombreIcono = "medalla-mini.png",
                            MonedasOtorgadas      = 5,
                        }
                    }
                }
            };

            _mockRepo
                .Setup(r => r.ObtenerPorGrupoIdAsync(grupoReal.Id))
                .ReturnsAsync(Resultado<List<LogicaNegocio.Entidades.PerfilEstudiante>>.Exitoso(new[] { perfil }.ToList()));

            // 5) Setup del repositorio de grupos para que la validación de profesor pase
            _mockRepoGrupos
                .Setup(r => r.GetByIdAsync(grupoReal.Id))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Grupo>.Exitoso(grupoReal));

            // 6) Capturo los DTOs que se pasen al generador de URLs
            List<PerfilEstudianteInformacionDto> dtosProcesados = null;
            _mockUrlGen
                .Setup(g => g.EjecutarProcesarUrlsAsync(
                    It.IsAny<IEnumerable<PerfilEstudianteInformacionDto>>(),
                    It.IsAny<(System.Func<PerfilEstudianteInformacionDto, string>, System.Action<PerfilEstudianteInformacionDto, string>)[]>()))
                .Callback<IEnumerable<PerfilEstudianteInformacionDto>, (System.Func<PerfilEstudianteInformacionDto, string>, System.Action<PerfilEstudianteInformacionDto, string>)[]>(
                    (dtos, _) => dtosProcesados = dtos.ToList()
                )
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _casoUso.EjecutarAsync(grupoReal.Id, "profX");

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.Single(resultado.Valor);

            var dto = resultado.Valor![0];
            Assert.Equal(42, dto.Id);
            Assert.Equal("est-001", dto.EstudianteId);
            Assert.Equal(50, dto.Monedas);
            Assert.Equal(85, dto.CalificacionActual);                    // ahora sale de tu propia TablaEquivalencia
            Assert.Single(dto.Medallas);
            Assert.Equal("avatar-mini.png", dto.EnlaceAvatarMiniatura);

            // Verifico la llamada al generador de URLs
            _mockUrlGen.Verify(
                g => g.EjecutarProcesarUrlsAsync(
                    It.IsAny<IEnumerable<PerfilEstudianteInformacionDto>>(),
                    It.IsAny<(System.Func<PerfilEstudianteInformacionDto, string>, System.Action<PerfilEstudianteInformacionDto, string>)[]>()),
                Times.Once);

            // Y que el listado procesado coincide con el del resultado
            Assert.Equal(resultado.Valor, dtosProcesados);
        }
    }
}