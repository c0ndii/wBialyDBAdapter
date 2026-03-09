import { useAuth } from "@/hooks/useAuth";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "@tanstack/react-router";
import { apiUrl } from "../client";
import {
  LogsScope,
  type LoginAuditLog,
  type LoginUser,
  type LogsScopeEnum,
  type RegisterUser,
  type User,
  type UserSecurityOverview,
  type UserSecuritySettings,
} from "../types";

export const useUsers = () => {
  return useQuery<User[]>({
    queryKey: ["users"],
    queryFn: async () => {
      const res = await fetch(apiUrl("/api/User"), {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (!res.ok) {
        throw new Error("Error fetching Users");
      }

      return res.json();
    },
    staleTime: 1000 * 60,
  });
};

export const useUser = (id: number) => {
  return useQuery<User>({
    queryKey: ["user", id],
    queryFn: async () => {
      const res = await fetch(apiUrl(`/api/User/${id}`), {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (!res.ok) {
        throw new Error("Error fetching user with id " + id);
      }

      return res.json();
    },
    staleTime: 1000 * 60,
  });
};

export const useMe = () => {
  return useQuery<User>({
    queryKey: ["me"],
    queryFn: async () => {
      const res = await fetch(apiUrl("/api/User/me"), {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (!res.ok) {
        throw new Error("Error fetching current user");
      }

      return res.json();
    },
    staleTime: 1000 * 60,
  });
};

export const useRegister = () => {
  const queryClient = useQueryClient();
  const navigate = useNavigate();

  return useMutation<number, unknown, RegisterUser, unknown>({
    mutationKey: ["register"],
    mutationFn: async (data) => {
      const res = await fetch(apiUrl("/api/Auth/register"), {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ ...data }),
      });

      if (!res.ok) {
        const text = await res.text();
        throw new Error("Error creating user: " + text);
      }
      return res.status;
    },
    onSuccess: () => {
      queryClient.invalidateQueries();
      navigate({ to: "/login" });
    },
  });
};

export const useLogin = () => {
  const queryClient = useQueryClient();
  const navigate = useNavigate();
  const { refreshAuth } = useAuth();

  return useMutation<number, unknown, LoginUser, unknown>({
    mutationKey: ["login"],
    mutationFn: async (data) => {
      const res = await fetch(apiUrl("/api/Auth/login"), {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ ...data }),
      });
      if (!res.ok) {
        let message = "Invalid credentials or account temporarily unavailable.";
        let retryAfterSeconds: number | null = null;

        try {
          const parsed = (await res.json()) as {
            message?: string;
            retryAfterSeconds?: number | null;
          };
          if (parsed.message) {
            message = parsed.message;
          }

          retryAfterSeconds = parsed.retryAfterSeconds ?? null;
        } catch {
          const text = await res.text();
          if (text) {
            message = text;
          }
        }

        const retryMessage =
          retryAfterSeconds && retryAfterSeconds > 0
            ? ` Retry after ${retryAfterSeconds}s.`
            : "";

        throw new Error(message + retryMessage);
      }
      return res.status;
    },
    onSuccess: async () => {
      await refreshAuth();
      queryClient.invalidateQueries();
      navigate({ to: "/messages" });
    },
  });
};

export const useLogout = () => {
  const queryClient = useQueryClient();
  const navigate = useNavigate();
  const { refreshAuth } = useAuth();

  return useMutation<number, unknown, void, unknown>({
    mutationKey: ["logout"],
    mutationFn: async () => {
      const res = await fetch(apiUrl("/api/User/logout"), {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      });

      if (!res.ok) {
        const text = await res.text();
        throw new Error("Error logging out: " + text);
      }

      return res.status;
    },
    onSuccess: async () => {
      await refreshAuth();
      queryClient.clear();
      navigate({ to: "/login" });
    },
  });
};

export const useSecurityOverview = () => {
  return useQuery<UserSecurityOverview>({
    queryKey: ["user-security-overview"],
    queryFn: async () => {
      const res = await fetch(apiUrl("/api/UserSecurity/overview"), {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (!res.ok) {
        throw new Error("Error fetching security overview");
      }

      return res.json();
    },
    staleTime: 1000 * 30,
  });
};

export const useSecurityLogs = (
  limit = 100,
  scope: LogsScopeEnum = LogsScope.Mine,
) => {
  return useQuery<LoginAuditLog[]>({
    queryKey: ["user-security-logs", limit, scope],
    queryFn: async () => {
      const query = new URLSearchParams();
      query.set("limit", String(limit));
      query.set("scope", scope);

      const res = await fetch(apiUrl(`/api/LoginAudit?${query.toString()}`), {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (!res.ok) {
        throw new Error("Error fetching security logs");
      }

      return res.json();
    },
    staleTime: 1000 * 20,
  });
};

export const useUpdateSecuritySettings = () => {
  const queryClient = useQueryClient();

  return useMutation<
    UserSecuritySettings,
    unknown,
    UserSecuritySettings,
    unknown
  >({
    mutationKey: ["update-user-security-settings"],
    mutationFn: async (data) => {
      const res = await fetch(apiUrl("/api/UserSecurity/settings"), {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify(data),
      });

      if (!res.ok) {
        const text = await res.text();
        throw new Error("Error updating security settings: " + text);
      }

      return res.json();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["user-security-overview"] });
    },
  });
};
