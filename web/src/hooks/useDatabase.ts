import { DatabaseContext } from "@/context/DatabaseContext"
import { useContext } from "react"

export function useDatabase() {
  const ctx = useContext(DatabaseContext)
  if (!ctx) {
    throw new Error("useDatabase must be used within <DatabaseProvider>")
  }
  return ctx
}
