using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.EquivalenciaDTOs;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaAplicacion.DTOs.TablaEquivalenciaDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.TablaEquivalencia;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsTablaEquivalencia
{
    public class PruebasEditarTablaEquivalencia
    {
        private readonly Mock<IRepositorioTablasEquivalencia> _repoTablaMock;
        private readonly Mock<IRepositorioMedallas> _repoMedallasMock;
        private readonly EditarTablaEquivalencia _casoUso;
        private const string ProfesorId = "prof-123";

        public PruebasEditarTablaEquivalencia()
        {
            _repoTablaMock = new Mock<IRepositorioTablasEquivalencia>();
            _repoMedallasMock = new Mock<IRepositorioMedallas>();
            _casoUso = new EditarTablaEquivalencia(_repoTablaMock.Object, _repoMedallasMock.Object);
        }

        [Fact]
        public async Task IdNoExiste_RetornaNotFound()
        {
            var dto = new TablaEquivalenciaDto { Id = 5 };

            _repoTablaMock
                .Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(Resultado<TablaEquivalencia>.Falla(Error.NotFound));

            var resultado = await _casoUso.EjecutarAsync(dto, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.NotFound");
            _repoMedallasMock.Verify(r => r.FindByIdsAsync(It.IsAny<List<int>>()), Times.Never);
            _repoTablaMock.Verify(r => r.UpdateAsync(It.IsAny<TablaEquivalencia>()), Times.Never);
        }

        [Fact]
        public async Task ProfesorNoEsPropietario_RetornaForbidden()
        {
            var tabla = new TablaEquivalencia("Nombre", "otro-prof");
            tabla.Id = 10;

            var dto = new TablaEquivalenciaDto
            {
                Id = 10,
                Nombre = "NuevoNombre",
                Equivalencias = new List<EquivalenciaDto>()
            };

            _repoTablaMock
                .Setup(r => r.GetByIdAsync(10))
                .ReturnsAsync(Resultado<TablaEquivalencia>.Exitoso(tabla));

            var resultado = await _casoUso.EjecutarAsync(dto, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Forbidden");
            _repoMedallasMock.Verify(r => r.FindByIdsAsync(It.IsAny<List<int>>()), Times.Never);
            _repoTablaMock.Verify(r => r.UpdateAsync(It.IsAny<TablaEquivalencia>()), Times.Never);
        }

        [Fact]
        public async Task MedallasNoExisten_RetornaNotFound()
        {
            var tabla = new TablaEquivalencia("Nombre", ProfesorId) { Id = 20 };

            var dto = new TablaEquivalenciaDto
            {
                Id = 20,
                Nombre = "Nombre",
                Equivalencias = new List<EquivalenciaDto>
                {
                    new EquivalenciaDto
                    {
                        Nota = 1,
                        MedallasNecesarias = new List<MedallaDto>
                        {
                            new MedallaDto { Id = 100 }
                        }
                    }
                }
            };

            _repoTablaMock
                .Setup(r => r.GetByIdAsync(20))
                .ReturnsAsync(Resultado<TablaEquivalencia>.Exitoso(tabla));

            _repoMedallasMock
                .Setup(r => r.FindByIdsAsync(It.Is<List<int>>(ids => ids.SequenceEqual(new[] { 100 }))))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Medalla>>.Falla(new Error("X", "")));

            var resultado = await _casoUso.EjecutarAsync(dto, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.NotFound");
            _repoTablaMock.Verify(r => r.UpdateAsync(It.IsAny<TablaEquivalencia>()), Times.Never);
        }

        [Fact]
        public async Task ValidacionDominioFalla_RetornaValidation()
        {
            // Nombre inválido y sin equivalencias (lista vacía)
            var tabla = new TablaEquivalencia("NombreValido", ProfesorId) { Id = 30 };

            var dto = new TablaEquivalenciaDto
            {
                Id = 30,
                Nombre = "No",  // menos de 3 caracteres
                Equivalencias = new List<EquivalenciaDto>()
            };

            _repoTablaMock
                .Setup(r => r.GetByIdAsync(30))
                .ReturnsAsync(Resultado<TablaEquivalencia>.Exitoso(tabla));

            // **Stubear FindByIdsAsync para lista vacía**
            _repoMedallasMock
                .Setup(r => r.FindByIdsAsync(It.Is<List<int>>(ids => ids.Count == 0)))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Medalla>>.Exitoso(Enumerable.Empty<LogicaNegocio.Entidades.Medalla>()));

            var resultado = await _casoUso.EjecutarAsync(dto, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Validation");
            _repoTablaMock.Verify(r => r.UpdateAsync(It.IsAny<TablaEquivalencia>()), Times.Never);
        }

        [Fact]
        public async Task CaminoFeliz_ActualizaCorrectamente()
        {
            var tabla = new TablaEquivalencia("Antiguo", ProfesorId) { Id = 40 };

            var dto = new TablaEquivalenciaDto
            {
                Id = 40,
                Nombre = "Nuevo",
                Equivalencias = new List<EquivalenciaDto>
                {
                    new EquivalenciaDto
                    {
                        Nota = 1,
                        MedallasNecesarias = new List<MedallaDto>
                        {
                            new MedallaDto { Id = 1 },
                            new MedallaDto { Id = 2 }
                        }
                    },
                    new EquivalenciaDto
                    {
                        Nota = 2,
                        MedallasNecesarias = new List<MedallaDto>
                        {
                            new MedallaDto { Id = 1 },
                            new MedallaDto { Id = 2 },
                            new MedallaDto { Id = 3 }
                        }
                    }
                }
            };

            var medallas = new List<LogicaNegocio.Entidades.Medalla>
            {
                new LogicaNegocio.Entidades.Medalla { Id = 1 },
                new LogicaNegocio.Entidades.Medalla { Id = 2 },
                new LogicaNegocio.Entidades.Medalla { Id = 3 }
            };

            _repoTablaMock
                .Setup(r => r.GetByIdAsync(40))
                .ReturnsAsync(Resultado<TablaEquivalencia>.Exitoso(tabla));

            _repoMedallasMock
                .Setup(r => r.FindByIdsAsync(It.Is<List<int>>(ids => ids.OrderBy(i => i).SequenceEqual(new[] { 1, 2, 3 }))))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Medalla>>.Exitoso(medallas));

            TablaEquivalencia capturada = null!;
            _repoTablaMock
                .Setup(r => r.UpdateAsync(It.IsAny<TablaEquivalencia>()))
                .Callback<TablaEquivalencia>(t => capturada = t)
                .ReturnsAsync(Resultado.Exitoso());

            var resultado = await _casoUso.EjecutarAsync(dto, ProfesorId);

            Assert.True(resultado.EsExitoso);
            // Verificamos que el objeto fue actualizado antes de llamar al repo
            Assert.Equal("Nuevo", capturada.Nombre);
            Assert.Collection(
                capturada.Equivalencias.OrderBy(e => e.Nota),
                e1 => Assert.Equal(1, e1.Nota),
                e2 => Assert.Equal(2, e2.Nota)
            );
            _repoTablaMock.Verify(r => r.UpdateAsync(capturada), Times.Once);
        }
    }
}
