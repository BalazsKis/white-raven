import { Error } from './error';

export class Response<T> {
  data: T;
  errors: Error[];
}
