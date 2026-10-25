import React, { useState } from "react";
import PropTypes from "prop-types";
import classNames from "classnames";
import styles from "./RequestView.module.css";

import ApplicationRequests from "./ApplicationRequests";
import MedalRequests from "./MedalRequests";

const RequestView = ({ grupo, urlCompleta, medallas }) => {
  const [mostrarUnion, setMostrarUnion] = useState(true);

  const handleSwitchChange = () => setMostrarUnion(!mostrarUnion);

  return (
    <div className={styles.requestView}>
      {/* Switch de selección */}
      <div className={styles.switchContainer}>
        <label className={styles.switch}>
          <input type="checkbox" checked={mostrarUnion} onChange={handleSwitchChange} className={styles.switchInput} />
          <span className={styles.slider}>
            <span className={classNames(styles.switchLabel, styles.union)}>Solicitudes de Unión</span>

            <span className={classNames(styles.switchLabel, styles.medalla)}>Solicitudes de Medalla</span>
          </span>
        </label>
      </div>

      {/* Contenedor deslizante */}
      <div className={styles.formSlider}>
        <div
          className={styles.formInner}
          style={{
            transform: mostrarUnion ? "translateX(-50%)" : "translateX(0)",
          }}
        >
          {/* Vista de solicitudes de medalla */}
          <div
            className={classNames(styles.formulario, {
              [styles.oculto]: mostrarUnion,
            })}
          >
            <h3>Solicitudes de Medalla</h3>
            <MedalRequests grupoId={grupo} medallas={medallas} />
          </div>

          {/* Vista de solicitudes de unión */}
          <div
            className={classNames(styles.formulario, {
              [styles.oculto]: !mostrarUnion,
            })}
          >
            <h3>Solicitudes de Unión</h3>
            <ApplicationRequests groupId={grupo} link={urlCompleta} />
          </div>
        </div>
      </div>
    </div>
  );
};

RequestView.propTypes = {
  grupo: PropTypes.number.isRequired,
  urlCompleta: PropTypes.string.isRequired,
  medallas: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      nombre: PropTypes.string.isRequired,
      nombreIcono: PropTypes.string.isRequired,
      descripcion: PropTypes.string.isRequired,
      cantidadMedallasBrinda: PropTypes.number.isRequired,
    })
  ).isRequired,
};

export default RequestView;
