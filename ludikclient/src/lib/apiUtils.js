// src/lib/apiUtils.js
import * as Toast from "./toastify";

/**
 * Normaliza un arreglo de errores (strings u objetos) a un arreglo de strings.
 * @param {any[]} arr
 * @param {string} defaultMsg
 * @returns {string[]}
 */
const normalizeErrorArray = (arr, defaultMsg) => {
  if (!Array.isArray(arr) || arr.length === 0) return [defaultMsg];

  const mapped = arr
    .map((it) => {
      if (!it && it !== 0) return null;

      if (typeof it === "string") return it;
      if (typeof it === "number" || typeof it === "boolean") return String(it);

      // objeto: buscar propiedades comunes
      if (typeof it === "object") {
        return (
          it.mensaje ??
          it.Mensaje ??
          it.message ??
          it.Message ??
          it.descripcion ??
          it.Description ??
          (it.codigo ? `${it.codigo} - ${it.mensaje ?? it.Mensaje ?? it.message ?? ""}`.trim() : null) ??
          JSON.stringify(it)
        );
      }

      return String(it);
    })
    .filter(Boolean);

  return mapped.length > 0 ? mapped : [defaultMsg];
};

/**
 * Parsea diferentes formas en que el backend puede devolver errores.
 * Soporta:
 * - Un array de errores [{ Mensaje | mensaje | message }, ...]
 * - Un objeto resultado { Errores: [...] } o { errores: [...] }
 * - Un objeto simple { mensaje: "..." } o { Message: "..." }
 * - Un string
 *
 * @param {any} errores
 * @param {string} defaultMsg
 * @returns {string[]}
 */
export const parseBackendErrors = (errores, defaultMsg = "Ocurrió un error.") => {
  // Si ya es un array (de strings u objetos)
  if (Array.isArray(errores)) {
    return normalizeErrorArray(errores, defaultMsg);
  }

  // Si es objeto que contiene colecciones de errores
  if (errores && typeof errores === "object") {
    // Buscar propiedades comunes en distintas variantes (PascalCase / camelCase / english)
    const candidates = [errores.errores, errores.Errores, errores.errors, errores.Errors, errores.detail, errores.Detalle, errores.detalle, errores.error, errores.errorsList];

    for (const c of candidates) {
      if (Array.isArray(c) && c.length > 0) {
        return normalizeErrorArray(c, defaultMsg);
      }
    }

    // Si vienen directamente como objeto con propiedad mensaje / Mensaje / message
    const message = errores.mensaje ?? errores.Mensaje ?? errores.message ?? errores.Message ?? errores.title ?? errores.Title;
    if (message) return [String(message)];
  }

  // Si es string o número o booleano
  if (typeof errores === "string" || typeof errores === "number" || typeof errores === "boolean") {
    return [String(errores)];
  }

  // fallback
  return [defaultMsg];
};

/**
 * Extrae mensajes desde un objeto AxiosError u otro tipo de error.
 * Intenta obtener response.data y parsearlo.
 *
 * @param {any} error Error capturado (axios error u otro)
 * @param {string} defaultMsg
 * @returns {string[]}
 */
const getMessagesFromError = (error, defaultMsg = "Ocurrió un error.") => {
  // Si ya es un array de mensajes
  if (Array.isArray(error)) {
    return normalizeErrorArray(error, defaultMsg);
  }

  // Si error es un objeto con response.data (axios)
  const responseData = error?.response?.data ?? error?.data ?? null;

  if (responseData) {
    // Si responseData es el Resultado de .NET (tiene Errores/errores)
    // parseBackendErrors es lo suficientemente robusto para manejarlo
    return parseBackendErrors(responseData, defaultMsg);
  }

  // Si el error trae mensaje directo
  if (error?.message) return [String(error.message)];

  // fallback
  return [defaultMsg];
};

/**
 * Función genérica para manejar errores en peticiones API.
 * Lanza un array de mensajes (para que el caller lo capture).
 *
 * @param {any} error Objeto error capturado en try/catch (axios error, string, array, etc.)
 * @param {string} defaultMsg Mensaje por defecto si no se encuentra nada más útil
 * @throws {string[]} Array de mensajes de error
 */
export const handleApiError = (error, defaultMsg = "Ocurrió un error.") => {
  // logs para debugging en desarrollo
  // eslint-disable-next-line no-console
  console.error("handleApiError -> raw error:", error);

  // Si ya es un array de strings -> lo lanzamos tal cual (normalizados)
  if (Array.isArray(error) && error.every((e) => typeof e === "string")) {
    throw error;
  }

  const mensajes = getMessagesFromError(error, defaultMsg);
  // eslint-disable-next-line no-console
  console.debug("handleApiError -> mensajes extraídos:", mensajes);

  throw mensajes;
};

/**
 * Maneja la visualización de errores (por ejemplo en onError de React Query).
 * Acepta:
 *  - Array de strings
 *  - AxiosError u objeto con response.data
 *  - String
 *
 * @param {any} error
 */
export const manejarVisualizacionDeErrores = (error) => {
  // eslint-disable-next-line no-console
  console.debug("manejarVisualizacionDeErrores -> error recibido:", error);

  const mensajes = getMessagesFromError(error, "Error inesperado");

  mensajes.forEach((m) => {
    // Intenta evitar toasts vacíos
    const texto = (m && String(m).trim()) || "Error inesperado";
    Toast.notificarError(texto);
  });
};
