// RewardAsignationForm.jsx
import React, { useMemo, useState } from "react";
import { useRecompensasProfesor, useAsignarRecompensaAGrupos } from "../../../rewards/hooks/useRewardMutation";
import * as Toast from "../../../../lib/toastify";
import PropTypes from "prop-types";
import styles from "./RewardAsignationForm.module.css";
import BarLoader, { PulseLoader } from "../../../generics/BarLoader.jsx";

/**
 * Recibe:
 * - grupoId (si se quiere asignar a un solo grupo)
 * - gruposProfesor (lista de grupos del profe)
 * - isLoadingGroups (loading de los grupos)
 * - tiendaRecompensas (recompensas ya presentes en la tienda — serán excluidas del select)
 * - onClose (callback cuando termina la asignación)
 */
const RewardAsignationForm = ({ grupoId, gruposProfesor = [], isLoadingGroups = false, tiendaRecompensas = [], onClose }) => {
  const [recompensaId, setRecompensaId] = useState("");
  const [gruposSeleccionados, setGruposSeleccionados] = useState(grupoId ? [grupoId] : []);

  // Recompensas del profesor (query)
  const { data: recompensasProfesor = [], isLoading: isLoadingRecompensas } = useRecompensasProfesor();

  // Mutación para asignar recompensa
  const { mutate: asignarRecompensa, isPending: isLoadingAsignacion } = useAsignarRecompensaAGrupos(() => {
    onClose?.();
  });

  // Conjunto de IDs de recompensas que ya están en la tienda (para comparar rápido)
  const tiendaIds = useMemo(() => new Set((tiendaRecompensas || []).map((r) => Number(r.id))), [tiendaRecompensas]);

  // Recompensas del profesor filtradas: sólo las que NO están en la tienda
  const recompensasDisponibles = useMemo(() => (recompensasProfesor || []).filter((r) => !tiendaIds.has(Number(r.id))), [recompensasProfesor, tiendaIds]);

  const toggleGrupoSeleccionado = (id) => {
    setGruposSeleccionados((prev) => (prev.includes(id) ? prev.filter((gid) => gid !== id) : [...prev, id]));
  };

  const handleAsignar = () => {
    if (!recompensaId) {
      Toast.notificarError("Por favor selecciona una recompensa.");
      return;
    }
    if (!gruposSeleccionados.length) {
      Toast.notificarError("Selecciona al menos un grupo.");
      return;
    }

    asignarRecompensa({
      recompensaId: Number(recompensaId),
      gruposIds: gruposSeleccionados,
    });
  };

  return (
    <div className={styles.containerForm}>
      <label htmlFor="selectRecompensa" className={styles.label}>
        Selecciona una recompensa (solo las no presentes en la tienda):
      </label>

      <select id="selectRecompensa" value={recompensaId} onChange={(e) => setRecompensaId(e.target.value)} disabled={isLoadingRecompensas || isLoadingAsignacion} className={styles.select}>
        {isLoadingRecompensas ? (
          <option value="">Cargando recompensas...</option>
        ) : (
          <>
            <option value="">-- Seleccionar --</option>

            {recompensasDisponibles && recompensasDisponibles.length > 0 ? (
              recompensasDisponibles.map((r) => {
                const nombreIcono = r?.datos?.nombreIcono ?? r?.datos?.nombreIcon ?? "";
                const label = `${r.nombre} — ${r.precio ?? ""} pts ${nombreIcono ? `(${nombreIcono})` : ""}`;
                return (
                  <option key={r.id} value={r.id}>
                    {label}
                  </option>
                );
              })
            ) : (
              <option value="" disabled>
                {recompensasProfesor?.length ? "Todas las recompensas del profesor ya están en la tienda." : "No hay recompensas disponibles."}
              </option>
            )}
          </>
        )}
      </select>

      {/* Si no estamos asignando a un solo grupo, mostramos los checkboxes */}
      {!grupoId && (
        <fieldset className={styles.fieldset}>
          {isLoadingGroups ? (
            <div className={styles.loaderWrapper}>
              <BarLoader />
            </div>
          ) : (
            <>
              <legend>Selecciona los grupos a asignar</legend>
              {(!gruposProfesor || gruposProfesor.length === 0) && <p>No tienes grupos disponibles.</p>}
              {gruposProfesor?.map((grupo) => (
                <div key={grupo.id} className={styles.checkboxContainer}>
                  <label className={styles.groupAsignationOption}>
                    <p>{grupo.nombre}</p>
                    <input type="checkbox" checked={gruposSeleccionados.includes(grupo.id)} onChange={() => toggleGrupoSeleccionado(grupo.id)} aria-checked={gruposSeleccionados.includes(grupo.id)} />
                  </label>
                </div>
              ))}
            </>
          )}
        </fieldset>
      )}

      <button onClick={handleAsignar} disabled={isLoadingAsignacion} className={`button ${styles.assignButton}`} type="button">
        {isLoadingAsignacion ? <PulseLoader /> : "Asignar"}
      </button>
    </div>
  );
};

RewardAsignationForm.propTypes = {
  grupoId: PropTypes.number, // si viene, asignación por defecto a ese grupo
  gruposProfesor: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      nombre: PropTypes.string.isRequired,
    })
  ),
  isLoadingGroups: PropTypes.bool,
  /** Recompensas que ya existen en la tienda (serán excluidas del select) */
  tiendaRecompensas: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      nombre: PropTypes.string.isRequired,
      precio: PropTypes.number,
      tipo: PropTypes.string,
      datos: PropTypes.shape({
        $type: PropTypes.string,
        nombreIcono: PropTypes.string,
      }),
    })
  ).isRequired,
  onClose: PropTypes.func.isRequired,
};

export default RewardAsignationForm;
