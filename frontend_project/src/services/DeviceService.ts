import axios from "axios";
import DeviceDto from "../interfaces/DeviceDto";
import { ResponseDto } from "../interfaces/ResponseDto";
import authHeader from "./authHeader";

const DeviceServiceUri = "http://localhost:7075/";

export default class DeviceService {
  public static async createDevice(
    device: DeviceDto
  ): Promise<ResponseDto<DeviceDto>> {
    try {
      const response = await axios.post(
        `${DeviceServiceUri}api/device`,
        JSON.stringify(device),
        { headers: { "Content-Type": "application/json", ...authHeader() } }
      );
      const data = response.data as ResponseDto<DeviceDto>;
      return data;
    } catch (error) {
      return {
        isSuccess: false,
        message: "Error in creating device!",
        result: undefined,
      };
    }
  }

  public static async getAllDevices(
    userId?: string
  ): Promise<ResponseDto<DeviceDto[]>> {
    try {
      const response = await axios.get(
        `${DeviceServiceUri}api/device/GetAllDevices${
          userId ? `/?userId=${userId}` : ""
        }`,
        { headers: { "Content-Type": "application/json", ...authHeader() } }
      );
      return response.data as ResponseDto<DeviceDto[]>;
    } catch (error) {
      console.log(error);
    }
    return { isSuccess: false, message: "Error" } as ResponseDto<DeviceDto[]>;
  }

  public static async getAllAvailableDevices(): Promise<
    ResponseDto<DeviceDto[]>
  > {
    try {
      const response = await axios.get(
        `${DeviceServiceUri}api/device/GetAllAvailableDevices`,
        { headers: { "Content-Type": "application/json", ...authHeader() } }
      );
      return response.data as ResponseDto<DeviceDto[]>;
    } catch (error) {
      console.log(error);
    }
    return { isSuccess: false, message: "Error" } as ResponseDto<DeviceDto[]>;
  }

  public static async deleteDevice(
    deviceId?: string
  ): Promise<ResponseDto<string>> {
    try {
      const response = await axios.delete(
        `${DeviceServiceUri}api/device/${deviceId}`,
        { headers: { "Content-Type": "application/json", ...authHeader() } }
      );
      return response.data as ResponseDto<string>;
    } catch (error) {
      console.log(error);
    }
    return { isSuccess: false, message: "Error" } as ResponseDto<string>;
  }

  public static async deleteDeviceMapping(request: {
    userId: string;
    deviceId: string;
  }): Promise<ResponseDto<string>> {
    try {
      const response = await axios.post(
        `${DeviceServiceUri}api/device/DeleteDeviceMapping`,
        JSON.stringify(request),
        { headers: { "Content-Type": "application/json", ...authHeader() } }
      );
      return response.data as ResponseDto<string>;
    } catch (error) {
      console.log(error);
    }
    return { isSuccess: false, message: "Error" } as ResponseDto<string>;
  }

  public static async addDeviceMapping(request: {
    userId: string;
    deviceId: string;
  }): Promise<ResponseDto<string>> {
    try {
      const response = await axios.post(
        `${DeviceServiceUri}api/device/AddDeviceMapping`,
        JSON.stringify(request),
        { headers: { "Content-Type": "application/json", ...authHeader() } }
      );
      return response.data as ResponseDto<string>;
    } catch (error) {
      console.log(error);
    }
    return { isSuccess: false, message: "Error" } as ResponseDto<string>;
  }

  public static async updateDevice(
    device: DeviceDto
  ): Promise<ResponseDto<string>> {
    try {
      const response = await axios.put(
        `${DeviceServiceUri}api/device`,
        JSON.stringify(device),
        { headers: { "Content-Type": "application/json", ...authHeader() } }
      );
      return response.data as ResponseDto<string>;
    } catch (error) {
      console.log(error);
    }
    return { isSuccess: false, message: "Error" } as ResponseDto<string>;
  }
}
