import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { apiUrl } from "../client";
import type {
  CreateMessageInput,
  MessageListResponse,
  UpdateMessageEditorsInput,
  UpdateMessageInput,
} from "../types";

export const useMessages = () => {
  return useQuery<MessageListResponse>({
    queryKey: ["messages"],
    queryFn: async () => {
      const res = await fetch(apiUrl("/api/Message"), {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (!res.ok) {
        throw new Error("Error fetching messages");
      }

      return res.json();
    },
    staleTime: 1000 * 60,
  });
};

export const useCreateMessage = () => {
  const queryClient = useQueryClient();

  return useMutation<number, unknown, CreateMessageInput, unknown>({
    mutationKey: ["createMessage"],
    mutationFn: async (data) => {
      const res = await fetch(apiUrl("/api/Message"), {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ ...data }),
      });

      if (!res.ok) {
        const text = await res.text();
        throw new Error("Error creating message: " + text);
      }

      return res.status;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["messages"] });
    },
  });
};

export const useUpdateMessage = () => {
  const queryClient = useQueryClient();

  return useMutation<number, unknown, UpdateMessageInput, unknown>({
    mutationKey: ["updateMessage"],
    mutationFn: async (data) => {
      const res = await fetch(apiUrl("/api/Message"), {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ ...data }),
      });

      if (!res.ok) {
        const text = await res.text();
        throw new Error("Error updating message: " + text);
      }

      return res.status;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["messages"] });
    },
  });
};

export const useUpdateMessageEditors = () => {
  const queryClient = useQueryClient();

  return useMutation<number, unknown, UpdateMessageEditorsInput, unknown>({
    mutationKey: ["updateMessageEditors"],
    mutationFn: async (data) => {
      const res = await fetch(apiUrl("/api/Message/editors"), {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ ...data }),
      });

      if (!res.ok) {
        const text = await res.text();
        throw new Error("Error updating message editors: " + text);
      }

      return res.status;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["messages"] });
    },
  });
};

export const useDeleteMessage = () => {
  const queryClient = useQueryClient();

  return useMutation<number, unknown, number, unknown>({
    mutationKey: ["deleteMessage"],
    mutationFn: async (messageId) => {
      const res = await fetch(apiUrl(`/api/Message/${messageId}`), {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      });

      if (!res.ok) {
        const text = await res.text();
        throw new Error("Error deleting message: " + text);
      }

      return res.status;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["messages"] });
    },
  });
};
