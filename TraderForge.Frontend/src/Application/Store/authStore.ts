import { create } from 'zustand';
import type { Trader } from '../../Domain/Entities/Trader';
import { TokenRepository } from '../../Infrastructure/Repositories/TokenRepository';
import { jwtDecode } from 'jwt-decode';

interface DecodedToken {
  role?: string;
  [key: string]: any;
}

interface AuthState {
  token: string | null;
  trader: Trader | null;
  isAuthenticated: boolean;
  role: 'Trader' | 'SystemAdmin' | null;
  setToken: (token: string) => void;
  setTrader: (trader: Trader) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>((set) => {
  const initialToken = TokenRepository.get();
  
  let initialRole = null;
  if (initialToken) {
    try {
      const decoded: DecodedToken = jwtDecode(initialToken);
      // El Claim de rol suele verse como 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
      const roleClaimKey = Object.keys(decoded).find(k => k.endsWith('role'));
      initialRole = roleClaimKey ? decoded[roleClaimKey] : decoded.role;
    } catch {
      // invalid token
    }
  }

  return {
    token: initialToken,
    trader: null,
    isAuthenticated: !!initialToken,
    role: initialRole as 'Trader' | 'SystemAdmin' | null,

    setToken: (token) => {
      TokenRepository.save(token);
      let newRole = null;
      try {
        const decoded: DecodedToken = jwtDecode(token);
        const roleClaimKey = Object.keys(decoded).find(k => k.endsWith('role'));
        newRole = roleClaimKey ? decoded[roleClaimKey] : decoded.role;
      } catch (err) {
        console.error("Failed to decode token", err);
      }
      set({ token, isAuthenticated: true, role: newRole as 'Trader' | 'SystemAdmin' | null });
    },

    setTrader: (trader) => set({ trader }),

    logout: () => {
      TokenRepository.clear();
      set({ token: null, trader: null, isAuthenticated: false, role: null });
    },
  };
});
