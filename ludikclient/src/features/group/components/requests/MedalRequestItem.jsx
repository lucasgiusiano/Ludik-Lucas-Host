import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import styles from "./MedalRequestItem.module.css";
import MedalCard from "../../../medals/components/MedalCard";
import { useAceptarSolicitudMedalla, useRechazarSolicitudMedalla } from "../../hooks/useGrupoMutation";
import { PulseLoader } from "../../../generics/BarLoader";

const MedalRequestItem = ({ solicitud, medallas }) => {
  const [medalla, setMedalla] = useState(null);

  useEffect(() => {
    if (medallas && medallas.length > 0) {
      const encontrada = medallas.find((m) => m.id === solicitud.medallaId);
      setMedalla(encontrada || null);
    }
  }, [medallas, solicitud.medallaId]);

  const aceptarMutation = useAceptarSolicitudMedalla();
  const rechazarMutation = useRechazarSolicitudMedalla();

  const handleAceptar = () => aceptarMutation.mutate(solicitud.id);
  const handleRechazar = () => rechazarMutation.mutate(solicitud.id);

  return (
    <div className={styles.medalRequestItem}>
      <div className={styles.requestInfo}>
        <span className={styles.requestDate}>
          {new Date(solicitud.fecha).toLocaleDateString("es-UY", {
            day: "2-digit",
            month: "2-digit",
            year: "numeric",
          })}
        </span>
        <span className={styles.requestState}>{solicitud.estado}</span>
        <div className={styles.medalInfo}>
          <strong>Medalla solicitada</strong>
          {medalla ? <MedalCard medal={medalla} showEditOption={false} /> : <p>Cargando medalla...</p>}
        </div>
        <div className={styles.requestDetails}>
          <strong>Argumento</strong> {solicitud.descripcion}
        </div>
      </div>
      <div className={styles.requestActions}>
        <button className="button-secondary" onClick={handleAceptar} disabled={aceptarMutation.isPending || solicitud.estado !== "Pendiente"}>
          {aceptarMutation.isPending ? <PulseLoader /> : "Aprobar"}
        </button>
        <button className="button-tertiary" onClick={handleRechazar} disabled={rechazarMutation.isPending || solicitud.estado !== "Pendiente"}>
          {rechazarMutation.isPending ? <PulseLoader /> : "Rechazar"}
        </button>
      </div>
    </div>
  );
};

MedalRequestItem.propTypes = {
  solicitud: PropTypes.shape({
    id: PropTypes.number.isRequired,
    perfilEstudianteId: PropTypes.number.isRequired,
    medallaId: PropTypes.number.isRequired,
    descripcion: PropTypes.string.isRequired,
    fecha: PropTypes.string.isRequired,
    estado: PropTypes.string.isRequired,
  }).isRequired,
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

export default MedalRequestItem;
