import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import styles from "./EnhancerView.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PulseLoader } from "../../generics/BarLoader";

const EnhancerView = ({ enhancerX, enhancerTime, isLoading }) => {
  const [timeLeft, setTimeLeft] = useState("");

  useEffect(() => {
    if (!enhancerTime) {
      setTimeLeft("");
      return;
    }

    const [horaStr, minStr, segConDecimales] = enhancerTime.split(":");
    const segStr = segConDecimales.split(".")[0];

    const targetDate = new Date();
    targetDate.setHours(Number(horaStr));
    targetDate.setMinutes(Number(minStr));
    targetDate.setSeconds(Number(segStr));
    targetDate.setMilliseconds(0);

    const now = new Date();
    if (targetDate <= now) {
      targetDate.setDate(targetDate.getDate() + 1);
    }

    const actualizarCuentaAtras = () => {
      const diferenciaMs = targetDate - new Date();
      if (diferenciaMs <= 0) {
        setTimeLeft("00:00");
      } else {
        const totalSeg = Math.floor(diferenciaMs / 1000);
        const horas = Math.floor(totalSeg / 3600);
        const minutos = Math.floor((totalSeg % 3600) / 60);
        // const segundos = totalSeg % 60; // si querés mostrar segundos también

        setTimeLeft(`${String(horas).padStart(2, "0")}:${String(minutos).padStart(2, "0")}`);
      }
    };

    actualizarCuentaAtras();
    const intervalo = setInterval(actualizarCuentaAtras, 1000);

    return () => clearInterval(intervalo);
  }, [enhancerTime]);

  return (
    <div
      className={styles.enhancerContainer}
      title={`Potenciador de monedas. Actualmente tienes un potenciador que multiplica tus monedas obtenidas por ${enhancerX} y dispones de ${timeLeft} para aprovecharlo`}
    >
      {isLoading ? (
        <PulseLoader />
      ) : (
        <div className={styles.enhancerContent}>
          <span>
            <FontAwesomeIcon icon="fa-solid fa-angles-up" bounce /> x{enhancerX}
          </span>
          <span>
            <FontAwesomeIcon icon="fa-solid fa-stopwatch" shake /> {timeLeft}
          </span>
        </div>
      )}
    </div>
  );
};

EnhancerView.propTypes = {
  enhancerX: PropTypes.number.isRequired,
  enhancerTime: PropTypes.string.isRequired,
  isLoading: PropTypes.bool.isRequired,
};

export default EnhancerView;
