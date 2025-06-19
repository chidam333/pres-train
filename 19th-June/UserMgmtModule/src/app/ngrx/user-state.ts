import { UserModel } from "../user-model";

export interface UserState {
  users: UserModel[];
  loading: boolean;
  error: string | null;
}

export const initialUserState: UserState = {
  users: [],
  loading: false,
  error: null
};