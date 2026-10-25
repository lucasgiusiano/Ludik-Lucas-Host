import React from "react";
import styles from "./RewardItem.module.css";
import PropTypes from "prop-types";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useClaimReward } from "../../group/hooks/useStudentMutation";
import ConfirmDialog from "../../generics/ConfirmDialog.jsx"; // ajusta la ruta si hace falta

const RewardItem = ({ reward, redeemed, perfilId, showProfesorOptions, storeView, onEdit, setShowModal, setModalContent, setModalTitle }) => {
  const { mutate: claimReward, isLoading: isClaiming } = useClaimReward(perfilId, reward.id, showProfesorOptions);

  const representacion = reward.representacion || reward.datos || {};
  const nombreIcono = representacion?.nombreIcono;
  const urlMiniatura = representacion?.urlMiniatura;

  const doClaim = () => {
    // Ejecuta la mutación (fallback)
    claimReward(perfilId, reward.id);
  };

  const openConfirmModal = () => {
    if (!setShowModal || !setModalContent || !setModalTitle) {
      // fallback: si no tenemos setters, ejecutamos la compra directamente
      doClaim();
      return;
    }

    const message = `¿Confirmás canjear "${reward.nombre}" por ${reward.precio} monedas?`;

    // Contenido del modal: ConfirmDialog que llama a la mutación al confirmar
    const handleConfirm = () => {
      // cerramos el modal y ejecutamos la mutación
      setShowModal(false);
      doClaim();
    };

    const handleCancel = () => {
      setShowModal(false);
    };

    setModalTitle("Confirmar canje");
    setModalContent(<ConfirmDialog message={message} onConfirm={handleConfirm} onCancel={handleCancel} isLoading={isClaiming} />);
    setShowModal(true);
  };

  const handleClaimClick = () => {
    // Si es profesor o no hay perfil, no permitir comprar
    if (showProfesorOptions || !perfilId) return;
    // Si se pasaron setters, abrimos modal; si no, ejecutamos la mutación directamente
    if (setShowModal && setModalContent && setModalTitle) {
      openConfirmModal();
    } else {
      doClaim();
    }
  };

  const handleEditClick = () => {
    if (onEdit) onEdit(reward);
  };

  return (
    <div className={`${styles.rewardCard} ${storeView && styles.storeViewCard}`}>
      <h4>{reward.nombre.substring(reward.nombre.indexOf(":") + 1)}</h4>
      <div className={styles.iconContainer}>
        {reward.tipo === "Imagen" ? (
          reward.nombre.includes("#") ? (
            <FontAwesomeIcon icon={`fa-solid fa-palette`} color={reward.nombre.substring(reward.nombre.indexOf("#"))} />
          ) : (
            <img src={`${urlMiniatura}`} alt={`Recompensa ${reward?.nombre}`} />
          )
        ) : (
          <FontAwesomeIcon icon={`fa-solid fa-${nombreIcono ? nombreIcono : "trophy"}`} />
        )}
      </div>

      {!redeemed && (
        <>
          <p className={styles.monedas}>
            <FontAwesomeIcon icon="fa-solid fa-coins" />
            {reward.precio}
          </p>
          {showProfesorOptions ? (
            !storeView && (
              <button className={`${styles.editarRecompensa} ${storeView && styles.storeViewButton}`} onClick={handleEditClick} type="button" aria-label={`Editar recompensa ${reward.nombre}`}>
                <FontAwesomeIcon icon="fa-solid fa-pen-to-square" />
              </button>
            )
          ) : (
            <button className={styles.canjearRecompensa} onClick={handleClaimClick} disabled={isClaiming} type="button" aria-label={`Canjear recompensa ${reward.nombre}`}>
              <FontAwesomeIcon icon="fa-solid fa-cart-shopping" />
            </button>
          )}
        </>
      )}
    </div>
  );
};

RewardItem.propTypes = {
  reward: PropTypes.shape({
    id: PropTypes.number.isRequired,
    nombre: PropTypes.string.isRequired,
    precio: PropTypes.number.isRequired,
    tipo: PropTypes.string.isRequired, // por ejemplo: "Icono"
    representacion: PropTypes.shape({
      nombreIcono: PropTypes.string,
    }),
    datos: PropTypes.shape({
      urlMiniatura: PropTypes.string,
      urlCompleta: PropTypes.string,
    }),
  }).isRequired,
  redeemed: PropTypes.bool.isRequired,
  perfilId: PropTypes.number, // ahora opcional: si no existe, se evita canjear
  showProfesorOptions: PropTypes.bool.isRequired,
  storeView: PropTypes.bool.isRequired,
  onEdit: PropTypes.func,
  // Nuevas props (opcionales) para abrir modal desde la card
  setShowModal: PropTypes.func,
  setModalContent: PropTypes.func,
  setModalTitle: PropTypes.func,
};

RewardItem.defaultProps = {
  onEdit: null,
  setShowModal: null,
  setModalContent: null,
  setModalTitle: null,
  perfilId: null,
};

export default RewardItem;
