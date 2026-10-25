using LogicaAplicacion.Servicios;
using LogicaNegocio.Entidades;
using LogicaNegocio.EntidadesAuxiliares;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionServicios;

public class CrearTiendaPorDefecto: ICrearTiendaPorDefecto
{
    private readonly IRepositorioAtributosAvatar _repositorioAtributosAvatar;
    private const int PRECIO_POR_DEFECTO_AVATAR = 5; // Precio estándar para ítems de avatar

    public CrearTiendaPorDefecto(IRepositorioAtributosAvatar repositorioAtributosAvatar)
    {
        _repositorioAtributosAvatar = repositorioAtributosAvatar;
    }
    public async Task<Resultado<Tienda>> CrearAsync()
    {
        var tienda = new Tienda();
        var resultadoTodosLosAtributos = await _repositorioAtributosAvatar.GetAllAsync();
        if (resultadoTodosLosAtributos.EsFallo)
            return Resultado<Tienda>.Falla(new Error("Error.Unexpected","No se pudieron obtener los atributos de avatar para crear la tienda por defecto."));
        
        IEnumerable<AtributoAvatar> todosLosAtributos = resultadoTodosLosAtributos.Valor;

        foreach (var atributo in todosLosAtributos)
        {
            var recompensa = new RecompensaPersonalizacionAvatar
            {
                Nombre = $"Ítem de Avatar: {atributo.Nombre}",
                Precio = PRECIO_POR_DEFECTO_AVATAR,
                AtributoAvatarId = atributo.Id,
                AtributoDesbloqueable = atributo
            };

            if (recompensa.Representacion is RepresentacionImagen repImagen)
            {
                repImagen.NombreImagenCompleta = atributo.NombreImagenRecurso;
                repImagen.NombreImagenMiniatura = atributo.NombreImagenRecurso;
            }

            tienda.AgregarRecompensaPrecargada(recompensa);
        }

        return Resultado<Tienda>.Exitoso(tienda);
    }
}