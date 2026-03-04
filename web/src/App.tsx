import { queryClient } from "@/api/client";
import {
  CssBaseline,
  darkScrollbar,
  GlobalStyles,
  ThemeProvider,
} from "@mui/material";
import { QueryClientProvider } from "@tanstack/react-query";
import { createRouter, RouterProvider } from "@tanstack/react-router";
import { useMemo } from "react";
import { AuthProvider } from "./context/AuthContext";
import { DatabaseProvider } from "./context/DatabaseContext";
import { useTheme } from "./hooks/useTheme";
import { routeTree } from "./routeTree.gen";
import { dark, light } from "./theme";

const router = createRouter({ routeTree });

// Register the router instance for type safety
declare module "@tanstack/react-router" {
  interface Register {
    router: typeof router;
  }
}

function App() {
  const { theme } = useTheme();
  const scrollbarStyle = useMemo(() => {
    return theme === "dark" ? darkScrollbar() : () => {};
  }, [theme]);

  return (
    <DatabaseProvider>
      <QueryClientProvider client={queryClient}>
        <CssBaseline />
        <GlobalStyles styles={{ ...scrollbarStyle }} />
        <ThemeProvider theme={theme === "light" ? light : dark}>
          <AuthProvider>
            <RouterProvider router={router} />
          </AuthProvider>
        </ThemeProvider>
      </QueryClientProvider>
    </DatabaseProvider>
  );
}

export default App;
