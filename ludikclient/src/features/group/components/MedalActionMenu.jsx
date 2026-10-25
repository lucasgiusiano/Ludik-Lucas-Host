// components/MedalActionMenu.jsx
import React, { useState, useRef, useEffect } from "react";
import { useQueryClient } from "@tanstack/react-query";
import PropTypes from "prop-types";
import styles from "./MedalActionMenu.module.css";
import * as Toast from "../../../lib/toastify.js";
import { PulseLoader } from "../../generics/BarLoader.jsx";

import { useAsignarMedalla, useEliminarMedalla } from "../hooks/useGrupoMutation";

const MedalActionMenu = ({ items, isAssign, onLoadingChange, isLoading: externalLoading = false, perfilId }) => {
  const queryClient = useQueryClient();

  const [expanded, setExpanded] = useState(false);
  const [amounts, setAmounts] = useState({});

  const { mutate: asignar, isPending: loadingAsignar } = useAsignarMedalla();
  const { mutate: eliminar, isPending: loadingEliminar } = useEliminarMedalla();

  const wrapperRef = useRef(null);

  const isLoading = externalLoading || loadingAsignar || loadingEliminar;

  useEffect(() => {
    onLoadingChange && onLoadingChange(isLoading);
  }, [isLoading, onLoadingChange]);

  const handleAmountChange = (id, value, max) => {
    if (value === "") {
      setAmounts((prev) => ({ ...prev, [id]: "" }));
      return;
    }

    const valNum = Number(value);
    if (valNum >= 0 && (isAssign || valNum <= max)) {
      setAmounts((prev) => ({ ...prev, [id]: valNum }));
    }
  };

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (wrapperRef.current && !wrapperRef.current.contains(event.target)) {
        setExpanded(false);
      }
    };
    if (expanded) {
      document.addEventListener("mousedown", handleClickOutside);
    } else {
      document.removeEventListener("mousedown", handleClickOutside);
    }
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [expanded]);

  useEffect(() => {
    if (expanded) {
      const initialAmounts = {};
      items.forEach((m) => {
        initialAmounts[m.id] = "";
      });
      setAmounts(initialAmounts);
    }
  }, [expanded, items]);

  const handleConfirmClick = (medallaId, amount) => {
    if (!amount || amount <= 0) {
      Toast.notificarWarning("Por favor, ingrese una cantidad mayor a cero antes de confirmar.");
      return;
    }

    if (!isAssign) {
      const confirmado = window.confirm(`¿Estás seguro de que deseas eliminar ${amount} medalla(s)? Esta acción no se puede deshacer.`);
      if (!confirmado) return;
    }

    const mutationFn = isAssign ? asignar : eliminar;

    mutationFn(
      { perfilId, medallaId, cantidad: amount },
      {
        onSuccess: () => {
          // ✅ invalidar el query de estudiantes para que se refetchée
          queryClient.invalidateQueries(["students"]);
          setAmounts((prev) => ({ ...prev, [medallaId]: "" }));
        },
      }
    );

    setAmounts((prev) => ({ ...prev, [medallaId]: "" }));
  };

  // Agrupar medallas repetidas para eliminar (si no es asignación)
  const getGroupedItems = () => {
    if (isAssign) return items;

    const countMap = {};
    items.forEach((m) => {
      if (!countMap[m.id]) {
        countMap[m.id] = { ...m, count: 1 };
      } else {
        countMap[m.id].count += 1;
      }
    });

    return Object.values(countMap);
  };

  const groupedItems = getGroupedItems();

  return (
    <div className={styles.menuWrapper} ref={wrapperRef}>
      <button type="button" className={`${styles.menuButton} ${isAssign ? "button" : "button-tertiary"}`} onClick={() => setExpanded((prev) => !prev)} aria-expanded={expanded} aria-haspopup="listbox">
        {
          // ✅ mostrar PulseLoader solo si esta cargando esta medalla
          (isAssign && loadingAsignar) || (!isAssign && loadingEliminar) ? (
            <PulseLoader size={8} /> // ✅ loader dentro del botón
          ) : isAssign ? (
            "Asignar medalla"
          ) : (
            "Eliminar medalla"
          )
        }
      </button>

      {expanded && (
        <div className={styles.menuList} role="listbox" tabIndex={-1} aria-label={isAssign ? "Medallas para asignar" : "Medallas para eliminar"}>
          {groupedItems.length === 0 ? (
            <p className={styles.emptyText}>{isAssign ? "No hay medallas disponibles" : "El estudiante no tiene medallas"}</p>
          ) : (
            groupedItems.map((medalla) => {
              const amount = amounts[medalla.id];
              const cantidadDisponible = !isAssign ? medalla.count : null;
              const isValidAmount = amount !== "" && amount > 0 && (isAssign || amount <= cantidadDisponible);

              return (
                <div key={medalla.id} className={styles.menuItem} role="option" tabIndex={0}>
                  <input
                    type="number"
                    min={0}
                    max={!isAssign ? cantidadDisponible : undefined}
                    value={amount ?? ""}
                    onChange={(e) => handleAmountChange(medalla.id, e.target.value, cantidadDisponible)}
                    className={styles.amountInput}
                    aria-label={`Cantidad de ${medalla.nombre}`}
                    placeholder="0"
                  />
                  <span className={styles.medalName}>
                    {medalla.nombre} {!isAssign && <span className={styles.medalCount}>x{cantidadDisponible}</span>}
                  </span>
                  <button
                    className={styles.confirmButton}
                    onClick={() => handleConfirmClick(medalla.id, Number(amount))}
                    disabled={!isValidAmount}
                    style={{
                      cursor: isValidAmount ? "pointer" : "not-allowed",
                      opacity: isValidAmount ? 1 : 0.5,
                    }}
                  >
                    {isAssign ? "Asignar" : "Eliminar"}
                  </button>
                </div>
              );
            })
          )}
        </div>
      )}
    </div>
  );
};

MedalActionMenu.propTypes = {
  items: PropTypes.array.isRequired,
  isAssign: PropTypes.bool.isRequired,
  onLoadingChange: PropTypes.func,
  isLoading: PropTypes.bool,
  perfilId: PropTypes.number.isRequired,
};

export default MedalActionMenu;
