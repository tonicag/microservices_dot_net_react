import React from "react";
import MessageComponent, { MessageComponentProps } from "./MessageComponent";
const chatMessages: MessageComponentProps[] = [
  {
    isIncoming: true,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: false,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: true,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: false,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: true,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: false,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: true,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: false,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: true,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: false,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: true,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: false,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: true,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: false,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: true,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: false,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: true,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: false,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: true,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: false,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: true,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: false,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: true,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
  {
    isIncoming: false,
    isSeen: false,
    messageContent: "lorem impsum",
    username: "Username1",
  },
];
const ChatComponent = () => {
  return (
    <div className="h-full w-full bg-slate-600">
      <div className="w-full h-[80%] bg-slate-200">
        <div className="flex h-full w-full flex-col gap-2 bg-slate-200 overflow-y-scroll p-2">
          {chatMessages.map((c) => (
            <MessageComponent {...c}></MessageComponent>
          ))}
        </div>
      </div>
      <div className="w-full h-[20%] flex">
        <textarea className="p-3 h-full w-[90%] border-black"></textarea>
        <button className="h-full w-[10%] text-black bg-red-100 text-center border-2 border-black">
          SEND
        </button>
      </div>
    </div>
  );
};

export default ChatComponent;
