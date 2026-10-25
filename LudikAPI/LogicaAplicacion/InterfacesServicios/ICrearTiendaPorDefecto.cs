using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.Servicios;

public interface ICrearTiendaPorDefecto
{
    Task<Resultado<Tienda>> CrearAsync();
}