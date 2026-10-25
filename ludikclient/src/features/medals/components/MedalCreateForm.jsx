// MedalCreateForm.jsx
import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import styles from "./MedalCreateForm.module.css";
import * as Toast from "../../../lib/toastify.js";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useCrearMedalla, useEditarMedalla, useObtenerMedallaPorId, useEliminarMedalla } from "../hooks/useMedalMutation.js";
import BarLoader, { PulseLoader } from "../../generics/BarLoader.jsx";

const iconOptions = [
  { label: "Estrella", value: "star" },
  { label: "Regalo", value: "gift" },
  { label: "Corazón", value: "heart" },
  { label: "Medalla", value: "medal" },
  { label: "Monedas", value: "coins" },
  { label: "Trofeo", value: "trophy" },
  { label: "Fuego", value: "fire" },
  { label: "Banderín", value: "flag" },
  { label: "Corona", value: "crown" },
  { label: "Caja", value: "box" },
  { label: "Dado", value: "dice" },
  { label: "Varita mágica", value: "wand-magic-sparkles" },
  { label: "Pincel", value: "paint-brush" },
  { label: "Paleta de colores", value: "palette" },
  { label: "Lápiz", value: "pencil" },
  { label: "Pluma", value: "pen-nib" },
  { label: "Bombilla", value: "lightbulb" },
  { label: "Fútbol", value: "futbol" },
  { label: "Baloncesto", value: "basketball" },
  { label: "Vóley", value: "volleyball" },
  { label: "Corriendo", value: "running" },
  { label: "Bicicleta", value: "bicycle" },
  { label: "Libro", value: "book" },
  { label: "Birrete", value: "graduation-cap" },
  { label: "Pizarra", value: "chalkboard" },
  { label: "Cerebro", value: "brain" },
  { label: "Lupa", value: "magnifying-glass" },
  { label: "Cara sonriente", value: "face-smile" },
  { label: "Cara riendo", value: "face-laugh-beam" },
  { label: "Cara con estrellas", value: "face-grin-stars" },
  { label: "Pulgar arriba", value: "thumbs-up" },
  { label: "Aplauso", value: "hands-clapping" },
  { label: "Gema", value: "gem" },
  { label: "Diamante", value: "diamond" },
  { label: "Billete", value: "money-bill" },
  { label: "Billetera", value: "wallet" },
  { label: "Robot", value: "robot" },
  { label: "Cohete", value: "rocket" },
  { label: "Martillo", value: "hammer" },
  { label: "Fantasma", value: "ghost" },
  { label: "Dragón", value: "dragon" },
  { label: "Helado", value: "ice-cream" },
  { label: "Porción de pizza", value: "pizza-slice" },
  { label: "Mapa", value: "map" },
  { label: "Brújula", value: "compass" },
  { label: "Binoculares", value: "binoculars" },
  { label: "Avión", value: "plane" },
  { label: "Pastel", value: "cake-candles" },
];

const extractIconName = (ruta) => {
  if (!ruta) return "";
  const match = ruta.match(/\/([^/]+)\.svg$/);
  return match ? match[1] : ruta;
};

