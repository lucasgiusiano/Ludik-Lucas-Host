// hooks/useGrupo.js
import { useMutation, useQueryClient, useQuery } from "@tanstack/react-query";
import * as Toast from "../../../lib/toastify";
import { manejarVisualizacionDeErrores } from "../../../lib/apiUtils";

// === Servicios ===
import {
  crearGrupo,
  editarGrupo,
  eliminarGrupo,
  reiniciarLogrosGrupo,
  obtenerGrupos,
  obtenerGrupo,
  obtenerAlumnosGrupo,
  obtenerAlumnosGrupoParaEstudiante,
  obtenerSolicitudesUnion,
  eliminarMedalla,
  asignarMedalla,
  solicitarUnirseGrupo,
  aceptarSolicitud,
  rechazarSolicitud,
  crearPac,
  obtenerPacsGrupo,
  obtenerUmbralesMedallas,
  crearUmbralMedalla,
  editarUmbralMedalla,
  eliminarUmbralMedalla,
  obtenerSolicitudesMedallas,
  aceptarSolicitudMedalla,
  rechazarSolicitudMedalla,
} from "../../../services/groupService";
import { obtenerTiposKudo } from "../../../services/medalService";
import { obtenerRecompensasTienda } from "../../../services/storeService";
import { obtenerRankings, crearRanking, eliminarRanking, obtenerRankingPorId } from "../../../services/rankingsService";
import { useNavigate } from "react-router-dom";

// ─────────────────────────────────────────────
// 🔄 PREFETCH
// ─────────────────────────────────────────────

export const usePrefetchGrupo = () => {
  const queryClient = useQueryClient();

  const prefetchGrupo = (id) => {
    if (!id) return;

    queryClient.prefetchQuery({
      queryKey: ["grupo", id], // ✅ array obligatorio en v4
      queryFn: () => obtenerGrupo(id),
      staleTime: 1000 * 60 * 5, // 5 minutos
    });
  };

  const invalidateGrupo = (id) => {
    if (!id) return;

    queryClient.invalidateQueries({
      queryKey: ["grupo", id], // ✅ array obligatorio en v4
    });
  };

  return { prefetchGrupo, invalidateGrupo };
};
// ─────────────────────────────────────────────
// 🧩 GRUPOS
// ─────────────────────────────────────────────

