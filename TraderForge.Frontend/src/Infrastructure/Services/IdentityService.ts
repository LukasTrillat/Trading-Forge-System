import { Result } from '../../Application/Common/Result';
import { httpClient } from '../Http/httpClient';
import { TokenRepository } from '../Repositories/TokenRepository';
import type { LoginTraderQuery } from '../../Application/DTOs/Queries/LoginTraderQuery';
import type { RegisterTraderCommand } from '../../Application/DTOs/Commands/RegisterTraderCommand';

export class IdentityService {
  
async login(query: LoginTraderQuery): Promise<Result<string>> {
    try {
      const { data } = await httpClient.post<any>('/api/identity/login', query);
      
      // Look for the token in both lowercase and PascalCase
      const token = data.token || data.Token;

      if (token) {
        return Result.ok(token);
      }
      return Result.fail('The server returned success, but no token was found in the response.');
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
