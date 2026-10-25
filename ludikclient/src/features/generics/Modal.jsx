import React from "react";
import PropTypes from "prop-types";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styles from "./Modal.module.css";

export const Modal = ({ onClose, modalTitle, content }) => {
  return (
    <div className={styles.modalBackdrop}>
      <section className={styles.modal}>
        <div className={styles.modalHeader}>
          <h2>{modalTitle}</h2>
          <button className={styles.closeButton} onClick={onClose} aria-label="Cerrar">
            <FontAwesomeIcon icon="fa-solid fa-xmark" />
          </button>
        </div>
        <hr />
        <div className={styles.modalContent}>{content}</div>
      </section>
    </div>
  );
};

Modal.propTypes = {
  onClose: PropTypes.func.isRequired,
  modalTitle: PropTypes.string.isRequired,
  content: PropTypes.node.isRequired,
};

export default Modal;
