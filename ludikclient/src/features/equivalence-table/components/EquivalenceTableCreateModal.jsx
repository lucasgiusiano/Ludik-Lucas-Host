import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import styles from "./EquivalenceTableCreateModal.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useMedallasProfesor } from "../../medals/hooks/useMedalMutation";
import { useCrearTablaEquivalencia, useEditarTablaEquivalencia } from "../hooks/useEquivalenceTableMutation";
import { PulseLoader } from "../../generics/BarLoader";
/*
  Estrategia:
  - medallasNecesariasRaw: array de instancias {id,nombre,nombreIcono,esHeredada}
    (una entrada por cada medalla, permiten duplicados).
  - medallasNecesariasUI: agrupado para mostrar (id,nombre,nombreIcono,cantidad).
  - syncInheritanceFrom(startIndex): recalcula la parte heredada para todas las equivalencias
    posteriores a startIndex (usando el raw de la previa como base) y preserva las medallas no heredadas.
*/

const EquivalenceTableCreateModal = ({ onClose, onSave, table }) => {
  const [equivalencia, setEquivalencia] = useState({ nombre: "", equivalencias: [] });

  const { mutateAsync: crearTablaEquivalencia, isPending: isCreating } = useCrearTablaEquivalencia();
  const { mutateAsync: editarTablaEquivalencia, isPending: isEditing } = useEditarTablaEquivalencia();
  const { data: medallas, isLoading } = useMedallasProfesor();

  const groupRawToUI = (raw = []) => {
    const map = new Map();
    for (const r of raw) {
      const key = String(r.id); // normalizamos la clave
      if (!map.has(key)) {
        map.set(key, {
          id: r.id,
          nombre: r.nombre,
          nombreIcono: r.nombreIcono || "",
          cantidad: 1,
        });
      } else {
        map.get(key).cantidad += 1;
      }
    }
    return Array.from(map.values());
  };

  useEffect(() => {
    if (!medallas) return;

    if (table && table.equivalencias) {
      const built = table.equivalencias.map((eq) => {
        const raw = (eq.medallasNecesarias || []).flatMap((m) =>
          Array.from({ length: m.cantidad || 1 }, () => ({
            id: m.id,
            nombre: m.nombre,
            nombreIcono: m.nombreIcono || "",
            esHeredada: false,
          }))
        );
        return {
          nota: eq.nota,
          medallasNecesariasRaw: raw,
          medallasNecesariasUI: groupRawToUI(raw),
        };
      });
      setEquivalencia({ nombre: table.nombre || "", equivalencias: built });
    } else if (medallas.length > 0) {
      const med = medallas[Math.floor(Math.random() * medallas.length)];
      const raw = [{ id: med.id, nombre: med.nombre, nombreIcono: med.nombreIcono || "", esHeredada: false }];
      setEquivalencia({
        nombre: "",
        equivalencias: [
          {
            nota: 1,
            medallasNecesariasRaw: raw,
            medallasNecesariasUI: groupRawToUI(raw),
          },
        ],
      });
    }
  }, [table, medallas]);

  // Recalcula la herencia de todas las equivalencias a partir de startIndex (aplica para j = startIndex+1 ...)
  const syncInheritanceFrom = (startIndex, equivalencias) => {
    const newEquivalencias = equivalencias.map((eq) => ({ ...eq, medallasNecesariasRaw: [...eq.medallasNecesariasRaw] }));
    for (let j = Math.max(1, startIndex + 1); j < newEquivalencias.length; j++) {
      const prevRaw = newEquivalencias[j - 1].medallasNecesariasRaw;
      // desired inherited instances = copia del raw de prev, marcadas como heredadas
      const desiredInherited = prevRaw.map((r) => ({ ...r, esHeredada: true }));
      // extras: todas las instancias actuales en j que NO sean heredadas (lo que añadió el usuario)
      const extras = newEquivalencias[j].medallasNecesariasRaw.filter((r) => !r.esHeredada);
      newEquivalencias[j].medallasNecesariasRaw = [...desiredInherited, ...extras];
      newEquivalencias[j].medallasNecesariasUI = groupRawToUI(newEquivalencias[j].medallasNecesariasRaw);
    }
    return newEquivalencias;
  };

  // Añadir equivalencia (hereda exactamente las instancias raw de la última)
  const handleAddEquivalencia = () => {
    setEquivalencia((prev) => {
      const ultimaNota = prev.equivalencias.length > 0 ? prev.equivalencias.at(-1).nota : 0;
      const ultima = prev.equivalencias.at(-1);
      const heredadasRaw = ultima ? ultima.medallasNecesariasRaw.map((r) => ({ ...r, esHeredada: true })) : [];
      const nuevas = [...prev.equivalencias, { nota: ultimaNota + 1, medallasNecesariasRaw: heredadasRaw, medallasNecesariasUI: groupRawToUI(heredadasRaw) }];
      return { ...prev, equivalencias: nuevas };
    });
  };

  // Añadir medalla a equIndex
  // Después sincronizamos herencia desde equIndex (para recalcular correctamente posteriores)
  const handleAddMedalla = (equIndex, medallaId) => {
    if (!medallaId || !medallas) return;
    const med = medallas.find((m) => Number(m.id) === Number(medallaId));
    if (!med) return;

    setEquivalencia((prev) => {
      const copy = prev.equivalencias.map((eq) => ({
        ...eq,
        medallasNecesariasRaw: [...eq.medallasNecesariasRaw],
      }));

      copy[equIndex].medallasNecesariasRaw.push({
        id: Number(med.id),
        nombre: med.nombre,
        nombreIcono: med.nombreIcono || "",
        esHeredada: false,
      });

      copy[equIndex].medallasNecesariasUI = groupRawToUI(copy[equIndex].medallasNecesariasRaw);

      const synced = syncInheritanceFrom(equIndex, copy);
      return { ...prev, equivalencias: synced };
    });
  };

  // Comprueba si se puede eliminar: cantidad actual > cantidad base (prev.raw count) y existe instancia !esHeredada
  const canDelete = (equivalenciasState, equIndex, medId) => {
    if (!equivalenciasState[equIndex]) return false;
    const currRaw = equivalenciasState[equIndex].medallasNecesariasRaw;
    const cantidadActual = currRaw.filter((r) => r.id === medId).length;
    const prevRaw = equivalenciasState[equIndex - 1]?.medallasNecesariasRaw ?? [];
    const cantidadBase = prevRaw.filter((r) => r.id === medId).length;
    const existeNoHeredada = currRaw.some((r) => r.id === medId && !r.esHeredada);
    return cantidadActual > cantidadBase && existeNoHeredada;
  };

  // Eliminar una instancia no heredada de equIndex, y luego recalcular herencia hacia adelante
  const handleRemoveMedalla = (equIndex, medallaId) => {
    setEquivalencia((prev) => {
      if (!prev.equivalencias[equIndex]) return prev;
      if (!canDelete(prev.equivalencias, equIndex, medallaId)) return prev;

      const copy = prev.equivalencias.map((eq) => ({ ...eq, medallasNecesariasRaw: [...eq.medallasNecesariasRaw] }));
      const idxToRemove = copy[equIndex].medallasNecesariasRaw.findIndex((r) => r.id === medallaId && !r.esHeredada);
      if (idxToRemove === -1) return prev;
      copy[equIndex].medallasNecesariasRaw.splice(idxToRemove, 1);
      copy[equIndex].medallasNecesariasUI = groupRawToUI(copy[equIndex].medallasNecesariasRaw);

      // Ahora sincronizamos herencia desde equIndex (porque la base cambió)
      const synced = syncInheritanceFrom(equIndex, copy);
      return { ...prev, equivalencias: synced };
    });
  };

  const handleRemoveEquivalencia = (index) => {
    setEquivalencia((prev) => {
      const nuevas = [...prev.equivalencias];
      if (index < 0 || index >= nuevas.length) return prev;
      nuevas.splice(index, 1);
      // Recalcular herencia a partir de index-1
      const synced = syncInheritanceFrom(index - 1, nuevas);
      return { ...prev, equivalencias: synced };
    });
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setEquivalencia((prev) => ({ ...prev, [name]: value }));
  };

  const handleEquivalenciaChange = (index, field, value) => {
    setEquivalencia((prev) => {
      const copy = prev.equivalencias.map((eq) => ({ ...eq }));
      if (field === "nota") copy[index].nota = Number(value);
      return { ...prev, equivalencias: copy };
    });
  };

  const handleSave = async (e) => {
    e.preventDefault();
    try {
      const idTabla = table?.id ?? 0;
      const equivalenciaToSave = {
        id: idTabla,
        nombre: equivalencia.nombre,
        equivalencias: equivalencia.equivalencias.map((eq) => ({
          nota: eq.nota,
          // se envían tantas entradas como instancias raw (duplicados permitidos)
          medallasNecesarias: eq.medallasNecesariasRaw.map((r) => ({
            id: r.id,
            nombre: r.nombre,
            nombreIcono: r.nombreIcono || "",
            descripcion: "",
          })),
        })),
      };

      if (table && table.id) await editarTablaEquivalencia({ data: equivalenciaToSave });
      else await crearTablaEquivalencia(equivalenciaToSave);

      if (onSave) onSave(equivalenciaToSave);
      onClose();
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <form className={styles.modalForm} onSubmit={handleSave}>
      <label className={styles.nombreTabla}>
        Nombre de la tabla de equivalencia
        <input type="text" name="nombre" value={equivalencia.nombre} onChange={handleChange} placeholder="Ej: Criterios de evaluación" required />
      </label>

      <div className={(styles.equivalencias, styles.nombreTabla)}>
        <h4>Equivalencias</h4>

        {equivalencia.equivalencias.map((eq, i) => (
          <div key={i} className={styles.equivalencia}>
            <button type="button" className={styles.eliminarEquivalencia} onClick={() => handleRemoveEquivalencia(i)} title="Eliminar equivalencia">
              <FontAwesomeIcon icon="fa-solid fa-xmark" />
            </button>

            <div className={styles.nota}>
              <label>Nota</label>
              <input type="number" min="1" value={eq.nota} onChange={(e) => handleEquivalenciaChange(i, "nota", e.target.value)} required />
            </div>

            <div className={styles.medallasNecesarias}>
              <label>Medallas necesarias</label>

              <select
                className={styles.selectMedalla}
                disabled={isLoading}
                onChange={(e) => {
                  const val = Number(e.target.value);
                  if (val) handleAddMedalla(i, val);
                  e.target.value = "";
                }}
              >
                <option value="">Seleccione una medalla</option>
                {medallas?.map((m) => (
                  <option key={m.id} value={m.id}>
                    {m.nombre}
                  </option>
                ))}
              </select>

              <div className={styles.medallasSeleccionadas}>
                {(eq.medallasNecesariasUI || []).map((ui) => {
                  const can = canDelete(equivalencia.equivalencias, i, ui.id);
                  return (
                    <div key={ui.id} className={styles.medalla}>
                      <h4>{ui.nombre}</h4>
                      <div className={styles.medallaImagenWrapper}>
                        <FontAwesomeIcon icon={`fa-solid fa-${ui.nombreIcono}`} />
                        {ui.cantidad > 1 && <span className={styles.medallaCantidad}>{ui.cantidad}</span>}
                      </div>

                      {can && (
                        <button type="button" className={`button-tertiary ${styles.eliminarMedalla}`} onClick={() => handleRemoveMedalla(i, ui.id)} title="Eliminar medalla">
                          <FontAwesomeIcon icon="fa-solid fa-trash" />
                        </button>
                      )}
                    </div>
                  );
                })}
              </div>
            </div>
          </div>
        ))}
      </div>

      <div className={styles.acciones}>
        <button type="button" className="button" onClick={handleAddEquivalencia} disabled={isLoading || !medallas || medallas.length === 0}>
          Agregar Equivalencia
        </button>
        <button type="submit" className="button-secondary">
          {isCreating || isEditing ? <PulseLoader /> : table ? "Guardar cambios" : "Crear tabla"}
        </button>
      </div>
    </form>
  );
};

EquivalenceTableCreateModal.propTypes = {
  onClose: PropTypes.func.isRequired,
  onSave: PropTypes.func,
  table: PropTypes.shape({
    id: PropTypes.number,
    nombre: PropTypes.string.isRequired,
    equivalencias: PropTypes.arrayOf(
      PropTypes.shape({
        nota: PropTypes.number.isRequired,
        medallasNecesarias: PropTypes.arrayOf(
          PropTypes.shape({
            id: PropTypes.number.isRequired,
            nombre: PropTypes.string.isRequired,
            nombreIcono: PropTypes.string,
          })
        ).isRequired,
      })
    ).isRequired,
  }),
};

export default EquivalenceTableCreateModal;
