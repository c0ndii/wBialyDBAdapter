import { queryClient } from "@/api/client"
import { ThemeProvider } from "@mui/material"
import { QueryClientProvider } from "@tanstack/react-query"
import { createRouter, RouterProvider } from "@tanstack/react-router"
import { DatabaseProvider } from "./context/DatabaseContext"
import { routeTree } from "./routeTree.gen"
import { light } from "./theme"

const router = createRouter({ routeTree })

// Register the router instance for type safety
declare module "@tanstack/react-router" {
  interface Register {
    router: typeof router
  }
}

function App() {
  return (
    <DatabaseProvider>
      <QueryClientProvider client={queryClient}>
        <ThemeProvider theme={light}>
          <RouterProvider router={router} />
        </ThemeProvider>
      </QueryClientProvider>
    </DatabaseProvider>
  )
}

export default App
