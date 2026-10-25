using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.PerfilEstudianteDTO;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObjects;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsPerfilEstudiante
{
    public class PruebasObtenerPerfilConMedallas
    {
        private readonly Mock<IRepositorioPerfilEstudianteGrupo> _mockRepoPerfil;
        private readonly Mock<IGeneradorUrlImagen> _mockGeneradorUrlImagen;
        private readonly ObtenerPerfilConMedallas _casoUso;

        private const string EstudianteId = "est-1";
        private const int GrupoId = 42;

        public PruebasObtenerPerfilConMedallas()
        {
            _mockRepoPerfil = new Mock<IRepositorioPerfilEstudianteGrupo>();
            _mockGeneradorUrlImagen = new Mock<IGeneradorUrlImagen>();

            _casoUso = new ObtenerPerfilConMedallas(
                _mockRepoPerfil.Object,
                _mockGeneradorUrlImagen.Object
            );
        }

        [Fact]
        public async Task RepoFailure_ReturnsFailure()
        {
            // Arrange: el repositorio devuelve fallo
            var error = new Error("Error.NotFound", "Perfil no existe");
            _mockRepoPerfil
                .Setup(r => r.GetPerfilEstudianteAsync(EstudianteId, GrupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Falla(error));

            // Act
            var resultado = await _casoUso.EjecutarAsync(EstudianteId, GrupoId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Collection(resultado.Errores,
                e => Assert.Equal("Error.NotFound", e.Codigo)
            );
        }

        [Fact]
        public async Task Success_ReturnsPerfilDtoWithMedallasAndUrl()
        {
            // Arrange: creamos el VO NombreCompleto a través de su fábrica
            var nombreResult = NombreCompleto.Crear("Juan", "Pérez");
            Assert.True(nombreResult.EsExitoso, "El VO NombreCompleto debería crearse sin errores");
            var nombreVO = nombreResult.Valor;

            // Creamos un PerfilEstudiante con datos y medallas
            var estudiante = new LogicaNegocio.Entidades.Estudiante
            {
                Id = EstudianteId,
                NombreCompleto = nombreVO
            };
            var grupo = new LogicaNegocio.Entidades.Grupo
            {
                Id = GrupoId,
                Nombre = "Grupo A"
            };

            var medalla1 = new LogicaNegocio.Entidades.Medalla
            {
                Id = 1,
                Nombre = "Excelencia",
                Descripcion = "Excelente rendimiento",
                MonedasOtorgadas = 10,
                NombreIcono = "excel.png"
            };
            var medalla2 = new LogicaNegocio.Entidades.Medalla
            {
                Id = 2,
                Nombre = "Participación",
                Descripcion = "Participa activamente",
                MonedasOtorgadas = 5,
                NombreIcono = "part.png"
            };

            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = 100,
                EstudianteId = EstudianteId,
                Estudiante = estudiante,
                GrupoId = GrupoId,
                Grupo = grupo,
                NombreImagenMiniatura = "avatar.png",
                MetaCalificacion = 80,
                Monedas = 123,
                MedallasObtenidas = new List<PerfilEstudianteMedalla>
                {
                    new PerfilEstudianteMedalla { Medalla = medalla1 },
                    new PerfilEstudianteMedalla { Medalla = medalla1 },
                    new PerfilEstudianteMedalla { Medalla = medalla2 }
                }
            };

            _mockRepoPerfil
                .Setup(r => r.GetPerfilEstudianteAsync(EstudianteId, GrupoId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));

            _mockGeneradorUrlImagen
                .Setup(g => g.GenerarUrlLecturaAsync("avatar.png"))
                .ReturnsAsync("http://cdn.example.com/avatar_read.png");

            // Act
            var resultado = await _casoUso.EjecutarAsync(EstudianteId, GrupoId);

            // Assert
            Assert.True(resultado.EsExitoso);
            var dto = resultado.Valor;

            Assert.Equal(100, dto.Id);
            Assert.Equal("http://cdn.example.com/avatar_read.png", dto.EnlaceAvatar);
            Assert.Equal(80, dto.MetaCalificacion);
            Assert.Equal(EstudianteId, dto.EstudianteId);
            Assert.Equal("Juan", dto.NombreEstudiante);
            Assert.Equal(123, dto.Monedas);
            Assert.Equal(GrupoId, dto.GrupoId);
            Assert.Equal("Grupo A", dto.NombreGrupo);

            // Verificamos el agrupamiento de medallas
            Assert.Equal(2, dto.Medallas.Count);

            var primera = dto.Medallas.Single(m => m.MedallaId == 1);
            Assert.Equal("Excelencia", primera.Nombre);
            Assert.Equal("excel.png", primera.Icono);
            Assert.Equal("Excelente rendimiento", primera.Descripcion);
            Assert.Equal(10, primera.MonedasOtorgadas);
            Assert.Equal(2, primera.Cantidad);

            var segunda = dto.Medallas.Single(m => m.MedallaId == 2);
            Assert.Equal("Participación", segunda.Nombre);
            Assert.Equal("part.png", segunda.Icono);
            Assert.Equal("Participa activamente", segunda.Descripcion);
            Assert.Equal(5, segunda.MonedasOtorgadas);
            Assert.Equal(1, segunda.Cantidad);

            _mockGeneradorUrlImagen.Verify(g => g.GenerarUrlLecturaAsync("avatar.png"), Times.Once);
        }
    }
}