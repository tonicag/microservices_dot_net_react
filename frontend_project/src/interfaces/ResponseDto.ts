export interface ResponseDto<T> {
  isSuccess: true | false;
  message: string;
  result: T | undefined;
}
