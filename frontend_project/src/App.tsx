import React, { useContext, useEffect, useState } from "react";
import "./App.css";
import AuthService from "./services/AuthService";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import LoginView from "./views/LoginView";
import { UserDto } from "./interfaces/UserDto";
import Navbar from "./components/Navbar";
import Home from "./views/Home";
import AuthorisedRoute from "./components/security/SecurityRoutes";
import LogOut from "./views/LogOut";
import AdminUsersView from "./views/AdminUsersView";
import AddNewUser from "./components/usersCRUD/AddNewUser";
import AdminDevicesView from "./views/AdminDevicesView";
import AddNewDevice from "./components/devicesCRUD/AddNewDevice";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import UserMappingsView from "./views/UserMappingsView";
import ClientDevicesView from "./views/ClientDevicesView";
import { createContext } from "react";
import AdminChat from "./views/AdminChat";

export interface AppState {
  user: UserDto;
}

export interface AuthenticationContextType {
  isAuthenticated: boolean;
  setAuthenticated: (value: boolean) => void;
  isAdmin: boolean;
  setAdmin: (value: boolean) => void;
}
export const AuthenticationContext = createContext<AuthenticationContextType>({
  isAuthenticated: false,
  setAuthenticated: () => {},
  isAdmin: false,
  setAdmin: () => {},
});
function App() {
  const [isAuthenticated, setAuthenticated] = useState<boolean>(false);
  const [isAdmin, setAdmin] = useState<boolean>(false);
  const cnt = useContext(AuthenticationContext);
  useEffect(() => {
    if (localStorage.getItem("user")) {
      setAuthenticated(true);
    }
  }, []);

  return (
    <>
      <AuthenticationContext.Provider
        value={{ isAuthenticated, setAuthenticated, isAdmin, setAdmin }}
      >
        <Navbar />
        <Routes>
          <Route index path="/" element={<Home />} />
          <Route path="/login" element={<LoginView />} />
          <Route path="/logout" element={<LogOut />} />
          <Route path="/admin" element={<AuthorisedRoute />}>
            <Route path="" element={<AdminUsersView />}></Route>
            <Route path="users" element={<AdminUsersView />}></Route>
            <Route path="users/add" element={<AddNewUser />}></Route>
            <Route
              path="mappings/:userId"
              element={<UserMappingsView />}
            ></Route>
            <Route path="devices/" element={<AdminDevicesView />}></Route>
            <Route path="devices/add" element={<AddNewDevice />}></Route>
          </Route>
          <Route path="/admin/chats" element={<AdminChat></AdminChat>}></Route>
          <Route path="/client/" element={<ClientDevicesView />} />
        </Routes>
        <ToastContainer
          position="bottom-right"
          autoClose={5000}
          hideProgressBar={false}
          newestOnTop={false}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss
          draggable
          pauseOnHover
          theme="light"
        />
      </AuthenticationContext.Provider>
    </>
  );
}
//element={<AuthorisedRoute />}
export default App;
