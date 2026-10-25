import api from "../lib/axios";
import { handleApiError } from "../lib/apiUtils";

export const crearTablaEquivalencia = async (equivalencia) => {
  try {
    const response = await api.post("/api/TablaEquivalencia", equivalencia);
    return response.data;
  } catch (error) {
    throw handleApiError(error, "Error al crear la tabla de equivalencia.");
  }
};

export const obtenerTablaEquivalencia = async (id) => {
  try {
    const response = await api.get(`/api/TablaEquivalencia/${id}`);
    return response.data;
  } catch (error) {
    throw handleApiError(error, "No se pudo obtener la tabla de equivalencia.");
  }
};

export const eliminarTablaEquivalencia = async (id) => {
  try {
    const response = await api.delete(`/api/TablaEquivalencia/${id}`);
    return response.data;
  } catch (error) {
    throw handleApiError(error, "Error al eliminar la tabla de equivalencia.");
  }
};

export const actualizarTablaEquivalencia = async (data) => {
  try {
    const response = await api.put(`/api/TablaEquivalencia/`, data);
    return response.data;
  } catch (error) {
    throw handleApiError(error, "Error al actualizar la tabla de equivalencia.");
  }
};

export const obtenerTablasEquivalencia = async () => {
  try {
    const response = await api.get("/api/TablaEquivalencia");
    return response.data;
  } catch (error) {
    throw handleApiError(error, "Error al obtener las tablas de equivalencia.");
  }
};
