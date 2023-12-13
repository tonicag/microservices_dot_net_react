import React, { useState } from "react";
import AuthService from "../../services/AuthService";
import { toast } from "react-toastify";
import { Navigate } from "react-router-dom";

const AddNewUser = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [name, setName] = useState("");
  const [role, setRole] = useState("");

  const register = async () => {
    const response = await AuthService.register({
      id: "",
      password: password,
      name: name,
      email: email,
      role: role,
    });
    if (response.isSuccess) {
      toast.success("Succcesfully created user!");
      <Navigate to="/admin/users"></Navigate>;
    } else {
      toast.error(response.message);
    }
  };

  const inputStyle =
    "mb-2 p-1 text-sm font-medium border-2 border-black rounded text-gray-900 w-[100%]";
  return (
    <div className="w-full flex flex-col items-center">
      <form className="flex flex-col gap-3 w-[50%]">
        <label>Email</label>
        <input
          type="text"
          className={inputStyle}
          onChange={(e) => {
            setEmail(e.target.value);
          }}
        ></input>
        <label>Password</label>
        <input
          type="text"
          className={inputStyle}
          onChange={(e) => {
            setPassword(e.target.value);
          }}
        ></input>
        <label>Name</label>
        <input
          type="text"
          className={inputStyle}
          onChange={(e) => {
            setName(e.target.value);
          }}
        ></input>
        <label>Role</label>
        <input
          type="text"
          className={inputStyle}
          onChange={(e) => {
            setRole(e.target.value);
          }}
        ></input>
        <button
          className="bg-slate-300 rounded border-black"
          onClick={async (e) => {
            e.preventDefault();
            register();
          }}
        >
          Register
        </button>
      </form>
    </div>
  );
};

export default AddNewUser;
