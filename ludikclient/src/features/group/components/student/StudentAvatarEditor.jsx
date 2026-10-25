// StudentAvatarEditor.jsx
import React, { useState, useEffect, useCallback } from "react";
import PropTypes from "prop-types";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styles from "./StudentAvatarEditor.module.css";
import BarLoader from "../../../generics/BarLoader";
import { useInventarioAvatar, useGuardarAvatar } from "../../hooks/useStudentMutation";
import { PulseLoader } from "../../../generics/BarLoader";

const iconosPorTipo = {
  posicion: "arrow-right-arrow-left",
  Encuadre: "bullseye",
  Pelo: "user",
  Barba: "face-smile",
  Ojos: "eye",
  Cejas: "eye",
  Sombrero: "graduation-cap",
  Gafas: "glasses",
  Accesorio: "face-grin-stars",
  Boca: "face-kiss-beam",
  Ropa: "shirt",
  ColorPelo: "palette",
  ColorBarba: "palette",
  ColorCejas: "palette",
  ColorPiel: "palette",
  ColorFondo: "palette",
  ColorRopa: "palette",
  ColorGafas: "palette",
  ColorSombrero: "palette",
};

const agruparPorTipoConColores = (items) => {
  const agrupados = {};

  items.forEach((item) => {
    const tipo = item.tipo;

    if (tipo === "ColorPiel" || tipo === "ColorFondo") {
      if (!agrupados["Encuadre"]) agrupados["Encuadre"] = [];
      agrupados["Encuadre"].push(item);
      return;
    }

    if (tipo && tipo.startsWith("Color")) {
      const baseTipo = tipo.replace(/^Color/, "");
      if (!agrupados[baseTipo]) agrupados[baseTipo] = [];
      agrupados[baseTipo].push(item);
      return;
    }

    if (!agrupados[tipo]) agrupados[tipo] = [];
    agrupados[tipo].push(item);
  });

  return agrupados;
};

