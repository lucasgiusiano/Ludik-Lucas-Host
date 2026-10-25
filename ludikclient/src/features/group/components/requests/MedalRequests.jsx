import React from "react";
import PropTypes from "prop-types";
import styles from "./MedalRequests.module.css";
import { useSolicitudesMedallas } from "../../hooks/useGrupoMutation";
import BarLoader from "../../../generics/BarLoader";
import MedalRequestItem from "./MedalRequestItem";

const MedalRequests = ({ grupoId, medallas }) => {
  const { data: solicitudes, isLoading } = useSolicitudesMedallas(grupoId);

  return (
    <div className={styles.medalRequests}>
      {isLoading ? (
        <BarLoader />
      ) : !solicitudes || solicitudes.length === 0 ? (
        <p>No hay solicitudes de medallas</p>
      ) : (
        solicitudes.map((solicitud) => <MedalRequestItem key={solicitud.id} solicitud={solicitud} medallas={medallas} />)
      )}
    </div>
  );
};

MedalRequests.propTypes = {
  grupoId: PropTypes.number.isRequired,
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

export default MedalRequests;
