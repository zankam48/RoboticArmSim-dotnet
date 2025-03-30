import React, { useState } from 'react';
import { register } from '../services/authService';

const Register = () => {
  const [form, setForm] = useState({ username: '', password: '' });

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await register(form);
      alert('Registered! Now login.');
    // eslint-disable-next-line no-unused-vars
    } catch (err) {
      alert('Register failed');
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input placeholder="Name" onChange={(e) => setForm({ ...form, username: e.target.value })} />
      <input placeholder="Password" type="password" onChange={(e) => setForm({ ...form, password: e.target.value })} />
      <button type="submit">Register</button>
    </form>
  );
};

export default Register;