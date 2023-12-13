import React, { ReactComponentElement, ReactElement, useState } from "react";
import DeviceDto from "../../interfaces/DeviceDto";
import { UserDto } from "../../interfaces/UserDto";
import { act } from "react-dom/test-utils";
export interface DeviceRowComponentProps {
  device: DeviceDto;
  action?: (id: string) => void;
  onEdit?: (device: DeviceDto) => void;
  icon?: ReactElement;
}
const DeviceRowComponent: React.FC<DeviceRowComponentProps> = ({
  device,
  action,
  icon,
  onEdit,
}) => {
  const tableCell = `table-cell  border-b-2 border-slate-400 p-2`;

  const [id, setId] = useState(device.id);
  const [address, setAddress] = useState(device.address);
  const [description, setDescription] = useState(device.description);
  const [maximumHourlyEnergyConsumption, setConsumption] = useState(
    device.maximumHourlyEnergyConsumption
  );
  return (
    <div className="table-row odd:bg-white ">
      <div className={tableCell}>{device.id}</div>
      <div className={tableCell}>
        <input
          className="bg-transparent focus:border-0"
          value={address}
          onChange={(e) => {
            setAddress(e.target.value);
          }}
          disabled={onEdit ? false : true}
          onBlur={() => {
            if (onEdit)
              onEdit({
                id,
                address,
                description,
                maximumHourlyEnergyConsumption,
              });
          }}
        ></input>
      </div>
      <div className={tableCell}>
        <input
          className="bg-transparent focus:border-0"
          value={description}
          onChange={(e) => {
            setDescription(e.target.value);
          }}
          disabled={onEdit ? false : true}
          onBlur={() => {
            if (onEdit)
              onEdit({
                id,
                address,
                description,
                maximumHourlyEnergyConsumption,
              });
          }}
        ></input>
      </div>
      <div className={tableCell}>
        <input
          className="bg-transparent focus:border-0"
          value={maximumHourlyEnergyConsumption}
          onChange={(e) => {
            setConsumption(e.target.value);
          }}
          disabled={onEdit ? false : true}
          onBlur={() => {
            if (onEdit)
              onEdit({
                id,
                address,
                description,
                maximumHourlyEnergyConsumption,
              });
          }}
        ></input>
      </div>
      <div className={tableCell}>
        <button
          onClick={(e) => {
            if (action) action(device.id!);
          }}
        >
          {icon}
        </button>
      </div>
    </div>
  );
};

export default DeviceRowComponent;
