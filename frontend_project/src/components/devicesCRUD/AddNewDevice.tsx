import React, { useState } from "react";
import DeviceService from "../../services/DeviceService";
import { toast } from "react-toastify";

const AddNewDevice = () => {
  const [id, setId] = useState("");
  const [address, setAddress] = useState("");
  const [description, setDescription] = useState("");
  const [maximumHourlyEnergyConsumption, setConsumption] = useState("");

  const createDevice = async () => {
    let number = parseFloat(maximumHourlyEnergyConsumption);
    const response = await DeviceService.createDevice({
      address,
      description,
      maximumHourlyEnergyConsumption,
    });
    if (response.isSuccess) {
      toast.success("Sucessfully created device!");
    } else {
      toast.error("Error in createing device!");
    }
  };

  const inputStyle =
    "mb-2 p-1 text-sm font-medium border-2 border-black rounded text-gray-900 w-[100%]";
  return (
    <div className="w-full flex flex-col items-center">
      <form className="flex flex-col gap-3 w-[50%]">
        <label>Address</label>
        <input
          type="text"
          className={inputStyle}
          value={address}
          onChange={(e) => {
            setAddress(e.target.value);
          }}
        ></input>
        <label>Description</label>
        <input
          type="text"
          className={inputStyle}
          value={description}
          onChange={(e) => {
            setDescription(e.target.value);
          }}
        ></input>
        <label>Conspumption</label>
        <input
          type="text"
          className={inputStyle}
          value={maximumHourlyEnergyConsumption}
          onChange={(e) => {
            setConsumption(e.target.value);
          }}
        ></input>
        <button
          className="bg-slate-300 rounded border-black"
          onClick={async (e) => {
            e.preventDefault();
            createDevice();
          }}
        >
          Register
        </button>
      </form>
    </div>
  );
};

export default AddNewDevice;
