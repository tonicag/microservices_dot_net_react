import { UserDto } from "../interfaces/UserDto";

export default function authHeader() {
  let userString = localStorage.getItem("user");
  if (!userString) return {};
  let user = JSON.parse(userString) as UserDto;

  if (user && user.token) {
    return { Authorization: "Bearer " + user.token };
  } else {
    return {};
  }
}
export function getToken() {
  let userString = localStorage.getItem("user");
  if (!userString) return {};
  let user = JSON.parse(userString) as UserDto;

  if (user && user.token) {
    return user.token;
  } else {
    return undefined;
  }
}
