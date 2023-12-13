import React, { useEffect, useState } from "react";
import AdminUsersDashboard from "../components/usersCRUD/AdminUsersDashboard";
import { UserDto } from "../interfaces/UserDto";
import AuthService from "../services/AuthService";

const AdminUsersView = () => {
  const [users, setUsers] = useState<UserDto[]>([]);
  useEffect(() => {
    AuthService.getAllUsers().then((users) => {
      if (users.isSuccess) setUsers(users.result!);
    });
  }, []);
  return (
    <div className="w-full h-full">
      <div className="w-full bg-slate-100">
        <AdminUsersDashboard users={users} />
      </div>
    </div>
  );
};

export default AdminUsersView;
