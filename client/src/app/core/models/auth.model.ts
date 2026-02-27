export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  displayName?: string;
}

export interface AuthResponse {
  userId: string;
  username: string;
  email: string;
  displayName?: string;
  token: string;
}
