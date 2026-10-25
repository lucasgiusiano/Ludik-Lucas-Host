// GroupPage.jsx
import React, { useState, useEffect } from "react";
import style from "./GroupPage.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import BarLoader, { PulseLoader } from "../generics/BarLoader.jsx";
import { useSelector } from "react-redux";
import { selectUserRole } from "../auth/hooks/userSlice";
import BaseManagerPage from "../generics/BaseManagerPage";
import StudentItem from "./components/StudentItem";
import GroupProfileView from "./components/student/GroupProfileView.jsx";
import GroupConfigView from "./components/configs/GroupConfigView.jsx";
import GroupPacView from "./components/pac/GroupPacView.jsx";
import MedalThresholdView from "./components/medalThreshold/MedalThresholdView.jsx";
import StoreGroupView from "./components/store/StoreGroupView";
import { useGrupo, useAlumnosGrupo, useRecompensasTienda, useTiposKudo } from "./hooks/useGrupoMutation";
import { usePerfilGrupo } from "./hooks/useStudentMutation.js";
import { useMedallasProfesor } from "../medals/hooks/useMedalMutation";
import { useParams } from "react-router-dom";
import RequestView from "./components/requests/RequestView.jsx";
import GroupRankingView from "./components/rankings/GroupRankingView.jsx";

const tabLabels = {
  perfil: "Perfil",
  alumnos: "Alumnos",
  tienda: "Tienda",
  rankings: "Rankings",
  pac: "Desafío",
  threshold: "Umbral",
  solicitudes: "Solicitudes",
  configs: "Configuración",
};

