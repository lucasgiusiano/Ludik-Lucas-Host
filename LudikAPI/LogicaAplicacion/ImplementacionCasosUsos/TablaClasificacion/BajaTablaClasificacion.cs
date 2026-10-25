using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesRepositorio;
using LogicaAplicacion.InterfacesCasosUsos.TablaClasificacion;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;

namespace LogicaAplicacion.ImplementacionCasosUsos.TablaClasificacion
{
    public class BajaTablaClasificacion:IBajaTablaClasificacion
    {
        private readonly IRepositorioTablasClasificacion _repositorioTablasClasificacion;
        private readonly IRepositorioProfesores _repositorioProfesores;


        public BajaTablaClasificacion(IRepositorioTablasClasificacion repositorioTablasClasificacion,IRepositorioProfesores repositorioProfesores)
        {
            _repositorioTablasClasificacion = repositorioTablasClasificacion;
            _repositorioProfesores = repositorioProfesores;
        }

        public async Task<Resultado> EjecutarAsync(int tablaId, string profesorId)
        {
            var resultadoObtenerTabla = await _repositorioTablasClasificacion.GetByIdAsync(tablaId);
            var tabla = resultadoObtenerTabla.Valor;
            var resultadoObtenerProfesor = await _repositorioProfesores.GetByStringIdAsync(profesorId);
            var profesor = resultadoObtenerProfesor.Valor;


            if(tabla == null)
            {
                return Resultado.Falla(new Error(
                    "Error.NotFound",
                    "No se encontró la tabla de clasificación con el ID proporcionado."));
            }

            bool tieneEseGrupo = profesor.Grupos
            .Any(g => g.Id == tabla.GrupoId);

            if (!tieneEseGrupo)
            {
                return Resultado.Falla(new Error(
                    "Error.Forbidden",
                    "No puedes eliminar una tabla de clasificación que le pertenece al grupo de otro profesor."));
            }
            var res = await _repositorioTablasClasificacion.RemoveAsync(tablaId);
            if (res.EsFallo)
                return Resultado.Falla(res.Errores);
            

            return Resultado.Exitoso();
        }
    }
}
