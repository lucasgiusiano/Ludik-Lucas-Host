// components/LoginForm.js
import React, { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { useDispatch } from "react-redux";
import { useLogin } from "../hooks/useAuthMutation.js";
import { logoutReset } from "../hooks/userSlice.js";
import * as Toast from "../../../lib/toastify.js";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PulseLoader } from "../../generics/BarLoader.jsx";
import styles from "../AuthPage.module.css";

const LoginForm = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const message = location.state?.message;
  const path = location.pathname;

  const dispatch = useDispatch();

  const { mutateAsync: login, isPending } = useLogin();

  const [usuario, setUsuario] = useState("");
  const [contrasena, setContrasena] = useState("");
  const [verContrasena, setVerContrasena] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    if (name === "usuario") setUsuario(value);
    if (name === "contrasena") setContrasena(value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await login({ usuario, contrasena });
    } catch (error) {
      console.warn("Error: ", error.message);
    }
  };

  useEffect(() => {
    if (message) {
      Toast.notificarError(message);
    }
  }, [message]);

  useEffect(() => {
    dispatch(logoutReset());
  }, []);

  return (
    <form className={styles.formulario} onSubmit={handleSubmit}>
      {path == "/" && (
        <>
          <h2>¡Comenza ahora!</h2>
          <hr />
        </>
      )}

      {/* campos de usuario y contraseña */}
      <div className={styles.campo}>
        <label htmlFor="usuario" className={styles.etiqueta}>
          Usuario
        </label>
        <input type="text" id="usuario" name="usuario" className={styles.input} value={usuario} onChange={handleChange} />
      </div>
      <div className={styles.campo}>
        <label htmlFor="contrasena" className={styles.etiqueta}>
          Contraseña
        </label>
        <div className={styles.inputWrapper}>
          <input type={verContrasena ? "text" : "password"} id="contrasena" name="contrasena" className={styles.input} value={contrasena} onChange={handleChange} />
          <button type="button" onClick={() => setVerContrasena((prev) => !prev)} className={styles.verContrasena} aria-label={verContrasena ? "Ocultar contraseña" : "Mostrar contraseña"}>
            <FontAwesomeIcon icon={verContrasena ? "eye-slash" : "eye"} size="lg" />
          </button>
        </div>
      </div>

      <button type="submit" className={`button ${styles.botonIniciar}`} disabled={isPending}>
        {isPending ? <PulseLoader /> : "Iniciar sesión"}
      </button>

      <div className={`${styles.acciones} ${styles.accionesLogin}`}>
        <a href="/passwordRecovery" className={styles.enlace}>
          ¿Olvidaste tu contraseña?
        </a>
        <hr />
        <a
          onClick={(e) => {
            e.preventDefault();
            navigate("/signup");
          }}
          className={`button-secondary ${styles.nuevaCuenta}`}
          href="/signup"
        >
          Crear nueva cuenta
        </a>
      </div>
    </form>
  );
};

export default LoginForm;
