// src/services/auth.js
import api from "../lib/axios";
import { loginSuccess, logout } from "../features/auth/hooks/userSlice";
import { persistor } from "../app/store";
import { handleApiError } from "../lib/apiUtils"; // 👈 importar handleApiError

export const iniciarSesion = async (credenciales, dispatch) => {
  try {
    const response = await api.post("/api/Login/login", {
      NombreUsuario: credenciales.usuario,
      Contrasenia: credenciales.contrasena,
    });

    const data = response.data;
    dispatch(loginSuccess(data));
    return data;
  } catch (error) {
    // handleApiError lanza un array de mensajes, convertimos a Error con primer mensaje
    try {
      handleApiError(error, "Credenciales inválidas o error al iniciar sesión.");
    } catch (mensajes) {
      throw new Error(mensajes[0]);
    }
  }
};

export const registrarse = async (data, tipoUsuario, dispatch) => {
  const endpoint = tipoUsuario === "profesor" ? "/api/profesor/alta" : "/api/estudiante/alta";

  try {
    const response = await api.post(endpoint, data);
    dispatch(loginSuccess(response.data));
    return response.data;
  } catch (error) {
    try {
      handleApiError(error, "Error desconocido al registrar.");
    } catch (mensajes) {
      throw new Error(mensajes[0]);
    }
  }
};

export const cerrarSesion = (dispatch) => {
  dispatch(logout());
  persistor.purge();
};

export const obtenerPreguntasSeguridad = async () => {
  const response = await api.get("/api/RecuperarContrasena/preguntas");
  return response.data.preguntas;
};

export const obtenerPreguntasPorUsuario = async (nombreUsuario) => {
  try {
    const response = await api.get(`/api/RecuperarContrasena/preguntas/${nombreUsuario}`);
    return response.data.preguntas;
  } catch (error) {
    try {
      handleApiError(error, "Error desconocido al obtener preguntas de seguridad.");
    } catch (mensajes) {
      throw new Error(mensajes[0]);
    }
  }
};

export const restablecerContrasena = async (payload) => {
  try {
    await api.post("/api/RecuperarContrasena/restablecer", payload);
  } catch (error) {
    try {
      handleApiError(error, "Error desconocido al restablecer la contraseña.");
    } catch (mensajes) {
      throw new Error(mensajes[0]);
    }
  }
};
