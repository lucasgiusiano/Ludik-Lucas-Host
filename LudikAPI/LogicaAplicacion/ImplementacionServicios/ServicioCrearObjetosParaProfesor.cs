using LogicaAplicacion.Servicios;
using LogicaNegocio.Entidades;

namespace LogicaAplicacion.ImplementacionServicios;

public class ServicioCrearObjetosParaProfesor: IServicioCrearObjetosParaProfesor
{
    public List<Medalla> CrearMedallasPredeterminadas(Profesor nuevoProfesor)
    {
        return new List<Medalla>
        {
            new Medalla
            {
                Nombre = "Participación Perfecta",
                Descripcion = "Asistencia y participación en todas las clases del mes.",
                NombreIcono = "fire",
                MonedasOtorgadas = 30,
                Creador = nuevoProfesor
            },
            new Medalla
            {
                Nombre = "Maestro de la Colaboración",
                Descripcion = "Ayuda destacada a compañeros en proyectos grupales.",
                NombreIcono = "graduation-cap",
                MonedasOtorgadas = 25,
                Creador = nuevoProfesor
            },
            new Medalla
            {
                Nombre = "Mente Curiosa",
                Descripcion = "Realización de preguntas perspicaces que enriquecen la clase.",
                NombreIcono = "binoculars",
                MonedasOtorgadas = 15,
                Creador = nuevoProfesor
            },

            // --- INICIO DE MEDALLAS ASOCIADAS A KUDOS ---

            // Medalla por Kudo "Gracias por la Ayuda"
            new Medalla
            {
                Nombre = "Compañerismo",
                Descripcion =
                    "Se otorga por ser un pilar de apoyo para tus compañeros. Demuestra que estás siempre dispuesto a ofrecer tu ayuda cuando alguien la necesita.",
                NombreIcono = "hands-clapping",
                MonedasOtorgadas = 20,
                Creador = nuevoProfesor
            },

            // Medalla por Kudo "Esa Pregunta Suma"
            new Medalla
            {
                Nombre = "Curiosidad Insaciable",
                Descripcion =
                    "Premia a las mentes que nunca dejan de preguntar. Se consigue al realizar preguntas que desafían al grupo y enriquecen el aprendizaje de todos.",
                NombreIcono = "compass",
                MonedasOtorgadas = 15,
                Creador = nuevoProfesor
            },

            // Medalla por Kudo "Inspirador"
            new Medalla
            {
                Nombre = "Faro del Grupo",
                Descripcion =
                    "Reconoce a quienes inspiran con su ejemplo. Se obtiene al demostrar una actitud y un esfuerzo que motivan a todo el grupo a superarse.",
                NombreIcono = "face-grin-stars",
                MonedasOtorgadas = 25,
                Creador = nuevoProfesor
            },

            // Medalla por Kudo "Conectando Ideas"
            new Medalla
            {
                Nombre = "Arquitecto de Ideas",
                Descripcion =
                    "Para aquellos que no solo tienen buenas ideas, sino que construyen sobre las de los demás para crear algo aún mejor.",
                NombreIcono = "lightbulb",
                MonedasOtorgadas = 20,
                Creador = nuevoProfesor
            },

            // Medalla por Kudo "Líder de Equipo"
            new Medalla
            {
                Nombre = "Capitán de Equipo",
                Descripcion =
                    "Se otorga por demostrar liderazgo natural, guiando y organizando al equipo para alcanzar metas comunes de forma efectiva.",
                NombreIcono = "crown",
                MonedasOtorgadas = 25,
                Creador = nuevoProfesor
            },

            // Medalla por Kudo "Recurso Valioso"
            new Medalla
            {
                Nombre = "Cazador de Tesoros",
                Descripcion =
                    "Premia la iniciativa de buscar y compartir recursos valiosos (videos, artículos, herramientas) que benefician a toda la clase.",
                NombreIcono = "box",
                MonedasOtorgadas = 15,
                Creador = nuevoProfesor
            },

            // Medalla por Kudo "Codo a Codo"
            new Medalla
            {
                Nombre = "Espíritu de Equipo",
                Descripcion =
                    "Se consigue al fomentar activamente un ambiente de respeto e inclusión, asegurando que cada miembro del grupo se sienta valorado.",
                NombreIcono = "medal",
                MonedasOtorgadas = 20,
                Creador = nuevoProfesor
            },

            // Medalla por Kudo "Crítica que Construye"
            new Medalla
            {
                Nombre = "Pulidor de Diamantes",
                Descripcion =
                    "Reconoce la habilidad de dar críticas constructivas que ayudan a los compañeros a mejorar su trabajo de forma positiva y amable.",
                NombreIcono = "diamond",
                MonedasOtorgadas = 15,
                Creador = nuevoProfesor
            },

            // Medalla por Kudo "Chispa Creativa"
            new Medalla
            {
                Nombre = "Mente Innovadora",
                Descripcion =
                    "Se otorga por aportar ideas creativas y soluciones originales que sacan al grupo de la rutina y abren nuevas posibilidades.",
                NombreIcono = "wand-magic-sparkles",
                MonedasOtorgadas = 20,
                Creador = nuevoProfesor
            },

            // Medalla por Kudo "Einstein"
            new Medalla
            {
                Nombre = "El Explicador",
                Descripcion =
                    "Premia la increíble habilidad de tomar un tema complejo y explicarlo de una manera tan clara y sencilla que todos puedan entenderlo.",
                NombreIcono = "brain",
                MonedasOtorgadas = 25,
                Creador = nuevoProfesor
            }

        };
    }

