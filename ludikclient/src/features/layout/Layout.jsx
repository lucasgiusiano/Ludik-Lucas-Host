// src/features/layout/Layout.jsx
import React from "react";
import { Outlet } from "react-router-dom";
import styles from "./Layout.module.css";
import HeaderMenu from "./components/HeaderMenu";
import EnhancerView from "./components/EnhancerView.jsx";
import logo from "../../assets/logo.png";
import { useSelector } from "react-redux";
import { selectIsAuthenticated, selectUserRole } from "../auth/hooks/userSlice";

import { useGruposPorRol } from "../group/hooks/useGrupoMutation";
import { usePerfilGrupo } from "../group/hooks/useStudentMutation.js";

function Layout() {
  const isLoggedIn = useSelector(selectIsAuthenticated);
  const role = useSelector(selectUserRole);

  const isProfesor = role === "Profesor";

  const { data: grupos } = useGruposPorRol(role, isLoggedIn);

  const { data: perfil, isLoading: isLoadingPerfil } = usePerfilGrupo(grupos?.[0]?.id, isProfesor, isLoggedIn);

  return (
    <div className={styles.layoutContainer}>
      <header className={styles.header}>
        <div className={styles.logoArea}>
          <img src={logo} alt="Ludik Logo" className={styles.logoImage} style={{ cursor: "pointer" }} />
        </div>

        {isLoggedIn && (
          <>
            <HeaderMenu />

            {!isProfesor && perfil?.multiplicadorPotenciador != null && (
              <EnhancerView enhancerX={perfil.multiplicadorPotenciador} enhancerTime={perfil.tiempoRestantePotenciador} isLoading={isLoadingPerfil} />
            )}
          </>
        )}
      </header>

      <Outlet />
    </div>
  );
}

export default Layout;
