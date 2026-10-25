import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import styles from "./GroupConfigView.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import * as Toast from "../../../../lib/toastify";
import { useEliminarGrupo, useReiniciarLogrosGrupo } from "../../hooks/useGrupoMutation";
import GroupCreateForm from "../teacher/GroupCreateForm";
import { PulseLoader } from "../../../generics/BarLoader.jsx";

const GroupConfigView = ({ id, group, setModalContent, setModalTitle, setShowModal }) => {
  const [grupo, setGrupo] = useState({
    nombre: "",
    materia: "",
    institucion: "",
    fCreacion: "",
    urlCompleta: "",
  });

  const [codigo, setCodigo] = useState("");

  useEffect(() => {
    if (group) {
      setGrupo({
        nombre: group.nombre || "",
        materia: group.materia || "",
        institucion: group.institucion || "",
        fCreacion: group.fCreacion || "",
        urlCompleta: group.urlCompleta || "",
      });

      setCodigo(new URL(group.urlCompleta).searchParams.get("codigo") || "");
    }
  }, [group]);

  const eliminarGrupo = useEliminarGrupo();
  const reiniciarLogros = useReiniciarLogrosGrupo();

  const handleEliminar = () => {
    const confirmado = window.confirm(`¿Seguro que quieres eliminar el grupo "${grupo.nombre}"?`);
    if (confirmado) eliminarGrupo.mutate(id);
  };

  const handleReiniciarLogros = () => {
    const confirmado = window.confirm(`¿Deseas reiniciar los logros del grupo "${grupo.nombre}"?`);
    if (confirmado) reiniciarLogros.mutate(id);
  };

  const handleEditar = () => {
    setModalContent(<GroupCreateForm grupoInicial={grupo} idGrupo={id} onClose={() => setShowModal(false)} />);
    setModalTitle("Editar grupo");
    setShowModal(true);
  };

  const handleCopy = () => {
    navigator.clipboard.writeText(codigo);
    Toast.notificarExito("Enlace copiado!");
  };

  return (
    <div className={styles.configsContainer}>
      <div className={styles.propsContainer}>
        <h3>Información</h3>
        <div className={styles.props}>
          <label>
            <strong>Nombre</strong> {grupo.nombre}
          </label>
          <label>
            <strong>Materia</strong> {grupo.materia}
          </label>
          <label>
            <strong>Institución</strong> {grupo.institucion}
          </label>
          <label>
            <strong>Fecha de creación</strong> {new Date(grupo.fCreacion).toLocaleDateString()}
          </label>
          <label className={styles.linkContainer}>
            <strong>Enlace de invitación</strong>
            <p className={styles.link}>{codigo}</p>
            <button aria-label="Copiar link" onClick={handleCopy} className={styles.copyBtn}>
              <FontAwesomeIcon icon="fa-solid fa-copy" />
            </button>
          </label>
        </div>
      </div>

      <div className={styles.actionsContainer}>
        <h3>Acciones</h3>
        <div className={styles.actions}>
          <button className="button" onClick={handleEditar}>
            Editar grupo
          </button>
          <button className="button-quinary" onClick={handleReiniciarLogros}>
            {reiniciarLogros.isPending ? <PulseLoader /> : "Reiniciar logros"}
          </button>
          <button className="button-tertiary" onClick={handleEliminar}>
            {eliminarGrupo.isPending ? <PulseLoader /> : "Eliminar grupo"}
          </button>
        </div>
      </div>
    </div>
  );
};

GroupConfigView.propTypes = {
  id: PropTypes.number.isRequired,
  group: PropTypes.shape({
    nombre: PropTypes.string.isRequired,
    tablaEquivalenciaId: PropTypes.number.isRequired,
    tablaEquivalenciaNotaMaxima: PropTypes.number.isRequired,
    profesorId: PropTypes.string.isRequired,
    institucion: PropTypes.string.isRequired,
    materia: PropTypes.string.isRequired,
    fCreacion: PropTypes.string.isRequired,
    urlCompleta: PropTypes.string.isRequired,
    idTienda: PropTypes.number.isRequired,
  }).isRequired,
  setModalContent: PropTypes.func.isRequired,
  setModalTitle: PropTypes.func.isRequired,
  setShowModal: PropTypes.func.isRequired,
};

export default GroupConfigView;
