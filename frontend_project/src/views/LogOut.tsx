import React, { useContext } from "react";
import { Navigate } from "react-router-dom";
import { AuthenticationContext } from "../App";

const LogOut = () => {
  let isLoggedIn = localStorage.getItem("user");
  const authContext = useContext(AuthenticationContext);
  if (isLoggedIn) {
    authContext.setAuthenticated(false);
    authContext.setAdmin(false);
    localStorage.removeItem("user");
  }
  return <Navigate to="/" />;
};

export default LogOut;
