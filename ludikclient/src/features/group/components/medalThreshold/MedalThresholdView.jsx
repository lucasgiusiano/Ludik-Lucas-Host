import React from "react";
import PropTypes from "prop-types";
import styles from "./MedalThresholdView.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ThresholdItem from "./ThresholdItem";
import ThresholdCreateForm from "./ThresholdCreateForm";
import { useUmbralesMedallas, useTiposKudo } from "../../hooks/useGrupoMutation";
import { useMedallasProfesor } from "../../../medals/hooks/useMedalMutation";
import BarLoader from "../../../generics/BarLoader";

const MedalThresholdView = ({ setModalContent, setModalTitle, setShowModal, groupId, showTeacherOptions }) => {
  const { data: thresholds, isLoading: isLoadingUmbrales } = useUmbralesMedallas(groupId);
  const { data: medallas, isLoading: loadingMedallas } = useMedallasProfesor(showTeacherOptions);
  const { data: tiposKudo, isLoading: loadingKudos } = useTiposKudo();

  const loadingAll = isLoadingUmbrales || loadingMedallas || loadingKudos;

  const handleOpenThresholdCreateForm = () => {
    setModalTitle("Crear nuevo umbral");
    setModalContent(
      <ThresholdCreateForm
        grupoId={groupId}
        medallas={medallas}
        tiposKudo={tiposKudo}
        onClose={() => setShowModal(false)} // si querés cerrar el modal
      />
    );
    setShowModal(true);
  };

  return (
    <>
      <div className={styles.thresholdsContainer}>
        {showTeacherOptions && (
          <button className={`button-creator ${styles.addThresholdButton}`} onClick={handleOpenThresholdCreateForm}>
            <FontAwesomeIcon icon="fa-solid fa-chart-bar" size="2xl" />
            Crear nuevo umbral de obtención
          </button>
        )}
        {thresholds?.length !== 0 && (
          <>
            {thresholds?.map((threshold) => (
              <ThresholdItem
                key={threshold.id}
                threshold={threshold}
                setModalContent={setModalContent}
                setModalTitle={setModalTitle}
                setShowModal={setShowModal}
                medallas={medallas}
                tiposKudo={tiposKudo}
                groupId={groupId}
                showTeacherOptions={showTeacherOptions}
              />
            ))}
          </>
        )}
      </div>

      {loadingAll ? <BarLoader /> : thresholds?.length === 0 && <p className={styles.emptyMessage}>No hay umbrales definidos aún.</p>}
    </>
  );
};

MedalThresholdView.propTypes = {
  thresholds: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      cantidadKudos: PropTypes.number.isRequired,
      medallaId: PropTypes.number.isRequired,
      medallaNombre: PropTypes.string.isRequired,
      rutaIconoMedalla: PropTypes.string.isRequired,
      tipoKudoId: PropTypes.number.isRequired,
      tipoKudoNombre: PropTypes.string.isRequired,
    })
  ).isRequired,
  setModalContent: PropTypes.func.isRequired,
  setModalTitle: PropTypes.func.isRequired,
  setShowModal: PropTypes.func.isRequired,
  groupId: PropTypes.number.isRequired,
  showTeacherOptions: PropTypes.bool.isRequired,
};

export default MedalThresholdView;
