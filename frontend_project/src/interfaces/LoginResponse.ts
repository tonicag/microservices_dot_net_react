import { UserDto } from "./UserDto";

export default interface LoginResponse {
  user: UserDto;
  token: string;
}
