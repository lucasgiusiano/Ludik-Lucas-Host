using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicaNegocio.Entidades;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.GrupoDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Profesores;
using LogicaNegocio.Resultados;
using Moq;
using Entidad = LogicaNegocio.Entidades;

using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.Profesor
{
    public class PruebasObtenerGrupoDeProfesor
    {
        private readonly Mock<IRepositorioGrupos> _repoGruposMock;

        public PruebasObtenerGrupoDeProfesor()
        {
            _repoGruposMock = new Mock<IRepositorioGrupos>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task EjecutarAsync_IdProfesorInvalido_RetornaFallo(string idProfesor)
        {
            // Arrange
            var casoUso = new ObtenerGruposDeProfesor(_repoGruposMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(idProfesor);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e =>
                e.Codigo == "Error.Validation" &&
                e.Mensaje.Contains("ID del profesor no puede ser nulo o vacío", StringComparison.OrdinalIgnoreCase));
            // No debe llamar al repositorio
            _repoGruposMock.Verify(r => r.ObtenerGruposPorProfesorId(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task EjecutarAsync_SinGrupos_RetornaListaVacia()
        {
            // Arrange
            string idProfesor = "prof123";
            _repoGruposMock
                .Setup(r => r.ObtenerGruposPorProfesorId(idProfesor))
                .ReturnsAsync(new List<Entidad.Grupo>()); // ningún grupo

            var casoUso = new ObtenerGruposDeProfesor(_repoGruposMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(idProfesor);

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Empty(resultado.Valor);
            _repoGruposMock.Verify(r => r.ObtenerGruposPorProfesorId(idProfesor), Times.Once);
        }

        [Fact]
        public async Task EjecutarAsync_GruposEncontrados_RetornaDtosMapeados()
        {
            // Arrange
            string idProfesor = "profABC";
            var gruposEntidad = new List<Entidad.Grupo>
    {
        new Entidad.Grupo
        {
            Id = 1,
            Nombre = "Grupo Uno",
            ProfesorId = idProfesor,
            Institucion = "InstA",
            Materia = "MatA",
            Alumnos = new List<Entidad.PerfilEstudiante>() 
        },
        new Entidad.Grupo
        {
            Id = 2,
            Nombre = "Grupo Dos",
            ProfesorId = idProfesor,
            Institucion = "InstB",
            Materia = "MatB",
            Alumnos = new List<Entidad.PerfilEstudiante>() 
        }
    };

            _repoGruposMock
                .Setup(r => r.ObtenerGruposPorProfesorId(idProfesor))
                .ReturnsAsync(gruposEntidad);

            var casoUso = new ObtenerGruposDeProfesor(_repoGruposMock.Object);

            // Act
            var resultado = await casoUso.EjecutarAsync(idProfesor);

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Equal(2, resultado.Valor.Count);

            var dto1 = resultado.Valor[0];
            Assert.Equal(1, dto1.Id);
            Assert.Equal("Grupo Uno", dto1.Nombre);
            Assert.Equal(idProfesor, dto1.ProfesorId);
            Assert.Equal("InstA", dto1.Institucion);
            Assert.Equal("MatA", dto1.Materia);
            Assert.Equal(0, dto1.CantAlumnos); // porque la lista está vacía

            var dto2 = resultado.Valor[1];
            Assert.Equal(2, dto2.Id);
            Assert.Equal("Grupo Dos", dto2.Nombre);
            Assert.Equal(idProfesor, dto2.ProfesorId);
            Assert.Equal("InstB", dto2.Institucion);
            Assert.Equal("MatB", dto2.Materia);
            Assert.Equal(0, dto2.CantAlumnos); // también vacía

            _repoGruposMock.Verify(r => r.ObtenerGruposPorProfesorId(idProfesor), Times.Once);
        }
    }
}