const MedalCreateForm = ({ onClose, medalId }) => {
  const [medalla, setMedalla] = useState({
    nombre: "",
    descripcion: "",
    cantidadMonedasBrinda: "",
    nombreIcono: "",
  });

  const { data, isLoading } = useObtenerMedallaPorId(medalId);

  const crear = useCrearMedalla(() => onClose?.());
  const editar = useEditarMedalla(() => onClose?.());
  const eliminar = useEliminarMedalla(() => onClose?.());

  useEffect(() => {
    if (medalId && data) {
      setMedalla({
        nombre: data.nombre ?? "",
        descripcion: data.descripcion ?? "",
        cantidadMonedasBrinda: String(data.cantidadMedallasBrinda ?? "0"),
        nombreIcono: data.nombreIcono ?? extractIconName(data.urlImagen) ?? "",
      });
    }
  }, [medalId, data]);

  const [showIconPicker, setShowIconPicker] = useState(false);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setMedalla((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleIconSelect = (icon) => {
    setMedalla((prev) => ({ ...prev, nombreIcono: icon }));
    setShowIconPicker(false);
  };

  const validar = () => {
    if (!medalla.nombre || medalla.nombre.trim().length < 3) {
      Toast.notificarWarning("El nombre debe tener al menos 3 caracteres.");
      return false;
    }
    if (!medalla.descripcion || medalla.descripcion.trim().length < 10) {
      Toast.notificarWarning("La descripción debe tener al menos 10 caracteres.");
      return false;
    }
    if (!medalla.nombreIcono) {
      Toast.notificarWarning("Debes seleccionar un ícono.");
      return false;
    }
    if (isNaN(medalla.cantidadMonedasBrinda) || medalla.cantidadMonedasBrinda < 0) {
      Toast.notificarWarning("La cantidad de monedas debe ser un número válido y positivo.");
      return false;
    }
    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validar()) return;

    const payload = {
      nombre: medalla.nombre,
      descripcion: medalla.descripcion,
      cantidadMonedasBrinda: Number(medalla.cantidadMonedasBrinda),
      nombreIcono: medalla.nombreIcono,
    };

    try {
      if (medalId) {
        await editar.mutateAsync({ id: medalId, ...payload });
      } else {
        await crear.mutateAsync(payload);
      }
    } catch (error) {
      console.error(error);
      Toast.notificarError(error?.message ?? "Error al guardar.");
    }
  };

  const handleDelete = () => {
    if (!medalId) return;
    if (window.confirm("¿Estás seguro que deseas eliminar esta medalla? Esta acción no se puede deshacer.")) {
      eliminar.mutate(medalId);
    }
    onClose?.();
  };

  return (
    <form className={styles.modalForm} onSubmit={handleSubmit}>
      {isLoading ? (
        <BarLoader />
      ) : (
        <>
          <label>
            Nombre
            <input
              type="text"
              name="nombre"
              value={medalla.nombre}
              onChange={handleChange}
              placeholder="Ej: Estrella de participación"
              disabled={crear.isPending || editar.isPending || eliminar.isPending}
              required
            />
          </label>

          <label>
            Descripción
            <input
              type="text"
              name="descripcion"
              value={medalla.descripcion}
              onChange={handleChange}
              placeholder="Ej: Se otorga por participar activamente"
              disabled={crear.isPending || editar.isPending || eliminar.isPending}
              required
            />
          </label>

          <label className={styles.iconButtonContainer}>
            Ícono representativo
            <button
              type="button"
              className={`button ${styles.iconSelectButton}`}
              onClick={() => setShowIconPicker((prev) => !prev)}
              disabled={crear.isPending || editar.isPending || eliminar.isPending}
            >
              {medalla.nombreIcono ? <FontAwesomeIcon icon={`fa-solid fa-${medalla.nombreIcono}`} size="xl" /> : "Seleccionar ícono"}
            </button>
            {showIconPicker && (
              <div className={styles.iconGrid}>
                {iconOptions.map((icon) => (
                  <button
                    key={icon.value}
                    type="button"
                    className={`${styles.iconOption} ${medalla.nombreIcono === icon.value ? styles.iconSelected : ""}`}
                    onClick={() => handleIconSelect(icon.value)}
                    disabled={crear.isPending || editar.isPending || eliminar.isPending}
                    aria-pressed={medalla.nombreIcono === icon.value}
                    title={icon.label}
                  >
                    <FontAwesomeIcon icon={`fa-solid fa-${icon.value}`} size="xl" />
                  </button>
                ))}
              </div>
            )}
          </label>

          <label>
            Monedas otorgadas
            <input
              type="number"
              name="cantidadMonedasBrinda"
              value={medalla.cantidadMonedasBrinda}
              onChange={handleChange}
              placeholder="Ej: 50"
              disabled={crear.isPending || editar.isPending || eliminar.isPending}
              required
              min={0}
            />
          </label>

          <div className={styles.botones}>
            <button type="submit" disabled={crear.isPending || editar.isPending} className={`button-secondary ${styles.btnSubmit}`}>
              {crear.isPending ? <PulseLoader /> : editar.isPending ? <PulseLoader /> : medalId ? "Guardar Cambios" : "Crear Medalla"}
            </button>

            {medalId && (
              <button type="button" disabled={eliminar.isPending} onClick={handleDelete} className={`button-tertiary ${styles.btnDelete}`} aria-label={`Eliminar medalla ${medalla.nombre}`}>
                {eliminar.isPending ? <PulseLoader /> : <FontAwesomeIcon icon="fa-solid fa-trash" />}
              </button>
            )}
          </div>
        </>
      )}
    </form>
  );
};

MedalCreateForm.propTypes = {
  onClose: PropTypes.func,
  medalId: PropTypes.number,
};

export default MedalCreateForm;
