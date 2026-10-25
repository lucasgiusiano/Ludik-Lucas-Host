import React, { useState } from "react";
import classNames from "classnames";
import styles from "../AuthPage.module.css";
import { useRegistro, usePreguntasSeguridad } from "../hooks/useAuthMutation.js";
import * as Toast from "../../../lib/toastify.js";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PulseLoader } from "../../generics/BarLoader.jsx";

import BarLoader from "../../generics/BarLoader.jsx";

const SignupForm = () => {
  const [verContrasenaProfesor, setVerContrasenaProfesor] = useState(false);
  const [verRepetirContrasenaProfesor, setVerRepetirContrasenaProfesor] = useState(false);
  const [verRepetirContrasenaAlumno, setVerRepetirContrasenaAlumno] = useState(false);
  const [verContrasenaAlumno, setVerContrasenaAlumno] = useState(false);

  const { data: preguntasDisponibles = [], isLoading: cargandoPreguntas } = usePreguntasSeguridad();
  const [isProfesor, setIsProfesor] = useState(true);
  const { mutateAsync: registrar, isPending: cargandoRegistro } = useRegistro();

  const [profesorData, setProfesorData] = useState({
    usuario: "",
    correo: "",
    nombre: "",
    apellido: "",
    contrasena: "",
    repetirContrasena: "",
  });

  const [alumnoData, setAlumnoData] = useState({
    usuario: "",
    nombre: "",
    apellido: "",
    contrasena: "",
    repetirContrasena: "",
  });

  const handleSwitchChange = () => setIsProfesor(!isProfesor);
  const handleChangeProfesor = (e) => setProfesorData({ ...profesorData, [e.target.name]: e.target.value });
  const handleChangeAlumno = (e) => setAlumnoData({ ...alumnoData, [e.target.name]: e.target.value });

  const validarCorreo = (correo) => {
    const emailRegex = /^[a-z0-9._-]+@[a-z0-9.-]+\.[a-z]{2,}$/i;
    if (!emailRegex.test(correo)) Toast.notificarWarning("Correo inválido.");
  };

  const validarContrasena = (c) => {
    const passRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;
    if (!passRegex.test(c)) Toast.notificarWarning("Contraseña insegura.");
  };

  const validarNombreApellido = (valor, campo) => {
    if (valor.length < 3 || valor.length > 20) Toast.notificarWarning(`${campo} debe tener entre 3 y 20 caracteres.`);
  };

  const validarContrasenasCoinciden = (a, b) => {
    if (a !== b) Toast.notificarWarning("Las contraseñas no coinciden.");
  };

  const handleSubmit = async () => {
    try {
      if (!isProfesor) {
        const { usuario, correo, nombre, apellido, contrasena, repetirContrasena } = profesorData;
        if (contrasena !== repetirContrasena) {
          Toast.notificarError("Las contraseñas no coinciden.");
          return;
        }
        const data = { nombreUsuario: usuario, correo, nombre, apellido, contrasenia: contrasena };
        await registrar({ data, tipoUsuario: "profesor" });
      } else {
        const { usuario, nombre, apellido, contrasena, repetirContrasena } = alumnoData;
        if (contrasena !== repetirContrasena) {
          Toast.notificarError("Las contraseñas no coinciden.");
          return;
        }

        const respuestasValidas = preguntasDisponibles
          .map((pregunta, idx) => ({
            preguntaId: pregunta.id,
            respuesta: alumnoData[`respuesta${idx + 1}`]?.trim(),
          }))
          .filter((r) => r.respuesta && r.respuesta.length >= 4);

        if (respuestasValidas.length < 2) {
          Toast.notificarWarning("Debes responder al menos 2 preguntas de seguridad con 4 caracteres o más.");
          return;
        }

        if (respuestasValidas.length > 2) {
          Toast.notificarWarning("Solo se tomarán en cuenta las primeras 2 respuestas válidas.");
        }

        const data = {
          nombreUsuario: usuario,
          nombre,
          apellido,
          contrasenia: contrasena,
          preguntasDeSeguridad: respuestasValidas.slice(0, 2),
        };

        await registrar({ data, tipoUsuario: "alumno" });
      }
    } catch (error) {
      console.error("Error inesperado en el registro:", error);
    }
  };

  const [preguntaIndex, setPreguntaIndex] = useState(0);

  return (
    <>
      <div className={styles.switchContainer}>
        <label className={styles.switch}>
          <input type="checkbox" checked={isProfesor} onChange={handleSwitchChange} className={styles.switchInput} />
          <span className={styles.slider}>
            <span className={classNames(styles.switchLabel, styles.profesor)}>Profesor</span>
            <span className={classNames(styles.switchLabel, styles.alumno)}>Alumno</span>
          </span>
        </label>
      </div>

      <div className={styles.formSlider}>
        <div
          className={styles.formInner}
          style={{
            transform: isProfesor ? "translateX(-50%)" : "translateX(0)",
            height: isProfesor ? (window.innerWidth < 768 ? "575px" : "535px") : "475px",
          }}
        >
          {/* Formulario Profesor */}
          <form className={classNames(styles.formulario, { [styles.oculto]: isProfesor })} onSubmit={(e) => e.preventDefault()}>
            <div className={styles.campo}>
              <label htmlFor="usuario_profesor" className={styles.etiqueta}>
                Nombre de usuario
              </label>
              <input
                type="text"
                id="usuario_profesor"
                name="usuario"
                className={styles.input}
                value={profesorData.usuario}
                onChange={handleChangeProfesor}
                onBlur={() => validarNombreApellido(profesorData.usuario, "Nombre de usuario del profesor")}
              />
            </div>

            <div className={styles.campo}>
              <label htmlFor="correo_profesor" className={styles.etiqueta}>
                Correo electrónico
              </label>
              <input
                type="email"
                id="correo_profesor"
                name="correo"
                className={styles.input}
                value={profesorData.correo}
                onChange={handleChangeProfesor}
                onBlur={() => validarCorreo(profesorData.correo)}
              />
            </div>

            <div className={styles.campo}>
              <label htmlFor="nombre_profesor" className={styles.etiqueta}>
                Nombre
              </label>
              <input
                type="text"
                id="nombre_profesor"
                name="nombre"
                className={styles.input}
                value={profesorData.nombre}
                onChange={handleChangeProfesor}
                onBlur={() => validarNombreApellido(profesorData.nombre, "Nombre del profesor")}
              />
            </div>

            <div className={styles.campo}>
              <label htmlFor="apellido_profesor" className={styles.etiqueta}>
                Apellido
              </label>
              <input
                type="text"
                id="apellido_profesor"
                name="apellido"
                className={styles.input}
                value={profesorData.apellido}
                onChange={handleChangeProfesor}
                onBlur={() => validarNombreApellido(profesorData.apellido, "Apellido del profesor")}
              />
            </div>

            <div className={styles.campo}>
              <label htmlFor="contrasena_profesor" className={styles.etiqueta}>
                Contraseña
              </label>
              <div className={styles.inputWrapper}>
                <input
                  type={verContrasenaProfesor ? "text" : "password"}
                  id="contrasena_profesor"
                  name="contrasena"
                  className={styles.input}
                  value={profesorData.contrasena}
                  onChange={handleChangeProfesor}
                  onBlur={() => validarContrasena(profesorData.contrasena)}
                />
                <button
                  type="button"
                  className={styles.verContrasena}
                  onClick={() => setVerContrasenaProfesor((prev) => !prev)}
                  aria-label={verContrasenaProfesor ? "Ocultar contraseña" : "Mostrar contraseña"}
                >
                  <FontAwesomeIcon icon={verContrasenaProfesor ? "eye-slash" : "eye"} size="lg" />
                </button>
              </div>
            </div>

            <div className={styles.campo}>
              <label htmlFor="repetirContrasena_profesor" className={styles.etiqueta}>
                Repite la contraseña
              </label>
              <div className={styles.inputWrapper}>
                <input
                  type={verRepetirContrasenaProfesor ? "text" : "password"}
                  id="repetirContrasena_profesor"
                  name="repetirContrasena"
                  className={styles.input}
                  value={profesorData.repetirContrasena}
                  onChange={handleChangeProfesor}
                  onBlur={() => validarContrasenasCoinciden(profesorData.contrasena, profesorData.repetirContrasena)}
                />
                <button
                  type="button"
                  className={styles.verContrasena}
                  onClick={() => setVerRepetirContrasenaProfesor((prev) => !prev)}
                  aria-label={verRepetirContrasenaProfesor ? "Ocultar contraseña" : "Mostrar contraseña"}
                >
                  <FontAwesomeIcon icon={verRepetirContrasenaProfesor ? "eye-slash" : "eye"} size="lg" />
                </button>
              </div>
            </div>
          </form>

          {/* Formulario del Alumno */}
          <form className={classNames(styles.formulario, { [styles.oculto]: !isProfesor })} onSubmit={(e) => e.preventDefault()}>
            <div className={styles.campo}>
              <label htmlFor="usuario_alumno" className={styles.etiqueta}>
                Nombre de usuario
              </label>
              <input
                type="text"
                id="usuario_alumno"
                name="usuario"
                className={styles.input}
                value={alumnoData.usuario}
                onChange={handleChangeAlumno}
                onBlur={() => validarNombreApellido(alumnoData.usuario, "Nombre de usuario del alumno")}
              />
            </div>
            <div className={styles.campo}>
              <label htmlFor="nombre_alumno" className={styles.etiqueta}>
                Nombre
              </label>
              <input
                type="text"
                id="nombre_alumno"
                name="nombre"
                className={styles.input}
                value={alumnoData.nombre}
                onChange={handleChangeAlumno}
                onBlur={() => validarNombreApellido(alumnoData.nombre, "Nombre del alumno")}
              />
            </div>
            <div className={styles.campo}>
              <label htmlFor="apellido_alumno" className={styles.etiqueta}>
                Apellido
              </label>
              <input
                type="text"
                id="apellido_alumno"
                name="apellido"
                className={styles.input}
                value={alumnoData.apellido}
                onChange={handleChangeAlumno}
                onBlur={() => validarNombreApellido(alumnoData.apellido, "Apellido del alumno")}
              />
            </div>

            <div className={styles.campo}>
              <label htmlFor="contrasena_alumno" className={styles.etiqueta}>
                Contraseña
              </label>
              <div className={styles.inputWrapper}>
                <input
                  type={verContrasenaAlumno ? "text" : "password"}
                  id="contrasena_alumno"
                  name="contrasena"
                  className={styles.input}
                  value={alumnoData.contrasena}
                  onChange={handleChangeAlumno}
                  onBlur={() => validarContrasena(alumnoData.contrasena)}
                />
                <button
                  type="button"
                  className={styles.verContrasena}
                  onClick={() => setVerContrasenaAlumno((prev) => !prev)}
                  aria-label={verContrasenaAlumno ? "Ocultar contraseña" : "Mostrar contraseña"}
                >
                  <FontAwesomeIcon icon={verContrasenaAlumno ? "eye-slash" : "eye"} size="lg" />
                </button>
              </div>
            </div>
            <div className={styles.campo}>
              <label htmlFor="repetirContrasena_alumno" className={styles.etiqueta}>
                Repite la contraseña
              </label>
              <div className={styles.inputWrapper}>
                <input
                  type={verRepetirContrasenaAlumno ? "text" : "password"}
                  id="repetirContrasena_alumno"
                  name="repetirContrasena"
                  className={styles.input}
                  value={alumnoData.repetirContrasena}
                  onChange={handleChangeAlumno}
                  onBlur={() => validarContrasenasCoinciden(alumnoData.contrasena, alumnoData.repetirContrasena)}
                />
                <button
                  type="button"
                  className={styles.verContrasena}
                  onClick={() => setVerRepetirContrasenaAlumno((prev) => !prev)}
                  aria-label={verRepetirContrasenaAlumno ? "Ocultar contraseña" : "Mostrar contraseña"}
                >
                  <FontAwesomeIcon icon={verRepetirContrasenaAlumno ? "eye-slash" : "eye"} size="lg" />
                </button>
              </div>
            </div>
            {cargandoPreguntas ? (
              <BarLoader />
            ) : (
              preguntasDisponibles.length > 0 && (
                <div className={styles.preguntas}>
                  <p>Responde dos de las preguntas de seguridad</p>

                  <div className={styles.paginacionPreguntas}>
                    {preguntasDisponibles.map((_, idx) => {
                      // Contamos cuántas respuestas ya tienen texto
                      const respuestasContestadas = preguntasDisponibles.map((p, i) => alumnoData[`respuesta${i + 1}`]?.trim()).filter((r) => r && r.length > 0);

                      // Si ya hay dos contestadas, ocultamos todos los puntos de preguntas que aún no fueron contestadas
                      const ocultar = respuestasContestadas.length >= 2 && !alumnoData[`respuesta${idx + 1}`];

                      return !ocultar ? (
                        <label key={idx} className={styles.puntoWrap}>
                          <input type="radio" name="paginacion" checked={preguntaIndex === idx} onChange={() => setPreguntaIndex(idx)} />
                          <span className={styles.punto}></span>
                        </label>
                      ) : null;
                    })}
                  </div>

                  <div className={styles.preguntaActiva}>
                    <label htmlFor={`pregunta-${preguntaIndex}`} className={styles.etiqueta}>
                      {preguntasDisponibles[preguntaIndex]?.pregunta || "Pregunta no disponible"}
                    </label>
                    <input
                      id={`pregunta-${preguntaIndex}`}
                      type="text"
                      className={styles.input}
                      value={alumnoData[`respuesta${preguntaIndex + 1}`] || ""}
                      onChange={(e) =>
                        setAlumnoData((prev) => ({
                          ...prev,
                          [`respuesta${preguntaIndex + 1}`]: e.target.value,
                        }))
                      }
                    />
                  </div>
                </div>
              )
            )}
          </form>
        </div>
      </div>

      <button type="button" onClick={handleSubmit} className={classNames(styles.botonRegistro, "button")} disabled={cargandoRegistro}>
        {cargandoRegistro ? (
          <>
            <PulseLoader />
            <span style={{ marginLeft: "8px" }}>Creando cuenta...</span>
          </>
        ) : (
          "Registrarse"
        )}
      </button>

      <div className={styles.acciones}>
        <a href="/login" className={styles.enlace}>
          Ya tengo una cuenta
        </a>
      </div>
    </>
  );
};

export default SignupForm;
