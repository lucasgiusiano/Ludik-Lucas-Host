import { toast } from "react-toastify";

// Función que retorna el estilo común para todos los toasts, calculando maxWidth dinámicamente
const obtenerEstiloToast = () => ({
  boxShadow: "0 3px 0 3px var(--blanco-secundario)",
  borderRadius: "10px",
  width: "100%",
  maxWidth: window.innerWidth < 768 ? "90%" : "600px",
  marginTop: "20px",
  whiteSpace: "normal", // permite saltos de línea
  overflow: "visible", // muestra todo el contenido
  textOverflow: "unset", // no usa puntos suspensivos
});

// Detectar si es móvil o escritorio para la posición
const obtenerPosicionToast = () => (window.innerWidth < 768 ? "top-center" : "top-right");

// Funciones reutilizables
export const notificarExito = (msg) =>
  toast.success(msg, {
    position: obtenerPosicionToast(),
    style: obtenerEstiloToast(),
  });

export const notificarError = (msg) =>
  toast.error(msg, {
    position: obtenerPosicionToast(),
    style: obtenerEstiloToast(),
  });

export const notificarInfo = (msg) =>
  toast.info(msg, {
    position: obtenerPosicionToast(),
    style: obtenerEstiloToast(),
  });

export const notificarWarning = (msg) =>
  toast.warning(msg, {
    position: obtenerPosicionToast(),
    style: obtenerEstiloToast(),
  });
