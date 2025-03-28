import axios from 'axios';
const API = import.meta.env.VITE_API_BASE_URL;

export const login = (data) => axios.post(`${API}/user/login`, data);
export const register = (data) => axios.post(`${API}/user/register`, data);