import React, { useState } from "react";
import { UserDto } from "../../interfaces/UserDto";
import { toast, ToastContainer } from "react-toastify";
import AuthService from "../../services/AuthService";
import { Link, Navigate } from "react-router-dom";

const UserRowComponent: React.FC<{ user: UserDto }> = ({ user }) => {
  const tableCell = `table-cell  border-b-2 border-slate-400 p-2`;

  const [username, setUsername] = useState(user.name);
  const [id, setId] = useState(user.id);
  const [email, setEmail] = useState(user.email);
  const [password, setPassword] = useState("");

  console.log(user);

  const handleUpdate = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const response = await AuthService.updateUser({
      id: id,
      email,
      password,
      name: username,
    });

    if (response.isSuccess) {
      toast.success("Success updating the user!");
    } else {
      toast.error("Error in updating the user!");
    }
  };
  const handleDelete = async () => {
    const response = await AuthService.deleteUser(id);
    if (response.isSuccess) {
      toast.success("Succesfully deleted user!");
      window.location.reload();
    } else {
      toast.error("Error in deleting user!");
    }
  };

  return (
    <>
      {" "}
      <div className="table-row odd:bg-white ">
        <div className={tableCell}>
          <Link to={`/admin/mappings/${id}`}>{id}</Link>
        </div>

        <div className={tableCell}>
          <input
            className="bg-transparent focus:border-0"
            value={email}
            onChange={(e) => {
              setEmail(e.target.value);
            }}
            onBlur={handleUpdate}
          ></input>
        </div>
        <div className={tableCell}>
          <input
            className="bg-transparent focus:border-0"
            value={username}
            onChange={(e) => {
              setUsername(e.target.value);
            }}
            onBlur={handleUpdate}
          ></input>
        </div>
        <div className={tableCell}>
          <input
            className="bg-transparent focus:border-0"
            value={password}
            onChange={(e) => {
              setPassword(e.target.value);
            }}
            onBlur={handleUpdate}
          ></input>
        </div>
        <div className={tableCell}>
          <button
            onClick={async (e) => {
              console.log("Deleting ", id);
              await handleDelete();
            }}
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
              className="w-6 h-6"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M14.74 9l-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 01-2.244 2.077H8.084a2.25 2.25 0 01-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 00-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 013.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 00-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 00-7.5 0"
              />
            </svg>
          </button>
        </div>
      </div>
    </>
  );
};

export default UserRowComponent;
