import z from "zod";

export const securitySettingsSchema = z.object({
  isLockoutEnabled: z.boolean(),
  maxFailedLoginAttempts: z
    .number()
    .int()
    .min(1, "Failed attempts must be at least 1")
    .max(20, "Failed attempts cannot exceed 20"),
  isPasswordManagerEnabled: z.boolean(),
});

export type SecuritySettingsSchema = z.infer<typeof securitySettingsSchema>;
