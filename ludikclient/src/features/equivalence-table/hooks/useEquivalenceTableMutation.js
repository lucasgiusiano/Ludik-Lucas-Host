import { useMutation, useQueryClient, useQuery } from "@tanstack/react-query";
import * as Toast from "../../../lib/toastify";
import { manejarVisualizacionDeErrores } from "../../../lib/apiUtils";

import { crearTablaEquivalencia, obtenerTablaEquivalencia, eliminarTablaEquivalencia, actualizarTablaEquivalencia, obtenerTablasEquivalencia } from "../../../services/equivalenceTableService";

export const useCrearTablaEquivalencia = (onSuccessCallback) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: crearTablaEquivalencia,
    onSuccess: (data) => {
      Toast.notificarExito("Tabla de equivalencia creada.");
      queryClient.invalidateQueries(["tablasEquivalencia"]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};

export const useEditarTablaEquivalencia = (onSuccessCallback) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ data }) => actualizarTablaEquivalencia(data),
    onSuccess: (data) => {
      Toast.notificarExito("Tabla de equivalencia actualizada.");
      queryClient.invalidateQueries(["tablasEquivalencia"]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};

export const useEliminarTablaEquivalencia = (options) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: eliminarTablaEquivalencia,
    onSuccess: (data) => {
      Toast.notificarExito("Tabla de equivalencia eliminada.");
      queryClient.invalidateQueries(["tablasEquivalencia"]);
      if (options?.onSuccess) options.onSuccess(data);
    },
    onError: (error) => {
      manejarVisualizacionDeErrores(error, Toast.notificarError);
      if (options?.onError) options.onError(error);
    },
  });
};

export const useObtenerTablaEquivalencia = (id) => {
  return useQuery({
    queryKey: ["tablaEquivalencia", id],
    queryFn: () => obtenerTablaEquivalencia(id),
    enabled: !!id,
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};

export const useTablasEquivalencia = () => {
  return useQuery({
    queryKey: ["tablasEquivalencia"],
    queryFn: obtenerTablasEquivalencia,
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};
