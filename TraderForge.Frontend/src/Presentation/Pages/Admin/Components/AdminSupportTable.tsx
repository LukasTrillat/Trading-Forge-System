import { SupportMessage } from '../../../../Domain/Entities/SupportMessage';

interface AdminSupportTableProps {
  messagesToDisplay: SupportMessage[];
}

export function AdminSupportTable(props: AdminSupportTableProps) {
  const messages = props.messagesToDisplay;

  function formatDate(isoString: string): string {
    const date = new Date(isoString);
    return date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
  }

  return (
    <div className="overflow-x-auto border border-neutral-800 rounded-lg">
      <table className="w-full text-left text-sm text-neutral-400">
        <thead className="border-b border-neutral-800 text-neutral-400">
          <tr>
            <th className="px-4 py-3 font-medium rounded-tl-lg">Date</th>
            <th className="px-4 py-3 font-medium">User Email</th>
            <th className="px-4 py-3 font-medium">Subject</th>
            <th className="px-4 py-3 font-medium">Status</th>
            <th className="px-4 py-3 font-medium rounded-tr-lg text-right">Actions</th>
          </tr>
        </thead>
        <tbody className="divide-y divide-neutral-800">
          {messages.map(function(message: SupportMessage) {
            return (
              <tr key={message.id} className="hover:bg-neutral-800/30 transition-colors">
                <td className="px-4 py-3 text-neutral-300">{formatDate(message.sentAt)}</td>
                <td className="px-4 py-3 font-medium text-neutral-200">{message.userEmail}</td>
                <td className="px-4 py-3">
                  <span className="block truncate max-w-xs">{message.subject}</span>
                </td>
                <td className="px-4 py-3">
                  {message.isResolved ? (
                    <span className="px-2 py-1 bg-emerald-900/50 text-emerald-400 rounded-md text-xs font-medium">Resolved</span>
                  ) : (
                    <span className="px-2 py-1 bg-yellow-500/10 text-yellow-400 rounded-md text-xs font-medium">Pending</span>
                  )}
                </td>
                <td className="px-4 py-3 text-right space-x-3">
                  <button className="text-red-400 hover:text-red-300 transition-colors">Reply</button>
                  {!message.isResolved && (
                    <button className="text-emerald-400 hover:text-emerald-300 transition-colors">Mark Resolved</button>
                  )}
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
