export interface ChatMessage {
    sender_id: string;
    receiver_id: string;
    message_content: string;
    type: string;
}