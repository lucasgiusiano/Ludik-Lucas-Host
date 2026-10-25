// services/groupService.js

import api from "../lib/axios";
import { handleApiError } from "../lib/apiUtils";

// ─────────────────────────────────────────────
// 🧩 GRUPOS
// ─────────────────────────────────────────────

export const crearGrupo = async (grupo) => {
  try {
    const response = await api.post("/api/grupo/alta", grupo);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al crear el grupo.");
  }
};

export const editarGrupo = async (grupo) => {
  try {
    const response = await api.put(`/api/Grupo/editar/${grupo.id}`, grupo);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al editar el grupo.");
  }
};

export const eliminarGrupo = async (id) => {
  try {
    const response = await api.delete(`/api/Grupo/eliminar/${id}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al eliminar el grupo.");
  }
};

export const reiniciarLogrosGrupo = async (grupoId) => {
  try {
    const response = await api.post(`/api/Profesor/reiniciar-logros`, null, {
      params: { grupoId },
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al reiniciar los logros del grupo.");
  }
};

export const obtenerGrupos = async (rol) => {
  try {
    const endpoint = rol === "Profesor" ? "/api/profesor/mis-grupos" : "/api/estudiante/mis-grupos";
    const response = await api.get(endpoint);
    return response.data;
  } catch (error) {
    handleApiError(error, "No se pudieron obtener los grupos.");
  }
};

export const obtenerGrupo = async (id) => {
  try {
    const response = await api.get(`/api/Grupo/info/${id}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al obtener el grupo.");
  }
};

export const obtenerAlumnosGrupo = async (id) => {
  try {
    const response = await api.get(`/api/Grupo/${id}/perfiles`);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al obtener los alumnos.");
  }
};

export const obtenerAlumnosGrupoParaEstudiante = async (id) => {
  try {
    const response = await api.get(`/api/PerfilEstudiante/grupo/${id}/companeros`);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al obtener los alumnos.");
  }
};

// ─────────────────────────────────────────────
// 📩 SOLICITUDES DE UNIÓN
// ─────────────────────────────────────────────

export const obtenerSolicitudesUnion = async (id) => {
  try {
    const response = await api.get("/api/Profesor/solicitudes-union", {
      params: { grupoId: id },
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al obtener las solicitudes de unión.");
  }
};

export const solicitarUnirseGrupo = async (codigo) => {
  try {
    const response = await api.post("/api/Estudiante/unirse-grupo", null, {
      params: { codigo },
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al solicitar unirte al grupo.");
  }
};

export const aceptarSolicitud = async (solicitudId) => {
  try {
    const response = await api.post("/api/Profesor/aceptar-solicitud", null, {
      params: { solicitudId },
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al aceptar la solicitud.");
  }
};

export const rechazarSolicitud = async (solicitudId) => {
  try {
    const response = await api.post("/api/Profesor/rechazar-solicitud", null, {
      params: { solicitudId },
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al rechazar la solicitud.");
  }
};

// ─────────────────────────────────────────────
// 📩 SOLICITUDES DE MEDALLAS
// ─────────────────────────────────────────────

export const obtenerSolicitudesMedallas = async (grupoId) => {
  try {
    const response = await api.get("/api/Profesor/solicitudes-medallas", {
      params: { grupoId },
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al obtener solicitudes de medallas.");
  }
};

export const aceptarSolicitudMedalla = async (solicitudId) => {
  try {
    const response = await api.post("/api/Profesor/aceptar-solicitud-medalla", null, {
      params: { solicitudId },
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al aceptar la solicitud de medalla.");
  }
};

export const rechazarSolicitudMedalla = async (solicitudId) => {
  try {
    const response = await api.post("/api/Profesor/rechazar-solicitud-medalla", null, {
      params: { solicitudId },
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al rechazar la solicitud de medalla.");
  }
};

// ─────────────────────────────────────────────
// 🏅 MEDALLAS
// ─────────────────────────────────────────────

export const asignarMedalla = async ({ perfilId, medallaId, cantidad }) => {
  try {
    const response = await api.post(`/api/AsignacionMedallas/perfil-estudiante/${perfilId}/medalla/${medallaId}/cantidad/${cantidad}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al asignar medalla.");
  }
};

export const eliminarMedalla = async ({ perfilId, medallaId }) => {
  try {
    const response = await api.delete(`/api/AsignacionMedallas/perfil-estudiante/${perfilId}/medalla/${medallaId}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al eliminar la medalla.");
  }
};

// ─────────────────────────────────────────────
// 🧱 PAC (Proyectos de Aula Colaborativos)
// ─────────────────────────────────────────────

export const crearPac = async ({ grupoId, pacData }) => {
  try {
    const response = await api.post("/api/Profesor/pac", pacData, {
      params: { grupoId },
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al crear el proyecto de aula.");
  }
};

export const obtenerPacsGrupo = async (grupoId) => {
  try {
    const response = await api.get("/api/Profesor/pac", {
      params: { grupoId },
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al obtener los proyectos colaborativos.");
  }
};

// ─────────────────────────────────────────────
// 🧱 UMBRALES PARA MEDALLAS
// ─────────────────────────────────────────────

export const obtenerUmbralesMedallas = async (grupoId) => {
  try {
    const response = await api.get("/api/ConfiguracionUmbralParaMedallas", {
      params: { grupoId },
    });
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al obtener los umbrales de medallas.");
  }
};

export const crearUmbralMedalla = async (umbral) => {
  try {
    const response = await api.post("/api/ConfiguracionUmbralParaMedallas", umbral);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al crear el umbral.");
  }
};

export const editarUmbralMedalla = async (umbral) => {
  try {
    const response = await api.put("/api/ConfiguracionUmbralParaMedallas", umbral);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al editar el umbral.");
  }
};

export const eliminarUmbralMedalla = async (id) => {
  try {
    const response = await api.delete(`/api/ConfiguracionUmbralParaMedallas/${id}`);
    return response.data;
  } catch (error) {
    handleApiError(error, "Error al eliminar el umbral.");
  }
};
