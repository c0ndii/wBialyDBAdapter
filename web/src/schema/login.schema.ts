import z from "zod";
import { registerSchema } from "./register.schema";

export const loginSchema = registerSchema.omit({
  password: true,
  confirmPassword: true,
  username: true,
});

export type LoginSchema = z.infer<typeof loginSchema>;
