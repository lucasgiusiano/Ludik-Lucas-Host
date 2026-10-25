// hooks/useUser.js
import { useQuery } from "@tanstack/react-query";
import { obtenerPerfilUsuario } from "../../../services/userService";
import { manejarVisualizacionDeErrores } from "../../../lib/apiUtils";

export const usePerfilUsuario = () => {
  return useQuery({
    queryKey: ["perfilUsuario"],
    queryFn: obtenerPerfilUsuario,
    onError: manejarVisualizacionDeErrores,
  });
};
