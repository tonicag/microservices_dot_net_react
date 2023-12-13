import React, { useEffect, useState } from "react";
import AdminDevicesDashboard from "../components/devicesCRUD/AdminDevicesDashboard";
import AddIcon from "../icons/addIcon";
import DeleteIcon from "../icons/deleteIcon";
import DeviceDto from "../interfaces/DeviceDto";
import AuthService from "../services/AuthService";
import DeviceService from "../services/DeviceService";
import { toast } from "react-toastify";

const AdminDevicesView = () => {
  const [devices, setDevices] = useState<DeviceDto[]>([]);

  const deleteDevice = async (id: string) => {
    const response = await DeviceService.deleteDevice(id);
    if (response.isSuccess) {
      toast.success("Sucess in deleting the device!");
      window.location.reload();
    } else {
      toast.error("Error in deleting the device!");
    }
  };

  const updateHandler = async (device: DeviceDto) => {
    if (!device.id) return;

    const response = await DeviceService.updateDevice(device);
    if (response.isSuccess) {
      toast.success("Succesfully updated device!");
    } else {
      toast.error("Error in updating the device!");
    }
  };

  useEffect(() => {
    console.log("SSS");
    DeviceService.getAllDevices().then((res) => {
      if (res.isSuccess) {
        setDevices(res.result!);
      } else {
      }
    });
  }, []);

  return (
    <div className="w-full h-full">
      <div className="w-full bg-slate-100">
        <AdminDevicesDashboard
          action={deleteDevice}
          icon={DeleteIcon()}
          onEdit={(device) => {
            updateHandler(device);
          }}
          devices={devices}
        />
      </div>
    </div>
  );
};

export default AdminDevicesView;
