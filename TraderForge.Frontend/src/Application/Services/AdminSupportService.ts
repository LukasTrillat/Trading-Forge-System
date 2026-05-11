import { SupportMessage } from '../../Domain/Entities/SupportMessage';

export class AdminSupportService {
  public async getAllMessages(): Promise<SupportMessage[]> {
    const messages: SupportMessage[] = [
      {
        id: '1',
        userEmail: 'trader123@example.com',
        subject: 'Issue with portfolio balance',
        content: 'My virtual balance is not updating after closing a trade.',
        sentAt: '2026-05-09T10:30:00Z',
        isResolved: false
      },
      {
        id: '2',
        userEmail: 'newuser@domain.com',
        subject: 'Basic plan question',
        content: 'Does the basic plan include access to strategy templates?',
        sentAt: '2026-05-08T14:15:00Z',
        isResolved: true
      }
    ];

    return messages;
  }
}
