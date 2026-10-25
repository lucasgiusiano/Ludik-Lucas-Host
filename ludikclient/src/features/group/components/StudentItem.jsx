// components/StudentItem.jsx
import React, { useState } from "react";
import PropTypes from "prop-types";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import genericProfileImage from "../../../assets/genericStudentAvatar2.png";
import styles from "./StudentItem.module.css";

import MedalActionMenu from "./MedalActionMenu.jsx";
import { useAsignarKudo } from "../hooks/useStudentMutation";

const StudentItem = ({ perfilEmisorId, student, medals, kudos, isLoadingKudos, showProfesorOptions, onSelectStudent, notaMax }) => {
  const [selectedKudo, setSelectedKudo] = useState("");
  const [isMedalLoading, setIsMedalLoading] = useState(false);
  const [isKudoLoading, setIsKudoLoading] = useState(false);

  const { mutate: asignarKudo } = useAsignarKudo();

  const tramo = notaMax / 3;

  const handleKudoChange = (e) => {
    const kudoId = Number(e.target.value);
    if (!kudoId) return;

    const kudo = kudos.find((k) => k.id === kudoId);
    if (!kudo) return;

    setIsKudoLoading(true);

    asignarKudo(
      {
        idPerfilEstudianteRecibe: student.id,
        idPerfilEstudianteEmisor: perfilEmisorId,
        kudo,
      },
      {
        onSuccess: () => {
          setSelectedKudo("");
          setIsKudoLoading(false);
        },
        onError: () => setIsKudoLoading(false),
      }
    );
  };

  return (
    <div className={styles.card}>
      <img src={student.enlaceAvatarMiniatura || genericProfileImage} alt="avatar" className={styles.avatar} />
      <div className={styles.centrales}>
        <p className={styles.nombreEstudiante}>{student.nombreEstudiante}</p>
        <div className={styles.actionsContainer}>
          {showProfesorOptions ? (
            <>
              <div className={styles.notaAlumno}>
                <p>Nota Actual</p>
                <span style={{ color: student.calificacionActual > tramo * 2 ? "green" : student.calificacionActual > tramo ? "orange" : "red" }}>{student.calificacionActual}</span>
              </div>
              <div className={styles.asignarMedalla}>
                <div className={styles.menuSection}>
                  <MedalActionMenu items={medals} isAssign={true} isLoading={isMedalLoading} onLoadingChange={setIsMedalLoading} perfilId={student.id} />
                </div>

                <div className={styles.menuSection}>
                  <MedalActionMenu items={student.medallas || []} isAssign={false} isLoading={isMedalLoading} onLoadingChange={setIsMedalLoading} perfilId={student.id} />
                </div>
              </div>
            </>
          ) : (
            <div className={styles.asignarMedalla}>
              <select
                className={`button ${styles.medallas}`}
                name="kudos"
                value={selectedKudo}
                onChange={(e) => {
                  setSelectedKudo(e.target.value);
                  handleKudoChange(e);
                }}
                disabled={isLoadingKudos || isKudoLoading}
              >
                {isLoadingKudos ? (
                  <option value="">Cargando reconocimientos...</option>
                ) : kudos?.length === 0 ? (
                  <option value="">No hay reconocimientos disponibles</option>
                ) : (
                  <>
                    <option value="">Brindar reconocimiento</option>
                    {kudos?.map((kudo) => (
                      <option key={kudo.id} value={kudo.id}>
                        {kudo.nombre}
                      </option>
                    ))}
                  </>
                )}
              </select>
            </div>
          )}
        </div>
      </div>

      <button onClick={() => onSelectStudent(student)} className={styles.showProfileButton}>
        <FontAwesomeIcon icon="fa-solid fa-arrow-right-from-bracket" size="lg" />
      </button>
    </div>
  );
};

StudentItem.propTypes = {
  perfilEmisorId: PropTypes.number.isRequired,
  student: PropTypes.shape({
    id: PropTypes.number.isRequired,
    avatarGrupoId: PropTypes.number.isRequired,
    enlaceAvatarCompleto: PropTypes.string.isRequired,
    enlaceAvatarMiniatura: PropTypes.string.isRequired,
    metaCalificacion: PropTypes.number.isRequired,
    estudianteId: PropTypes.string.isRequired,
    nombreEstudiante: PropTypes.string.isRequired,
    monedas: PropTypes.number.isRequired,
    grupoId: PropTypes.number.isRequired,
    nombreGrupo: PropTypes.string.isRequired,
    calificacionActual: PropTypes.number.isRequired,
    medallas: PropTypes.arrayOf(
      PropTypes.shape({
        id: PropTypes.number.isRequired,
        nombre: PropTypes.string.isRequired,
        urlImagen: PropTypes.string.isRequired,
        descripcion: PropTypes.string.isRequired,
        cantidadMedallasBrinda: PropTypes.number.isRequired,
        esAsignacionMutua: PropTypes.bool.isRequired,
      })
    ).isRequired,
  }).isRequired,
  kudos: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      nombre: PropTypes.string.isRequired,
      enlaceImagenMiniatura: PropTypes.string,
    })
  ).isRequired,
  isLoadingKudos: PropTypes.bool.isRequired,
  medals: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      nombre: PropTypes.string.isRequired,
    })
  ).isRequired,
  showProfesorOptions: PropTypes.bool.isRequired,
  onSelectStudent: PropTypes.func.isRequired,
  notaMax: PropTypes.number.isRequired,
};

export default StudentItem;
