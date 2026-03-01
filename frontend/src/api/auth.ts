import { api } from "./client";

export type RegisterRequest = {
    email: string;
    password: string;
};

export type LoginRequest = {
    email: string;
    password: string;
};

export type AuthResponse = {
    accessToken: string;
    refreshToken?: string;
    user?: {
        id: string;
        email: string;
    };
};

export async function login(body: LoginRequest) {
    const res = await api.post<AuthResponse>("/auth/login", body);
    return res.data;
}

export async function register(body: RegisterRequest) {
    const res = await api.post<AuthResponse>("/auth/register", body);
    return res.data;
}
