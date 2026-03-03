import { useDatabase } from "@/hooks/useDatabase"
import type { EditEventSchema, EventSchema } from "@/schema/events.schema"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { useNavigate } from "@tanstack/react-router"
import { apiUrl } from "../client"
import type { EndpointRequest, EndpointResponse, Post, Tag } from "../types"

export interface Event extends Post {
  eventDate: Date
  tags: Tag[]
}

export const useEvents = (request: EndpointRequest) => {
  const { databaseType } = useDatabase()

  return useQuery<EndpointResponse<Event[]>>({
    queryKey: ["events", databaseType],
    queryFn: async () => {
      const res = await fetch(apiUrl("/api/event/filter"), {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ ...request, databaseType }),
      })

      if (!res.ok) {
        throw new Error("Error fetching events")
      }

      return res.json()
    },
    staleTime: 1000 * 60,
  })
}

export const useEvent = (id: string) => {
  const { databaseType } = useDatabase()

  return useQuery<EndpointResponse<Event>>({
    queryKey: ["event", databaseType, id],
    queryFn: async () => {
      const params = new URLSearchParams([
        ["databaseType", databaseType.toString()],
      ])
      const res = await fetch(
        apiUrl(`/api/event/${id}`) + "?" + params.toString(),
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        }
      )

      if (!res.ok) {
        throw new Error("Error fetching gastro: " + id)
      }

      return res.json()
    },
    staleTime: 1000 * 60,
  })
}

export const useDeleteEvent = () => {
  const navigate = useNavigate({ from: "/events/$eventId" })
  const { databaseType } = useDatabase()
  const queryClient = useQueryClient()

  return useMutation<EndpointResponse<boolean>, unknown, string, unknown>({
    mutationKey: ["deleteEvent", databaseType],
    mutationFn: async (id) => {
      const res = await fetch(apiUrl(`/api/event/${id}`), {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ databaseType }),
      })

      if (!res.ok) {
        const text = await res.text()
        throw new Error("Error deleting event: " + text)
      }

      return res.json()
    },
    onSuccess: () => {
      navigate({ to: "/" })
      queryClient.invalidateQueries({ queryKey: ["events", databaseType] })
    },
  })
}

export const useCreateEvent = () => {
  const { databaseType } = useDatabase()
  const queryClient = useQueryClient()

  return useMutation<EndpointResponse<Event[]>, unknown, EventSchema, unknown>({
    mutationKey: ["createEvent", databaseType],
    mutationFn: async (data) => {
      const res = await fetch(apiUrl("/api/event"), {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ data: { ...data }, databaseType }),
      })

      if (!res.ok) {
        const text = await res.text()
        throw new Error("Error creating event: " + text)
      }

      return res.json()
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["events", databaseType] })
    },
  })
}

export const useUpdateEvent = () => {
  const { databaseType } = useDatabase()
  const queryClient = useQueryClient()

  return useMutation<
    EndpointResponse<Event[]>,
    unknown,
    { id: string; data: EditEventSchema },
    unknown
  >({
    mutationKey: ["updateEvent", databaseType],
    mutationFn: async ({ id, data }) => {
      const res = await fetch(apiUrl(`/api/event/${id}`), {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ data: { ...data }, databaseType }),
      })

      if (!res.ok) {
        const text = await res.text()
        throw new Error("Error updating event: " + text)
      }

      return res.json()
    },
    onSuccess: (_res, { id }) => {
      queryClient.invalidateQueries({ queryKey: ["events", databaseType] })
      queryClient.invalidateQueries({ queryKey: ["event", databaseType, id] })
    },
  })
}
