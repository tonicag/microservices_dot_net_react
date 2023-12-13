import { log } from "console";
import React from "react";
import { Link, Navigate, Outlet } from "react-router-dom";

const AuthorisedRoute = () => {
  let role = localStorage.getItem("role");
  console.log("Admin auth");
  const navItemClass =
    "p-3 hover:bg-slate-100 w-full border-b-2 border-slate-800 bg-slate-300";
  console.log("SECURITY");

  if (role && role === "ADMIN") {
    return (
      <div className="w-full h-full bg-slate-100 flex">
        <div className="w-[25%] border-r-4 border-slate-800">
          <div className="flex flex-col ">
            <Link className={navItemClass} to="users">
              Users
            </Link>

            <Link className={navItemClass} to="users/add">
              Add new user
            </Link>
            <Link className={navItemClass} to="devices/">
              Devices
            </Link>
            <Link className={navItemClass} to="devices/add">
              Add new device
            </Link>
          </div>
        </div>
        <div className=" w-full">
          <Outlet />
        </div>
      </div>
    );
  }
  return <Navigate to={"/login"} />;
};

export default AuthorisedRoute;
