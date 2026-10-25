import React from "react";
import styles from "./BarLoader.module.css";
import { BarLoader as ReactBarLoader, PulseLoader as ReactPulseLoader } from "react-spinners";
import PropTypes from "prop-types";

const BarLoader = () => {
  return (
    <div className={styles.barLoaderContainer}>
      <ReactBarLoader color="var(--blanco-secundario)" size={10} />
    </div>
  );
};

const PulseLoader = ({ color = "var(--blanco-secundario)" }) => {
  return (
    <div className={styles.pulseLoaderContainer}>
      <ReactPulseLoader color={color} size={10} />
    </div>
  );
};

PulseLoader.propTypes = {
  color: PropTypes.string,
  size: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
};

export default BarLoader;
export { PulseLoader };
