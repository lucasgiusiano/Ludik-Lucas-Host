import api from "../lib/axios";
import { handleApiError } from "../lib/apiUtils"; // Asegurate de que esta ruta sea correcta

export const obtenerImagenPerfil = async (idPerfilEstudiante) => {
  try {
    const response = await api.get(`/api/Imagen/${idPerfilEstudiante}`);
    return response.data;
  } catch (error) {
    throw handleApiError(error, "No se pudo obtener la imagen del perfil.");
  }
};
