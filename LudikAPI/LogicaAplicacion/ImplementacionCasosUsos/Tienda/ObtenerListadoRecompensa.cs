using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.DTOs.RecompensaDTOs;
using LogicaAplicacion.DTOsMappers.RecompensaMappers;
using LogicaAplicacion.InterfacesCasosUsos.Tienda;
using LogicaAplicacion.Servicios;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.Tienda
{
    public class ObtenerListadoRecompensa:IObtenerListadoRecompensa
    {
        private readonly IRepositorioTiendas _repositorioTiendas;
        private readonly IRecompensaEnricher _recompensaEnricher;

        public ObtenerListadoRecompensa(
            IRepositorioTiendas repositorioTiendas,
            IRecompensaEnricher recompensaEnricher)
        {
            _repositorioTiendas = repositorioTiendas;
            _recompensaEnricher = recompensaEnricher;
        }

        public async Task<Resultado<IEnumerable<RecompensaClienteDto>>> EjecutarAsync(int tiendaId)
        {

            var resultadoTienda = await _repositorioTiendas.GetByIdAsync(tiendaId);
            if (resultadoTienda.EsFallo)
                return Resultado<IEnumerable<RecompensaClienteDto>>.Falla(resultadoTienda.Errores);

            var recompensas = resultadoTienda.Valor.Recompesas;

            var dtos = await _recompensaEnricher.EnrichAsync(recompensas);

            return Resultado<IEnumerable<RecompensaClienteDto>>.Exitoso(dtos);
        }

    }
}
