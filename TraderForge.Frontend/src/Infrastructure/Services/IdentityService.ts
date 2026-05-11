import { Result } from '../../Application/Common/Result';
import { httpClient } from '../Http/httpClient';
import { TokenRepository } from '../Repositories/TokenRepository';
import type { LoginTraderQuery } from '../../Application/DTOs/Queries/LoginTraderQuery';
import type { RegisterTraderCommand } from '../../Application/DTOs/Commands/RegisterTraderCommand';

export class IdentityService {
  
async login(query: LoginTraderQuery): Promise<Result<any>> {
    try {
      const response = await httpClient.post('/api/identity/login', query);
      const data = response.data;

      // Robust check for the token key (handles both "token" and "Token")
      const token = data.token || data.Token;

      if (token) {
        TokenRepository.set(token);
        return Result.ok({ token }); // Explicitly return it as lowercase for useAuth
      }
      
      return Result.fail('Backend returned success but no token was found.');
    } catch (error: any) {
      // Look specifically for the error key returned by IdentityController
      const errorMessage = error.response?.data?.error || error.response?.data?.message || 'Login failed.';
      return Result.fail(errorMessage);
    }
  }


  async register(command: RegisterTraderCommand): Promise<Result<void>> {
    try {
      await httpClient.post('/api/identity/register', command);
      return Result.ok(undefined);
    } catch (error: any) {
      return Result.fail(error.response?.data?.message || 'Registration failed.');
    }
  }

  logout(): void {
    TokenRepository.clear();
    window.location.href = '/login';
  }
}
