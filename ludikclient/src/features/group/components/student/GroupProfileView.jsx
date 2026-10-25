// GroupProfileView.jsx
import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import BarLoader from "../../../generics/BarLoader";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import { notificarExito, notificarWarning } from "../../../../lib/toastify";

import { useRecompensasPerfil, useImagenPerfil, useBarraProgresoPerfil, useDefinirMetaCalificacion, useSolicitarMedalla } from "../../hooks/useStudentMutation";
import { useMedallasAlumno } from "../../../medals/hooks/useMedalMutation";
import RewardItem from "../../../rewards/components/RewardItem";
import MedalCard from "../../../medals/components/MedalCard";
import StudentAvatarEditor from "./StudentAvatarEditor";

import styles from "./GroupProfileView.module.css";
import imagenDefaultPerfil from "../../../../assets/genericStudentAvatar2.png";
import { PulseLoader } from "../../../generics/BarLoader";

const GroupProfileView = ({ perfil, groupid, isLoading, setModalContent, setModalTitle, setShowModal, isOwnProfile }) => {
  const { data: recompensas, isLoading: isLoadingRecompensas } = useRecompensasPerfil(perfil?.id);
  const { data: imagenPerfil, isLoading: isLoadingImagen, isError: isErrorImagen } = useImagenPerfil(perfil?.id);
  const { data: barraProgreso, isLoading: isLoadingBarra } = useBarraProgresoPerfil(perfil?.id);
  const { data: medallas, isLoading: isLoadingMedallas } = useMedallasAlumno(groupid);
  const { mutate: setMeta, isPending: isSettingMeta } = useDefinirMetaCalificacion(perfil?.id);
  const { mutate: solicitarMedallaMutate, isPending: isSolicitando } = useSolicitarMedalla(perfil?.id);

  const [metaTemporal, setMetaTemporal] = useState(perfil.metaCalificacion);
  const [editandoMeta, setEditandoMeta] = useState(false);

  const [medallaSeleccionada, setMedallaSeleccionada] = useState("");
  const [mensajeSolicitud, setMensajeSolicitud] = useState("");

  useEffect(() => {
    setMetaTemporal(perfil.metaCalificacion);
  }, [perfil.metaCalificacion]);

  const avatarUrl = imagenPerfil?.urlCompleta;

  useEffect(() => {
    try {
      if (!perfil?.id || typeof perfil?.metaCalificacion !== "number" || !barraProgreso) return;

      const actual = Number(barraProgreso.calificacionActual ?? -Infinity);
      const meta = Number(perfil.metaCalificacion);
      const key = `metaReached_${perfil.id}_${meta}`;

      if (actual >= meta && !localStorage.getItem(key)) {
        localStorage.setItem(key, "1");

        notificarExito(`¡Felicidades ${perfil.nombreEstudiante}! Has alcanzado tu meta: ${meta}.`);
      }
    } catch (err) {
      console.error(err);
    }
  }, [barraProgreso, barraProgreso?.calificacionActual, perfil?.metaCalificacion, perfil?.id, perfil?.nombreEstudiante, setModalContent, setModalTitle, setShowModal]);

  const handleSolicitarMedalla = () => {
    var error = false;

    if (medallaSeleccionada === "") {
      notificarWarning("Debes seleccionar una medalla");
      error = true;
    }

    if (mensajeSolicitud.length == 0) {
      notificarWarning("El mensaje de solicitud no puede estar vacío");
      error = true;
    }

    if (error) return;

    solicitarMedallaMutate(
      {
        medallaId: Number(medallaSeleccionada),
        descripcion: mensajeSolicitud || "Sin descripción",
      },
      {
        onSuccess: () => {
          setMedallaSeleccionada("");
        },
      }
    );
  };

  return isLoading ? (
    <BarLoader />
  ) : (
    <div className={styles.container}>
      <section>
        <h3>Avatar</h3>
        {isLoadingImagen && !imagenPerfil && !isErrorImagen ? <BarLoader /> : <img src={avatarUrl || imagenDefaultPerfil} alt={`Avatar de ${perfil.nombreEstudiante}`} className={styles.avatar} />}
        {isOwnProfile && (
          <button
            className={styles.editIconContainer}
            onClick={() => {
              setModalContent(<StudentAvatarEditor idPerfil={perfil.id} />);
              setModalTitle("Personalizar avatar");
              setShowModal(true);
            }}
          >
            <FontAwesomeIcon icon="fa fa-pen-to-square" />
          </button>
        )}
      </section>

      <section>
        <h3>Nombre</h3>
        <p>{perfil.nombreEstudiante}</p>
      </section>

      <section>
        <h3>Resumen</h3>
        <div className={styles.resumen}>
          <div>
            <p>
              <FontAwesomeIcon icon="fa fa-coins" /> {perfil.monedas}
            </p>
            <p>Monedas disponibles</p>
          </div>
          <div>
            <p>
              <FontAwesomeIcon icon="fa fa-award" /> {(perfil.medallas || []).reduce((acc, medalla) => acc + (medalla.cantidad || 0), 0)}
            </p>
            <p>Medallas obtenidas</p>
          </div>
          {isOwnProfile && (
            <div className={styles.metaDisplay}>
              {editandoMeta ? (
                // MODO EDICIÓN
                <div className={styles.metaContainer}>
                  <p>¡Elige una nueva meta!</p>
                  <div className={styles.accionesSeleccionMeta}>
                    <input
                      type="number"
                      value={metaTemporal}
                      onChange={(e) => setMetaTemporal(Number(e.target.value))}
                      className={styles.metaInput}
                      min={barraProgreso?.calificacionMinima || 0}
                      max={barraProgreso?.calificacionMaxima || 100}
                    />
                    <button
                      className="button-secondary"
                      onClick={() => {
                        setMeta(metaTemporal);
                        setEditandoMeta(false); // Vuelve a modo vista
                      }}
                      title="Guardar nueva meta"
                    >
                      {isSettingMeta ? <PulseLoader /> : <FontAwesomeIcon icon="fa fa-check" />}
                    </button>
                  </div>
                </div>
              ) : (
                // MODO VISTA
                <div>
                  <p>
                    <FontAwesomeIcon icon="fa fa-bullseye" /> {perfil?.metaCalificacion}
                  </p>
                  <p>Meta personal</p>
                </div>
              )}

              <button onClick={() => setEditandoMeta(!editandoMeta)} className={styles.editIconContainer}>
                <FontAwesomeIcon icon={editandoMeta ? "fa fa-times" : "fa fa-pen-to-square"} />
              </button>
            </div>
          )}
        </div>

        {isOwnProfile && (
          <div className={styles.progressBarManager}>
            <div className={styles.progressBarContainer}>
              <p>Progreso actual</p>
              {!isLoadingBarra && barraProgreso && (
                <div className={styles.progressBar}>
                  {Array.from({ length: barraProgreso?.calificacionMaxima }, (_, index) => {
                    const numero = index + barraProgreso?.calificacionMinima;
                    const alcanzado = numero <= barraProgreso?.calificacionActual;
                    const esMeta = numero === perfil?.metaCalificacion;

                    return (
                      <label key={`progreso-${numero}`} htmlFor={`progreso-${numero}`} className={`${styles.label} ${alcanzado ? styles.alcanzado : styles.noAlcanzado}`}>
                        {esMeta ? <FontAwesomeIcon icon="fa fa-bullseye" className={styles.icono} /> : <p>{index + 1}</p>}
                      </label>
                    );
                  })}
                </div>
              )}
              <hr />
              {/* Medallas necesarias para la siguiente nota */}
              <div className={styles.medallasNecesariasNotaContainer}>
                <p>Medallas para avanzar</p>
                <div className={styles.medallasNecesariasNotaGrid}>
                  {barraProgreso?.medallasNecesariasParaSiguienteNota?.map((medalla) => (
                    <MedalCard key={medalla.medallaId + medalla.nombre} medal={medalla} onEdit={() => {}} showEditOption={false} />
                  ))}
                </div>
              </div>
            </div>
          </div>
        )}
      </section>

      {/* Inventario de Medallas */}
      <section className={styles.inventorySection}>
        <h3>Inventario de Medallas</h3>
        <div className={styles.medallasGrid}>
          {perfil.medallas.length === 0 ? (
            <p>No tienes medallas aún.</p>
          ) : (
            perfil.medallas.map((medalla) => <MedalCard key={medalla.medallaId + medalla.nombre} medal={medalla} cantidad={medalla.cantidad} onEdit={() => {}} showEditOption={false} />)
          )}
        </div>
        <hr />
        <div>
          <h3>Solicitar medalla al profesor</h3>
          <div className={styles.solicitarMedallaContainer}>
            <div className={styles.medallasSeleccionContainer}>
              <select name="medallas" id="medallas" className="button" onChange={(e) => setMedallaSeleccionada(e.target.value)} value={medallaSeleccionada || ""}>
                {isLoadingMedallas ? (
                  <option value="">Cargando medallas...</option>
                ) : (
                  <>
                    <option value="">Selecciona una medalla</option>
                    {medallas?.map((medalla) => (
                      <option key={medalla.id} value={medalla.id}>
                        {medalla.nombre}
                      </option>
                    ))}
                  </>
                )}
              </select>

              <button className="button-secondary" disabled={isLoadingMedallas || medallaSeleccionada === "" || isSolicitando} onClick={handleSolicitarMedalla}>
                {isSolicitando ? <PulseLoader /> : "Solicitar"}
              </button>
            </div>

            {medallaSeleccionada !== "" && (
              <textarea name="mensaje" id="mensaje" value={mensajeSolicitud} onChange={(e) => setMensajeSolicitud(e.target.value)} placeholder="Creo que merezco esta medalla porque..." />
            )}
          </div>
        </div>
      </section>

      {/* Recompensas canjeadas */}
      <section>
        <h3>Recompensas</h3>
        {isLoadingRecompensas ? (
          <BarLoader />
        ) : (
          <div className={styles.recompensasGrid}>
            {recompensas?.length > 0 ? (
              recompensas.map((reward, index) => <RewardItem key={`${reward.id}-${index}`} reward={reward} redeemed={true} />)
            ) : (
              <p className={styles.noRewardsMessage}>No tienes recompensas aún.</p>
            )}
          </div>
        )}
      </section>
    </div>
  );
};

