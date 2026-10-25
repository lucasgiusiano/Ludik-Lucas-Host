import React from "react";
import PropTypes from "prop-types";
import styles from "./BaseManagerPage.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import Modal from "../generics/Modal.jsx";

export const BaseManagerPage = ({ actions, modalTitle, modalContent, searchValue, onSearchChange, items, searchPlaceholder, showModal, setShowModal }) => {
  return (
    <div className={styles.userItems}>
      <div className={styles.acciones}>
        <div className={styles.accionesPrincipales}>{actions}</div>

        <div className={styles.accionesBusqueda}>
          <input type="text" placeholder={`Buscar ${searchPlaceholder}`} className={styles.buscador} value={searchValue} onChange={(e) => onSearchChange(e.target.value)} />
          <FontAwesomeIcon className={styles.filtros} icon="fa-solid fa-filter" size="2xl" />
        </div>
      </div>

      <div className={styles.itemsContainer}>{items}</div>

      {showModal && <Modal onClose={() => setShowModal(false)} modalTitle={modalTitle} content={modalContent} />}
    </div>
  );
};

BaseManagerPage.propTypes = {
  actions: PropTypes.func.isRequired, // Debe ser una función que recibe props (como setShowModal)
  modalTitle: PropTypes.string.isRequired, // Título del modal
  modalContent: PropTypes.func.isRequired, // Debe devolver un componente React, recibe { onClose }
  searchValue: PropTypes.string.isRequired,
  onSearchChange: PropTypes.func.isRequired,
  items: PropTypes.arrayOf(PropTypes.node).isRequired, // Lista de elementos JSX (cards, etc)
  searchPlaceholder: PropTypes.string.isRequired, // Placeholder del input de búsqueda
  showModal: PropTypes.bool.isRequired,
  setShowModal: PropTypes.func.isRequired,
};

export default BaseManagerPage;
