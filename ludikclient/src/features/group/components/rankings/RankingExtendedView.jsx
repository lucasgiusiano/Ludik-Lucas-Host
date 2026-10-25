// features/group/components/rankings/RankingExtendedView.jsx
import React from "react";
import PropTypes from "prop-types";
import { useRankingPorId } from "../../hooks/useGrupoMutation";
import BarLoader from "../../../generics/BarLoader";
import styles from "./RankingExtendedView.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const RankingExtendedView = ({ id, idEstudiante, showTeacherOptions }) => {
  const { data: ranking, isLoading } = useRankingPorId(id);

  if (isLoading) return <BarLoader />;
  if (!ranking) return <p>No se encontró el ranking.</p>;

  return (
    <div className={styles.extendedView}>
      <h2>{ranking.nombre}</h2>
      <p>
        <FontAwesomeIcon icon="fa-solid fa-award" /> Medalla asociada: <strong>{ranking.medallaAsociadaNombre}</strong>
      </p>

      <ul className={styles.participantes}>
        <li>
          <span className={styles.posicion}>Posición</span>
          <span>Nombre</span>
          <span>Medallas</span>
        </li>
        {ranking.participantes.map((p, i) => {
          // Clases según posición 1, 2, 3
          let clasePosicion = "";
          if (i === 0) clasePosicion = styles.primero;
          else if (i === 1) clasePosicion = styles.segundo;
          else if (i === 2) clasePosicion = styles.tercero;

          // Agregar clase destacado si es el estudiante actual
          const clasesLi = [clasePosicion, p.perfilEstudianteId === idEstudiante && !showTeacherOptions ? styles.destacado : ""].filter(Boolean).join(" ");

          return (
            <li key={p.perfilEstudianteId} className={clasesLi}>
              <span className={styles.posicion}>
                {i + 1 === 1 ? (
                  <FontAwesomeIcon icon="fa-solid fa-award" className={styles.oro} />
                ) : i + 1 === 2 ? (
                  <FontAwesomeIcon icon="fa-solid fa-award" className={styles.plata} />
                ) : i + 1 === 3 ? (
                  <FontAwesomeIcon icon="fa-solid fa-award" className={styles.bronce} />
                ) : (
                  `#${i + 1}`
                )}
              </span>
              <span>{p.nombreEstudiante}</span>
              <span>{p.cantidadMedallas}</span>
            </li>
          );
        })}
      </ul>
    </div>
  );
};

RankingExtendedView.propTypes = {
  id: PropTypes.number.isRequired,
  idEstudiante: PropTypes.number.isRequired,
  showTeacherOptions: PropTypes.bool.isRequired,
};

export default RankingExtendedView;
