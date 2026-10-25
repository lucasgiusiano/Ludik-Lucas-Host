// src/features/reward/RewardsPage.jsx
import React, { useState } from "react";
import styles from "./RewardsPage.module.css";
import { useSelector } from "react-redux";
import { selectUserRole } from "../auth/hooks/userSlice";
import { useRecompensasProfesor } from "./hooks/useRewardMutation";
import { useGruposPorRol } from "../group/hooks/useGrupoMutation";

import BaseManagerPage from "../generics/BaseManagerPage";
import RewardItem from "./components/RewardItem";
import RewardCreateForm from "./components/RewardCreateForm";
import RewardAsignationForm from "../group/components/store/RewardAsignationForm";

import BarLoader from "../generics/BarLoader";

const RewardsPage = () => {
  const [modalType, setModalType] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [rewardToEdit, setRewardToEdit] = useState(null);
  const [search, setSearch] = useState("");

  const role = useSelector(selectUserRole);
  const isProfesor = role === "Profesor";

  const { data: recompensas, isLoadingRewards } = useRecompensasProfesor();
  const { data: grupos, isLoadingGroups } = useGruposPorRol(role);

  const handleOpenCreate = () => {
    setModalType("crear");
    setRewardToEdit(null);
    setShowModal(true);
  };

  const handleOpenEdit = (reward) => {
    setModalType("editar");
    setRewardToEdit(reward);
    setShowModal(true);
  };

  const handleOpenAsignToGroups = () => {
    setModalType("asignar");
    setShowModal(true);
  };

  let modalTitle = "";
  let modalContent = null;

  if (modalType === "crear") {
    modalTitle = "Crear nueva recompensa";
    modalContent = <RewardCreateForm onClose={() => setShowModal(false)} reward={null} />;
  } else if (modalType === "editar") {
    modalTitle = "Editar recompensa";
    modalContent = <RewardCreateForm onClose={() => setShowModal(false)} reward={rewardToEdit} />;
  } else if (modalType === "asignar") {
    modalTitle = "Asignar recompensas a grupos";
    modalContent = (
      <RewardAsignationForm
        grupoId={null} // o uno real si querés
        gruposProfesor={grupos} // pasá los grupos si querés habilitar selección
        isLoadingGroups={isLoadingGroups}
        onClose={() => setShowModal(false)}
      />
    );
  }

  const actions = (
    <div className={styles.rewardActions}>
      <button className="button-secondary" onClick={handleOpenCreate}>
        <span className={styles.clamped}> Crear recompensa</span>
      </button>
      <button className={`button ${styles.botonAsignar}`} onClick={handleOpenAsignToGroups}>
        <span className={styles.clamped}>Asignar recompensas</span>
      </button>
    </div>
  );

  const filteredRewards = recompensas?.filter((r) => r.nombre.toLowerCase().includes(search.toLowerCase())) || [];

  const items = isLoadingRewards ? (
    <BarLoader />
  ) : (
    <div className={styles.rewardsGrid}>
      {filteredRewards.map((reward) => (
        <RewardItem
          key={reward.id}
          reward={reward}
          redeemed={false}
          perfilId={0}
          onEdit={() => handleOpenEdit(reward)} // paso el objeto reward completo aquí
          showProfesorOptions={isProfesor}
        />
      ))}
    </div>
  );

  return (
    <BaseManagerPage
      actions={actions}
      modalTitle={modalTitle}
      modalContent={modalContent}
      items={items}
      searchPlaceholder="recompensa"
      showModal={showModal}
      setShowModal={setShowModal}
      searchValue={search}
      onSearchChange={setSearch}
    />
  );
};

export default RewardsPage;
