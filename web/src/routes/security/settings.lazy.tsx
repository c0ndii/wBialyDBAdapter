import {
  useChangeMasterPassword,
  useLogout,
  useSecurityOverview,
  useUpdateSecuritySettings,
} from "@/api/hooks/user";
import {
  changeMasterPasswordSchema,
  type ChangeMasterPasswordSchema,
} from "@/schema/change-master-password.schema";
import {
  securitySettingsSchema,
  type SecuritySettingsSchema,
} from "@/schema/security-settings.schema";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Alert,
  Box,
  Button,
  Card,
  CardContent,
  CardHeader,
  FormControlLabel,
  Switch,
  TextField,
  Typography,
} from "@mui/material";
import { createLazyFileRoute } from "@tanstack/react-router";
import { useEffect, useMemo } from "react";
import { useForm, useWatch } from "react-hook-form";

const PASSWORD_MANAGER_STORAGE_KEY = "security.passwordManagerEnabled";

export const Route = createLazyFileRoute("/security/settings")({
  component: SecuritySettingsView,
});

function SecuritySettingsView() {
  const { data: overview, isLoading, isError } = useSecurityOverview();
  const updateSettings = useUpdateSecuritySettings();
  const changeMasterPassword = useChangeMasterPassword();
  const logout = useLogout();

  const form = useForm<SecuritySettingsSchema>({
    defaultValues: {
      isLockoutEnabled: true,
      maxFailedLoginAttempts: 5,
      isPasswordManagerEnabled: true,
    },
    resolver: zodResolver(securitySettingsSchema),
  });

  const passwordForm = useForm<ChangeMasterPasswordSchema>({
    defaultValues: {
      oldPassword: "",
      newPassword: "",
      confirmNewPassword: "",
    },
    resolver: zodResolver(changeMasterPasswordSchema),
  });

  useEffect(() => {
    if (!overview) {
      return;
    }

    form.reset({
      isLockoutEnabled: overview.settings.isLockoutEnabled,
      maxFailedLoginAttempts: overview.settings.maxFailedLoginAttempts,
      isPasswordManagerEnabled: overview.settings.isPasswordManagerEnabled,
    });
  }, [form, overview]);

  const isLockoutEnabled = useWatch({
    control: form.control,
    name: "isLockoutEnabled",
  });
  const isPasswordManagerEnabled = useWatch({
    control: form.control,
    name: "isPasswordManagerEnabled",
  });
  const maxFailedLoginAttempts = useWatch({
    control: form.control,
    name: "maxFailedLoginAttempts",
  });

  const canSave = useMemo(
    () => Number.isFinite(maxFailedLoginAttempts) && maxFailedLoginAttempts > 0,
    [maxFailedLoginAttempts],
  );

  const handleSave = form.handleSubmit(async (data) => {
    await updateSettings.mutateAsync({
      isLockoutEnabled: data.isLockoutEnabled,
      maxFailedLoginAttempts: data.maxFailedLoginAttempts,
      isPasswordManagerEnabled: data.isPasswordManagerEnabled,
    });

    localStorage.setItem(
      PASSWORD_MANAGER_STORAGE_KEY,
      String(data.isPasswordManagerEnabled),
    );
  });

  const handleChangeMasterPassword = passwordForm.handleSubmit(async (data) => {
    await changeMasterPassword.mutateAsync({
      oldPassword: data.oldPassword,
      newPassword: data.newPassword,
    });

    passwordForm.reset();
    await logout.mutateAsync();
  });

  return (
    <Box sx={{ display: "grid", gap: 2 }}>
      <Card>
        <CardHeader
          title="Settings"
          subheader="Configure user specific settings"
        />
        <CardContent sx={{ display: "grid", gap: 2 }}>
          {isLoading && (
            <Typography variant="body2" color="text.secondary">
              Loading settings...
            </Typography>
          )}

          {isError && (
            <Typography variant="body2" color="error">
              Failed to load settings.
            </Typography>
          )}

          {!isLoading && !isError && (
            <Box
              component="form"
              sx={{ display: "grid", gap: 2 }}
              noValidate
              onSubmit={handleSave}
            >
              <FormControlLabel
                control={
                  <Switch
                    checked={isLockoutEnabled ?? true}
                    onChange={(event) =>
                      form.setValue("isLockoutEnabled", event.target.checked, {
                        shouldDirty: true,
                        shouldValidate: true,
                      })
                    }
                  />
                }
                label="Enable account lockout"
              />

              <TextField
                type="number"
                label="Failed attempts before lock"
                value={maxFailedLoginAttempts ?? 5}
                {...form.register("maxFailedLoginAttempts", {
                  valueAsNumber: true,
                })}
                inputProps={{ min: 1, max: 20 }}
                error={!!form.formState.errors.maxFailedLoginAttempts}
                helperText={
                  form.formState.errors.maxFailedLoginAttempts?.message
                }
              />

              <FormControlLabel
                control={
                  <Switch
                    checked={isPasswordManagerEnabled ?? true}
                    onChange={(event) =>
                      form.setValue(
                        "isPasswordManagerEnabled",
                        event.target.checked,
                        {
                          shouldDirty: true,
                          shouldValidate: true,
                        },
                      )
                    }
                  />
                }
                label="Enable password manager support"
              />

              <Button
                type="submit"
                variant="contained"
                disabled={!canSave || updateSettings.isPending}
              >
                Save settings
              </Button>

              {updateSettings.isError && (
                <Alert severity="error">
                  Failed to update security settings.
                </Alert>
              )}

              {updateSettings.isSuccess && (
                <Alert severity="success">Security settings updated.</Alert>
              )}
            </Box>
          )}
        </CardContent>
      </Card>

      <Card>
        <CardHeader
          title="Master password"
          subheader="Change your master password. After change you will be logged out."
        />
        <CardContent>
          <Box
            component="form"
            sx={{ display: "grid", gap: 2 }}
            noValidate
            onSubmit={handleChangeMasterPassword}
          >
            <TextField
              type="password"
              label="Current password"
              autoComplete="current-password"
              {...passwordForm.register("oldPassword")}
              error={!!passwordForm.formState.errors.oldPassword}
              helperText={passwordForm.formState.errors.oldPassword?.message}
            />

            <TextField
              type="password"
              label="New password"
              autoComplete="new-password"
              {...passwordForm.register("newPassword")}
              error={!!passwordForm.formState.errors.newPassword}
              helperText={passwordForm.formState.errors.newPassword?.message}
            />

            <TextField
              type="password"
              label="Confirm new password"
              autoComplete="new-password"
              {...passwordForm.register("confirmNewPassword")}
              error={!!passwordForm.formState.errors.confirmNewPassword}
              helperText={
                passwordForm.formState.errors.confirmNewPassword?.message
              }
            />

            <Button
              type="submit"
              variant="contained"
              disabled={changeMasterPassword.isPending || logout.isPending}
            >
              Change password
            </Button>

            {changeMasterPassword.isError && (
              <Alert severity="error">
                {changeMasterPassword.error instanceof Error
                  ? changeMasterPassword.error.message
                  : "Failed to change password."}
              </Alert>
            )}
          </Box>
        </CardContent>
      </Card>

      <Card>
        <CardHeader title="Security statistics" />
        <CardContent sx={{ display: "grid", gap: 1 }}>
          <Typography variant="body2">
            Last failed login:{" "}
            {formatUtcDate(overview?.stats.lastFailedLoginAtUtc)}
          </Typography>
          <Typography variant="body2">
            Last successful login:{" "}
            {formatUtcDate(overview?.stats.lastSuccessfulLoginAtUtc)}
          </Typography>
          <Typography variant="body2">
            Failed logins since last success:{" "}
            {overview?.stats.failedLoginCountSinceLastSuccess ?? 0}
          </Typography>
          <Typography variant="body2">
            Total failed logins: {overview?.stats.failedLoginCountTotal ?? 0}
          </Typography>
          <Typography variant="body2">
            Total successful logins: {overview?.stats.successfulLoginCount ?? 0}
          </Typography>
          <Typography variant="body2">
            Account locked: {overview?.stats.isLocked ? "Yes" : "No"}
          </Typography>
          <Typography variant="body2">
            Locked until: {formatUtcDate(overview?.stats.lockedUntilUtc)}
          </Typography>
          <Typography variant="body2">
            Next allowed login at:{" "}
            {formatUtcDate(overview?.stats.nextAllowedLoginAtUtc)}
          </Typography>
        </CardContent>
      </Card>
    </Box>
  );
}

function formatUtcDate(value?: string | null) {
  if (!value) {
    return "-";
  }

  return new Date(value).toLocaleString("pl-PL");
}
