using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Estudiantes;
using Entidad = LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.Estudiantes
{
    public class PruebasObtenerGruposDeEstudiante
    {
        private readonly Mock<IRepositorioGrupos> _repoGruposMock;

        public PruebasObtenerGruposDeEstudiante()
        {
            _repoGruposMock = new Mock<IRepositorioGrupos>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task EjecutarAsync_IdInvalido_RetornaFallo(string idEstudiante)
        {
            // Arrange
            var casoUso = new ObtenerGruposDeEstudiante(_repoGruposMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(idEstudiante);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e =>
                e.Codigo == "Estudiante.IdInvalido" &&
                e.Mensaje.Contains("no puede ser nulo o vacío", StringComparison.OrdinalIgnoreCase));
            // No se debe invocar al repositorio cuando el id es inválido
            _repoGruposMock.Verify(r => r.ObtenerGruposPorEstudianteId(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_SinGrupos_RetornaListaVacia()
        {
            // Arrange
            string idEstudiante = "est123";
            _repoGruposMock
                .Setup(r => r.ObtenerGruposPorEstudianteId(idEstudiante))
                .ReturnsAsync(new List<Entidad.Grupo>()); // ningún grupo

            var casoUso = new ObtenerGruposDeEstudiante(_repoGruposMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(idEstudiante);

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Empty(resultado.Valor);
            _repoGruposMock.Verify(r => r.ObtenerGruposPorEstudianteId(idEstudiante), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_GruposEncontrados_RetornaDtosMapeados()
        {
            // Arrange
            string idEstudiante = "est456";
            var gruposEntidad = new List<Entidad.Grupo>
    {
        new Entidad.Grupo
        {
            Id = 1,
            Nombre = "Grupo A",
            ProfesorId = "prof1",
            Institucion = "Inst1",
            Materia = "Mat1",
            Alumnos = new List<Entidad.PerfilEstudiante>() // ← aquí
        },
        new Entidad.Grupo
        {
            Id = 2,
            Nombre = "Grupo B",
            ProfesorId = "prof2",
            Institucion = "Inst2",
            Materia = "Mat2",
            Alumnos = new List<Entidad.PerfilEstudiante>() // ← y aquí
        }
    };

            _repoGruposMock
                .Setup(r => r.ObtenerGruposPorEstudianteId(idEstudiante))
                .ReturnsAsync(gruposEntidad);

            var casoUso = new ObtenerGruposDeEstudiante(_repoGruposMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(idEstudiante);

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Equal(2, resultado.Valor.Count);

            // Verificar mapeo de propiedades al DTO
            var dto1 = resultado.Valor[0];
            Assert.Equal(1, dto1.Id);
            Assert.Equal("Grupo A", dto1.Nombre);
            Assert.Equal("prof1", dto1.ProfesorId);
            Assert.Equal("Inst1", dto1.Institucion);
            Assert.Equal("Mat1", dto1.Materia);

            var dto2 = resultado.Valor[1];
            Assert.Equal(2, dto2.Id);
            Assert.Equal("Grupo B", dto2.Nombre);
            Assert.Equal("prof2", dto2.ProfesorId);
            Assert.Equal("Inst2", dto2.Institucion);
            Assert.Equal("Mat2", dto2.Materia);

            _repoGruposMock.Verify(r => r.ObtenerGruposPorEstudianteId(idEstudiante), Times.Once);
        }
    }
}