const StudentAvatarEditor = ({ idPerfil, onClose }) => {
  const [selectedTab, setSelectedTab] = useState("posicion");
  const [selecciones, setSelecciones] = useState({});
  const { data: inventario, isLoading, isError, error } = useInventarioAvatar(idPerfil);

  const [voltear, setVoltear] = useState(false);
  const [rotacion, setRotacion] = useState(0);
  const [zoom, setZoom] = useState(100);
  const [temaRopa, setTemaRopa] = useState("");
  const [avatarSvg, setAvatarSvg] = useState(null);
  const [loadingAvatar, setLoadingAvatar] = useState(false);

  const guardarAvatarMutation = useGuardarAvatar(idPerfil);

  const handleGuardarAvatar = async () => {
    if (!avatarSvg) return;

    const svgImage = new Image();
    const svgBlob = new Blob([avatarSvg], { type: "image/svg+xml" });
    const url = URL.createObjectURL(svgBlob);

    svgImage.onload = async () => {
      const canvas = document.createElement("canvas");
      canvas.width = svgImage.width || 512;
      canvas.height = svgImage.height || 512;
      const ctx = canvas.getContext("2d");

      ctx.fillStyle = "#ffffff";
      ctx.fillRect(0, 0, canvas.width, canvas.height);
      ctx.drawImage(svgImage, 0, 0);

      canvas.toBlob(
        async (jpegBlob) => {
          if (!jpegBlob) return;

          const atributosSeleccionados = Object.entries(selecciones)
            .filter(([, codigo]) => !!codigo)
            .map(([tipo]) => {
              const item = Array.isArray(inventario) ? inventario.find((i) => i.tipo === tipo && i.codigoUnico === selecciones[tipo]) : null;
              return item?.id;
            })
            .filter(Boolean);

          const avatarDto = {
            ColorFondo: selecciones.ColorFondo || "",
            Voltear: voltear,
            Rotacion: rotacion,
            Zoom: zoom,
            AtributosIds: atributosSeleccionados,
          };

          guardarAvatarMutation.mutate(
            { avatarDto, jpegBlob },
            {
              onSuccess: () => {
                // Cierra el modal al guardar correctamente
                if (onClose) onClose();
              },
            }
          );
          URL.revokeObjectURL(url);
        },
        "image/jpeg",
        1.0
      );
    };

    svgImage.onerror = () => {
      console.error("No se pudo cargar el SVG para convertir a JPEG.");
    };

    svgImage.src = url;
  };

  const tiposAgrupados = Array.isArray(inventario) ? agruparPorTipoConColores(inventario) : {};

  const tabs = [
    { key: "posicion", label: "Posición", icon: iconosPorTipo["posicion"] },
    ...Object.keys(tiposAgrupados).map((tipo) => ({
      key: tipo,
      label: tipo,
      icon: iconosPorTipo[tipo] || null,
    })),
  ];

  const handleChange = (tipo, codigoUnico) => {
    setSelecciones((prev) => ({ ...prev, [tipo]: codigoUnico }));
  };

  const colorParamMap = {
    ColorFondo: "backgroundColor",
    ColorPiel: "skinColor",
    ColorPelo: "hairColor",
    ColorBarba: "facialHairColor",
    ColorCejas: "eyebrowsColor",
    ColorGafas: "accessoriesColor",
    ColorRopa: "clothesColor",
    ColorSombrero: "hatColor",
  };

  const cleanColor = (val) => {
    if (!val) return "";
    return val.replace(/^#/, "");
  };

  // ---- NUEVO: establecer defaults iniciales usando inventario (solo si no hay selecciones) ----
  useEffect(() => {
    if (!Array.isArray(inventario) || inventario.length === 0) return;
    // Si ya hay selecciones, no sobreescribimos (evitamos impactar al usuario)
    if (Object.keys(selecciones).length > 0) return;

    const pickFirst = (tipo) => {
      const it = inventario.find((i) => i.tipo === tipo);
      return it ? it.codigoUnico : undefined;
    };

    const pickFirstColor = (tipoColor) => {
      // colores pueden venir como "ColorPiel", "ColorRopa", etc.
      const it = inventario.find((i) => i.tipo === tipoColor);
      return it ? it.codigoUnico : undefined;
    };

    const defaults = {};

    // estilos que normalmente querrás ver por defecto si existen
    defaults.Ojos = pickFirst("Ojos") || "";
    defaults.Boca = pickFirst("Boca") || "";
    defaults.Cejas = pickFirst("Cejas") || "";
    defaults.Pelo = pickFirst("Pelo") || "";
    defaults.Ropa = pickFirst("Ropa") || "";

    // colores por defecto (si existen)
    const piel = pickFirstColor("ColorPiel");
    if (piel) defaults.ColorPiel = piel;

    const fondo = pickFirstColor("ColorFondo");
    if (fondo) defaults.ColorFondo = fondo;

    const colorPelo = pickFirstColor("ColorPelo");
    if (colorPelo) defaults.ColorPelo = colorPelo;

    const colorRopa = pickFirstColor("ColorRopa");
    if (colorRopa) defaults.ColorRopa = colorRopa;

    // no forzamos barba/gafas/sombrero; quedan vacíos por defecto
    setSelecciones((prev) => ({ ...defaults, ...prev }));
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [inventario]); // solo al cambiar inventario

  // ----------------------------------------------------------

  const generarUrlAvatar = useCallback(() => {
    const params = [];

    Object.entries(colorParamMap).forEach(([tipo, paramName]) => {
      const val = selecciones[tipo];
      if (val) {
        params.push(`${paramName}=${encodeURIComponent(cleanColor(val))}`);
      }
    });

    params.push(`flip=${voltear ? "true" : "false"}`);
    params.push(`rotate=${rotacion}`);
    params.push(`scale=${zoom}`);

    if (selecciones.Cejas) params.push(`eyebrows=${encodeURIComponent(selecciones.Cejas)}`);
    if (selecciones.Ojos) params.push(`eyes=${encodeURIComponent(selecciones.Ojos)}`);
    if (selecciones.Boca) params.push(`mouth=${encodeURIComponent(selecciones.Boca)}`);

    const barbaVal = selecciones.Barba || "";
    if (barbaVal) {
      params.push(`facialHair=${encodeURIComponent(barbaVal)}`);
      params.push(`facialHairProbability=100`);
    } else {
      params.push(`facialHairProbability=0`);
    }
    if (selecciones.ColorBarba) {
      params.push(`facialHairColor=${encodeURIComponent(cleanColor(selecciones.ColorBarba))}`);
    }

    const gorroVal = selecciones.Sombrero || "";
    if (gorroVal) {
      params.push(`top=${encodeURIComponent(gorroVal)}`);
      if (selecciones.ColorSombrero) params.push(`hatColor=${encodeURIComponent(cleanColor(selecciones.ColorSombrero))}`);
    } else {
      if (selecciones.Pelo) params.push(`top=${encodeURIComponent(selecciones.Pelo)}`);
      if (selecciones.ColorPelo) params.push(`hairColor=${encodeURIComponent(cleanColor(selecciones.ColorPelo))}`);
    }

    const gafasVal = selecciones.Gafas || "";
    if (gafasVal) {
      params.push(`accessories=${encodeURIComponent(gafasVal)}`);
      params.push(`accessoriesProbability=100`);
      if (selecciones.ColorGafas) params.push(`accessoriesColor=${encodeURIComponent(cleanColor(selecciones.ColorGafas))}`);
    } else {
      params.push(`accessoriesProbability=0`);
    }

    if (selecciones.Ropa) {
      params.push(`clothing=${encodeURIComponent(selecciones.Ropa)}`);
      if (selecciones.ColorRopa) params.push(`clothesColor=${encodeURIComponent(cleanColor(selecciones.ColorRopa))}`);
    }

    if (temaRopa) params.push(`clothingGraphic=${encodeURIComponent(temaRopa)}`);

    params.push("radius=50");

    const query = params.filter(Boolean).join("&");

    return `https://api.dicebear.com/9.x/avataaars/svg?${query}`;
  }, [selecciones, voltear, rotacion, zoom, temaRopa, colorParamMap]);

  useEffect(() => {
    const url = generarUrlAvatar();
    setLoadingAvatar(true);
    fetch(url)
      .then((res) => res.text())
      .then((svg) => {
        setAvatarSvg(svg);
        setLoadingAvatar(false);
      })
      .catch(() => {
        setAvatarSvg(null);
        setLoadingAvatar(false);
      });
  }, [generarUrlAvatar, inventario]);

  const mostrarColorBarba = !!selecciones.Barba;
  const mostrarColorCabello = !selecciones.Sombrero && !!selecciones.Pelo;

  const renderTabContent = () => {
    if (selectedTab === "posicion") {
      return (
        <div className={styles.tabContent}>
          {/* Voltear */}
          <div className={styles.optionRow}>
            <label className={styles.optionLabel}>Voltear</label>
            <div className={styles.controlWrap}>
              <input aria-label="Voltear avatar" type="checkbox" className={`${styles.checkbox} ${styles.smallControl}`} checked={voltear} onChange={(e) => setVoltear(e.target.checked)} />
            </div>
          </div>

          {/* Rotación */}
          <div className={styles.optionRow}>
            <label className={styles.optionLabel}>Ángulo</label>
            <div className={styles.controlWrap}>
              <div className={styles.sliderRow}>
                <input aria-label="Rotación avatar" type="range" min="0" max="360" className={styles.slider} value={rotacion} onChange={(e) => setRotacion(Number(e.target.value))} />
                <div className={styles.valueBadge}>{rotacion}°</div>
              </div>
            </div>
          </div>

          {/* Zoom */}
          <div className={styles.optionRow}>
            <label className={styles.optionLabel}>Zoom</label>
            <div className={styles.controlWrap}>
              <div className={styles.sliderRow}>
                <input aria-label="Zoom avatar" type="range" min="100" max="200" className={styles.slider} value={zoom} onChange={(e) => setZoom(Number(e.target.value))} />
                <div className={styles.valueBadge}>{zoom - 100}%</div>
              </div>
            </div>
          </div>
        </div>
      );
    }

    const items = tiposAgrupados[selectedTab];
    if (!items) return null;

    const itemsPrincipales = items.filter((item) => !item.tipo.startsWith("Color"));
    const itemsColor = items.filter((item) => item.tipo.startsWith("Color") || selectedTab === "Encuadre");

    if (isLoading)
      return (
        <div className={styles.loaderWrap}>
          <BarLoader />
        </div>
      );
    if (isError) return <p className={styles.errorText}>Error: {error?.[0] || "Error al cargar inventario"}</p>;

    const colorGroups = itemsColor.reduce((acc, item) => {
      const key = item.tipo || "Color";
      if (!acc[key]) acc[key] = [];
      acc[key].push(item);
      return acc;
    }, {});

    return (
      <div className={styles.tabContentGrid}>
        {itemsPrincipales.length > 0 && (
          <>
            <h4 className={styles.subTitle}>Estilos</h4>
            <div className={styles.itemGrid}>
              <label className={styles.avatarItemCard}>
                <input
                  aria-label={`Ninguno estilo ${selectedTab}`}
                  type="radio"
                  name={`estilo-${selectedTab}`}
                  value=""
                  checked={(selecciones[selectedTab] || "") === ""}
                  onChange={() => handleChange(selectedTab, "")}
                  className={styles.radioInputHidden}
                />
                <div className={styles.avatarNoneOption}>Ninguno</div>
              </label>

              {itemsPrincipales.map((item) => (
                <label key={item.id} className={styles.avatarItemCard}>
                  <input
                    aria-label={`${item.nombre} estilo ${selectedTab}`}
                    type="radio"
                    name={`estilo-${selectedTab}`}
                    value={item.codigoUnico}
                    checked={selecciones[selectedTab] === item.codigoUnico}
                    onChange={() => handleChange(selectedTab, item.codigoUnico)}
                    className={styles.radioInputHidden}
                  />
                  <img className={styles.avatarPreviewImg} src={item.enlaceImagen} alt={item.nombre} />
                </label>
              ))}
            </div>
          </>
        )}

        {Object.keys(colorGroups).length > 0 && (
          <>
            <h4 className={styles.subTitle}>Colores</h4>
            <div className={styles.colorGroupList}>
              {Object.entries(colorGroups).map(([colorTipo, itemsArr]) => (
                <div key={colorTipo} className={styles.colorGroup}>
                  <p className={styles.colorGroupTitle}>{colorTipo.replace(/^Color/, "")}</p>

                  <div className={styles.itemGrid}>
                    <label className={styles.avatarItemCard}>
                      <input
                        aria-label={`Ninguno color ${colorTipo}`}
                        type="radio"
                        name={`color-${selectedTab}-${colorTipo}`}
                        value=""
                        checked={(selecciones[colorTipo] || "") === ""}
                        onChange={() => handleChange(colorTipo, "")}
                        className={styles.radioInputHidden}
                      />
                      <div className={styles.colorNoneOption}>Ninguno</div>
                    </label>

                    {itemsArr.map((item) => (
                      <label key={item.id} className={styles.avatarItemCard}>
                        <input
                          aria-label={`${item.nombre} color ${colorTipo}`}
                          type="radio"
                          name={`color-${selectedTab}-${colorTipo}`}
                          value={item.codigoUnico}
                          checked={selecciones[colorTipo] === item.codigoUnico}
                          onChange={() => handleChange(colorTipo, item.codigoUnico)}
                          className={styles.radioInputHidden}
                        />
                        <div className={styles.colorBox} title={item.nombre} style={{ ["--color-value"]: item.nombre ? item.nombre.toLowerCase() : "transparent" }} />
                        <p className={styles.colorLabel}>{item.nombre}</p>
                      </label>
                    ))}
                  </div>
                </div>
              ))}
            </div>
          </>
        )}

        {selectedTab === "Barba" && !mostrarColorBarba && <p className={styles.infoText}>Selecciona un estilo de barba para elegir el color</p>}
        {selectedTab === "Pelo" && !mostrarColorCabello && <p className={styles.infoText}>Selecciona un estilo de pelo para elegir el color</p>}
      </div>
    );
  };

  return (
    <div className={styles.editorContainer}>
      <div className={styles.avatarPreviewWrap}>
        <div className={styles.avatarPreview} id="avatar">
          {loadingAvatar ? (
            <div className={styles.loaderWrap}>
              <BarLoader />
            </div>
          ) : avatarSvg ? (
            <div className={styles.avatarSvgWrap} dangerouslySetInnerHTML={{ __html: avatarSvg }} />
          ) : (
            <span className={styles.avatarText}>[Avatar]</span>
          )}
        </div>

        <button className={`${styles.confirmButton} button-secondary`} title="Confirmar selección" onClick={handleGuardarAvatar} disabled={guardarAvatarMutation.isLoading}>
          {guardarAvatarMutation.isPending ? <PulseLoader /> : <FontAwesomeIcon icon="floppy-disk" />}
        </button>
      </div>

      <nav className={styles.tabs} role="tablist" aria-label="Pestañas de avatar">
        {tabs.map((tab) => (
          <label key={tab.key} className={styles.tabLabel} role="presentation">
            <input aria-label={`Pestana ${tab.label}`} type="radio" name="avatarTab" className={styles.tabRadio} checked={selectedTab === tab.key} onChange={() => setSelectedTab(tab.key)} />
            <div className={`${styles.tabButton} ${selectedTab === tab.key ? styles.tabActive : ""}`}>{tab.icon && <FontAwesomeIcon icon={["fas", tab.icon]} />}</div>
            <span className={styles.tabText}>{tab.label}</span>
          </label>
        ))}
      </nav>

      <section className={styles.tabPanel} aria-live="polite">
        {renderTabContent()}
      </section>
    </div>
  );
};

StudentAvatarEditor.propTypes = {
  idPerfil: PropTypes.number.isRequired,
  onClose: PropTypes.func.isRequired
};

export default StudentAvatarEditor;
