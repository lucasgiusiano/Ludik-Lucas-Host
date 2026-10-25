import React, { useState } from "react";
import styles from "./EquivalenceTablePage.module.css";
import BarLoader from "../generics/BarLoader";
import BaseManagerPage from "../generics/BaseManagerPage";
import EquivalenceTableItem from "./components/EquivalenceTableItem";
import EquivalenceTableCreateModal from "./components/EquivalenceTableCreateModal";
import { useTablasEquivalencia } from "./hooks/useEquivalenceTableMutation";

const EquivalenceTablePage = () => {
  const [showModal, setShowModal] = useState(false);
  const [modalTipo, setModalTipo] = useState(null);
  const [editedTable, setEditedTable] = useState(null);
  const [search, setSearch] = useState("");

  const { data: equivalences, isLoading, refetch } = useTablasEquivalencia();
  const equivalencesFiltradas = (equivalences ?? []).filter((item) => (item.nombre ?? "").toLowerCase().includes(search.toLowerCase()));

  const handleOpenModal = (tipo, table = null) => {
    setModalTipo(tipo);
    setEditedTable(table);
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
    setEditedTable(null);
  };

  const handleSave = () => {
    refetch();
    handleCloseModal();
  };

  const modalContent = <EquivalenceTableCreateModal onClose={handleCloseModal} onSave={handleSave} table={modalTipo === "editar" ? editedTable : null} />;

  const actions = (
    <button className="button-secondary" onClick={() => handleOpenModal("crear")}>
      Crear equivalencia
    </button>
  );

  const items = isLoading ? (
    <BarLoader />
  ) : (
    <div className={styles.tablesContainer}>
      {equivalencesFiltradas.map((item) => (
        <EquivalenceTableItem key={item.id} item={item} onEdit={() => handleOpenModal("editar", item)} />
      ))}
    </div>
  );

  return (
    <BaseManagerPage
      actions={actions}
      modalTitle={modalTipo === "crear" ? "Crear Tabla de Equivalencia" : "Editar Tabla de Equivalencia"}
      modalContent={modalContent}
      items={items}
      searchPlaceholder="Tabla de Equivalencia"
      showModal={showModal}
      setShowModal={setShowModal}
      searchValue={search}
      onSearchChange={setSearch}
    />
  );
};

export default EquivalenceTablePage;
