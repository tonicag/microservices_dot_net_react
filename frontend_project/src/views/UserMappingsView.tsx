import React, { useEffect, useState } from "react";
import { UserDto } from "../interfaces/UserDto";
import AdminDevicesDashboard from "../components/devicesCRUD/AdminDevicesDashboard";
import AddIcon from "../icons/addIcon";
import DeleteIcon from "../icons/deleteIcon";
import DeviceDto from "../interfaces/DeviceDto";
import DeviceService from "../services/DeviceService";
import { useParams } from "react-router-dom";
import { ResponseDto } from "../interfaces/ResponseDto";
import { toast } from "react-toastify";
interface UserMappingsViewProps {
  user: UserDto;
}
const UserMappingsView = () => {
  const params = useParams();
  const tableCell = `table-cell border-b-2 border-slate-400 p-2`;

  const [userId, setUserId] = useState("");
  const [currentMappings, setCurrentMappings] = useState<DeviceDto[]>([
    {
      id: "1",
      address: "Tonica",
      description: "Manuel",
      maximumHourlyEnergyConsumption: "1234",
    },
    {
      id: "2",
      address: "Tonica",
      description: "Manuel",
      maximumHourlyEnergyConsumption: "1234",
    },
    {
      id: "3",
      address: "Tonica",
      description: "Manuel",
      maximumHourlyEnergyConsumption: "1234",
    },
    {
      id: "4",
      address: "Tonica",
      description: "Manuel",
      maximumHourlyEnergyConsumption: "1234",
    },
    {
      id: "5",
      address: "Tonica",
      description: "Manuel",
      maximumHourlyEnergyConsumption: "1234",
    },
  ]);
  const [availableDevices, setAvailableDevices] = useState<DeviceDto[]>([
    {
      id: "6",
      address: "Tonica",
      description: "Manuel",
      maximumHourlyEnergyConsumption: "1234",
    },
    {
      id: "7",
      address: "Tonica",
      description: "Manuel",
      maximumHourlyEnergyConsumption: "1234",
    },
    {
      id: "8",
      address: "Tonica",
      description: "Manuel",
      maximumHourlyEnergyConsumption: "1234",
    },
    {
      id: "9",
      address: "Tonica",
      description: "Manuel",
      maximumHourlyEnergyConsumption: "1234",
    },
    {
      id: "10",
      address: "Tonica",
      description: "Manuel",
      maximumHourlyEnergyConsumption: "1234",
    },
  ]);
  useEffect(() => {
    let id = params.userId;
    if (id) setUserId(id);
    DeviceService.getAllDevices(id).then((res: ResponseDto<DeviceDto[]>) => {
      if (res.isSuccess) {
        setCurrentMappings(res.result!);
      }
    });
    DeviceService.getAllAvailableDevices().then(
      (res: ResponseDto<DeviceDto[]>) => {
        if (res.isSuccess) {
          setAvailableDevices(res.result!);
        }
      }
    );
  }, []);
  const addMapping = async (id: string) => {
    const response = await DeviceService.addDeviceMapping({
      deviceId: id,
      userId: userId,
    });
    if (response.isSuccess) {
      let available = [...availableDevices];
      let toBeAdded = available.find((item) => item.id === id);
      available = available.filter((item) => item.id !== id);
      setAvailableDevices(available);
      if (toBeAdded) setCurrentMappings([...currentMappings, toBeAdded]);
    }
  };
  const deleteMapping = async (id: string) => {
    const response = await DeviceService.deleteDeviceMapping({
      deviceId: id,
      userId: userId,
    });
    if (!response.isSuccess) return;

    let current = [...currentMappings];
    let toBeAdded = current.find((item) => item.id === id);
    current = current.filter((item) => item.id !== id);
    setCurrentMappings(current);
    if (toBeAdded) setAvailableDevices([...availableDevices, toBeAdded]);
  };
  return (
    <>
      <div className="w-full h-20 bg-slate-300 flex items-center justify-center">
        <h1 className="text-[20pt] text-black ">Current user</h1>
      </div>
      <div className="table w-full">
        <div className="table-header-group">
          <div className="table-row ">
            <div className={tableCell}>Email</div>
            <div className={tableCell}>Password</div>
            <div className={tableCell}>Name</div>
            <div className={tableCell}></div>
          </div>
        </div>
      </div>
      <div className="w-full h-20 bg-slate-300 flex items-center justify-center">
        <h1 className="text-[20pt] text-black ">Devices owned</h1>
      </div>
      <AdminDevicesDashboard
        action={deleteMapping}
        icon={<DeleteIcon />}
        devices={currentMappings}
      />
      <div className="w-full h-20 bg-slate-300 flex items-center justify-center">
        <h1 className="text-[20pt] text-black ">Available devices</h1>
      </div>
      <AdminDevicesDashboard
        icon={<AddIcon />}
        action={addMapping}
        devices={availableDevices}
      />
    </>
  );
};

export default UserMappingsView;
