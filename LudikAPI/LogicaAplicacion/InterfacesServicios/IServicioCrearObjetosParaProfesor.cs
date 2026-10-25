using LogicaNegocio.Entidades;

namespace LogicaAplicacion.Servicios;

public interface IServicioCrearObjetosParaProfesor
{
    List<Medalla> CrearMedallasPredeterminadas(Profesor nuevoProfesor);
    List<TablaEquivalencia> CrearTablasEquivalenciaPredeterminadas(Profesor nuevoProfesor, List<Medalla> medallas);
}