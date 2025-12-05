import z from "zod"
import { postSchema } from "./post.schema"
import { tagSchema } from "./tags.schema"

export const eventSchema = postSchema.extend({
  eventDate: z.date().min(1, "Event date is required"),
  tags: z.array(tagSchema).optional(),
})

export const editEventSchema = eventSchema.extend({
  id: z.string().min(1, "ID is required"),
})

export type EventSchema = z.infer<typeof eventSchema>

export type EditEventSchema = z.infer<typeof editEventSchema>
