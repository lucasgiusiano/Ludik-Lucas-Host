// services/storeService.js
import api from "../lib/axios";
import { handleApiError } from "../lib/apiUtils";

export const crearRecompensa = async (reward) => {
  try {
    const response = await api.post("/api/Recompensa/alta", reward);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al crear la recompensa.");
  }
};

export const obtenerRecompensasTienda = async (tiendaId) => {
  try {
    const response = await api.get(`/api/Tienda/${tiendaId}/recompensas`);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al obtener las recompensas de la tienda.");
  }
};

export const canjearRecompensa = async ({ perfilId, recompensaId }) => {
  if (!perfilId || !recompensaId) {
    throw ["ID de perfil o recompensa inválido."];
  }

  try {
    const response = await api.post(`/api/Estudiante/perfiles/${perfilId}/recompensas/${recompensaId}/canjear`);
    if (!response || !response.data) {
      throw ["Respuesta vacía al intentar canjear la recompensa."];
    }

    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudo canjear la recompensa.");
  }
};

export const obtenerRecompensasProfesor = async () => {
  try {
    const response = await api.get("/api/Recompensa/obtener-recompensas-profesor");
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudieron obtener las recompensas del profesor.");
  }
};

export const editarRecompensa = async ({ recompensaId, data }) => {
  try {
    const response = await api.put(`/api/Recompensa/${recompensaId}`, data);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al editar la recompensa.");
  }
};

export const eliminarRecompensa = async (recompensaId) => {
  try {
    const response = await api.delete(`/api/Recompensa/${recompensaId}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al eliminar la recompensa.");
  }
};

export const asignarRecompensaAGrupos = async ({ recompensaId, gruposIds }) => {
  try {
    const response = await api.post("/api/Recompensa/asignar-a-grupos", {
      recompensaId,
      gruposIds,
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al asignar la recompensa a los grupos.");
  }
};
