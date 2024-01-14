import React from "react";

export interface MessageComponentProps {
  id?: string;
  isSeen: boolean;
  isIncoming: boolean;
  messageContent: string;
  username: string;
}

const MessageComponent: React.FC<MessageComponentProps> = (
  props: MessageComponentProps
) => {
  let infoClassName = `flex flex-col-reverse ${
    !props.isIncoming ? "" : "items-end"
  } justify-between`;
  let orderClassName = `flex w-full ${
    props.isIncoming === true ? "flex-row" : "flex-row-reverse"
  } justify-between bg-slate-300`;
  console.log(orderClassName);
  return (
    <div className={orderClassName}>
      <div className="max-w-[50%] rounded-lg border-2 bg-blue-500 p-2">
        Lorem ipsum dolor sit amet consectetur, adipisicing elit. Laboriosam,
        dolores in, nisi fugit repellendus eum iusto voluptatum suscipit
        provident, itaque magni numquam! Obcaecati officia, ducimus voluptatum
        illo repudiandae debitis minima.
      </div>
      <div className={infoClassName}>
        <span>Sent</span>
        <div>User Name</div>
      </div>
    </div>
  );
};

export default MessageComponent;
