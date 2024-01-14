import React from "react";
interface UserChatDisplayProps {
  username: string;
  userId: string;
}
const UserChatDisplay: React.FC<UserChatDisplayProps> = (
  props: UserChatDisplayProps
) => {
  return (
    <div className="w-full h-14 bg-slate-100 flex justify-center items-center">
      <div className="text-bold cursor-pointer">{props.username}</div>
    </div>
  );
};

export default UserChatDisplay;
