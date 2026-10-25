import { useNavigate } from "react-router-dom";
import { useMutation, useQuery } from "@tanstack/react-query";
import { iniciarSesion, registrarse, obtenerPreguntasSeguridad, obtenerPreguntasPorUsuario, restablecerContrasena } from "../../../services/authService";
import { useDispatch } from "react-redux";
import * as Toast from "../../../lib/toastify";
import { manejarVisualizacionDeErrores } from "../../../lib/apiUtils";

export const useLogin = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  return useMutation({
    mutationFn: (credenciales) => iniciarSesion(credenciales, dispatch),
    onSuccess: (user) => {
      navigate("/groups");
      Toast.notificarExito(`¡Bienvenid@ ${user.nombreUsuario}!`);
    },
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};

export const useRegistro = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  return useMutation({
    mutationFn: ({ data, tipoUsuario }) => registrarse(data, tipoUsuario, dispatch),
    onSuccess: (user) => {
      navigate("/groups");
      Toast.notificarExito("Registro exitoso");
      Toast.notificarExito(`¡Bienvenid@ ${user.nombreUsuario}!`);
    },
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};

export const usePreguntasSeguridad = () => {
  return useQuery({
    queryKey: ["preguntasSeguridad"],
    queryFn: obtenerPreguntasSeguridad,
    staleTime: 1000 * 60 * 10, // 10 minutos
    // No hay error handler porque quizás quieras un fallback visual aparte
  });
};

export const usePreguntasPorUsuario = () => {
  return useMutation({
    mutationFn: obtenerPreguntasPorUsuario,
    onSuccess: (preguntas) => {
      if (!preguntas || preguntas.length === 0) {
        Toast.notificarWarning("Este usuario no tiene preguntas de seguridad registradas.");
      }
    },
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};

export const useRestablecerContrasena = () => {
  const navigate = useNavigate();
  return useMutation({
    mutationFn: restablecerContrasena,
    onSuccess: () => {
      Toast.notificarExito("Contraseña restablecida con éxito. Inicia sesión.");
      navigate("/login");
    },
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};
