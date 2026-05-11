import { useState } from 'react';
import { useAuthStore } from '../Store/authStore';
import type { RegisterTraderCommand } from '../DTOs/Commands/RegisterTraderCommand';
import type { LoginTraderQuery } from '../DTOs/Queries/LoginTraderQuery';
import { IdentityService } from '../../Infrastructure/Services/IdentityService';

// Instantiate the real service we set up earlier
const identityService = new IdentityService();

export function useAuth() {
  const { setToken, logout, isAuthenticated, trader } = useAuthStore();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function register(command: RegisterTraderCommand): Promise<boolean> {
    setIsLoading(true);
    setError(null);
    
    // Call the real C# backend via IdentityService
    const result = await identityService.register(command);
    
    setIsLoading(false);
    if (result.isSuccess) {
      return true;
    } else {
      setError(result.error || 'Registration failed');
      return false;
    }
  }

async function login(query: LoginTraderQuery): Promise<boolean> {
    setIsLoading(true);
    setError(null);
    
    const result = await identityService.login(query);
    
    setIsLoading(false);
    
    if (result.isSuccess && result.value?.token) {
      setToken(result.value.token);
      return true;
    } else {
      // This will now show the actual error (e.g., "Password mismatch") or the login failure
      setError(result.error || 'Login failed.');
      return false;
    }
  }


  return { register, login, logout, isAuthenticated, trader, isLoading, error };
}
