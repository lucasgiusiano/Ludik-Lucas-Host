import React from "react";
import PropTypes from "prop-types";
import style from "./StoreGroupView.module.css";
import BarLoader from "../../../generics/BarLoader.jsx";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import RewardItem from "../../../rewards/components/RewardItem.jsx";
import RewardAsignationForm from "./RewardAsignationForm.jsx";

const StoreGroupView = ({ recompensas, isLoading, isProfesor, perfil, grupoId, setShowModal, setModalContent, setModalTitle }) => {
  const handleAsignNewReward = () => {
    setModalTitle("Asignar recompensa a grupos");
    setModalContent(
      <RewardAsignationForm
        grupoId={grupoId} // solo pasamos este id
        gruposProfesor={[]} // sin grupos, array vacío
        tiendaRecompensas={recompensas} // sin recompensas en la tienda, array vacío
        onClose={() => setShowModal(false)}
      />
    );
    setShowModal(true);
  };

  return (
    <div className={style.storeContent}>
      <div className={style.storeRewards}>
        {isProfesor && (
          <button className={`button-creator ${style.addRewardButton}`} onClick={handleAsignNewReward}>
            <FontAwesomeIcon icon="trophy" size="2xl" />
            Asignar recompensa a esta tienda
          </button>
        )}

        {recompensas?.length > 0 &&
          recompensas.map((reward) => (
            <RewardItem
              key={reward.id + reward.nombre}
              reward={reward}
              redeemed={false}
              perfilId={perfil?.id}
              showProfesorOptions={isProfesor}
              storeView={true}
              setShowModal={setShowModal}
              setModalContent={setModalContent}
              setModalTitle={setModalTitle}
            />
          ))}
      </div>
      {isLoading ? (
        <BarLoader />
      ) : recompensas?.length === 0 ? (
        <div className={style.mensaje}>
          <p>No hay recompensas disponibles.</p>
        </div>
      ) : null}
    </div>
  );
};

StoreGroupView.propTypes = {
  recompensas: PropTypes.array,
  isLoading: PropTypes.bool,
  isProfesor: PropTypes.bool,
  perfil: PropTypes.object,
  grupoId: PropTypes.number, // <-- aquí la prop
  setShowModal: PropTypes.func.isRequired,
  setModalContent: PropTypes.func.isRequired,
  setModalTitle: PropTypes.func.isRequired,
};

export default StoreGroupView;
