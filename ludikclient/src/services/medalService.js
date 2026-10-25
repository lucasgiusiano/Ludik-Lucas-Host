// services/medallaService.js
import api from "../lib/axios";
import { handleApiError } from "../lib/apiUtils";

export const crearMedalla = async (medalla) => {
  try {
    const response = await api.post("/api/medalla/alta", medalla);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al crear la medalla.");
  }
};

export const obtenerMedallasProfesor = async () => {
  try {
    const response = await api.get("/api/Medalla");
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudieron obtener las medallas del profesor.");
  }
};

export const obtenerMedallasAlumno = async (grupoId) => {
  try {
    const response = await api.get(`/api/Medalla/grupo/${grupoId}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudieron obtener las medallas del grupo.");
    throw error; 
  }
};

export const obtenerMedallaPorId = async (id) => {
  try {
    const response = await api.get(`/api/Medalla/${id}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudo obtener la medalla.");
  }
};

export const editarMedalla = async ({ id, ...data }) => {
  try {
    const response = await api.put(`/api/medalla/${id}`, data);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al editar la medalla.");
  }
};

export const eliminarMedalla = async (id) => {
  try {
    const response = await api.delete(`/api/Medalla/${id}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al eliminar la medalla.");
  }
};

export const obtenerTiposKudo = async () => {
  try {
    const response = await api.get("/api/Kudo");
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudieron obtener los tipos de kudo.");
  }
};
