import z from "zod";
import { registerSchema } from "./register.schema";

export const loginSchema = registerSchema.omit({
  confirmPassword: true,
  username: true,
});

export type LoginSchema = z.infer<typeof loginSchema>;
