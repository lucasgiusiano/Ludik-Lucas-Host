import React from "react";
import PropTypes from "prop-types";
import styles from "./ConfirmDialog.module.css"; // Asegúrate de que la ruta es correcta

const ConfirmDialog = ({ message, confirmText = "Confirmar", cancelText = "Cancelar", onConfirm, onCancel, isLoading = false }) => {
  return (
    <div className={styles.confirmDialog}>
      <p className={styles.confirmMessage}>{message}</p>
      <div className={styles.confirmActions}>
        <button type="button" className="button-tertiary" onClick={onCancel} disabled={isLoading}>
          {cancelText}
        </button>
        <button type="button" className="button-secondary" onClick={onConfirm} disabled={isLoading}>
          {isLoading ? "Procesando..." : confirmText}
        </button>
      </div>
    </div>
  );
};

ConfirmDialog.propTypes = {
  message: PropTypes.string.isRequired,
  confirmText: PropTypes.string,
  cancelText: PropTypes.string,
  onConfirm: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  isLoading: PropTypes.bool,
};

export default ConfirmDialog;
