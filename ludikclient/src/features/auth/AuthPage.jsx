import React, { useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import LoginForm from "./components/LoginForm";
import SignupForm from "./components/SignupForms";
import PasswordRecoveryForm from "./components/PasswordRecoveryForm";
import styles from "./AuthPage.module.css";
import logo from "../../assets/logo.png";

const AuthPage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const path = location.pathname;
  const isLogin = path === "/login";
  const isPasswordRecovery = path === "/passwordRecovery";

  // ✅ Obtenemos la información del usuario desde Redux
  const { token, isAuthenticated } = useSelector((state) => state.userData);

  // ✅ Si ya está autenticado, redirigir a /groups
  useEffect(() => {
    if (token && isAuthenticated) {
      navigate("/groups");
    }
  }, [token, isAuthenticated, navigate]);

  return (
    <main className={styles.container}>
      <div className={styles.cerrarIcono} onClick={() => navigate("/")}>
        <FontAwesomeIcon icon="fa-solid fa-xmark" size="xl" />
      </div>

      <div className={styles.logoArea}>
        <img src={logo} alt="Ludik Logo" className={styles.logoImage} />
      </div>

      <div className={styles.formBox}>{isPasswordRecovery ? <PasswordRecoveryForm /> : isLogin ? <LoginForm /> : <SignupForm />}</div>
    </main>
  );
};

export default AuthPage;
