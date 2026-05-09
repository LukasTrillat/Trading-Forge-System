import { useState } from 'react';
import { useAuthStore } from '../Store/authStore';
import { IdentityService } from '../../Infrastructure/Services/IdentityService';
import type { RegisterTraderCommand } from '../DTOs/Commands/RegisterTraderCommand';
import type { LoginTraderQuery } from '../DTOs/Queries/LoginTraderQuery';

const identityService = new IdentityService();

/** Mirrors RegisterTraderCommandHandler + LoginTraderQueryHandler from the backend. */
export function useAuth() {
  const { setToken, logout, isAuthenticated, trader } = useAuthStore();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function register(command: RegisterTraderCommand): Promise<boolean> {
    setIsLoading(true);
    setError(null);
    const result = await identityService.register(command);
    setIsLoading(false);
    if (!result.isSuccess) {
      setError(result.errorMessage ?? 'Registration failed.');
      return false;
    }
    return true;
  }

  async function login(query: LoginTraderQuery): Promise<boolean> {
    setIsLoading(true);
    setError(null);
    const result = await identityService.login(query);
    setIsLoading(false);
    if (!result.isSuccess) {
      setError(result.errorMessage ?? 'Invalid credentials.');
      return false;
    }
    setToken(result.value!);
    return true;
  }

  return { register, login, logout, isAuthenticated, trader, isLoading, error };
}
