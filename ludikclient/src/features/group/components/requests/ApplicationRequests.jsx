import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";

import { useSolicitudesUnion } from "../../hooks/useGrupoMutation";

import styles from "./ApplicationRequests.module.css";

import * as Toast from "../../../../lib/toastify";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import RequestItem from "./ApplicationRequestItem";
import BarLoader from "../../../generics/BarLoader";

const ApplicationRequests = ({ groupId = null, link = "" }) => {
  // Carga de las solicitudes de unión

  const { data: solicitudes, isLoading } = useSolicitudesUnion(groupId);

  const [codigo, setCodigo] = useState("");

  useEffect(() => {
    if (!link) return setCodigo("");

    try {
      const url = new URL(link);
      setCodigo(url.searchParams.get("codigo") || "");
    } catch (e) {
      console.warn("Link inválido:", link);
      setCodigo("");
    }
  }, [link]);

  const handleCopy = () => {
    navigator.clipboard.writeText(codigo);
    Toast.notificarExito("Enlace copiado!");
  };

  return (
    <div className={styles.applicationRequests}>
      <div className={styles.linkContainer}>
        <p className={styles.link}>{codigo}</p>
        <button aria-label="Copiar link" onClick={handleCopy} className={styles.copyBtn}>
          <FontAwesomeIcon icon="fa-solid fa-copy" />
        </button>
      </div>
      {isLoading ? (
        <BarLoader />
      ) : solicitudes && solicitudes.length ? (
        <ul>
          {solicitudes.map((item, index) => (
            <li key={index}>
              <RequestItem solicitud={item} />
            </li>
          ))}
        </ul>
      ) : (
        <p>Sin solicitudes de unión</p>
      )}
    </div>
  );
};

ApplicationRequests.propTypes = {
  groupId: PropTypes.number.isRequired,
  link: PropTypes.string.isRequired,
};

export default ApplicationRequests;
