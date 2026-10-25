import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import styles from "./RewardCreateForm.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PulseLoader } from "../../generics/BarLoader.jsx";
import { useCrearRecompensa, useEditarRecompensa, useEliminarRecompensa } from "../hooks/useRewardMutation";
import * as Toast from "../../../lib/toastify.js";

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

const RewardCreateForm = ({ reward, onClose }) => {
  const [recompensa, setRecompensa] = useState({
    id: 0,
    nombre: "",
    nombreIcono: "",
    precio: 0,
  });

  const [showIconPicker, setShowIconPicker] = useState(false);

  useEffect(() => {
    if (reward) {
      setRecompensa({
        id: reward.id || 0,
        nombre: reward.nombre || "",
        nombreIcono: reward.representacion?.nombreIcono || "",
        precio: reward.precio || 0,
      });
    } else {
      setRecompensa({ id: 0, nombre: "", nombreIcono: "", precio: 0 });
    }
  }, [reward]);

  const { mutateAsync: crearRecompensa, isPending: isCreating } = useCrearRecompensa(() => {
    setRecompensa({ id: 0, nombre: "", nombreIcono: "", precio: 0 });
    onClose?.();
  });
  const { mutateAsync: editarRecompensa, isPending: isEditing } = useEditarRecompensa(() => {
    setRecompensa({ id: 0, nombre: "", nombreIcono: "", precio: 0 });
    onClose?.();
  });

  const { mutateAsync: eliminarRecompensa, isPending: isDeleting } = useEliminarRecompensa(() => {
    setRecompensa({ id: 0, nombre: "", nombreIcono: "", precio: 0 });
    onClose?.();
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setRecompensa((prev) => ({
      ...prev,
      [name]: name === "precio" ? parseInt(value) || 0 : value,
    }));
  };

  const handleIconSelect = (value) => {
    setRecompensa((prev) => ({ ...prev, nombreIcono: value }));
    setShowIconPicker(false);
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    // Validar que el nombre no esté vacío
    if (!recompensa.nombre.trim()) {
      Toast.notificarWarning("El nombre de la recompensa no puede estar vacío.");
      return;
    }

    // Validar longitud del nombre entre 3 y 50 caracteres
    if (recompensa.nombre.trim().length < 3 || recompensa.nombre.trim().length > 50) {
      Toast.notificarWarning("El nombre de la recompensa debe tener entre 3 y 50 caracteres.");
      return;
    }

    // Validar que se haya seleccionado un ícono
    if (!recompensa.nombreIcono) {
      Toast.notificarWarning("Debes seleccionar un ícono representativo.");
      return;
    }

    // Validar que el precio no sea negativo
    if (recompensa.precio < 0) {
      Toast.notificarWarning("El precio no puede ser negativo.");
      return;
    }

    // Validar que el precio esté entre 10 y 10,000
    if (recompensa.precio < 10 || recompensa.precio > 10000) {
      Toast.notificarWarning("La cantidad de monedas necesarias debe estar entre 10 y 10,000.");
      return;
    }

    const payload = {
      nombre: recompensa.nombre,
      nombreIcono: recompensa.nombreIcono,
      precio: recompensa.precio,
    };

    if (recompensa.id && recompensa.id !== 0) {
      // Editar
      editarRecompensa({ recompensaId: recompensa.id, data: payload });
    } else {
      // Crear
      crearRecompensa(payload);
    }
  };

  const handleDelete = () => {
    if (!recompensa.id) return;
    if (window.confirm("¿Estás seguro que deseas eliminar esta recompensa?")) {
      eliminarRecompensa(recompensa.id);
    }
  };
  return (
    <form className={styles.modalForm} onSubmit={handleSubmit}>
      <label>
        Nombre de la recompensa
        <input type="text" name="nombre" placeholder="Ej: Alfajor" value={recompensa.nombre} onChange={handleChange} required />
      </label>

      <label className={styles.iconButtonContainer}>
        Ícono representativo
        <button type="button" className={`button ${styles.iconSelectButton}`} onClick={() => setShowIconPicker((prev) => !prev)}>
          {recompensa.nombreIcono ? <FontAwesomeIcon icon={`fa-solid fa-${recompensa.nombreIcono}`} size="xl" /> : "Seleccionar ícono"}
        </button>
        {showIconPicker && (
          <div className={styles.iconGrid}>
            {iconOptions.map((icon) => (
              <button
                key={icon.value}
                type="button"
                className={`${styles.iconOption} ${recompensa.nombreIcono === icon.value ? styles.iconSelected : ""}`}
                onClick={() => handleIconSelect(icon.value)}
              >
                <FontAwesomeIcon icon={`fa-solid fa-${icon.value}`} size="xl" />
              </button>
            ))}
          </div>
        )}
      </label>

      <label>
        Precio (en monedas)
        <input type="number" name="precio" placeholder="Ej: 150" value={recompensa.precio} onChange={handleChange} min={0} required />
      </label>

      <div className={styles.acciones}>
        <button type="submit" className={`${styles.btnSubmit} button-secondary`} disabled={isCreating || isEditing}>
          {isCreating || isEditing ? <PulseLoader /> : recompensa.id ? "Guardar cambios" : "Crear recompensa"}
        </button>

        {recompensa.id !== 0 && (
          <button type="button" className={`${styles.btnDelete} button-tertiary`} onClick={handleDelete} disabled={isDeleting} aria-label={`Eliminar recompensa ${recompensa.nombre}`}>
            {isDeleting ? <PulseLoader /> : <FontAwesomeIcon icon="fa-solid fa-trash" />}
          </button>
        )}
      </div>
    </form>
  );
};

RewardCreateForm.propTypes = {
  reward: PropTypes.shape({
    id: PropTypes.number,
    nombre: PropTypes.string,
    precio: PropTypes.number,
    representacion: PropTypes.shape({
      $type: PropTypes.string,
      nombreIcono: PropTypes.string,
    }),
    tipo: PropTypes.string,
  }),
  onClose: PropTypes.func,
};

export default RewardCreateForm;