GroupProfileView.propTypes = {
  perfil: PropTypes.shape({
    id: PropTypes.number.isRequired,
    avatarGrupoId: PropTypes.number.isRequired,
    enlaceAvatar: PropTypes.string.isRequired,
    metaCalificacion: PropTypes.number.isRequired,
    estudianteId: PropTypes.string.isRequired,
    nombreEstudiante: PropTypes.string.isRequired,
    monedas: PropTypes.number.isRequired,
    grupoId: PropTypes.number.isRequired,
    nombreGrupo: PropTypes.string.isRequired,
    medallas: PropTypes.arrayOf(
      PropTypes.shape({
        id: PropTypes.number.isRequired,
        nombre: PropTypes.string.isRequired,
        urlImagen: PropTypes.string,
        descripcion: PropTypes.string.isRequired,
        cantidadMedallasBrinda: PropTypes.number.isRequired,
        cantidad: PropTypes.number.isRequired,
        esAsignacionMutua: PropTypes.bool,
      })
    ).isRequired,
  }).isRequired,
  groupid: PropTypes.number.isRequired,
  isLoading: PropTypes.bool.isRequired,
  setModalContent: PropTypes.func.isRequired,
  setModalTitle: PropTypes.func.isRequired,
  setShowModal: PropTypes.func.isRequired,
  isOwnProfile: PropTypes.bool.isRequired,
};

export default GroupProfileView;
