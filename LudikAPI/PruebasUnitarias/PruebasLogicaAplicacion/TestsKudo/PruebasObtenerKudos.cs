using LogicaAplicacion.DTOs.KudoDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Kudo;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsKudo
{
    public class PruebasObtenerKudos
    {
        private readonly Mock<IRepositorioTiposKudo> _mockRepositorioKudos;
        private readonly ObtenerKudos _casoUso;

        public PruebasObtenerKudos()
        {
            _mockRepositorioKudos = new Mock<IRepositorioTiposKudo>();
            _casoUso = new ObtenerKudos(
                _mockRepositorioKudos.Object
            );
        }

        [Fact]
        public async Task EjecutarAsync_RepositorioFalla_DebeRetornarResultadoDeFallo()
        {
            // Arrange
            var errorEsperado = Error.Unexpected;
            _mockRepositorioKudos
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Resultado<IEnumerable<TipoKudo>>.Falla(errorEsperado));

            // Act
            var resultado = await _casoUso.EjecutarAsync();

            // Assert
            Assert.True(resultado.EsFallo);
            Assert.Equal(errorEsperado.Codigo, resultado.Errores.First().Codigo);
        }

        [Fact]
        public async Task EjecutarAsync_NoExistenKudos_DebeRetornarListaVaciaExitosamente()
        {
            // Arrange
            var listaVaciaDeKudos = new List<TipoKudo>();
            _mockRepositorioKudos
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Resultado<IEnumerable<TipoKudo>>.Exitoso(listaVaciaDeKudos));

            // Act
            var resultado = await _casoUso.EjecutarAsync();

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Empty(resultado.Valor);
        }

        [Fact]
        public async Task EjecutarAsync_ExistenKudos_DebeRetornarListaDeDtosConUrlsGeneradas()
        {
            // Arrange
            // devuelvo los NombreIcono ya como URL porque no se modificará el caso de uso
            var kudosDesdeRepo = new List<TipoKudo>
            {
                new TipoKudo { Id = 1, Nombre = "Trabajo en Equipo", NombreIcono = "https://storage.azure.com/equipo.png" },
                new TipoKudo { Id = 2, Nombre = "Gran Ayuda",        NombreIcono = "https://storage.azure.com/ayuda.png" }
            };

            _mockRepositorioKudos
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Resultado<IEnumerable<TipoKudo>>.Exitoso(kudosDesdeRepo));

            // Act
            var resultado = await _casoUso.EjecutarAsync();

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Equal(2, resultado.Valor.Count());

            var primerKudo = resultado.Valor.First();
            Assert.Equal(1, primerKudo.Id);
            Assert.Equal("Trabajo en Equipo", primerKudo.Nombre);
            Assert.Equal("https://storage.azure.com/equipo.png", primerKudo.NombreIcono);
        }
    }
}