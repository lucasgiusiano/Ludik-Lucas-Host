// MedalManagerPage.jsx
import React, { useState } from "react";
import styles from "./MedalManagerPage.module.css";
import MedalCard from "./components/MedalCard";
import MedalCreateForm from "./components/MedalCreateForm.jsx";
import BarLoader from "../generics/BarLoader.jsx";
import BaseManagerPage from "../generics/BaseManagerPage";
import PropTypes from "prop-types";
import { useMedallasProfesor } from "./hooks/useMedalMutation.js";

const MedalCreateModal = ({ onClose, medalId }) => <MedalCreateForm onClose={onClose} medalId={medalId} />;

MedalCreateModal.propTypes = {
  onClose: PropTypes.func.isRequired,
  medalId: PropTypes.number, // puede ser undefined para crear
};

const MedalManagerPage = () => {
  const [showModal, setShowModal] = useState(false);
  const [medalToEditId, setMedalToEditId] = useState(null); // ID de medalla a editar
  const [search, setSearch] = useState("");

  const { data: medallas, isLoading } = useMedallasProfesor();

  const handleOpenCreate = () => {
    setMedalToEditId(null);
    setShowModal(true);
  };

  const handleOpenEdit = (id) => {
    setMedalToEditId(id);
    setShowModal(true);
  };

  const modalContent = <MedalCreateModal onClose={() => setShowModal(false)} medalId={medalToEditId} />;

  const actions = (
    <button className="button-secondary" onClick={handleOpenCreate}>
      Crear medalla
    </button>
  );

  const filteredMedallas = medallas?.filter((m) => m.nombre.toLowerCase().includes(search.toLowerCase())) || [];

  const items = isLoading ? (
    <BarLoader />
  ) : (
    <div className={styles.medalsContainer}>
      {filteredMedallas.map((medalla) => (
        <MedalCard key={medalla.id} medal={medalla} onEdit={() => handleOpenEdit(medalla.id)} showEditOption={true} />
      ))}
    </div>
  );

  return (
    <BaseManagerPage
      actions={actions}
      modalTitle={medalToEditId ? "Editar medalla" : "Crear nueva medalla"}
      modalContent={modalContent}
      items={items}
      searchPlaceholder="medalla"
      showModal={showModal}
      setShowModal={setShowModal}
      searchValue={search}
      onSearchChange={setSearch}
    />
  );
};

export default MedalManagerPage;
