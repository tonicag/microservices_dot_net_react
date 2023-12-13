import React, { ReactElement } from "react";
import DeviceDto from "../../interfaces/DeviceDto";
import DeviceRowComponent from "./DeviceRowComponent";
import { Icons } from "react-toastify";
interface AdminDevicesDashboardProps {
  devices: DeviceDto[];
  action: (id: string) => void;
  icon: ReactElement;
  onEdit?: (device: DeviceDto) => void;
}
const AdminDevicesDashboard: React.FC<AdminDevicesDashboardProps> = ({
  devices,
  action,
  icon,
  onEdit,
}) => {
  const tableCell = `table-cell border-b-2 border-slate-400 p-2`;
  return (
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
              icon={icon}
              key={index}
              device={d}
              action={action}
              onEdit={onEdit}
            />
          );
        })}
      </div>
    </div>
  );
};

export default AdminDevicesDashboard;
