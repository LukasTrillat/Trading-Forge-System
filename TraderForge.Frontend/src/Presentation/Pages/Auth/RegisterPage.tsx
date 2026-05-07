import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../../Application/Handlers/useAuth';
import { Button } from '../../Components/UI/Button';
import { Input } from '../../Components/UI/Input';

export function RegisterPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirm, setConfirm] = useState('');
  const [localError, setLocalError] = useState('');

  const { register, isLoading, error } = useAuth();
  const navigate = useNavigate();

  const passwordRules = [
    { label: 'At least 8 characters', ok: password.length >= 8 },
    { label: 'One uppercase letter (A-Z)', ok: /[A-Z]/.test(password) },
    { label: 'One number (0-9)', ok: /[0-9]/.test(password) },
    { label: 'One special character (!@#$…)', ok: /[^a-zA-Z0-9]/.test(password) },
  ];

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setLocalError('');

    if (password !== confirm) {
      setLocalError('Passwords do not match.');
      return;
    }
    if (passwordRules.some((r) => !r.ok)) {
      setLocalError('Password does not meet the requirements below.');
      return;
    }

    const success = await register({ email, password });
    if (success) navigate('/login', { state: { registered: true } });
  }

  const displayError = localError || error;

  return (
    <div className="bg-neutral-900 border border-neutral-800 rounded-xl p-6 flex flex-col gap-5">
      <div>
        <h2 className="text-lg font-semibold text-neutral-100">Create account</h2>
        <p className="text-sm text-neutral-500 mt-0.5">Start your 7-day free trial — no credit card required</p>
      </div>

      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
        <Input
          id="email"
          label="Email"
          type="email"
          placeholder="trader@example.com"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
          autoComplete="email"
        />
        <Input
          id="password"
          label="Password"
          type="password"
          placeholder="Min. 8 characters, include a number"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          autoComplete="new-password"
        />
        <Input
          id="confirm"
          label="Confirm password"
          type="password"
          placeholder="••••••••"
          value={confirm}
          onChange={(e) => setConfirm(e.target.value)}
          required
          autoComplete="new-password"
        />

        {password.length > 0 && (
          <ul className="flex flex-col gap-1">
            {passwordRules.map((r) => (
              <li key={r.label} className={`flex items-center gap-2 text-xs ${r.ok ? 'text-emerald-400' : 'text-neutral-500'}`}>
                <span>{r.ok ? '✓' : '○'}</span>
                {r.label}
              </li>
            ))}
          </ul>
        )}

        {displayError && (
          <p className="text-sm text-red-400 bg-red-500/10 border border-red-500/20 rounded-lg px-3 py-2">
            {displayError}
          </p>
        )}

        <Button type="submit" isLoading={isLoading} className="w-full">
          Create account
        </Button>
      </form>

      <p className="text-center text-sm text-neutral-500">
        Already have an account?{' '}
        <Link to="/login" className="text-emerald-400 hover:text-emerald-300 transition-colors">
          Sign in
        </Link>
      </p>
    </div>
  );
}
