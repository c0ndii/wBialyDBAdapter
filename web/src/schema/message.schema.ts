import z from "zod";

export const messageSchema = z.object({
  content: z
    .string()
    .trim()
    .min(1, "Message content is required")
    .max(1000, "Message is too long"),
});

export const deleteMessageSchema = z.object({
  messageId: z.number().int().positive().nullable(),
});

export const editMessageSchema = z.object({
  messageId: z.number().int().positive().nullable(),
  content: z
    .string()
    .trim()
    .min(1, "Message content is required")
    .max(1000, "Message is too long"),
});

export const messagePermissionsSchema = z.object({
  messageId: z.number().int().positive().nullable(),
  userIds: z.array(z.number().int().positive()),
});

export type MessageSchema = z.infer<typeof messageSchema>;
export type DeleteMessageSchema = z.infer<typeof deleteMessageSchema>;
export type EditMessageSchema = z.infer<typeof editMessageSchema>;
export type MessagePermissionsSchema = z.infer<typeof messagePermissionsSchema>;
