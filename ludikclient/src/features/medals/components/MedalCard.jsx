import React, { useState, useRef, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import PropTypes from "prop-types";
import styles from "./MedalCard.module.css";

const MedalCard = ({ medal, cantidad = 0, onEdit, showEditOption }) => {
  const [showPopoverTitulo, setShowPopoverTitulo] = useState(false);
  const [showPopoverDesc, setShowPopoverDesc] = useState(false);
  const popoverTituloRef = useRef(null);
  const popoverDescRef = useRef(null);

  useEffect(() => {
    const handleClickOutside = (e) => {
      if (popoverTituloRef.current && !popoverTituloRef.current.contains(e.target)) {
        setShowPopoverTitulo(false);
      }
      if (popoverDescRef.current && !popoverDescRef.current.contains(e.target)) {
        setShowPopoverDesc(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  return (
    <div className={`${styles.medallaCard} ${!showEditOption ? styles.bottomPadding : ""}`}>
      <div className={styles.medallaContainer}>
        <div className={styles.medallaImagenWrapper}>
          <FontAwesomeIcon icon={`fa fa-${medal?.nombreIcono || medal?.icono}`} />
          {cantidad != 0 && <span className={styles.cantidadMedallas}>{cantidad}</span>}
        </div>

        <div className={styles.tituloWrapper}>
          <p className={styles.medallaTitle} onClick={() => setShowPopoverTitulo((prev) => !prev)}>
            {medal?.nombre}
          </p>
          {showPopoverTitulo && (
            <div ref={popoverTituloRef} className={`${styles.popover} ${styles.popoverTitulo}`}>
              {medal?.nombre}
            </div>
          )}
        </div>

        <div className={styles.descripcionWrapper}>
          <p className={styles.medallaDescripcion} onClick={() => setShowPopoverDesc((prev) => !prev)}>
            {medal?.descripcion}
          </p>
          {showPopoverDesc && (
            <div ref={popoverDescRef} className={`${styles.popover} ${styles.popoverDescripcion}`}>
              {medal?.descripcion}
            </div>
          )}
        </div>

        {showEditOption && (
          <p className={styles.medallaPuntos}>
            <FontAwesomeIcon icon="fa-solid fa-coins" /> {medal?.cantidadMedallasBrinda}
          </p>
        )}
      </div>

      {showEditOption && (
        <button className={styles.editBtn} onClick={onEdit}>
          <FontAwesomeIcon icon="fa-solid fa-pen-to-square" size="lg" />
        </button>
      )}
    </div>
  );
};

MedalCard.propTypes = {
  medal: PropTypes.shape({
    id: PropTypes.number.isRequired,
    nombre: PropTypes.string.isRequired,
    nombreIcono: PropTypes.string.isRequired,
    icono: PropTypes.string.isRequired,
    descripcion: PropTypes.string.isRequired,
    cantidadMedallasBrinda: PropTypes.number.isRequired,
    esAsignacionMutua: PropTypes.bool.isRequired,
  }).isRequired,
  cantidad: PropTypes.number.isRequired,
  onEdit: PropTypes.func.isRequired,
  showEditOption: PropTypes.bool.isRequired,
};

export default MedalCard;
