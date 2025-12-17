import z from "zod"

export const tagSchema = z.object({
  id: z.string().min(1, "ID is required"),
  name: z.string().min(1, "Name is required"),
})
