import { tanstackRouter } from "@tanstack/router-plugin/vite"
import react from "@vitejs/plugin-react"
import path from "path"
import { defineConfig } from "vite"

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    tanstackRouter({
      target: "react",
      autoCodeSplitting: true,
    }),
    react(),
  ],
  server: {
    proxy: {
      "/api": {
        target: "https://localhost:7147/",
        changeOrigin: true,
        secure: false,
        ws: true,
        // TODO: Configure
        configure: (proxy) => {
          proxy.on("error", (err) => {
            console.error("proxy error", err)
          })
          proxy.on("proxyReq", (_, req) => {
            console.error("Sending Request to the Target:", req.method, req.url)
          })
          proxy.on("proxyRes", (proxyRes, req) => {
            console.error(
              "Received Response from the Target:",
              proxyRes.statusCode,
              req.url
            )
          })
        },
      },
    },
  },
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
})
