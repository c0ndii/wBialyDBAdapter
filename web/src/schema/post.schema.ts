import z from "zod"

export const postSchema = z.object({
  title: z.string().min(1, "Title is required"),
  description: z.string().min(1, "Description is required"),
  author: z.string().min(1, "Description is required"),
  addDate: z.date(),
  link: z.url().min(1, "Link is required"),
  place: z.string().min(1, "Place is required"),
})
