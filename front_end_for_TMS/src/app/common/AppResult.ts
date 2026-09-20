export interface AppResult<T> {
  path: string;
  httpStatus: number;
  isSuccess: boolean;
  value: T;
  code: number;
  message: string;
}