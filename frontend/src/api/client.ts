import axios from "axios";

// const baseURL = import.meta.env.VITE_API_BASE_URL;

export const api = axios.create({
    baseURL: "https://localhost:7049/",
    withCredentials: false,
    timeout: 15000,
});