    /// <summary>
    /// Crea un conjunto de tablas de equivalencia predeterminadas para un nuevo profesor.
    /// </summary>
    /// <param name="nuevoProfesor">El profesor al que se le asignarán las tablas.</param>
    /// <param name="medallas1"></param>
    /// <param name="medallasDisponibles">La lista de medallas ya creadas para este profesor.</param>
    /// <returns>Una lista de tres tablas de equivalencia configuradas.</returns>
    public List<TablaEquivalencia> CrearTablasEquivalenciaPredeterminadas(Profesor nuevoProfesor,
        List<Medalla> medallasDisponibles)
    {
        var medallasTablaGeneral = new List<Medalla>
        {
            medallasDisponibles.First(m => m.Nombre == "Mente Curiosa"),
            medallasDisponibles.First(m => m.Nombre == "Compañerismo"),
            medallasDisponibles.First(m => m.Nombre == "Participación Perfecta"),
            medallasDisponibles.First(m => m.Nombre == "Cazador de Tesoros"),
            medallasDisponibles.First(m => m.Nombre == "Faro del Grupo"),
            medallasDisponibles.First(m => m.Nombre == "Maestro de la Colaboración"),
            medallasDisponibles.First(m => m.Nombre == "Pulidor de Diamantes"),
            medallasDisponibles.First(m => m.Nombre == "Arquitecto de Ideas"),
            medallasDisponibles.First(m => m.Nombre == "El Explicador"),
            medallasDisponibles.First(m => m.Nombre == "Capitán de Equipo")
        };

        var medallasTablaColaboracion = new List<Medalla>
        {
            medallasDisponibles.First(m => m.Nombre == "Compañerismo"),
            medallasDisponibles.First(m => m.Nombre == "Espíritu de Equipo"),
            medallasDisponibles.First(m => m.Nombre == "Maestro de la Colaboración"),
            medallasDisponibles.First(m => m.Nombre == "Arquitecto de Ideas"),
            medallasDisponibles.First(m => m.Nombre == "Pulidor de Diamantes"),
            medallasDisponibles.First(m => m.Nombre == "Faro del Grupo"),
            medallasDisponibles.First(m => m.Nombre == "El Explicador"),
            medallasDisponibles.First(m => m.Nombre == "Mente Curiosa"),
            medallasDisponibles.First(m => m.Nombre == "Capitán de Equipo"),
            medallasDisponibles.First(m => m.Nombre == "Participación Perfecta")
        };

        var medallasTablaIniciativa = new List<Medalla>
        {
            medallasDisponibles.First(m => m.Nombre == "Curiosidad Insaciable"),
            medallasDisponibles.First(m => m.Nombre == "Cazador de Tesoros"),
            medallasDisponibles.First(m => m.Nombre == "Mente Innovadora"),
            medallasDisponibles.First(m => m.Nombre == "Mente Curiosa"),
            medallasDisponibles.First(m => m.Nombre == "Faro del Grupo"),
            medallasDisponibles.First(m => m.Nombre == "El Explicador"),
            medallasDisponibles.First(m => m.Nombre == "Participación Perfecta"),
            medallasDisponibles.First(m => m.Nombre == "Compañerismo"),
            medallasDisponibles.First(m => m.Nombre == "Maestro de la Colaboración"),
            medallasDisponibles.First(m => m.Nombre == "Capitán de Equipo")
        };

        // Creamos las tablas utilizando el método auxiliar
        var tablaGeneral = GenerarTablaProgresiva("Progreso General", nuevoProfesor, medallasTablaGeneral);
        var tablaColaboracion = GenerarTablaProgresiva("Foco en Colaboración", nuevoProfesor, medallasTablaColaboracion);
        var tablaIniciativa = GenerarTablaProgresiva("Foco en Iniciativa", nuevoProfesor, medallasTablaIniciativa);

        return new List<TablaEquivalencia> { tablaGeneral, tablaColaboracion, tablaIniciativa };
    }

    /// <summary>
    /// Método auxiliar para construir una TablaEquivalencia con notas progresivas del 1 al 10.
    /// </summary>
    private TablaEquivalencia GenerarTablaProgresiva(string nombreTabla, Profesor profesor, List<Medalla> secuenciaDeMedallas)
    {
        var tabla = new TablaEquivalencia
        {
            Nombre = nombreTabla,
            ProfesorId = profesor.Id,
            Equivalencias = new List<Equivalencia>()
        };

        var medallasAcumuladas = new List<Medalla>();
        for (int i = 0; i < 10; i++)
        {
            medallasAcumuladas.Add(secuenciaDeMedallas[i]);

            // Es crucial crear un `new List<Medalla>(medallasAcumuladas)` para "congelar" el estado
            // de la lista en este punto para esta nota
            var equivalencia = new Equivalencia(i + 1, new List<Medalla>(medallasAcumuladas));
            tabla.Equivalencias.Add(equivalencia);
        }

        return tabla;
    }


}