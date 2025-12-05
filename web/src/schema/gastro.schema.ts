import z from "zod"
import { postSchema } from "./post.schema"
import { tagSchema } from "./tags.schema"

export const gastroSchema = postSchema.extend({
  day: z.date().min(1, "Day is required"),
  tags: z.array(tagSchema).optional(),
})

export const editGastroSchema = gastroSchema.extend({
  id: z.string().min(1, "ID is required"),
})

export type GastroSchema = z.infer<typeof gastroSchema>

export type EditGastroSchema = z.infer<typeof editGastroSchema>
