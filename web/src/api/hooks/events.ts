import { useQuery } from "@tanstack/react-query"
import type { EndpointRequest } from "../types"

interface Event {
  id: string
  name: string
}

export const useEvents = (request: EndpointRequest) => {
  return useQuery<Event[]>({
    queryKey: ["events"],
    queryFn: async () => {
      const res = await fetch("/api/event/filter", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ request }),
      })

      if (!res.ok) {
        throw new Error("Error fetching events")
      }

      return res.json()
    },
    staleTime: 1000 * 60,
  })
}
