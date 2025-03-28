import axios from 'axios';
const API = import.meta.env.VITE_API_BASE_URL;

export const moveArm = (data, token) =>
  axios.post(`${API}/roboticarm/move`, data, {
    headers: { Authorization: `Bearer ${token}` },
  });