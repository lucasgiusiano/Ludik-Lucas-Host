// src/features/auth/components/PasswordRecoveryForm.jsx
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styles from "../AuthPage.module.css";
import { usePreguntasPorUsuario, useRestablecerContrasena } from "../hooks/useAuthMutation";
import * as Toast from "../../../lib/toastify";

const validarContrasena = (c) => {
  const passRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;
  return passRegex.test(c);
};

const PasswordRecoveryForm = () => {
  const [nombreUsuario, setNombreUsuario] = useState("");
  const [mostrarPreguntas, setMostrarPreguntas] = useState(false);
  const [preguntas, setPreguntas] = useState([]);
  const [respuestas, setRespuestas] = useState({});
  const [nuevaContrasena, setNuevaContrasena] = useState("");
  const [repetirContrasena, setRepetirContrasena] = useState("");

  const navigate = useNavigate();
  const { mutate: buscarPreguntas, isPending } = usePreguntasPorUsuario();
  const { mutate: restablecerContrasena, isPending: restableciendo } = useRestablecerContrasena();

  const handleSubmitUsuario = (e) => {
    e.preventDefault();
    if (nombreUsuario.trim().length < 3) {
      Toast.notificarWarning("El nombre de usuario debe tener al menos 3 caracteres.");
      return;
    }

    buscarPreguntas(nombreUsuario, {
      onSuccess: (res) => {
        setPreguntas(res);
        setMostrarPreguntas(true);
        setRespuestas({});
      },
    });
  };

  const handleSubmitRespuestas = (e) => {
    e.preventDefault();

    if (nuevaContrasena !== repetirContrasena) {
      Toast.notificarWarning("Las contraseñas no coinciden.");
      return;
    }

    if (!validarContrasena(nuevaContrasena)) {
      Toast.notificarWarning("Contraseña insegura.");
      return;
    }

    const payload = {
      nombreUsuario,
      nuevaContrasena,
      respuestas: preguntas.map((p) => ({
        preguntaDeSeguridadId: p.id,
        preguntaRespuestaSeguridadId: p.preguntaRespuestaSeguridadId,
        respuesta: respuestas[p.id] || "",
      })),
    };

    restablecerContrasena(payload);
  };

  const todasRespondidas = preguntas.every((p) => (respuestas[p.id] || "").trim().length > 4);

  return (
    <>
      {/* Botón para volver al login */}
      <button className={styles.botonVolver} onClick={() => navigate("/login")} type="button" aria-label="Volver">
        <FontAwesomeIcon icon="fa-solid fa-chevron-left" />
      </button>

      <form onSubmit={handleSubmitUsuario} className={styles.formularioRecuperacion}>
        <h2>Recuperar Contraseña</h2>
        <div className={styles.campo}>
          <label htmlFor="nombreUsuario" className={styles.etiqueta}>
            Nombre de usuario
          </label>
          <input type="text" id="nombreUsuario" className={styles.input} value={nombreUsuario} onChange={(e) => setNombreUsuario(e.target.value)} />
        </div>
        <button type="submit" className={`button ${styles.botonRegistro}`} disabled={isPending}>
          {isPending ? "Buscando..." : "Buscar preguntas"}
        </button>
      </form>

      {mostrarPreguntas && preguntas.length > 0 && (
        <form onSubmit={handleSubmitRespuestas} className={styles.formularioPreguntas}>
          <h4>Responde tus preguntas de seguridad</h4>
          {preguntas.map((p) => (
            <div className={styles.campo} key={p.id}>
              <label className={styles.etiqueta}>{p.pregunta}</label>
              <input type="text" className={styles.input} value={respuestas[p.id] || ""} onChange={(e) => setRespuestas({ ...respuestas, [p.id]: e.target.value })} />
            </div>
          ))}

          {todasRespondidas && (
            <>
              <div className={styles.campo}>
                <label className={styles.etiqueta}>Nueva contraseña</label>
                <input type="password" className={styles.input} value={nuevaContrasena} onChange={(e) => setNuevaContrasena(e.target.value)} />
              </div>

              <div className={styles.campo}>
                <label className={styles.etiqueta}>Repetir contraseña</label>
                <input type="password" className={styles.input} value={repetirContrasena} onChange={(e) => setRepetirContrasena(e.target.value)} />
              </div>

              <button type="submit" className={`button ${styles.botonRegistro}`} disabled={restableciendo}>
                {restableciendo ? "Enviando..." : "Restablecer contraseña"}
              </button>
            </>
          )}
        </form>
      )}
    </>
  );
};

export default PasswordRecoveryForm;
