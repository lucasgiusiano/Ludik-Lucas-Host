// services/studentService.js
import api from "../lib/axios";
import { handleApiError } from "../lib/apiUtils";

export const obtenerPerfilGrupo = async (grupoId) => {
  try {
    const response = await api.get(`/api/PerfilEstudiante/mi-perfil/grupo/${grupoId}/medallas`);
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudo obtener el perfil del grupo.");
  }
};

export const obtenerRecompensasPerfil = async (perfilId) => {
  try {
    const response = await api.get(`/api/Estudiante/perfiles/${perfilId}/recompensas`);
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudo obtener el inventario de recompensas.");
  }
};

export const obtenerBarraProgresoPerfil = async (perfilId) => {
  try {
    const response = await api.get(`/api/BarraProgreso/${perfilId}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudo obtener la barra de progreso del estudiante.");
  }
};

export const definirMetaCalificacion = async ({ perfilEstudianteId, metaCalificacion }) => {
  try {
    const response = await api.post(`/api/PerfilEstudiante/definir-meta-califiacion`, {
      perfilEstudianteId,
      metaCalificacion,
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudo establecer la meta de calificación.");
  }
};

export const obtenerInventarioAvatar = async (idPerfilEstudiante) => {
  try {
    const response = await api.get(`/api/Avatar/${idPerfilEstudiante}/inventario-avatar`);
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudo obtener el inventario de avatar.");
  }
};

export const generarAvatar = async (idPerfilEstudiante, query) => {
  try {
    const response = await api.get(`/api/Avatar/${idPerfilEstudiante}/generar?${query}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudo generar el avatar.");
  }
};

export const guardarAvatarPersonalizado = async (idPerfilEstudiante, avatarDto, svgBlob) => {
  try {
    const formData = new FormData();
    formData.append("ColorFondo", avatarDto.ColorFondo || "#FFFFFF");
    formData.append("Voltear", avatarDto.Voltear ? "true" : "false");
    formData.append("Rotacion", avatarDto.Rotacion.toString());
    formData.append("Zoom", avatarDto.Zoom.toString());
    avatarDto.AtributosIds.forEach((id) => formData.append("AtributosIds", id.toString()));
    formData.append("imagen", svgBlob, "avatar.svg");

    await api.put(`/api/Avatar/${idPerfilEstudiante}/personalizar`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
  } catch (error) {
    handleApiError(error, "No se pudo guardar el avatar.");
  }
};

export const asignarKudo = async ({ idPerfilEstudianteRecibe, idPerfilEstudianteEmisor, kudo }) => {
  try {
    const response = await api.post("/api/Kudo", {
      idPerfilEstudianteRecibe,
      idPerfilEstudianteEmisor,
      kudo,
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudo asignar el kudo.");
  }
};

export const solicitarMedalla = async ({ perfilEstudianteId, medallaId, descripcion }) => {
  try {
    const response = await api.post(`/api/Estudiante/solicitudes-medalla`, {
      perfilEstudianteId,
      medallaId,
      descripcion,
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudo solicitar la medalla.");
  }
};