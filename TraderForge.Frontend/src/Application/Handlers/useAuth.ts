import { useState } from 'react';
import { useAuthStore } from '../Store/authStore';
import type { RegisterTraderCommand } from '../DTOs/Commands/RegisterTraderCommand';
import type { LoginTraderQuery } from '../DTOs/Queries/LoginTraderQuery';
import { IdentityService } from '../../Infrastructure/Services/IdentityService';

const identityService = new IdentityService();

export function useAuth() {
  const { setToken, logout, isAuthenticated, trader } = useAuthStore();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function register(command: RegisterTraderCommand): Promise<boolean> {
    setIsLoading(true);
    setError(null);
    const result = await identityService.register(command);
    setIsLoading(false);
    if (!result.isSuccess) setError(result.error || 'Registration failed.');
    return result.isSuccess;
  }

  async function login(query: LoginTraderQuery): Promise<boolean> {
    setIsLoading(true);
    setError(null);
    
    const result = await identityService.login(query);
    
    setIsLoading(false);
    
    // Check if the token string exists in the result value
    if (result.isSuccess && result.value) {
      setToken(result.value); // result.value is the actual JWT string
      return true;
    } else {
      setError(result.error || 'Login failed.');
      return false;
    }
  }

  return { register, login, logout, isAuthenticated, trader, isLoading, error };
}
