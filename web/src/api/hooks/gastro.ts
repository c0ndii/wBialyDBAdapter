import { useDatabase } from "@/hooks/useDatabase"
import type { EditGastroSchema, GastroSchema } from "@/schema/gastro.schema"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { useNavigate } from "@tanstack/react-router"
import { apiUrl } from "../client"
import type { EndpointRequest, EndpointResponse, Post, Tag } from "../types"

export interface Gastro extends Post {
  day: Date
  tags: Tag[]
}

export const useGastros = (request: EndpointRequest) => {
  const { databaseType } = useDatabase()

  return useQuery<EndpointResponse<Gastro[]>>({
    queryKey: ["gastros", databaseType],
    queryFn: async () => {
      const res = await fetch(apiUrl("/api/gastro/filter"), {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ ...request, databaseType }),
      })

      if (!res.ok) {
        throw new Error("Error fetching gastros")
      }

      return res.json()
    },
    staleTime: 1000 * 60,
  })
}

export const useGastro = (id: string) => {
  const { databaseType } = useDatabase()

  return useQuery<EndpointResponse<Gastro>>({
    queryKey: ["gastro", databaseType, id],
    queryFn: async () => {
      const params = new URLSearchParams([
        ["databaseType", databaseType.toString()],
      ])
      const res = await fetch(
        apiUrl(`/api/gastro/${id}`) + "?" + params.toString(),
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

export const useDeleteGastro = () => {
  const navigate = useNavigate({ from: "/gastros/$gastroId" })
  const { databaseType } = useDatabase()
  const queryClient = useQueryClient()

  return useMutation<EndpointResponse<boolean>, unknown, string, unknown>({
    mutationKey: ["deleteGastro", databaseType],
    mutationFn: async (id) => {
      const res = await fetch(apiUrl(`/api/gastro/${id}`), {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ databaseType }),
      })
      if (!res.ok) {
        const text = await res.text()
        throw new Error("Error deleting gastro: " + text)
      }
      return res.json()
    },
    onSuccess: () => {
      navigate({ to: "/gastros" })
      queryClient.invalidateQueries({ queryKey: ["gastros", databaseType] })
    },
  })
}

export const useCreateGastro = () => {
  const { databaseType } = useDatabase()
  const queryClient = useQueryClient()

  return useMutation<
    EndpointResponse<Gastro[]>,
    unknown,
    GastroSchema,
    unknown
  >({
    mutationKey: ["createGastro", databaseType],
    mutationFn: async (data) => {
      const res = await fetch(apiUrl("/api/gastro"), {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ data: { ...data }, databaseType }),
      })
      if (!res.ok) {
        const text = await res.text()
        throw new Error("Error creating gastro: " + text)
      }
      return res.json()
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["gastros", databaseType] })
    },
  })
}

export const useUpdateGastro = () => {
  const { databaseType } = useDatabase()
  const queryClient = useQueryClient()

  return useMutation<
    EndpointResponse<Gastro[]>,
    unknown,
    { id: string; data: EditGastroSchema },
    unknown
  >({
    mutationKey: ["updateGastro", databaseType],
    mutationFn: async ({ id, data }) => {
      const res = await fetch(apiUrl(`/api/gastro/${id}`), {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ data: { ...data }, databaseType }),
      })
      if (!res.ok) {
        const text = await res.text()
        throw new Error("Error updating gastro: " + text)
      }
      return res.json()
    },
    onSuccess: (_res, { id }) => {
      queryClient.invalidateQueries({ queryKey: ["gastros", databaseType] })
      queryClient.invalidateQueries({ queryKey: ["gastro", databaseType, id] })
    },
  })
}
