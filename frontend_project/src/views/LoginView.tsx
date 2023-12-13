import React, { useContext, useEffect, useState } from "react";
import { UserDto } from "../interfaces/UserDto";
import { LoginRequest } from "../interfaces/LoginRequest";
import AuthService from "../services/AuthService";
import { Navigate, useNavigate } from "react-router-dom";
import jwt from "jwt-decode";
import { toast } from "react-toastify";
import { AuthenticationContext } from "../App";

const LoginView = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const navigate = useNavigate();

  const authContext = useContext(AuthenticationContext);

  const loginUser = async () => {
    if (!username || !password) return;
    let request: LoginRequest = {
      password: password,
      username: username,
    };
    const user = await AuthService.login(request);
    if (user.isSuccess) {
      //erroare
      authContext.setAuthenticated(true);
      if (localStorage.getItem("role") === "ADMIN") {
        authContext.setAdmin(true);
        navigate("/admin");
        console.log(localStorage.getItem("role"));
      } else {
        navigate(`/client`);
      }
    } else {
      toast.error("Authentication failed!");
    }
    console.log(user);
  };

  useEffect(() => {
    console.log(username, password);
  }, [username, password]);

  if (authContext.isAuthenticated) return <Navigate to="/" />;

  return (
    <>
      <div className="w-full h-full bg-slate-100 flex justify-center">
        <form className="flex flex-col gap-3">
          <label>Username</label>
          <input
            type="text"
            className="mb-2 p-1 text-sm font-medium border-2 border-black rounded text-gray-900 "
            onChange={(e) => {
              setUsername(e.target.value);
            }}
          ></input>
          <label>Password</label>
          <input
            type="text"
            className="mb-2 p-1 text-sm font-medium border-2 border-black rounded text-gray-900 "
            onChange={(e) => {
              setPassword(e.target.value);
            }}
          ></input>
          <button
            className="bg-slate-300 rounded border-black"
            onClick={async (e) => {
              e.preventDefault();
              await loginUser();
            }}
          >
            Login
          </button>
        </form>
      </div>
    </>
  );
};

export default LoginView;
