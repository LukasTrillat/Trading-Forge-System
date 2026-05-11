import { Result } from '../../Application/Common/Result';
import { httpClient } from '../Http/httpClient';
import { TokenRepository } from '../Repositories/TokenRepository';
import type { LoginTraderQuery } from '../../Application/DTOs/Queries/LoginTraderQuery';
import type { RegisterTraderCommand } from '../../Application/DTOs/Commands/RegisterTraderCommand';

export class IdentityService {
  
async login(query: LoginTraderQuery): Promise<Result<string>> {
    try {
      const { data } = await httpClient.post<any>('/api/identity/login', query);
      
      // Check for both cases just in case
      const token = data.token || data.Token;
      
      if (token) {
        return Result.ok(token);
      }
      return Result.fail('Backend success but no token key found.');
    } catch (error: unknown) {
      return Result.fail(extractErrorMessage(error, 'Invalid credentials.'));
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
