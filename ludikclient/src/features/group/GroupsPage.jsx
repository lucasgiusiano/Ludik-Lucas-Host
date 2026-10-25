import React, { useState } from "react";
import { useSelector } from "react-redux";
import { selectUserRole } from "../auth/hooks/userSlice";
import { useGruposPorRol } from "./hooks/useGrupoMutation";
import styles from "./GroupsPage.module.css";
import BarLoader from "../generics/BarLoader";

import GroupItem from "./components/GroupItem";
import GroupCreateModal from "./components/teacher/GroupCreateForm";
import GroupUnionLinkModal from "./components/student/GroupUnionLinkForm";

import genericGroupImage from "../../assets/genericGroupImage.png";
import BaseManagerPage from "../generics/BaseManagerPage";

const GroupsPage = () => {
  const role = useSelector(selectUserRole);
  const isProfesor = role === "Profesor";

  const [showModal, setShowModal] = useState(false);
  const [modalTipo, setModalTipo] = useState(null);
  const [search, setSearch] = useState("");

  const { data: grupos, isLoading } = useGruposPorRol(role);

  const gruposFormateados =
    grupos?.map((g) => ({
      id: g.id,
      name: g.nombre,
      grade: g.materia,
      students: g.cantAlumnos == 0 ? "-" : g.cantAlumnos,
      imgSrc: genericGroupImage,
    })) || [];

  const gruposFiltrados = gruposFormateados.filter((grupo) => grupo.name.toLowerCase().includes(search.toLowerCase()) || grupo.grade.toLowerCase().includes(search.toLowerCase()));

  const handleOpenModal = (tipo) => {
    setModalTipo(tipo);
    setShowModal(true);
  };

  const modalContent = modalTipo === "crear" ? <GroupCreateModal onClose={() => setShowModal(false)} /> : <GroupUnionLinkModal onClose={() => setShowModal(false)} />;

  const actions = (
    <button className="button-secondary" onClick={() => handleOpenModal(isProfesor ? "crear" : "unir")}>
      {isProfesor ? "Crear grupo" : "Unirse a una asignatura"}
    </button>
  );

  const items = isLoading ? (
    <BarLoader />
  ) : (
    <div className={styles.groupList}>
      {gruposFiltrados.map((group, index) => (
        <GroupItem key={index} {...group} />
      ))}
    </div>
  );

  return (
    <BaseManagerPage
      actions={actions}
      modalTitle={modalTipo === "crear" ? "Crea una nuevo grupo" : "Únete a una asignatura"}
      modalContent={modalContent}
      items={items}
      searchPlaceholder={isProfesor ? "grupo" : "asignatura"}
      showModal={showModal}
      setShowModal={setShowModal}
      searchValue={search}
      onSearchChange={setSearch}
    />
  );
};

export default GroupsPage;
