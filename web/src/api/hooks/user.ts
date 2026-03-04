import { useAuth } from "@/hooks/useAuth";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "@tanstack/react-router";
import { apiUrl } from "../client";
import type { LoginUser, RegisterUser, User } from "../types";

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
      const res = await fetch(apiUrl("/api/User/register"), {
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
      const res = await fetch(apiUrl("/api/User/login"), {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ ...data }),
      });
      if (!res.ok) {
        const text = await res.text();
        throw new Error("Error logging in: " + text);
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
