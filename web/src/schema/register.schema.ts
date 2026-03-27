import z from "zod";

export const registerSchema = z
  .object({
    login: z.string().min(1, "Login is required"),
    username: z.string().min(1, "Username is required"),
    password: z
      .string()
      .min(12, "Password must be at least 6 characters long")
      .max(18, "Password must be at most 18 characters long"),
    confirmPassword: z
      .string()
      .min(12, "Confirm password must be at least 6 characters long")
      .max(18, "Confirm password must be at most 18 characters long"),
  })
  .superRefine(({ password, confirmPassword }, ctx) => {
    if (password !== confirmPassword) {
      ctx.addIssue({
        code: "custom",
        path: ["confirmPassword"],
        message: "Passwords do not match",
      });
    }
  });

export type RegisterSchema = z.infer<typeof registerSchema>;
