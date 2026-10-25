using InterfacesRepositorio;
using LogicaAplicacion.DTOs.MedallaDTOs;
using LogicaAplicacion.ImplementacionCasosUsos.Medallas;
using LogicaNegocio.Resultados;
using Moq;
using Entidades = LogicaNegocio.Entidades;

namespace PruebasUnitarias.PruebasLogicaAplicacion.TestsMedalla;

public class PruebasObtenerMedallasDeProfesorPorGrupoId
{
    private readonly Mock<IRepositorioGrupos> _mockRepositorioGrupos;
    private readonly ObtenerMedallasProfesorPorGrupo _casoDeUso;

    public PruebasObtenerMedallasDeProfesorPorGrupoId()
    {
        _mockRepositorioGrupos = new Mock<IRepositorioGrupos>();
        _casoDeUso = new ObtenerMedallasProfesorPorGrupo(_mockRepositorioGrupos.Object);
    }

    [Fact]
    public async Task EjecutarAsync_ConGrupoExistenteYMedallasAsociadas_DebeRetornarResultadoExitosoConMedallasDto()
    {
        // Arrange
        int grupoIdValido = 1;
        var listaMedallasEntidad = new List<Entidades.Medalla>
        {
            new Entidades.Medalla { Id = 1, Nombre = "Medalla de Oro", Descripcion = "Por rendimiento excepcional", NombreIcono = "oro.png", MonedasOtorgadas = 100 },
            new Entidades.Medalla { Id = 2, Nombre = "Medalla de Plata", Descripcion = "Por gran esfuerzo", NombreIcono = "plata.png", MonedasOtorgadas = 50 }
        };
        var resultadoRepoExitoso = Resultado<IEnumerable<Entidades.Medalla>>.Exitoso(listaMedallasEntidad);

        _mockRepositorioGrupos
            .Setup(repo => repo.GetMedallasDelProfesorPorGrupoAsync(grupoIdValido))
            .ReturnsAsync(resultadoRepoExitoso);

        // Act
        var resultado = await _casoDeUso.EjecutarAsync(grupoIdValido);

        // Assert
        Assert.True(resultado.EsExitoso);
        Assert.NotNull(resultado.Valor);
        Assert.Equal(2, resultado.Valor.Count());
        Assert.IsAssignableFrom<IEnumerable<MedallaDto>>(resultado.Valor);
        Assert.Equal("Medalla de Oro", resultado.Valor.First().Nombre);
    }

    [Fact]
    public async Task EjecutarAsync_CuandoRepositorioRetornaFallo_DebeRetornarResultadoFallidoConMismosErrores()
    {
        // Arrange
        int grupoIdInvalido = 99;
        var errorEsperado = Error.NotFound;
        var resultadoRepoFallido = Resultado<IEnumerable<Entidades.Medalla>>.Falla(errorEsperado);

        _mockRepositorioGrupos
            .Setup(repo => repo.GetMedallasDelProfesorPorGrupoAsync(grupoIdInvalido))
            .ReturnsAsync(resultadoRepoFallido);

        // Act
        var resultado = await _casoDeUso.EjecutarAsync(grupoIdInvalido);

        // Assert
        Assert.True(resultado.EsFallo);
        Assert.Null(resultado.Valor);
        Assert.Single(resultado.Errores);
        Assert.Equal(errorEsperado.Codigo, resultado.Errores.First().Codigo);
    }

    [Fact]
    public async Task EjecutarAsync_CuandoRepositorioRetornaExitoPeroListaVacia_DebeRetornarResultadoExitosoConListaVacia()
    {
        // Arrange
        int grupoIdValidoSinMedallas = 2;
        var listaVaciaMedallas = Enumerable.Empty<Entidades.Medalla>();
        var resultadoRepoExitosoVacio = Resultado<IEnumerable<Entidades.Medalla>>.Exitoso(listaVaciaMedallas);

        _mockRepositorioGrupos
            .Setup(repo => repo.GetMedallasDelProfesorPorGrupoAsync(grupoIdValidoSinMedallas))
            .ReturnsAsync(resultadoRepoExitosoVacio);

        // Act
        var resultado = await _casoDeUso.EjecutarAsync(grupoIdValidoSinMedallas);

        // Assert
        Assert.True(resultado.EsExitoso);
        Assert.NotNull(resultado.Valor);
        Assert.Empty(resultado.Valor);
    }

    [Fact]
    public async Task EjecutarAsync_CuandoRepositorioRetornaExitoPeroValorNulo_DebeRetornarResultadoExitosoConListaVacia()
    {
        // Arrange
        int grupoId = 3;
        // Simulamos el caso borde donde el repositorio devuelve éxito pero el valor es nulo.
        var resultadoRepoExitosoConNulo = Resultado<IEnumerable<Entidades.Medalla>>.Exitoso(null);

        _mockRepositorioGrupos
            .Setup(repo => repo.GetMedallasDelProfesorPorGrupoAsync(grupoId))
            .ReturnsAsync(resultadoRepoExitosoConNulo);

        // Act
        var resultado = await _casoDeUso.EjecutarAsync(grupoId);

        // Assert
        Assert.True(resultado.EsExitoso);
        Assert.NotNull(resultado.Valor); // La lógica del caso de uso convierte el nulo en una lista vacía.
        Assert.Empty(resultado.Valor);

    }
}