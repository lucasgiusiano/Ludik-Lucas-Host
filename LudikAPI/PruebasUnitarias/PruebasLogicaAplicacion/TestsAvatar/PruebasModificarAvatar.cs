using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.AvatarDTOs;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using LogicaAplicacion.ImplementacionCasosUsos.Avatar;
using Moq;
using Xunit;
using LogicaAplicacion.DTOs.ImagenDto;
using LogicaAplicacion.InterfacesCasosUsos.Imagenes;
using LogicaNegocio.InterfacesRepositorios;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsAvatar
{
    public class PruebasModificarAvatar
    {
        private readonly Mock<IRepositorioPerfilEstudianteGrupo> _mockPerfilRepo;
        private readonly Mock<IRepositorioAtributosAvatar> _mockAtribRepo;
        private readonly Mock<IRepositorioAvatares> _mockAvatarRepo;
        private readonly Mock<IServicioGestionImagen> _mockImgService;
        private readonly ModificarAvatar _casoUso;

        private const int PerfilId = 10;
        private const string UserId = "user-1";
        private readonly ActualizarAvatarDto _dto;
        private readonly MemoryStream _streamImagen;

        public PruebasModificarAvatar()
        {
            _mockPerfilRepo = new Mock<IRepositorioPerfilEstudianteGrupo>();
            _mockAtribRepo = new Mock<IRepositorioAtributosAvatar>();
            _mockAvatarRepo = new Mock<IRepositorioAvatares>();
            _mockImgService = new Mock<IServicioGestionImagen>();

            _casoUso = new ModificarAvatar(
                _mockPerfilRepo.Object,
                _mockAvatarRepo.Object,
                _mockAtribRepo.Object,
                _mockImgService.Object
            );

            _dto = new ActualizarAvatarDto
            {
                ColorFondo = "#fff",
                Rotacion = 45,
                Voltear = false,
                Zoom = 1,
                AtributosIds = new List<int> { 100, 200 }
            };
            _streamImagen = new MemoryStream(new byte[] { 1, 2, 3 });
        }

        [Fact]
        public async Task PerfilNoExiste_RetornaNotFound()
        {
            _mockPerfilRepo
                .Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Falla(Error.NotFound));

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId, _dto, _streamImagen);

            Assert.True(res.EsFallo);
            Assert.Equal("Error.NotFound", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task UsuarioNoPertenece_RetornaForbidden()
        {
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante { Id = PerfilId, EstudianteId = "otro-user" };
            _mockPerfilRepo.Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId, _dto, _streamImagen);

            Assert.True(res.EsFallo);
            Assert.Equal("Error.Forbidden", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task ItemNoDesbloqueado_RetornaForbidden()
        {
            var pa = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 100 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = PerfilId,
                EstudianteId = UserId,
                InventarioRecompensas = new List<PerfilEstudianteRecompensa>
                {
                    new PerfilEstudianteRecompensa { Recompensa = pa }
                }
            };
            _mockPerfilRepo.Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId, _dto, _streamImagen);

            Assert.True(res.EsFallo);
            Assert.Equal("Error.Forbidden", res.Errores.First().Codigo);
            Assert.Contains("Atributo ID: 200", res.Errores.First().Mensaje);
        }

        [Fact]
        public async Task AtributosRepoFalla_PropagaError()
        {
            var pa1 = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 100 };
            var pa2 = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 200 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = PerfilId,
                EstudianteId = UserId,
                InventarioRecompensas = new List<PerfilEstudianteRecompensa>
                {
                    new PerfilEstudianteRecompensa { Recompensa = pa1 },
                    new PerfilEstudianteRecompensa { Recompensa = pa2 }
                }
            };
            _mockPerfilRepo.Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));
            _mockAtribRepo.Setup(r => r.GetByIdsAsync(_dto.AtributosIds))
                .ReturnsAsync(Resultado<IEnumerable<AtributoAvatar>>.Falla(new Error("Error.Unexpected", "DB failure")));

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId, _dto, _streamImagen);
            Assert.True(res.EsFallo);
            Assert.Equal("Error.Unexpected", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task AtributosCountMismatch_RetornaNotFound()
        {
            var pa1 = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 100 };
            var pa2 = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 200 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = PerfilId,
                EstudianteId = UserId,
                InventarioRecompensas = new List<PerfilEstudianteRecompensa>
                {
                    new PerfilEstudianteRecompensa { Recompensa = pa1 },
                    new PerfilEstudianteRecompensa { Recompensa = pa2 }
                }
            };
            _mockPerfilRepo.Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));
            _mockAtribRepo.Setup(r => r.GetByIdsAsync(_dto.AtributosIds))
                .ReturnsAsync(Resultado<IEnumerable<AtributoAvatar>>.Exitoso(new List<AtributoAvatar> { new AtributoAvatar { Id = 100 } }));

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId, _dto, _streamImagen);
            Assert.True(res.EsFallo);
            Assert.Equal("Error.NotFound", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task AvatarRepoFalla_RetornaNotFound()
        {
            var pa1 = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 100 };
            var pa2 = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 200 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = PerfilId,
                EstudianteId = UserId,
                InventarioRecompensas = new List<PerfilEstudianteRecompensa>
                {
                    new PerfilEstudianteRecompensa { Recompensa = pa1 },
                    new PerfilEstudianteRecompensa { Recompensa = pa2 }
                }
            };
            var atributos = _dto.AtributosIds.Select(id => new AtributoAvatar { Id = id });
            _mockPerfilRepo.Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));
            _mockAtribRepo.Setup(r => r.GetByIdsAsync(_dto.AtributosIds))
                .ReturnsAsync(Resultado<IEnumerable<AtributoAvatar>>.Exitoso(atributos));
            _mockAvatarRepo.Setup(r => r.GetByPerfilIdAsync(PerfilId))
                .ReturnsAsync(Resultado<Avatar>.Falla(Error.NotFound));

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId, _dto, _streamImagen);
            Assert.True(res.EsFallo);
            Assert.Equal("Error.NotFound", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task FallaAlUpdateAvatar_PropagaError()
        {
            var pa1 = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 100 };
            var pa2 = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 200 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = PerfilId,
                EstudianteId = UserId,
                InventarioRecompensas = new List<PerfilEstudianteRecompensa>
                {
                    new PerfilEstudianteRecompensa { Recompensa = pa1 },
                    new PerfilEstudianteRecompensa { Recompensa = pa2 }
                }
            };
            var avatar = new Avatar();
            var atributos = _dto.AtributosIds.Select(id => new AtributoAvatar { Id = id });
            _mockPerfilRepo.Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));
            _mockAtribRepo.Setup(r => r.GetByIdsAsync(_dto.AtributosIds))
                .ReturnsAsync(Resultado<IEnumerable<AtributoAvatar>>.Exitoso(atributos));
            _mockAvatarRepo.Setup(r => r.GetByPerfilIdAsync(PerfilId))
                .ReturnsAsync(Resultado<Avatar>.Exitoso(avatar));
            _mockAvatarRepo.Setup(r => r.UpdateAsync(avatar))
                .ReturnsAsync(Resultado.Falla(new Error("Error.Unexpected", "Update failed")));

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId, _dto, _streamImagen);
            Assert.True(res.EsFallo);
            Assert.Equal("Error.Unexpected", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task FallaAlSubirImagen_PropagaError()
        {
            var pa1 = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 100 };
            var pa2 = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 200 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = PerfilId,
                EstudianteId = UserId,
                InventarioRecompensas = new List<PerfilEstudianteRecompensa>
                {
                    new PerfilEstudianteRecompensa { Recompensa = pa1 },
                    new PerfilEstudianteRecompensa { Recompensa = pa2 }
                }
            };
            var avatar = new Avatar();
            var atributos = _dto.AtributosIds.Select(id => new AtributoAvatar { Id = id });
            _mockPerfilRepo.Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));
            _mockAtribRepo.Setup(r => r.GetByIdsAsync(_dto.AtributosIds))
                .ReturnsAsync(Resultado<IEnumerable<AtributoAvatar>>.Exitoso(atributos));
            _mockAvatarRepo.Setup(r => r.GetByPerfilIdAsync(PerfilId))
                .ReturnsAsync(Resultado<Avatar>.Exitoso(avatar));
            _mockAvatarRepo.Setup(r => r.UpdateAsync(avatar))
                .ReturnsAsync(Resultado.Exitoso());
            _mockImgService.Setup(r => r.SubirImagenAsync(It.IsAny<SubirImagenDto>()))
                .ReturnsAsync(Resultado.Falla(new Error("Error.Unexpected", "Image upload failed")));

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId, _dto, _streamImagen);
            Assert.True(res.EsFallo);
            Assert.Equal("Error.Unexpected", res.Errores.First().Codigo);
        }

        [Fact]
        public async Task CaminoFeliz_ModificaYSubeImagenCorrectamente()
        {
            var pa1 = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 100 };
            var pa2 = new RecompensaPersonalizacionAvatar { AtributoAvatarId = 200 };
            var perfil = new LogicaNegocio.Entidades.PerfilEstudiante
            {
                Id = PerfilId,
                EstudianteId = UserId,
                InventarioRecompensas = new List<PerfilEstudianteRecompensa>
                {
                    new PerfilEstudianteRecompensa { Recompensa = pa1 },
                    new PerfilEstudianteRecompensa { Recompensa = pa2 }
                }
            };
            var avatar = new Avatar();
            var atributos = _dto.AtributosIds.Select(id => new AtributoAvatar { Id = id });
            _mockPerfilRepo.Setup(r => r.GetByIdAsync(PerfilId))
                .ReturnsAsync(Resultado<LogicaNegocio.Entidades.PerfilEstudiante>.Exitoso(perfil));
            _mockAtribRepo.Setup(r => r.GetByIdsAsync(_dto.AtributosIds))
                .ReturnsAsync(Resultado<IEnumerable<AtributoAvatar>>.Exitoso(atributos));
            _mockAvatarRepo.Setup(r => r.GetByPerfilIdAsync(PerfilId))
                .ReturnsAsync(Resultado<Avatar>.Exitoso(avatar));
            _mockAvatarRepo.Setup(r => r.UpdateAsync(avatar))
                .ReturnsAsync(Resultado.Exitoso());
            _mockImgService.Setup(r => r.SubirImagenAsync(It.IsAny<SubirImagenDto>()))
                .ReturnsAsync(Resultado.Exitoso());

            var res = await _casoUso.EjecutarAsync(PerfilId, UserId, _dto, _streamImagen);
            Assert.True(res.EsExitoso);
            _mockAvatarRepo.Verify(r => r.UpdateAsync(avatar), Times.Once);
            _mockImgService.Verify(r => r.SubirImagenAsync(It.Is<SubirImagenDto>(d => d.EntidadAsociadaId == PerfilId)), Times.Once);
        }
    }
}

