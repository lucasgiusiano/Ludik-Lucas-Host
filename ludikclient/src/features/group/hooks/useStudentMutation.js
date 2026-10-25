// hooks/useStudentMutation.js
import { useMutation, useQueryClient, useQuery } from "@tanstack/react-query";
import * as Toast from "../../../lib/toastify";
import { manejarVisualizacionDeErrores } from "../../../lib/apiUtils";

import {
  obtenerPerfilGrupo,
  obtenerRecompensasPerfil,
  obtenerBarraProgresoPerfil,
  definirMetaCalificacion,
  obtenerInventarioAvatar,
  guardarAvatarPersonalizado,
  asignarKudo,
  solicitarMedalla,
} from "../../../services/studentService";
import { canjearRecompensa } from "../../../services/storeService";
import { obtenerImagenPerfil } from "../../../services/imagesService";

export const usePerfilGrupo = (grupoId, isProfesor, isLoggedIn = true) => {
  return useQuery({
    queryKey: ["perfilGrupo", grupoId],
    queryFn: () => obtenerPerfilGrupo(grupoId),
    enabled: isLoggedIn && !!grupoId && !isProfesor,
    refetchInterval: 300000,
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};

export const useRecompensasPerfil = (perfilId) => {
  return useQuery({
    queryKey: ["recompensasPerfil", perfilId],
    queryFn: () => obtenerRecompensasPerfil(perfilId),
    enabled: !!perfilId,
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};

export const useClaimReward = (perfilId, recompensaId, isProfesor) => {
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: () => canjearRecompensa({ perfilId, recompensaId }),
    onSuccess: () => {
      Toast.notificarExito("¡Recompensa canjeada exitosamente!");
      queryClient.invalidateQueries(["perfilGrupo", perfilId]);
      queryClient.invalidateQueries(["recompensasPerfil", perfilId]);
    },
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });

  if (isProfesor) {
    return { mutate: () => {}, isLoading: false };
  }

  return mutation;
};

export const useImagenPerfil = (perfilId) => {
  return useQuery({
    queryKey: ["imagenPerfil", perfilId],
    queryFn: () => obtenerImagenPerfil(perfilId),
    enabled: !!perfilId,
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
    retry: (failureCount, error) => {
      if (error?.response?.status === 404) return false;
      return failureCount < 1;
    },
  });
};

export const useBarraProgresoPerfil = (perfilId) => {
  return useQuery({
    queryKey: ["barraProgresoPerfil", perfilId],
    queryFn: () => obtenerBarraProgresoPerfil(perfilId),
    enabled: !!perfilId,
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};

export const useDefinirMetaCalificacion = (perfilId, grupoId = null) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (metaCalificacion) => definirMetaCalificacion({ perfilEstudianteId: perfilId, metaCalificacion }),
    onSuccess: (result) => {
      Toast.notificarExito("Meta de calificación actualizada.");

      // actualizo cache de barra para respuesta instantánea si está en el result
      queryClient.setQueryData(["barraProgresoPerfil", perfilId], (old) => {
        if (!old) return old;
        return { ...old, metaCalificacion: result?.metaCalificacion ?? old.metaCalificacion };
      });

      // invalidar perfilGrupo: si tengo grupoId uso exacto, si no invalido por prefijo
      if (grupoId) {
        queryClient.invalidateQueries(["perfilGrupo", grupoId]);
      } else {
        queryClient.invalidateQueries(["perfilGrupo"]);
      }

      // invalidar la barra para forzar refetch si es necesario
      queryClient.invalidateQueries(["barraProgresoPerfil", perfilId]);
    },
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};

export const useInventarioAvatar = (idPerfilEstudiante, enabled = true) => {
  return useQuery({
    queryKey: ["inventario-avatar", idPerfilEstudiante],
    queryFn: () => obtenerInventarioAvatar(idPerfilEstudiante),
    enabled: !!idPerfilEstudiante && enabled,
  });
};

export const useGuardarAvatar = (idPerfilEstudiante) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ avatarDto, jpegBlob }) => guardarAvatarPersonalizado(idPerfilEstudiante, avatarDto, jpegBlob),
    onSuccess: (result) => {
      Toast.notificarExito("¡Avatar guardado correctamente!");

      // 1) Actualiza inmediatamente el cache de la imagen si el result contiene la nueva URL
      // Ajusta `result` según lo que devuelva tu API (ej: result.urlCompleta, result.urlMiniatura, etc.)
      if (result) {
        queryClient.setQueryData(["imagenPerfil", idPerfilEstudiante], result);
      }

      // 2) Fuerza refetch de la imagen (asegura que todo quede sincronizado)
      queryClient.invalidateQueries(["imagenPerfil", idPerfilEstudiante]);

      // 3) Si el inventario de avatar cambia (p. ej. se crea una versión nueva),
      // invalidamos también la query del inventario para que se refresque.
      queryClient.invalidateQueries(["inventario-avatar", idPerfilEstudiante]);
    },
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};

export const useAsignarKudo = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ idPerfilEstudianteRecibe, idPerfilEstudianteEmisor, kudo }) => asignarKudo({ idPerfilEstudianteRecibe, idPerfilEstudianteEmisor, kudo }),
    onSuccess: (_, { idPerfilEstudianteRecibe }) => {
      Toast.notificarExito("¡Kudo asignado exitosamente!");
      queryClient.invalidateQueries(["perfilGrupo", idPerfilEstudianteRecibe]);
    },
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};

export const useSolicitarMedalla = (perfilId) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ medallaId, descripcion }) => solicitarMedalla({ perfilEstudianteId: perfilId, medallaId, descripcion }),
    onSuccess: () => {
      Toast.notificarExito("¡Solicitud de medalla enviada al profesor!");
      queryClient.invalidateQueries(["perfilGrupo", perfilId]);
    },
    onError: (error) => manejarVisualizacionDeErrores(error, Toast.notificarError),
  });
};
