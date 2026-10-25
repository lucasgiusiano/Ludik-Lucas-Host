import React, { useState } from "react";
import PropTypes from "prop-types";

import styles from "./GroupUnionLinkForm.module.css";

import { useSolicitarUnirseGrupo } from "../../hooks/useGrupoMutation";

import { PulseLoader } from "../../../generics/BarLoader";

const GroupUnionLinkModal = ({ onClose }) => {
  const [codigo, setCodigo] = useState("");

  const { mutate: unirse, isPending: isUnirsePending } = useSolicitarUnirseGrupo(onClose);

  const handleSubmit = (e) => {
    e.preventDefault();

    if (codigo.trim().length < 5) {
      alert("Ingrese un código válido.");
      return;
    }

    unirse(codigo.trim()); // Envía el CODIGO
  };

  return (
    <form className={styles.modalFormLink} onSubmit={handleSubmit}>
      <label>
        Código del Grupo
        <input type="text" name="codigo" value={codigo} onChange={(e) => setCodigo(e.target.value)} disabled={isUnirsePending} placeholder="Ingrese el código del grupo" />
      </label>

      <button type="submit" disabled={isUnirsePending || codigo.trim().length < 5} className="button-secondary">
        {isUnirsePending ? <PulseLoader /> : "Unirme al Grupo"}
      </button>
    </form>
  );
};

GroupUnionLinkModal.propTypes = {
  onClose: PropTypes.func.isRequired,
};

export default GroupUnionLinkModal;
