import axios from "axios";
import { url } from "../app/url";
import { store } from "../app/store";
import { logout } from "../features/auth/hooks/userSlice";
import { notificarWarning } from "../lib/toastify"; // ajustá la ruta según corresponda

const api = axios.create({
  baseURL: url,
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use((config) => {
  const state = store.getState();
  const token = state.userData.token;

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      store.dispatch(logout());

      notificarWarning("Sesión expirada, por favor inicia sesión nuevamente.");

      window.location.href = "/login";
    }
    return Promise.reject(error);
  }
);

export default api;
