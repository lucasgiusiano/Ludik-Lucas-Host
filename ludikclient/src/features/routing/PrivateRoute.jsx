import PropTypes from "prop-types";
import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useSelector } from "react-redux";
import { selectUserRole, selectLoggedOutManually } from "../auth/hooks/userSlice";

const PrivateRoute = ({ allowedRoles }) => {
  const userRole = useSelector(selectUserRole);
  const loggedOutManually = useSelector(selectLoggedOutManually);
  const location = useLocation();

  if (!userRole) {
    // Si el logout fue manual, NO mostrar mensaje
    const shouldShowMessage = !loggedOutManually;

    return (
      <Navigate
        to="/login"
        state={{
          from: location,
          ...(shouldShowMessage && { message: "Debes iniciar sesión para acceder." }),
        }}
        replace
      />
    );
  }

  if (!allowedRoles.includes(userRole)) {
    return <Navigate to="/login" state={{ from: location, message: "No tienes autorización para acceder a esta sección." }} replace />;
  }

  return <Outlet />;
};

PrivateRoute.propTypes = {
  allowedRoles: PropTypes.arrayOf(PropTypes.string).isRequired,
};

export default PrivateRoute;
