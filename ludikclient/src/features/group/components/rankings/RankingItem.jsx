import React from "react";
import PropTypes from "prop-types";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styles from "./RankingItem.module.css";
import { useEliminarRanking } from "../../hooks/useGrupoMutation";

const RankingItem = ({ tabla, showTeacherOptions, onView, idEstudiante = null }) => {
  const eliminarRanking = useEliminarRanking();

  const handleDelete = (e) => {
    e.stopPropagation();
    const confirmacion = window.confirm(`¿Estás seguro de que deseas eliminar el ranking "${tabla.nombre}"?`);
    if (confirmacion) {
      eliminarRanking.mutate(tabla.id);
    }
  };

  // Buscar índice del estudiante en la lista
  const index = tabla.participantes.findIndex((p) => p.perfilEstudianteId === idEstudiante);

  // Calcular índices para slice de 3 elementos: anterior, actual y siguiente
  // Si no encontró al estudiante (index === -1), mostrar los primeros 3 como fallback
  let sliceStart = 0;

  if (index !== -1) {
    // Si está al principio, no hay anterior, empieza en 0
    if (index === 0) {
      sliceStart = 0;
    }
    // Si está al final, empieza en index - 2 para incluir anterior y actual
    else if (index === tabla.participantes.length - 1) {
      sliceStart = Math.max(index - 2, 0);
    }
    // En otro caso normal, muestra uno antes, el propio y uno después
    else {
      sliceStart = index - 1;
    }
  }

  // Sacar slice de hasta 3 participantes a mostrar
  const participantesAMostrar = tabla.participantes.slice(sliceStart, sliceStart + 3);

  return (
    <div className={styles.rankingCard} onClick={() => onView(tabla.id)}>
      <h3>{tabla.nombre}</h3>
      <ul className={styles.participantes}>
        {participantesAMostrar.map((p, i) => {
          const posicionReal = sliceStart + i + 1;

          let clasePosicion = "";
          if (posicionReal === 1) clasePosicion = styles.primero;
          else if (posicionReal === 2) clasePosicion = styles.segundo;
          else if (posicionReal === 3) clasePosicion = styles.tercero;

          const clasesLi = [styles.participante, p.perfilEstudianteId === idEstudiante && !showTeacherOptions ? styles.destacado : "", clasePosicion].filter(Boolean).join(" ");

          return (
            <li key={p.perfilEstudianteId} className={clasesLi}>
              <span className={styles.posicion}>
                {posicionReal === 1 ? (
                  <FontAwesomeIcon icon="fa-solid fa-award" className={styles.oro} />
                ) : posicionReal === 2 ? (
                  <FontAwesomeIcon icon="fa-solid fa-award" className={styles.plata} />
                ) : posicionReal === 3 ? (
                  <FontAwesomeIcon icon="fa-solid fa-award" className={styles.bronce} />
                ) : (
                  `#${posicionReal}`
                )}
              </span>
              <span className={styles.nombre}>{p.nombreEstudiante}</span>
              <span className={styles.medallas}>{p.cantidadMedallas} medallas</span>
            </li>
          );
        })}
      </ul>
      {showTeacherOptions && (
        <div className={styles.accionesRanking} onClick={(e) => e.stopPropagation()}>
          <button className="button-tertiary" onClick={handleDelete}>
            <FontAwesomeIcon icon="fa-solid fa-trash" size="xl" />
          </button>
        </div>
      )}
      <button title="Ver ver el ranking completo" className={styles.verMasRanking} onClick={() => onView(tabla.id)}>
        <FontAwesomeIcon icon="fa-solid fa-eye" size="xl" />
      </button>
    </div>
  );
};

RankingItem.propTypes = {
  tabla: PropTypes.shape({
    id: PropTypes.number.isRequired,
    nombre: PropTypes.string.isRequired,
    participantes: PropTypes.arrayOf(
      PropTypes.shape({
        perfilEstudianteId: PropTypes.number.isRequired,
        nombreEstudiante: PropTypes.string.isRequired,
        cantidadMedallas: PropTypes.number.isRequired,
      })
    ).isRequired,
  }).isRequired,
  showTeacherOptions: PropTypes.bool.isRequired,
  onView: PropTypes.func.isRequired,
  idEstudiante: PropTypes.number, // Nuevo prop para id de estudiante a enfocar
};

export default RankingItem;
