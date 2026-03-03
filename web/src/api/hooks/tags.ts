import { useDatabase } from "@/hooks/useDatabase"
import { useQuery } from "@tanstack/react-query"
import { apiUrl } from "../client"
import type { EndpointRequest, EndpointResponse, Tag } from "../types"

export const useTags = (request: EndpointRequest) => {
  const { databaseType } = useDatabase()

  return useQuery<EndpointResponse<Tag[]>>({
    queryKey: ["tags", databaseType],
    queryFn: async () => {
      const res = await fetch(apiUrl("/api/tag/filter"), {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ ...request, databaseType }),
      })

      if (!res.ok) {
        throw new Error("Error fetching tags")
      }

      return res.json()
    },
    staleTime: 1000 * 60,
  })
}
