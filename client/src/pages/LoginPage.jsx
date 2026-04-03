import { Link, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { login } from '../api/authApi';
import { useAuth } from '../contexts/AuthContext';

export default function LoginPage() {
  const [form, setForm] = useState({ email: '', password: '' });
  const [error, setError] = useState('');
  const { loginUser } = useAuth();
  const navigate = useNavigate();

  const submit = async (event) => {
    event.preventDefault();
    try {
      const data = await login(form);
      loginUser(data);
      navigate('/chat');
    } catch (err) {
      setError(err?.response?.data?.message || 'Login failed');
    }
  };

  return (
    <main className="min-h-screen grid place-items-center p-4">
      <form onSubmit={submit} className="w-full max-w-md bg-white rounded-xl shadow p-6 space-y-4">
        <h1 className="text-2xl font-semibold">Welcome back</h1>
        <input className="w-full border rounded-lg px-3 py-2" placeholder="Email" type="email" value={form.email} onChange={(e) => setForm((prev) => ({ ...prev, email: e.target.value }))} />
        <input className="w-full border rounded-lg px-3 py-2" placeholder="Password" type="password" value={form.password} onChange={(e) => setForm((prev) => ({ ...prev, password: e.target.value }))} />
        {error && <p className="text-red-500 text-sm">{error}</p>}
        <button type="submit" className="w-full bg-emerald-500 text-white rounded-lg py-2">Login</button>
        <p className="text-sm text-slate-500">No account? <Link to="/register" className="text-emerald-600">Register</Link></p>
      </form>
    </main>
  );
}
