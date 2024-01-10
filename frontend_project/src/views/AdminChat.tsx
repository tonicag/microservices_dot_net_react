import React from "react";
import { ChatMessage } from "../interfaces/ChatMessage";
import UserChatDisplay from "../components/chat/UserChatDisplay";
import ChatComponent from "../components/chat/ChatComponent";

const chats: ChatMessage[] = [
  {
    message_content: "message",
    receiver_id: "123456",
    sender_id: "123456",
    type: "CHAT_INIT",
  },
  {
    message_content: "message",
    receiver_id: "123456",
    sender_id: "123456",
    type: "CHAT_INIT",
  },
  {
    message_content: "message",
    receiver_id: "123456",
    sender_id: "123456",
    type: "CHAT_INIT",
  },
  {
    message_content: "message",
    receiver_id: "123456",
    sender_id: "123456",
    type: "CHAT_INIT",
  },
  {
    message_content: "message",
    receiver_id: "123456",
    sender_id: "123456",
    type: "CHAT_INIT",
  },
];

const AdminChat: React.FC = () => {
  return (
    <div className="w-full h-full flex bg-blue-500">
      <div className="bg-red-100 w-1/3 h-full flex flex-col">
        {chats.map((t) => (
          <UserChatDisplay
            username={"username"}
            userId={t.receiver_id}
          ></UserChatDisplay>
        ))}
      </div>
      <div className="bg-green-100 w-2/3 h-full">
        <ChatComponent></ChatComponent>
      </div>
    </div>
  );
};

export default AdminChat;
