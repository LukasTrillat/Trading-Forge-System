import { useState } from 'react';
import { useAuthStore } from '../Store/authStore';
import type { RegisterTraderCommand } from '../DTOs/Commands/RegisterTraderCommand';
import type { LoginTraderQuery } from '../DTOs/Queries/LoginTraderQuery';
import { httpClient } from '../../Infrastructure/Http/httpClient';

export function useAuth() {
  const { setToken, logout, isAuthenticated, trader } = useAuthStore();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function register(command: RegisterTraderCommand): Promise<boolean> {
    setIsLoading(true);
    setError(null);
    try {
      await httpClient.post('/api/identity/register', command);
      setIsLoading(false);
      return true;
    } catch (err: any) {
      setIsLoading(false);
      setError(err.response?.data?.error || 'Registration failed.');
      return false;
    }
  }

  async function login(query: LoginTraderQuery): Promise<boolean> {
    setIsLoading(true);
    setError(null);
    
    try {
      const response = await httpClient.post<{ token: string }>('/api/identity/login', query);
      if (response.data && response.data.token) {
        setToken(response.data.token);
        setIsLoading(false);
        return true;
      }
      setIsLoading(false);
      setError('Login failed: Token missing.');
      return false;
    } catch (err: any) {
      setIsLoading(false);
      setError(err.response?.data?.error || 'Invalid credentials.');
      return false;
    }
  }

  return { register, login, logout, isAuthenticated, trader, isLoading, error };
}
