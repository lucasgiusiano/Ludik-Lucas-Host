import { useMutation, useQueryClient, useQuery } from "@tanstack/react-query";
import * as Toast from "../../../lib/toastify.js";
import { manejarVisualizacionDeErrores } from "../../../lib/apiUtils.js";
import { crearMedalla, obtenerMedallasProfesor, obtenerMedallasAlumno, obtenerMedallaPorId, editarMedalla, eliminarMedalla } from "../../../services/medalService.js";

export const useCrearMedalla = (onSuccessCallback) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: crearMedalla,
    onSuccess: (data) => {
      Toast.notificarExito("Medalla creada exitosamente");
      queryClient.invalidateQueries(["medallas"]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useMedallasProfesor = (isProfesor) => {
  return useQuery({
    queryKey: ["medallas", "profesor"],
    queryFn: obtenerMedallasProfesor,
    enabled: isProfesor,
    onError: manejarVisualizacionDeErrores,
  });
};

export const useMedallasAlumno = (grupoId) => {
  return useQuery({
    queryKey: ["medallas", "grupo", grupoId],
    queryFn: () => obtenerMedallasAlumno(grupoId),
    enabled: !!grupoId, // evita ejecutar si no hay grupoId
    onError: manejarVisualizacionDeErrores,
  });
};

export const useObtenerMedallaPorId = (id) => {
  return useQuery({
    queryKey: ["medalla", id],
    queryFn: () => obtenerMedallaPorId(id),
    enabled: id !== null && id !== undefined,
    onError: manejarVisualizacionDeErrores,
  });
};

export const useEditarMedalla = (onSuccessCallback) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: editarMedalla,
    onSuccess: (data) => {
      Toast.notificarExito("Medalla actualizada correctamente");
      queryClient.invalidateQueries(["medallas"]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useEliminarMedalla = (options) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: eliminarMedalla,
    onSuccess: (data) => {
      Toast.notificarExito("Medalla eliminada correctamente.");
      queryClient.invalidateQueries(["medallas"]);
      if (options?.onSuccess) options.onSuccess(data);
    },
    onError: (error) => {
      manejarVisualizacionDeErrores(error);
      if (options?.onError) options.onError(error);
    },
  });
};
