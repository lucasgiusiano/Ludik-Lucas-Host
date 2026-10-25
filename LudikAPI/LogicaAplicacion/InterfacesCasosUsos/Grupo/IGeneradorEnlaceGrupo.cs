using LogicaNegocio.Resultados;

namespace LogicaAplicacion.InterfacesCasosUsos.Grupo;

public interface IGeneradorEnlaceGrupo
{
    Resultado<string> GenerarEnlace(string codigo);
}