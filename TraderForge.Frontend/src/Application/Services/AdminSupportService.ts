import { SupportMessage } from '../../Domain/Entities/SupportMessage';
import { httpClient } from '../../Infrastructure/Http/httpClient';

export class AdminSupportService {
  public async getAllMessages(): Promise<SupportMessage[]> {
    try {
      const response = await httpClient.get<SupportMessage[]>('/api/admin/support');
      return response.data;
    } catch (error) {
      console.warn("Backend for support messages not implemented yet. Returning empty array.");
      return [];
    }
  }
}
