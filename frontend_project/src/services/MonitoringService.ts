import axios from "axios";
import DeviceDto from "../interfaces/DeviceDto";
import { ResponseDto } from "../interfaces/ResponseDto";
import authHeader from "./authHeader";
import MeasurementRequest from "../interfaces/MeasurementRequest";
import HourlyEntity from "../interfaces/HourlyEntity";
const MonitoringServiceUri = `http://localhost:7012/api/`;
export default class MonitoringService {
  public static async getAllMeasurementsForDay(
    request: MeasurementRequest
  ): Promise<ResponseDto<HourlyEntity[]>> {
    try {
      const response = await axios.post(
        `${MonitoringServiceUri}Measurements`,
        JSON.stringify(request),
        {
          headers: { "Content-Type": "application/json", ...authHeader() },
        }
      );
      let data = response.data as HourlyEntity[];
      return { isSuccess: true, message: "", result: data };
    } catch (error) {
      console.log(error);
    }
    return { isSuccess: false, message: "Error" } as ResponseDto<
      HourlyEntity[]
    >;
  }
}