const GroupPage = () => {
  const { id } = useParams();
  const groupId = Number(id);

  const role = useSelector(selectUserRole);
  const isProfesor = role === "Profesor";

  const [search, setSearch] = useState("");
  const [showModal, setShowModal] = useState(false);
  const [modalContent, setModalContent] = useState(null);
  const [modalTitle, setModalTitle] = useState("");
  const [selectedView, setSelectedView] = useState("alumnos");
  const [perfilSeleccionado, setPerfilSeleccionado] = useState(null);
  const [perfil, setPerfil] = useState(null);

  const { data: group, isLoading: isLoadingGroup } = useGrupo(groupId);
  const { data: students, isLoading: isLoadingStudents } = useAlumnosGrupo(groupId, isProfesor);
  const { data: medals, isLoading: isLoadingMedals } = useMedallasProfesor(isProfesor);
  const { data: tiposKudo, isLoading: isLoadingKudos } = useTiposKudo();
  const { data: recompensas, isLoading: isLoadingRecompensas } = useRecompensasTienda(group?.idTienda);
  const { data: perfilHook, isLoadingPerfil } = usePerfilGrupo(groupId, isProfesor);

  const userProfileId = perfil?.id;

  const studentsFiltrados = students?.filter((item) => item.nombreEstudiante.toLowerCase().includes(search.toLowerCase()));

  const getLabelClass = (view) => {
    const base = selectedView === view ? style.activeLabel : "";
    const specific = selectedView === view ? style[`active${view.charAt(0).toUpperCase() + view.slice(1)}`] : "";
    return `${style.actionLabel} ${base} ${specific}`.trim();
  };

  useEffect(() => {
    if (!isProfesor && perfilHook) {
      setPerfil(perfilHook);
      setPerfilSeleccionado(perfilHook);
    } else {
      setPerfil(null);
    }
  }, [perfilHook, isProfesor]);

  const actions = (
    <div className={style.acciones}>
      {!isProfesor && (
        <label className={getLabelClass("perfil")} aria-label={tabLabels.perfil}>
          <input type="radio" value="perfil" checked={selectedView === "perfil"} onChange={() => setSelectedView("perfil")} />
          <FontAwesomeIcon icon="fa-solid fa-user" size="xl" />
          <span className={style.actionText}>{tabLabels.perfil}</span>
        </label>
      )}

      <label className={getLabelClass("alumnos")} aria-label={tabLabels.alumnos}>
        <input type="radio" value="alumnos" checked={selectedView === "alumnos"} onChange={() => setSelectedView("alumnos")} />
        <FontAwesomeIcon icon="fa-solid fa-people-group" size="xl" />
        <span className={style.actionText}>{tabLabels.alumnos}</span>
      </label>

      <label className={getLabelClass("tienda")} aria-label={tabLabels.tienda}>
        <input type="radio" value="tienda" checked={selectedView === "tienda"} onChange={() => setSelectedView("tienda")} />
        <FontAwesomeIcon icon="fa-solid fa-store" size="xl" />
        <span className={style.actionText}>{tabLabels.tienda}</span>
      </label>

      <label className={getLabelClass("rankings")} aria-label={tabLabels.rankings}>
        <input type="radio" value="rankings" checked={selectedView === "rankings"} onChange={() => setSelectedView("rankings")} />
        <FontAwesomeIcon icon="fa-solid fa-ranking-star" size="xl" />
        <span className={style.actionText}>{tabLabels.rankings}</span>
      </label>

      <label className={getLabelClass("pac")} aria-label={tabLabels.pac}>
        <input type="radio" value="pac" checked={selectedView === "pac"} onChange={() => setSelectedView("pac")} />
        <FontAwesomeIcon icon="fa-solid fa-handshake" size="xl" />
        <span className={style.actionText}>{tabLabels.pac}</span>
      </label>

      <label className={getLabelClass("threshold")} aria-label={tabLabels.threshold}>
        <input type="radio" value="threshold" checked={selectedView === "threshold"} onChange={() => setSelectedView("threshold")} />
        <FontAwesomeIcon icon="fa-solid fa-chart-bar" size="xl" />
        <span className={style.actionText}>{tabLabels.threshold}</span>
      </label>

      {isProfesor && (
        <>
          <label className={getLabelClass("solicitudes")} aria-label={tabLabels.solicitudes}>
            <input type="radio" value="solicitudes" checked={selectedView === "solicitudes"} onChange={() => setSelectedView("solicitudes")} />
            <FontAwesomeIcon icon="fa-solid fa-inbox" size="xl" />
            <span className={style.actionText}>{tabLabels.solicitudes}</span>
          </label>

          <label className={getLabelClass("configs")} aria-label={tabLabels.configs}>
            <input type="radio" value="configs" checked={selectedView === "configs"} onChange={() => setSelectedView("configs")} />
            <FontAwesomeIcon icon="fa-solid fa-gear" size="xl" />
            <span className={style.actionText}>{tabLabels.configs}</span>
          </label>
        </>
      )}
    </div>
  );

  const items = (
    <div className={style.groupContainer}>
      {isLoadingGroup ? (
        <PulseLoader />
      ) : (
        <div className={style.infoGrupo} style={isProfesor ? { justifyContent: "space-between" } : { justifyContent: "center" }}>
          <h3>
            <FontAwesomeIcon icon="fa-solid fa-book-bookmark" /> {group?.materia.toUpperCase()}
          </h3>
          {isProfesor && (
            <h3>
              <FontAwesomeIcon icon="fa-solid fa-school" /> {group?.institucion.toUpperCase()} - {group?.nombre.toUpperCase()}
            </h3>
          )}
        </div>
      )}

      <div className={style.itemsContainer}>
        {!isProfesor && selectedView == "tienda" && (
          <div className={style.resumen}>
            <div>
              <p>
                <FontAwesomeIcon icon="fa fa-coins" />
                {perfilHook ? perfilHook?.monedas : <PulseLoader color={"var(--monedas)"} />}
              </p>
            </div>
          </div>
        )}

        {selectedView === "tienda" ? (
          <StoreGroupView
            recompensas={recompensas}
            isLoading={isLoadingRecompensas || isLoadingGroup}
            isProfesor={isProfesor}
            perfil={perfilHook}
            setShowModal={setShowModal}
            setModalContent={setModalContent}
            setModalTitle={setModalTitle}
            grupoId={groupId}
          />
        ) : selectedView === "alumnos" ? (
          isLoadingStudents || isLoadingMedals ? (
            <BarLoader />
          ) : students?.length === 0 || students === undefined ? (
            <div className={style.noStudentsMessage}>
              <p>
                {isProfesor ? (
                  <>
                    El grupo aún no tiene alumnos. Dirígete a la sección de <strong onClick={() => setSelectedView("configs")}> configuración </strong> o
                    <strong onClick={() => setSelectedView("solicitudes")}> solicitudes </strong> para compartir el código de unión con tus estudiantes.
                  </>
                ) : (
                  "Aún no hay mas compañeros en este grupo"
                )}
              </p>
            </div>
          ) : (
            <div className={style.studentsContainer}>
              {studentsFiltrados?.map((item) => (
                <StudentItem
                  key={item.id}
                  perfilEmisorId={perfilHook?.id}
                  student={item}
                  medals={medals}
                  kudos={tiposKudo}
                  isLoadingKudos={isLoadingKudos}
                  showProfesorOptions={isProfesor}
                  onSelectStudent={(student) => {
                    setSelectedView("perfil");
                    setPerfilSeleccionado(student);
                  }}
                  notaMax={group?.tablaEquivalenciaNotaMaxima}
                />
              ))}
            </div>
          )
        ) : selectedView === "perfil" && perfilSeleccionado ? (
          <GroupProfileView
            perfil={perfilSeleccionado}
            groupid={groupId}
            isLoading={isLoadingPerfil}
            setModalContent={setModalContent}
            setShowModal={setShowModal}
            setModalTitle={setModalTitle}
            isOwnProfile={perfilSeleccionado.id === userProfileId}
          />
        ) : selectedView === "rankings" ? (
          <GroupRankingView
            setModalContent={setModalContent}
            setModalTitle={setModalTitle}
            setShowModal={setShowModal}
            groupId={groupId}
            showTeacherOptions={isProfesor}
            idPerfilEstudiante={perfilHook?.id}
          />
        ) : selectedView === "pac" ? (
          <GroupPacView recompensas={recompensas} setModalContent={setModalContent} setModalTitle={setModalTitle} setShowModal={setShowModal} groupId={groupId} showTeacherOptions={isProfesor} />
        ) : selectedView === "threshold" ? (
          <MedalThresholdView setModalContent={setModalContent} setModalTitle={setModalTitle} setShowModal={setShowModal} groupId={groupId} showTeacherOptions={isProfesor} />
        ) : selectedView === "solicitudes" ? (
          <RequestView grupo={groupId} medallas={medals} urlCompleta={group?.urlCompleta} />
        ) : selectedView === "configs" ? (
          <GroupConfigView id={groupId} group={group} setModalContent={setModalContent} setModalTitle={setModalTitle} setShowModal={setShowModal} />
        ) : null}
      </div>
    </div>
  );
  return (
    <BaseManagerPage
      actions={actions}
      modalTitle={modalTitle}
      modalContent={modalContent}
      items={items}
      searchPlaceholder={isProfesor ? "alumno" : "compañero"}
      showModal={showModal}
      setShowModal={setShowModal}
      searchValue={search}
      onSearchChange={setSearch}
    />
  );
};

export default GroupPage;
