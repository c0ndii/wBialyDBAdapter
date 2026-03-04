import { apiUrl } from "@/api/client";
import type { User } from "@/api/types";

import {
  useCallback,
  useEffect,
  useMemo,
  useState,
  type PropsWithChildren,
} from "react";

import { createContext } from "react";

export type AuthContextValue = {
  isAuthenticated: boolean;
  isLoading: boolean;
  me: User | null;
  refreshAuth: () => Promise<void>;
};

// eslint-disable-next-line react-refresh/only-export-components
export const AuthContext = createContext<AuthContextValue | null>(null);

export const AuthProvider = ({ children }: PropsWithChildren) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [me, setMe] = useState<User | null>(null);

  const refreshAuth = useCallback(async () => {
    setIsLoading(true);

    try {
      const res = await fetch(apiUrl("/api/User/me"), {
        method: "GET",
        credentials: "include",
      });

      if (!res.ok) {
        setIsAuthenticated(false);
        setMe(null);
        return;
      }

      const user = (await res.json()) as User;
      setMe(user);
      setIsAuthenticated(true);
    } catch {
      setIsAuthenticated(false);
      setMe(null);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    refreshAuth();
  }, [refreshAuth]);

  const value = useMemo(
    () => ({ isAuthenticated, isLoading, me, refreshAuth }),
    [isAuthenticated, isLoading, me, refreshAuth],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
