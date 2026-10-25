import api from "../lib/axios";
import { handleApiError } from "../lib/apiUtils";

export const obtenerPerfilUsuario = async () => {
  try {
    const response = await api.get("/api/Usuario/me");

    return response.data;
  } catch (error) {
    handleApiError(error, "Error al obtener datos del usuario.");
  }
};
