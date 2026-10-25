import React from "react";
import PropTypes from "prop-types";
import styles from "./PacItem.module.css";

const PacItem = ({ pac }) => {
  const totalNiveles = pac.cantidadMedallasNecesarias || 0;
  const totalContribuciones = pac.totalContribuciones || 0;

  // Calcular porcentaje de progreso
  const porcentaje = totalNiveles > 0 ? Math.min((totalContribuciones / totalNiveles) * 100, 100) : 0;

  return (
    <div className={styles.card}>
      <h3 className={styles.title}>
        <strong>Recompensa</strong> {pac.nombre}
      </h3>

      <div className={styles.progressBarContainer}>
        <p>Progreso</p>
        <div className={styles.progressBarWrapper}>
          <div className={styles.progressFill} style={{ width: `${porcentaje}%` }} />
        </div>
      </div>

      <div className={styles.infoProgressBar}>
        <p>
          <strong>Contribuciones</strong> {totalContribuciones}
        </p>
        <p>
          <strong>Medallas Necesarias</strong> {totalNiveles}
        </p>
      </div>
    </div>
  );
};

PacItem.propTypes = {
  pac: PropTypes.shape({
    id: PropTypes.number.isRequired,
    grupoId: PropTypes.number,
    nombre: PropTypes.string.isRequired,
    visual: PropTypes.number,
    cantidadMedallasNecesarias: PropTypes.number,
    totalContribuciones: PropTypes.number,
    recompensaClaseId: PropTypes.number,
    estado: PropTypes.number,
  }).isRequired,
};

export default PacItem;