export const useCrearGrupo = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: crearGrupo,
    onSuccess: (data) => {
      Toast.notificarExito("Grupo creado exitosamente.");
      queryClient.invalidateQueries(["grupos"]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useEditarGrupo = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: editarGrupo,
    onSuccess: (data) => {
      Toast.notificarExito("Grupo editado correctamente.");
      queryClient.invalidateQueries(["grupo", data.id]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useEliminarGrupo = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  const navigate = useNavigate(); // 👈 correcto

  return useMutation({
    mutationFn: eliminarGrupo,
    onSuccess: (_, id) => {
      navigate("/groups"); // 👈 usar navigate, no Navigate
      Toast.notificarExito("Grupo eliminado correctamente.");
      queryClient.invalidateQueries(["grupos"]);
      if (onSuccessCallback) onSuccessCallback(id);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useReiniciarLogrosGrupo = (onSuccessCallback) => {
  return useMutation({
    mutationFn: reiniciarLogrosGrupo,
    onSuccess: () => {
      Toast.notificarExito("Logros reiniciados correctamente.");
      if (onSuccessCallback) onSuccessCallback();
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useGruposPorRol = (rol, isLoggedIn) => {
  return useQuery({
    queryKey: ["grupos", rol],
    queryFn: () => obtenerGrupos(rol),
    enabled: isLoggedIn && !!rol,
    onError: manejarVisualizacionDeErrores,
  });
};

export const useGrupo = (id) => {
  return useQuery({
    queryKey: ["grupo", id],
    queryFn: () => obtenerGrupo(id),
    enabled: !!id,
    onError: manejarVisualizacionDeErrores,
  });
};

export const useAlumnosGrupo = (id, isProfesor) => {
  return useQuery({
    queryKey: ["alumnos", id],
    queryFn: isProfesor ? () => obtenerAlumnosGrupo(id) : () => obtenerAlumnosGrupoParaEstudiante(id),
    onError: manejarVisualizacionDeErrores,
    retry: (failureCount, error) => {
      if (error?.response?.status === 400) return false;
      return failureCount < 1;
    },
  });
};

// ─────────────────────────────────────────────
// 📩 SOLICITUDES DE UNIÓN
// ─────────────────────────────────────────────

export const useSolicitudesUnion = (id) => {
  return useQuery({
    queryKey: ["solicitudesUnion", id],
    queryFn: () => obtenerSolicitudesUnion(id),
    enabled: !!id,
    onError: manejarVisualizacionDeErrores,
    staleTime: 0,
    cacheTime: 0,
    refetchOnMount: true,
    refetchOnWindowFocus: true,
    refetchInterval: 5000,
  });
};

export const useSolicitarUnirseGrupo = (onSuccessCallback) => {
  return useMutation({
    mutationFn: solicitarUnirseGrupo,
    onSuccess: (data) => {
      Toast.notificarExito("Solicitud de unión creada.");
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useAceptarSolicitud = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: aceptarSolicitud,
    onSuccess: (data) => {
      Toast.notificarExito("Solicitud aceptada.");
      queryClient.invalidateQueries(["solicitudesUnion"]);
      queryClient.invalidateQueries(["alumnos"]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useRechazarSolicitud = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: rechazarSolicitud,
    onSuccess: (data) => {
      Toast.notificarExito("Solicitud rechazada.");
      queryClient.invalidateQueries(["solicitudesUnion"]);
      queryClient.invalidateQueries(["alumnos"]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

// ─────────────────────────────────────────────
// 📩 SOLICITUDES DE MEDALLAS
// ─────────────────────────────────────────────

export const useSolicitudesMedallas = (grupoId) => {
  return useQuery({
    queryKey: ["solicitudesMedallas", grupoId],
    queryFn: () => obtenerSolicitudesMedallas(grupoId),
    enabled: !!grupoId, // solo hace la llamada si hay grupoId
    onError: manejarVisualizacionDeErrores,
  });
};

export const useAceptarSolicitudMedalla = (onSuccessCallback) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: aceptarSolicitudMedalla,
    onSuccess: () => {
      Toast.notificarExito("Solicitud de medalla aceptada.");
      queryClient.invalidateQueries(["solicitudesMedallas"]);
      if (onSuccessCallback) onSuccessCallback();
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useRechazarSolicitudMedalla = (onSuccessCallback) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: rechazarSolicitudMedalla,
    onSuccess: () => {
      Toast.notificarExito("Solicitud de medalla rechazada.");
      queryClient.invalidateQueries(["solicitudesMedallas"]);
      if (onSuccessCallback) onSuccessCallback();
    },
    onError: manejarVisualizacionDeErrores,
  });
};
// ─────────────────────────────────────────────
// 🏅 MEDALLAS
// ─────────────────────────────────────────────

export const useAsignarMedalla = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: asignarMedalla,
    onSuccess: (data) => {
      Toast.notificarExito("Medalla asignada exitosamente.");
      queryClient.invalidateQueries(["alumnos"]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useEliminarMedalla = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: eliminarMedalla,
    onSuccess: (data) => {
      Toast.notificarExito("Medalla eliminada exitosamente.");
      queryClient.invalidateQueries(["alumnos"]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

// ─────────────────────────────────────────────
// 🛍️ TIENDA
// ─────────────────────────────────────────────

export const useRecompensasTienda = (tiendaId) => {
  return useQuery({
    queryKey: ["recompensas", tiendaId],
    queryFn: () => obtenerRecompensasTienda(tiendaId),
    enabled: !!tiendaId,
    onError: manejarVisualizacionDeErrores,
  });
};

// ─────────────────────────────────────────────
// 📊 RANKINGS
// ─────────────────────────────────────────────

export const useRankings = (grupoId) => {
  return useQuery({
    queryKey: ["rankings", grupoId], // mejor práctica: usar grupoId en la key
    queryFn: () => obtenerRankings(grupoId), // ← corrección aquí
    onError: manejarVisualizacionDeErrores,
    enabled: !!grupoId, // evita llamada si grupoId es falsy
  });
};

export const useCrearRanking = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ grupoId, ranking }) => crearRanking(grupoId, ranking),
    onSuccess: (data) => {
      Toast.notificarExito("Ranking creado correctamente.");
      queryClient.invalidateQueries(["rankings"]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useEliminarRanking = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id) => eliminarRanking(id),
    onSuccess: () => {
      Toast.notificarExito("Ranking eliminado correctamente.");
      queryClient.invalidateQueries(["rankings"]);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useRankingPorId = (id) => {
  return useQuery({
    queryKey: ["ranking", id],
    queryFn: () => obtenerRankingPorId(id),
    enabled: !!id,
    onError: manejarVisualizacionDeErrores,
  });
};

// ─────────────────────────────────────────────
// 🧱 PAC (Proyectos de Aula Colaborativos)
// ─────────────────────────────────────────────

export const useCrearPac = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: crearPac,
    onSuccess: (data) => {
      Toast.notificarExito("PAC creado correctamente.");
      queryClient.invalidateQueries(["pacs"]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const usePacsGrupo = (grupoId) => {
  return useQuery({
    queryKey: ["pacs", grupoId],
    queryFn: () => obtenerPacsGrupo(grupoId),
    enabled: !!grupoId,
    onError: manejarVisualizacionDeErrores,
  });
};

// ─────────────────────────────────────────────
// 🧱 Umbrales para las medallas obtenidas por kudos
// ─────────────────────────────────────────────

export const useUmbralesMedallas = (grupoId) => {
  return useQuery({
    queryKey: ["umbralesMedallas", grupoId],
    queryFn: () => obtenerUmbralesMedallas(grupoId),
    enabled: !!grupoId,
    onError: manejarVisualizacionDeErrores,
  });
};

export const useCrearUmbralMedalla = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: crearUmbralMedalla,
    onSuccess: (data) => {
      Toast.notificarExito("Umbral creado correctamente.");
      queryClient.invalidateQueries(["umbralesMedallas", data.grupoId]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useEditarUmbralMedalla = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: editarUmbralMedalla,
    onSuccess: (data) => {
      Toast.notificarExito("Umbral editado correctamente.");
      queryClient.invalidateQueries(["umbralesMedallas", data.grupoId]);
      if (onSuccessCallback) onSuccessCallback(data);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

export const useEliminarUmbralMedalla = (onSuccessCallback) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: eliminarUmbralMedalla,
    onSuccess: (data, id) => {
      Toast.notificarExito("Umbral eliminado correctamente.");
      queryClient.invalidateQueries(["umbralesMedallas"]);
      if (onSuccessCallback) onSuccessCallback(id);
    },
    onError: manejarVisualizacionDeErrores,
  });
};

// ─────────────────────────────────────────────
// 🧱 Kudos
// ─────────────────────────────────────────────

export const useTiposKudo = () => {
  return useQuery({
    queryKey: ["tiposKudo"],
    queryFn: obtenerTiposKudo,
    enabled: true,
    onError: manejarVisualizacionDeErrores,
  });
};
