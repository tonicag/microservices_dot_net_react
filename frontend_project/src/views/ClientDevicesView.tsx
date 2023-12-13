import React, { useEffect, useState } from "react";
import { UserDto } from "../interfaces/UserDto";
import DeviceService from "../services/DeviceService";
import DeviceDto from "../interfaces/DeviceDto";
import DeviceRowComponent from "../components/devicesCRUD/DeviceRowComponent";
import { useNavigate } from "react-router-dom";
import { getToken } from "../services/authHeader";
import AlertDto from "../interfaces/AlertDto";
import { toast } from "react-toastify";
import Calendar from "react-calendar";
import { LineChart } from "@mui/x-charts/LineChart";

import "react-calendar/dist/Calendar.css";
import MonitoringService from "../services/MonitoringService";
import HourlyEntity from "../interfaces/HourlyEntity";
import DeleteIcon from "../icons/deleteIcon";
import SelectIcon from "../icons/selectIcon";
type ValuePiece = Date | null;

type Value = ValuePiece | [ValuePiece, ValuePiece];
const ClientDevicesView = () => {
  const [user, setUser] = useState<UserDto>();
  const [devices, setDevices] = useState<DeviceDto[]>([]);
  const [socket, setSocket] = useState<WebSocket>();
  const [dateValue, setDate] = useState<Value>(new Date());
  const [measurements, setMeasurements] = useState<HourlyEntity[]>();

  const [selectedDevice, setDevice] = useState<string>("");

  const chartData = [10, 20, 15, 25, 30];
  const chartLabels = ["Label 1", "Label 2", "Label 3", "Label 4", "Label 5"];

  const tableCell = `table-cell border-b-2 border-slate-400 p-2`;
  const navigate = useNavigate();

  const handleDataChange = async () => {
    if (!selectedDevice) return;
    if (!dateValue) return;
    const resp = await MonitoringService.getAllMeasurementsForDay({
      date: dateValue as Date,
      device_id: selectedDevice,
    });
    if (resp.isSuccess) {
      setMeasurements(resp.result);
    }
  };

  useEffect(() => {
    if (!localStorage.getItem("user")) {
      navigate("/login");
      return;
    }
    const token = getToken();

    const response = JSON.parse(localStorage.getItem("user")!);

    setUser(response.user);
    console.log(token);
    const ws = new WebSocket(`ws://localhost:7012/ws?token=${token}`);
    ws.onopen = () => {
      console.log("WebSocket connection opened");
    };

    ws.onmessage = async (event) => {
      const alert = (await JSON.parse(event.data)) as AlertDto;
      console.log(alert);
      toast.warning(`Device: ${alert.DeviceID}\n Message: ${alert.Message}`);
    };

    ws.onclose = () => {
      console.log("WebSocket connection closed");
    };

    // Save the WebSocket instance in state
    setSocket(ws);

    // Clean up the WebSocket connection when the component is unmounted

    DeviceService.getAllDevices(response.user!.id).then((response) => {
      setDevices(response.result!);
    });
    return () => {
      ws.close();
    };
  }, []);

  return (
    <>
      <div className="table w-full">
        <div className="table-header-group">
          <div className="table-row ">
            <div className={tableCell}>Id</div>
            <div className={tableCell}>Address</div>
            <div className={tableCell}>Descripiton</div>
            <div className={tableCell}>Consumption</div>
            <div className={tableCell}></div>
          </div>
        </div>
        <div className="table-row-group">
          {devices.map((d, index) => {
            return (
              <DeviceRowComponent
                key={index}
                device={d}
                action={(device_id: string) => {
                  setDevice(device_id);
                  handleDataChange();
                }}
                icon={SelectIcon()}
              />
            );
          })}
        </div>
      </div>
      <Calendar
        onChange={(value) => {
          console.log(value);
          setDate(value);
          handleDataChange();
        }}
        value={dateValue}
      />
      {measurements ? (
        <LineChart
          xAxis={[
            {
              data: measurements.map((m) => m.id),
            },
          ]}
          series={[
            {
              data: measurements?.map((m) => m.value),
            },
          ]}
          width={500}
          height={300}
        />
      ) : (
        <></>
      )}
    </>
  );
};

export default ClientDevicesView;
