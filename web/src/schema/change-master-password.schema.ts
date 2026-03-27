import z from "zod";

export const changeMasterPasswordSchema = z
  .object({
    oldPassword: z
      .string()
      .min(12, "Current password must be at least 12 characters long")
      .max(18, "Current password must be at most 18 characters long"),
    newPassword: z
      .string()
      .min(12, "New password must be at least 12 characters long")
      .max(18, "New password must be at most 18 characters long"),
    confirmNewPassword: z
      .string()
      .min(12, "Confirm password must be at least 12 characters long")
      .max(18, "Confirm password must be at most 18 characters long"),
  })
  .superRefine(({ newPassword, confirmNewPassword }, ctx) => {
    if (newPassword !== confirmNewPassword) {
      ctx.addIssue({
        code: "custom",
        path: ["confirmNewPassword"],
        message: "Passwords do not match",
      });
    }
  });

export type ChangeMasterPasswordSchema = z.infer<
  typeof changeMasterPasswordSchema
>;
