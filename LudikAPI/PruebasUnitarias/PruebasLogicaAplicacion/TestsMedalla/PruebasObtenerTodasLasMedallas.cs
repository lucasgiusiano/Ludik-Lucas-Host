using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Medallas;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Moq;
using Xunit;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsMedalla
{
    public class PruebasObtenerTodasLasMedallas
    {
        private readonly Mock<IRepositorioMedallas> _repoMock;
        private readonly Mock<IGeneradorUrlImagen> _generadorUrlImagenMock;
        private readonly ObtenerTodasLasMedallas _casoUso;

        private const string ProfesorId = "profesor123";

        public PruebasObtenerTodasLasMedallas()
        {
            _repoMock = new Mock<IRepositorioMedallas>();
            _generadorUrlImagenMock = new Mock<IGeneradorUrlImagen>();

            // Mock para que GenerarUrlLecturaAsync devuelva la misma url que recibe
            _generadorUrlImagenMock
                .Setup(g => g.GenerarUrlLecturaAsync(It.IsAny<string>()))
                .ReturnsAsync((string url) => url);

            _casoUso = new ObtenerTodasLasMedallas(_repoMock.Object);
        }

        [Fact]
        public async Task RepositorioFalla_RetornaFallo()
        {
            var errores = new List<Error> { new Error("Error.DB", "Fallo en base de datos") };
            _repoMock
                .Setup(r => r.GetByProfesorAsync(ProfesorId))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Medalla>>.Falla(errores));

            var resultado = await _casoUso.EjecutarAsync(ProfesorId);

            Assert.True(resultado.EsFallo);
            Assert.Equal(errores, resultado.Errores);
        }

        [Fact]
        public async Task RepositorioExitoso_RetornaListaDto()
        {
            var medallas = new List<LogicaNegocio.Entidades.Medalla>
            {
                new LogicaNegocio.Entidades.Medalla
                {
                    Id = 1,
                    Nombre = "Responsable",
                    Descripcion = "Cumple siempre con sus tareas",
                    NombreIcono = "img/responsable.png",
                    MonedasOtorgadas = 20,
                },
                new LogicaNegocio.Entidades.Medalla
                {
                    Id = 2,
                    Nombre = "Colaborador",
                    Descripcion = "Ayuda a otros estudiantes",
                    NombreIcono = "img/colaborador.png",
                    MonedasOtorgadas = 30,
                }
            };

            _repoMock
                .Setup(r => r.GetByProfesorAsync(ProfesorId))
                .ReturnsAsync(Resultado<IEnumerable<LogicaNegocio.Entidades.Medalla>>.Exitoso(medallas));

            var resultado = await _casoUso.EjecutarAsync(ProfesorId);

            Assert.True(resultado.EsExitoso);
            var lista = resultado.Valor!.ToList();

            Assert.Equal(2, lista.Count);
            Assert.Contains(lista, m => m.Nombre == "Responsable" && m.CantidadMedallasBrinda == 20);
        }
    }
}
