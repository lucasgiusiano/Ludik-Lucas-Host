// src/features/common/NotFoundPage.jsx
import React from "react";
import { useNavigate } from "react-router-dom";

const NotFoundPage = () => {
  const navigate = useNavigate();

  return (
    <div style={{ padding: 20, textAlign: "center" }}>
      <h1>404 - Página no encontrada</h1>
      <p>La página que buscas no existe.</p>
      <button className="button" onClick={() => navigate("/")}>
        Ir al inicio
      </button>
    </div>
  );
};

export default NotFoundPage;
