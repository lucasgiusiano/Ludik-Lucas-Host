import React, { useEffect, useState } from "react";
import styles from "./GroupCreateForm.module.css";
import * as Toast from "../../../../lib/toastify.js";
import PropTypes from "prop-types";

import { useSelector } from "react-redux";
import { selectUserId } from "../../../auth/hooks/userSlice";

import { useCrearGrupo, useEditarGrupo } from "../../hooks/useGrupoMutation.js";
import { useTablasEquivalencia } from "../../../equivalence-table/hooks/useEquivalenceTableMutation";

import { PulseLoader } from "../../../generics/BarLoader.jsx";

export const GroupCreateModal = ({ onClose, idGrupo, grupoInicial }) => {
  const profesorId = useSelector(selectUserId);

  const [grupo, setGrupo] = useState({
    nombre: "",
    institucion: "",
    materia: "",
    tablaEquivalenciaId: "",
  });

  const { mutateAsync: crearGrupo, isPending: cargandoCreacion } = useCrearGrupo();
  const { mutateAsync: editarGrupo, isPending: cargandoEdicion } = useEditarGrupo();
  const { data: tablasEquivalencia, isLoading } = useTablasEquivalencia();

  useEffect(() => {
    if (grupoInicial) {
      setGrupo({
        nombre: grupoInicial.nombre || "",
        institucion: grupoInicial.institucion || "",
        materia: grupoInicial.materia || "",
        tablaEquivalenciaId: grupoInicial.tablaEquivalenciaId?.toString() || "",
      });
    }
  }, [grupoInicial]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setGrupo((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const validar = () => {
    const campos = [
      ["Nombre del grupo", grupo.nombre],
      ["Institución", grupo.institucion],
      ["Materia", grupo.materia],
    ];

    for (const [campo, valor] of campos) {
      if (valor.trim().length <= 1) {
        Toast.notificarWarning(`El ${campo} debe tener más de 1 carácter.`);
        return false;
      }
    }

    if (!grupo.tablaEquivalenciaId) {
      Toast.notificarWarning("Debes seleccionar una rúbrica.");
      return false;
    }

    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validar()) return;

    const grupoFinal = {
      ...grupo,
      tablaEquivalenciaId: Number(grupo.tablaEquivalenciaId),
      profesorId: profesorId,
    };

    try {
      if (grupoInicial) {
        await editarGrupo({ ...grupoFinal, id: idGrupo });
      } else {
        await crearGrupo(grupoFinal);
      }

      onClose();
    } catch (error) {
      // manejo de errores ya se hace en los hooks
    }
  };

  return (
    <form className={styles.modalForm} onSubmit={handleSubmit}>
      <label>
        Nombre del grupo
        <input type="text" name="nombre" placeholder="Ej: 2° A" value={grupo.nombre} onChange={handleChange} />
      </label>

      <label>
        Institución
        <input type="text" name="institucion" placeholder="Ej: Liceo 12" value={grupo.institucion} onChange={handleChange} />
      </label>

      <label>
        Materia
        <input type="text" name="materia" placeholder="Ej: Matemática" value={grupo.materia} onChange={handleChange} />
      </label>

      <label>
        Rúbrica
        <div className={styles.selectWrapper}>
          <select name="tablaEquivalenciaId" value={grupo.tablaEquivalenciaId} onChange={handleChange}>
            <option value="">Seleccionar</option>
            {isLoading ? (
              <option disabled>Cargando...</option>
            ) : (
              tablasEquivalencia?.map((tabla) => (
                <option key={tabla.id} value={tabla.id}>
                  {`${tabla.nombre} - ${tabla.equivalencias.length} ${tabla.equivalencias.length > 1 ? "notas" : "nota"}`}
                </option>
              ))
            )}
          </select>
        </div>
      </label>

      <button type="submit" className={`${styles.btnSubmit} button-secondary`}>
        {cargandoCreacion || cargandoEdicion ? <PulseLoader /> : grupoInicial ? "Guardar cambios" : "Crear grupo"}
      </button>
    </form>
  );
};

GroupCreateModal.propTypes = {
  onClose: PropTypes.func.isRequired,
  idGrupo: PropTypes.number.isRequired,
  grupoInicial: PropTypes.shape({
    id: PropTypes.number.isRequired,
    nombre: PropTypes.string.isRequired,
    institucion: PropTypes.string.isRequired,
    materia: PropTypes.string.isRequired,
    tablaEquivalenciaId: PropTypes.number.isRequired,
  }),
};

export default GroupCreateModal;
