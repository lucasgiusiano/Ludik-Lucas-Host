// ThresholdCreateForm.jsx
import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import styles from "./ThresholdCreateForm.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useCrearUmbralMedalla, useEditarUmbralMedalla, useEliminarUmbralMedalla } from "../../hooks/useGrupoMutation";
import { notificarError } from "../../../../lib/toastify";
import { PulseLoader } from "../../../generics/BarLoader.jsx"; // importamos loader

const ThresholdCreateForm = ({ grupoId, medallas, tiposKudo, onClose, initialValues = null }) => {
  const [medallaId, setMedallaId] = useState("");
  const [tipoKudoId, setTipoKudoId] = useState("");
  const [cantidadKudos, setCantidadKudos] = useState("");

  useEffect(() => {
    if (initialValues) {
      setMedallaId(initialValues.medallaId);
      setTipoKudoId(initialValues.tipoKudoId);
      setCantidadKudos(initialValues.cantidadKudos);
    }
  }, [initialValues]);

  const { mutate: crearUmbral, isPending: isLoadingCrear } = useCrearUmbralMedalla(() => {
    onClose?.();
  });

  const { mutate: editarUmbral, isPending: isLoadingEditar } = useEditarUmbralMedalla(() => {
    onClose?.();
  });

  const { mutate: eliminarUmbral, isPending: isLoadingEliminar } = useEliminarUmbralMedalla(() => {
    onClose?.();
  });

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!medallaId) return notificarError("Por favor, selecciona una medalla.");
    if (!tipoKudoId) return notificarError("Por favor, selecciona un tipo de kudo.");
    const cantidad = Number(cantidadKudos);
    if (!cantidad || cantidad < 1) return notificarError("Ingresa una cantidad válida de kudos.");

    const medallaSeleccionada = medallas.find((m) => m.id === Number(medallaId));
    if (!medallaSeleccionada) return notificarError("Medalla seleccionada inválida.");

    const tipoKudoSeleccionado = tiposKudo.find((k) => k.id === Number(tipoKudoId));
    if (!tipoKudoSeleccionado) return notificarError("Tipo de kudo seleccionado inválido.");

    const data = {
      id: initialValues?.id || 0,
      medallaId: medallaSeleccionada.id,
      medallaNombre: medallaSeleccionada.nombre,
      rutaIconoMedalla: medallaSeleccionada.rutaIconoMedalla || "",
      tipoKudoId: tipoKudoSeleccionado.id,
      tipoKudoNombre: tipoKudoSeleccionado.nombre,
      cantidadKudos: cantidad,
    };

    if (initialValues && initialValues.id) {
      editarUmbral(data);
    } else {
      const crearData = {
        medallaId: medallaSeleccionada.id,
        tipoKudoId: tipoKudoSeleccionado.id,
        grupoId,
        cantidadKudos: cantidad,
      };
      crearUmbral(crearData);
    }
  };

  const handleDelete = () => {
    if (!initialValues?.id) return;
    if (window.confirm("¿Estás seguro de que deseas eliminar este umbral?")) {
      eliminarUmbral(initialValues.id);
    }
  };

  const isLoading = isLoadingCrear || isLoadingEditar || isLoadingEliminar;

  return (
    <form className={styles.formThreshold} onSubmit={handleSubmit}>
      <label>
        Medalla:
        <select value={medallaId} onChange={(e) => setMedallaId(e.target.value)} required disabled={isLoading}>
          <option value="" disabled>
            Selecciona una medalla
          </option>
          {medallas.map((m) => (
            <option key={m.id} value={m.id}>
              {m.nombre}
            </option>
          ))}
        </select>
      </label>

      <label>
        Tipo de Kudo:
        <select value={tipoKudoId} onChange={(e) => setTipoKudoId(e.target.value)} required disabled={isLoading}>
          <option value="" disabled>
            Selecciona un tipo de kudo
          </option>
          {tiposKudo.map((k) => (
            <option key={k.id} value={k.id}>
              {k.nombre}
            </option>
          ))}
        </select>
      </label>

      <label>
        Cantidad de kudos:
        <input type="number" min={1} value={cantidadKudos} onChange={(e) => setCantidadKudos(e.target.value)} required disabled={isLoading} />
      </label>

      <div className={styles.accionesThreshold}>
        <button type="submit" className="button-secondary" disabled={isLoading}>
          {isLoading ? <PulseLoader /> : initialValues ? "Guardar cambios" : "Crear umbral"}
        </button>

        {initialValues && (
          <button type="button" className="button-tertiary" onClick={handleDelete} disabled={isLoading} title="Eliminar umbral">
            <FontAwesomeIcon icon="fa-solid fa-trash" size="lg" />
          </button>
        )}
      </div>
    </form>
  );
};

ThresholdCreateForm.propTypes = {
  grupoId: PropTypes.number.isRequired,
  medallas: PropTypes.array.isRequired,
  tiposKudo: PropTypes.array.isRequired,
  onClose: PropTypes.func,
  initialValues: PropTypes.object,
};

export default ThresholdCreateForm;
