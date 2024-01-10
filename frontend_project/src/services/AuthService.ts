import axios from "axios";
import { UserDto } from "../interfaces/UserDto";
import { LoginRequest } from "../interfaces/LoginRequest";
import { ResponseDto } from "../interfaces/ResponseDto";
import LoginResponse from "../interfaces/LoginResponse";
import authHeader from "./authHeader";
import DeviceDto from "../interfaces/DeviceDto";

export const AuthServiceURL = "https://localhost:7005/";
const DeviceServiceUri = "https://localhost:7075";
export default class AuthService {
  constructor() {}
  public static async login(
    user: LoginRequest
  ): Promise<ResponseDto<LoginResponse>> {
    try {
      const response = await axios.post(
        `${AuthServiceURL}api/auth/login`,
        JSON.stringify(user),
        { headers: { "Content-Type": "application/json" } }
      );
      let dtoResponse = response.data as ResponseDto<LoginResponse>;
      if (dtoResponse.result)
        dtoResponse.result.user.token = dtoResponse.result.token;
      localStorage.setItem("user", JSON.stringify(dtoResponse.result));
      const parsedToken = JSON.parse(
        atob(dtoResponse.result!.token.split(".")[1])
      );
      localStorage.setItem("role", parsedToken.role);
      return dtoResponse;
    } catch (error) {
      return { isSuccess: false, message: "Error", result: undefined };
    }
  }

  public static async register(user: UserDto): Promise<ResponseDto<string>> {
    try {
      const response = await axios.post(
        `${AuthServiceURL}api/auth/register`,
        JSON.stringify(user),
        { headers: { "Content-Type": "application/json", ...authHeader() } }
      );
      let data = response.data as ResponseDto<string>;
      console.log(data);
      return data;
    } catch (error) {
      return {
        isSuccess: false,
        message: "Registration error!",
        result: undefined,
      };
    }
  }

  public static async getAllUsers(): Promise<ResponseDto<UserDto[]>> {
    try {
      const response = await axios.get(
        `${AuthServiceURL}api/auth/GetAllUsers`,
        { headers: { "Content-Type": "application/json", ...authHeader() } }
      );
      let data = response.data as ResponseDto<UserDto[]>;
      console.log(data);
      return data;
    } catch (error) {
      return {
        isSuccess: false,
        message: "Registration error!",
        result: undefined,
      };
    }
  }
  public static async deleteUser(id: string): Promise<ResponseDto<string>> {
    try {
      const response = await axios.delete(`${AuthServiceURL}api/auth/${id}`, {
        headers: { "Content-Type": "application/json", ...authHeader() },
      });
      let data = response.data as ResponseDto<string>;
      console.log(data);
      return data;
    } catch (error) {
      return {
        isSuccess: false,
        message: "Error in deleting user",
        result: undefined,
      };
    }
  }
  public static async updateUser(user: UserDto): Promise<ResponseDto<string>> {
    try {
      const response = await axios.put(
        `${AuthServiceURL}api/auth/`,
        JSON.stringify(user),
        {
          headers: { "Content-Type": "application/json", ...authHeader() },
        }
      );
      let data = response.data as ResponseDto<string>;
      return data;
    } catch (error) {
      return {
        isSuccess: false,
        message: "Error in updating user",
        result: undefined,
      };
    }
  }
}
