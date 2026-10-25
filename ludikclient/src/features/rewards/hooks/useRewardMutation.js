// src/features/reward/hooks/useRewardMutation.js
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { manejarVisualizacionDeErrores } from "../../../lib/apiUtils";

import { obtenerRecompensasProfesor, crearRecompensa, editarRecompensa, eliminarRecompensa, asignarRecompensaAGrupos } from "../../../services/storeService";
import * as Toast from "../../../lib/toastify.js";

export const useRecompensasProfesor = () => {
  return useQuery({
    queryKey: ["recompensasProfesor"],
    queryFn: obtenerRecompensasProfesor,
    enabled: true,
    onError: manejarVisualizacionDeErrores,
  });
};

export const useCrearRecompensa = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: crearRecompensa,
    onSuccess: (data) => {
      console.log(data);
      Toast.notificarExito("Recompensa creada correctamente.");
      queryClient.invalidateQueries(["recompensas"]); // Ajusta la key según cómo cargues recompensas
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useEditarRecompensa = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ recompensaId, data }) => editarRecompensa({ recompensaId, data }),
    onSuccess: (data) => {
      Toast.notificarExito("Recompensa editada correctamente.");
      queryClient.invalidateQueries(["recompensasProfesor"]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useEliminarRecompensa = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (recompensaId) => eliminarRecompensa(recompensaId),
    onSuccess: () => {
      Toast.notificarExito("Recompensa eliminada correctamente.");
      queryClient.invalidateQueries(["recompensasProfesor"]);
      if (onSuccessCallback) onSuccessCallback();
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useAsignarRecompensaAGrupos = (onSuccess) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: asignarRecompensaAGrupos,
    onSuccess: (data) => {
      Toast.notificarExito("Recompensa asignada correctamente.");
      queryClient.invalidateQueries(["recompensas"]);
      if (onSuccess) onSuccess(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};
