import React from "react";
import { UserDto } from "../../interfaces/UserDto";
import UserRowComponent from "./UserRowComponent";

const AdminUsersDashboard: React.FC<{ users: UserDto[] }> = ({ users }) => {
  const tableCell = `table-cell border-b-2 border-slate-400 p-2`;
  return (
    <div className="table w-full">
      <div className="table-header-group">
        <div className="table-row ">
          <div className={tableCell}>Id</div>
          <div className={tableCell}>Email</div>
          <div className={tableCell}>Name</div>
          <div className={tableCell}>Password</div>
          <div className={tableCell}></div>
        </div>
      </div>
      <div className="table-row-group">
        {users.map((user, index) => {
          return <UserRowComponent key={index} user={user} />;
        })}
      </div>
    </div>
  );
};

export default AdminUsersDashboard;
