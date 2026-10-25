using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.AtributoAvatarDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Avatar;
using LogicaAplicacion.ImplementacionServicios;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Entidades;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.PruebasAvatar
{
    public class PruebasObtenerAtributosAvatarDisponiblesParaPerfil
    {
        private readonly Mock<IRepositorioPerfilEstudianteGrupo> _mockPerfilRepo;
        private readonly IGeneradorUrlsParaColeccionesImagenes _generadorUrls;
        private readonly ObtenerAtributosAvatarDisponiblesParaPerfil _casoUso;

        private const int PerfilId = 5;
        private const string UserId = "user-5";

        public PruebasObtenerAtributosAvatarDisponiblesParaPerfil()
        {
            _mockPerfilRepo = new Mock<IRepositorioPerfilEstudianteGrupo>();

            // Setup por defecto para GetByIdAsync
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(
                    new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId, EstudianteId = UserId }
                ));

            // Setup por defecto para ObtenerItemsAvatarAdquiridosAsync
            _mockPerfilRepo
                .Setup(r => r.ObtenerItemsAvatarAdquiridosAsync(It.IsAny<int>()))
                .ReturnsAsync(Resultado<IEnumerable<RecompensaPersonalizacionAvatar>>.Exitoso(
                    new List<RecompensaPersonalizacionAvatar>()
                ));

            // Mock para las URLs SAS
            var mockAlmacen = new Mock<IRepositorioAlmacenamientoArchivos>();
            mockAlmacen
                .Setup(a => a.ObtenerArchivoSasUrlAsync(It.IsAny<string>()))
                .Returns<string>(n =>
                    Task.FromResult(Resultado<string>.Exitoso($"url-fake/{n}"))
                );

            _generadorUrls = new GeneradorUrlsParaColeccionesImagenes(
                new GeneradorUrlImagen(mockAlmacen.Object)
            );

            _casoUso = new ObtenerAtributosAvatarDisponiblesParaPerfil(
                _mockPerfilRepo.Object,
                _generadorUrls
            );
        }

        [Fact]
        public async Task PerfilNoExiste_RetornaNotFound()
        {
            // Sobre escribo solo la llamada a GetByIdAsync
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Falla(
                    new Error("Error.NotFound", "Perfil no encontrado")
                ));

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId);

            Assert.True(res.EsFallo);
            Assert.Equal("Error.NotFound", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task ErrorAlObtenerItems_RetornaEseError()
        {
            // Perfil válido por defecto, sobre escribo ObtenerItemsAvatarAdquiridosAsync
            _mockPerfilRepo
                .Setup(r => r.ObtenerItemsAvatarAdquiridosAsync(PerfilId))
                .ReturnsAsync(Resultado<IEnumerable<RecompensaPersonalizacionAvatar>>.Falla(
                    new Error("Error.DB", "Fallo al cargar items")
                ));

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId);

            Assert.True(res.EsFallo);
            Assert.Equal("Error.DB", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task UsuarioNoPertenece_RetornaForbidden()
        {
            // Perfil con otro usuario
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(
                    new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId, EstudianteId = "otro-user" }
                ));

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId);

            Assert.True(res.EsFallo);
            Assert.Equal("Error.Forbidden", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task CaminoFeliz_RetornaAtributosDtoCorrectosConUrls()
        {
            // Preparo dos atributos
            var attrA = new AtributoAvatar
            {
                Id = 10,
                Nombre = "Pelo",
                Tipo = TipoAtributo.Pelo,
                NombreImagenRecurso = "r1",
                CodigoUnico = "c1"
            };
            var attrB = new AtributoAvatar
            {
                Id = 20,
                Nombre = "Gafas",
                Tipo = TipoAtributo.Gafas,
                NombreImagenRecurso = "r2",
                CodigoUnico = "c2"
            };
            var items = new[]
            {
                new RecompensaPersonalizacionAvatar { AtributoDesbloqueable = attrA },
                new RecompensaPersonalizacionAvatar { AtributoDesbloqueable = attrB }
            };

            // Sobre escribo solo la llamada a ObtenerItemsAvatarAdquiridosAsync
            _mockPerfilRepo
                .Setup(r => r.ObtenerItemsAvatarAdquiridosAsync(PerfilId))
                .ReturnsAsync(Resultado<IEnumerable<RecompensaPersonalizacionAvatar>>.Exitoso(items));

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId);

            Assert.True(res.EsExitoso);
            var lista = res.Valor!.ToList();
            Assert.Equal(2, lista.Count);
            Assert.Contains(lista, d => d.Id == 10 && d.CodigoUnico == "c1" && d.EnlaceImagen == "url-fake/r1");
            Assert.Contains(lista, d => d.Id == 20 && d.CodigoUnico == "c2" && d.EnlaceImagen == "url-fake/r2");
        }
    }
}
