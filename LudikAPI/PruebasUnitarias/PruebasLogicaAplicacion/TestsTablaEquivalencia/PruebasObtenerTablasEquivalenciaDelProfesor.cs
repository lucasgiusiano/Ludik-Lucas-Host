using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.TablaEquivalenciaDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.TablaEquivalencia;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsTablaEquivalencia
{
    public class PruebasObtenerTablasEquivalenciaDelProfesor
    {
        private readonly Mock<IRepositorioTablasEquivalencia> _mockTablaRepo;
        private readonly Mock<IRepositorioProfesores> _mockProfesorRepo;
        private readonly ObtenerTablasEquivalenciaDelProfesor _useCase;
        private const string ProfesorId = "prof-abc";

        public PruebasObtenerTablasEquivalenciaDelProfesor()
        {
            _mockTablaRepo = new Mock<IRepositorioTablasEquivalencia>();
            _mockProfesorRepo = new Mock<IRepositorioProfesores>();
            _useCase = new ObtenerTablasEquivalenciaDelProfesor(
                _mockTablaRepo.Object,
                _mockProfesorRepo.Object
            );
        }

        [Fact]
        public async Task ProfesorNoExiste_RetornaNotFound()
        {
            // Arrange
            _mockProfesorRepo
                .Setup(r => r.GetByStringIdAsync(ProfesorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Falla(Error.NotFound));

            // Act
            var resultado = await _useCase.EjecutarAsync(ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.NotFound");
            _mockTablaRepo.Verify(r => r.GetByProfesorIdAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RepositorioTablasFalla_RetornaFalloConErroresDelRepo()
        {
            // Arrange
            var repoErrors = new List<Error>
            {
                new Error("Error.DB", "Error de base de datos")
            };

            _mockProfesorRepo
                .Setup(r => r.GetByStringIdAsync(ProfesorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(new LogicaNegocio.Entidades.Profesor { Id = ProfesorId }));

            _mockTablaRepo
                .Setup(r => r.GetByProfesorIdAsync(ProfesorId))
                .ReturnsAsync(Resultado<IEnumerable<TablaEquivalencia>>.Falla(repoErrors));

            // Act
            var resultado = await _useCase.EjecutarAsync(ProfesorId);

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Equal(repoErrors, resultado.Errores);
        }

        [Fact]
        public async Task CaminoFeliz_RetornaListaDeDtos()
        {
            // Arrange
            var tablas = new[]
            {
                new TablaEquivalencia("T1", ProfesorId) { Id = 1 },
                new TablaEquivalencia("T2", ProfesorId) { Id = 2 }
            };

            _mockProfesorRepo
                .Setup(r => r.GetByStringIdAsync(ProfesorId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Profesor>.Exitoso(new LogicaNegocio.Entidades.Profesor { Id = ProfesorId }));

            _mockTablaRepo
                .Setup(r => r.GetByProfesorIdAsync(ProfesorId))
                .ReturnsAsync(Resultado<IEnumerable<TablaEquivalencia>>.Exitoso(tablas));

            // Act
            var resultado = await _useCase.EjecutarAsync(ProfesorId);

            // Assert
            Assert.True(resultado.EsExitoso);
            var listaDto = resultado.Valor!.ToList();

            Assert.Equal(2, listaDto.Count);
            Assert.Contains(listaDto, dto => dto.Id == 1 && dto.Nombre == "T1");
            Assert.Contains(listaDto, dto => dto.Id == 2 && dto.Nombre == "T2");
        }
    }
}
