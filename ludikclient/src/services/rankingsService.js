// services/rankingsService.js
import api from "../lib/axios";
import { handleApiError } from "../lib/apiUtils";

export const obtenerRankings = async (grupoId) => {
  try {
    const response = await api.get(`/api/TablaClasificacion/grupo/${grupoId}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al obtener los rankings.");
  }
};

export const crearRanking = async (grupoId, data) => {
  try {
    const response = await api.post("/api/TablaClasificacion", data, {
      params: { grupoId },
      headers: { "Content-Type": "application/json" },
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al crear el ranking.");
  }
};

export const eliminarRanking = async (id) => {
  try {
    await api.delete(`/api/TablaClasificacion/${id}`);
  } catch (error) {
    handleApiError(error, "Error al eliminar el ranking.");
  }
};

export const obtenerRankingPorId = async (id) => {
  try {
    const response = await api.get(`/api/TablaClasificacion/${id}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al obtener el detalle del ranking.");
  }
};
