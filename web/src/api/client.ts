import { QueryClient } from "@tanstack/react-query"

export const queryClient = new QueryClient({
  defaultOptions: {
    mutations: {
      retry: false,
    },
    queries: {
      retry: false,
    },
  },
})

// Build-time API base, e.g. http://localhost:8080
export const apiBase: string =
  (import.meta.env.VITE_API_BASE as string | undefined) ?? ""
export const apiUrl = (path: string) => `${apiBase}${path}`
