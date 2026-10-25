using InterfacesRepositorio;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Tienda;
using LogicaAplicacion.ImplementacionServicios;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Entidades;
using LogicaNegocio.EntidadesAuxiliares;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.Tienda
{
    public class PruebasObtenerListadoRecompensa
    {
        private readonly Mock<IRepositorioTiendas> _mockRepoTiendas;
        private readonly Mock<IRecompensaEnricher> _mockRecompensaEnricher;
        private readonly ObtenerListadoRecompensa _useCase;

        public PruebasObtenerListadoRecompensa()
        {
            _mockRepoTiendas = new Mock<IRepositorioTiendas>();
            _mockRecompensaEnricher = new Mock<IRecompensaEnricher>(); // <-- agregado

            var mockRepoArchivos = new Mock<IRepositorioAlmacenamientoArchivos>();
            mockRepoArchivos
                .Setup(a => a.ObtenerArchivoSasUrlAsync(It.IsAny<string>()))
                .Returns<string>(nombre =>
                    Task.FromResult(Resultado<string>.Exitoso($"url-fake/{nombre}")));

            var generadorUrlsParaColecciones = new GeneradorUrlsParaColeccionesImagenes(
                new GeneradorUrlImagen(mockRepoArchivos.Object));

            _useCase = new ObtenerListadoRecompensa(
                _mockRepoTiendas.Object,
                _mockRecompensaEnricher.Object
            );
        }

        [Fact]
        public async Task TiendaNoExiste_RetornaNotFound()
        {
            _mockRepoTiendas
                .Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Tienda>.Falla(new Error("Error.NotFound", "No se encontró la tienda")));

            var resultado = await _useCase.EjecutarAsync(5);

            Assert.True(resultado.EsFallo);
            Assert.Contains(resultado.Errores, e =>
                e.Codigo == "Error.NotFound" &&
                e.Mensaje.Contains("No se encontró la tienda"));
        }

        [Fact]
        public async Task CaminoFeliz_RetornaDtosConUrls()
        {
            // --- ARRANGE ---

            // 1. Preparamos los datos del dominio que devolverá el repositorio.
            //    Usamos una mezcla de tipos para una prueba más robusta.
            var recompensaIcono = new RecompensaSimple { Id = 1, Nombre = "Icono Genial", Precio = 5 };
            ((RepresentacionIcono)recompensaIcono.Representacion).NombreIcono = "fa-icon";

            var recompensaImagen = new RecompensaPersonalizacionAvatar { Id = 2, Nombre = "Avatar Increíble", Precio = 10 };
            ((RepresentacionImagen)recompensaImagen.Representacion).NombreImagenCompleta = "completa.png";
            ((RepresentacionImagen)recompensaImagen.Representacion).NombreImagenMiniatura = "mini.png";

            var recompensasDeDominio = new List<LogicaNegocio.Entidades.Recompensa> { recompensaIcono, recompensaImagen };
            var tienda = new LogicaNegocio.Entidades.Tienda { Id = 20, Recompesas = recompensasDeDominio };

            _mockRepoTiendas
                .Setup(r => r.GetByIdAsync(20))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.Tienda>.Exitoso(tienda));

            // 2. Preparamos la respuesta que ESPERAMOS que el enricher nos devuelva.
            //    Esta es la "verdad" contra la que compararemos el resultado final.
            var dtosEsperados = new List<RecompensaClienteDto>
            {
                new RecompensaClienteDto
                {
                    Id = 1, Nombre = "Icono Genial", Precio = 5,
                    Datos = new RespuestaIconoDto("fa-icon")
                },
                new RecompensaClienteDto
                {
                    Id = 2, Nombre = "Avatar Increíble", Precio = 10,
                    Datos = new RespuestaImagenDto("url-fake/mini.png", "url-fake/completa.png")
                }
            };

            // 3. Configuramos el mock del enricher para que devuelva nuestra lista esperada.
            _mockRecompensaEnricher
                .Setup(e => e.EnrichAsync(recompensasDeDominio))
                .ReturnsAsync(dtosEsperados);

            // --- ACT ---
            var resultado = await _useCase.EjecutarAsync(20);

            // --- ASSERT ---
            Assert.True(resultado.EsExitoso);
            var listaResultado = resultado.Valor!.ToList();

            // 4. Verificamos que el resultado del caso de uso es exactamente lo que devolvió el mock.
            Assert.Equal(2, listaResultado.Count);
            Assert.Equal(dtosEsperados, listaResultado);

            // 5. (Opcional pero recomendado) Verificamos que los mocks fueron llamados como se esperaba.
            _mockRepoTiendas.Verify(r => r.GetByIdAsync(20), Times.Once);
            _mockRecompensaEnricher.Verify(e => e.EnrichAsync(recompensasDeDominio), Times.Once);

        }


    }
}