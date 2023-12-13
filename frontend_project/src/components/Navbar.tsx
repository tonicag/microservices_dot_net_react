import React, { useContext } from "react";
import { Link } from "react-router-dom";
import { AuthenticationContext } from "../App";

const Navbar = () => {
  const navItemClass = "p-3 px-7 hover:bg-orange-600 hover:text-white";
  const authContext = useContext(AuthenticationContext);
  return (
    <nav className="flex border-b-8 border-orange-600 justify-start">
      <Link className={navItemClass} to={"/"}>
        Home
      </Link>
      {authContext.isAuthenticated && !authContext.isAdmin ? (
        <Link className={navItemClass} to={"/client"}>
          Client
        </Link>
      ) : (
        <></>
      )}

      {authContext.isAuthenticated && authContext.isAdmin ? (
        <Link className={navItemClass} to={"/admin"}>
          Admin
        </Link>
      ) : (
        <></>
      )}

      {!authContext.isAuthenticated ? (
        <Link className={navItemClass} to={"/login"}>
          Login
        </Link>
      ) : (
        <Link className={navItemClass} to={"/logout"}>
          Logout
        </Link>
      )}

      {authContext.isAuthenticated ? (
        <Link className={`${navItemClass} ml-auto`} to={"/"}>
          Hello
        </Link>
      ) : (
        <></>
      )}
    </nav>
  );
};

export default Navbar;
