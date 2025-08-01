export interface Auth {
  token: string;
  userId: number; // Assuming the backend returns userId upon login
}