import React from "react";
import PropTypes from "prop-types";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import styles from "./ApplicationRequests.module.css";

import { useAceptarSolicitud, useRechazarSolicitud } from "../../hooks/useGrupoMutation";

const RequestItem = ({ solicitud }) => {
  const { mutate: aceptar, isLoading: isAccepting } = useAceptarSolicitud();
  const { mutate: rechazar, isLoading: isRejecting } = useRechazarSolicitud();

  return (
    <div className={styles.request}>
      <span title={solicitud.nombreEstudiante}>{solicitud.nombreEstudiante}</span>
      <div>
        <button disabled={isAccepting} aria-label="Aceptar solicitud" className="button-secondary" onClick={() => aceptar(solicitud.idSolicitud)}>
          <FontAwesomeIcon icon="fa-solid fa-user-check" size="xl" />
        </button>
        <button disabled={isRejecting} aria-label="Rechazar solicitud" className="button-tertiary" onClick={() => rechazar(solicitud.idSolicitud)}>
          <FontAwesomeIcon icon="fa-solid fa-user-xmark" size="xl" />
        </button>
      </div>
    </div>
  );
};

RequestItem.propTypes = {
  solicitud: PropTypes.shape({
    idSolicitud: PropTypes.number.isRequired,
    idEstudiante: PropTypes.string.isRequired,
    nombreEstudiante: PropTypes.string.isRequired,
    nombre: PropTypes.string.isRequired,
    fecha: PropTypes.shape({
      year: PropTypes.number.isRequired,
      month: PropTypes.number.isRequired,
      day: PropTypes.number.isRequired,
    }).isRequired,
  }).isRequired,
};

export default RequestItem;
