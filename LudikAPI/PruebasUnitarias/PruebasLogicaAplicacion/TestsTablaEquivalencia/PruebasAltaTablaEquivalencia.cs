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
    public class PruebasAltaTablaEquivalencia
    {
        private readonly Mock<IRepositorioTablasEquivalencia> _repoTablaMock;
        private readonly Mock<IRepositorioMedallas> _repoMedallasMock;
        private readonly AltaTablaEquivalencia _casoUso;
        private const string ProfesorId = "prof-123";

        public PruebasAltaTablaEquivalencia()
        {
            _repoTablaMock = new Mock<IRepositorioTablasEquivalencia>();
            _repoMedallasMock = new Mock<IRepositorioMedallas>();
            _casoUso = new AltaTablaEquivalencia(_repoTablaMock.Object, _repoMedallasMock.Object);
        }

        [Fact]
        public async Task MedallasNoExisten_RetornaFallo()
        {
            var dto = new TablaEquivalenciaAltaDto
            {
                Nombre = "Tabla X",
                Equivalencias = new List<EquivalenciaAltaDto>
                {
                    new EquivalenciaAltaDto
                    {
                        Nota = 1,
                        MedallasNecesarias = new List<MedallaBasicaDto>
                        {
                            new MedallaBasicaDto { Id = 1, Nombre = "M1", NombreIcono = "url" }
                        }
                    }
                }
            };

            _repoMedallasMock
                .Setup(r => r.FindByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Medalla>>.Falla(new Error("X", "Fallo")));

            var resultado = await _casoUso.EjecutarAsync(dto, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Validation.NotFound");
            _repoTablaMock.Verify(r => r.AddAsync(It.IsAny<TablaEquivalencia>()), Times.Never);
        }

        [Fact]
        public async Task ValidacionDominioFalla_RetornaFallo()
        {
            var dto = new TablaEquivalenciaAltaDto
            {
                Nombre = "", // inválido
                Equivalencias = new List<EquivalenciaAltaDto>()
            };

            var resultado = await _casoUso.EjecutarAsync(dto, ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e => e.Codigo == "Error.Validation");
            _repoTablaMock.Verify(r => r.AddAsync(It.IsAny<TablaEquivalencia>()), Times.Never);
        }

        [Fact]
        public async Task TablaValidaSinEquivalencias_SeGuardaCorrectamente()
        {
            var dto = new TablaEquivalenciaAltaDto
            {
                Nombre = "Tabla sin equivalencias",
                Equivalencias = new List<EquivalenciaAltaDto>()
            };

            var resultado = await _casoUso.EjecutarAsync(dto, ProfesorId);

            Assert.True(resultado.EsExitoso);
            _repoTablaMock.Verify(r => r.AddAsync(It.IsAny<TablaEquivalencia>()), Times.Once);
        }

        [Fact]
        public async Task DatosValidos_SeAgregaCorrectamente()
        {
            // Ahora usamos Nota = 1 para pasar la validación secuencial
            var dto = new TablaEquivalenciaAltaDto
            {
                Nombre = "Tabla con equivalencias",
                Equivalencias = new List<EquivalenciaAltaDto>
                {
                    new EquivalenciaAltaDto
                    {
                        Nota = 1,
                        MedallasNecesarias = new List<MedallaBasicaDto>
                        {
                            new MedallaBasicaDto { Id = 1, Nombre = "M1", NombreIcono = "url" },
                            new MedallaBasicaDto { Id = 2, Nombre = "M2", NombreIcono = "url2" }
                        }
                    }
                }
            };

            var medallas = new List<LogicaNegocio.Entidades.Medalla>
            {
                new LogicaNegocio.Entidades.Medalla { Id = 1, Nombre = "M1", NombreIcono = "url" },
                new LogicaNegocio.Entidades.Medalla { Id = 2, Nombre = "M2", NombreIcono = "url2" }
            };

            _repoMedallasMock
                .Setup(r => r.FindByIdsAsync(It.Is<List<int>>(ids => ids.Contains(1) && ids.Contains(2))))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Medalla>>.Exitoso(medallas));

            var resultado = await _casoUso.EjecutarAsync(dto, ProfesorId);

            // En caso de fallo, imprimimos los errores
            if (resultado.EsFallo)
            {
                var detalles = string.Join(" | ", resultado.Errores.Select(e => $"{e.Codigo}: {e.Mensaje}"));
                Assert.False(true, $"Se esperaba éxito pero hubo errores: {detalles}");
            }

            Assert.True(resultado.EsExitoso);
            _repoTablaMock.Verify(r => r.AddAsync(It.Is<TablaEquivalencia>(t => t.Nombre == dto.Nombre)), Times.Once);
        }
    }
}
