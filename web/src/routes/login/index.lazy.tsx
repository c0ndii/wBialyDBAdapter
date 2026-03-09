import { useLogin } from "@/api/hooks/user";
import { loginSchema, type LoginSchema } from "@/schema/login.schema";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Box,
  Button,
  Card,
  CardContent,
  CardHeader,
  TextField,
  Typography,
} from "@mui/material";
import { createLazyFileRoute } from "@tanstack/react-router";
import { useMemo } from "react";
import { useForm } from "react-hook-form";

const PASSWORD_MANAGER_STORAGE_KEY = "security.passwordManagerEnabled";

export const Route = createLazyFileRoute("/login/")({
  component: LoginView,
});

function LoginView() {
  const isPasswordManagerEnabled = useMemo(() => {
    const stored = localStorage.getItem(PASSWORD_MANAGER_STORAGE_KEY);
    return stored == null ? true : stored === "true";
  }, []);

  const form = useForm<LoginSchema>({
    defaultValues: {
      login: "",
      password: "",
    },
    resolver: zodResolver(loginSchema),
  });

  const login = useLogin();

  const handleLogin = form.handleSubmit((data) =>
    login.mutateAsync({
      login: data.login,
      password: data.password,
    }),
  );

  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        flex: 1,
      }}
    >
      <Card sx={{ mb: 20 }}>
        <CardHeader title="Login" subheader="Sign in to your account" />
        <CardContent>
          <Box
            component="form"
            sx={{ display: "flex", flexDirection: "column", gap: 2 }}
            noValidate
            autoComplete={isPasswordManagerEnabled ? "on" : "off"}
            onSubmit={handleLogin}
          >
            <TextField
              id="login"
              label="Login"
              autoComplete={isPasswordManagerEnabled ? "username" : "off"}
              {...form.register("login")}
              error={!!form.formState.errors.login}
              helperText={form.formState.errors.login?.message}
              sx={{ width: 350 }}
            />
            <TextField
              id="password"
              label="Password"
              type="password"
              autoComplete={
                isPasswordManagerEnabled ? "current-password" : "new-password"
              }
              {...form.register("password")}
              error={!!form.formState.errors.password}
              helperText={form.formState.errors.password?.message}
              sx={{ width: 350 }}
            />

            <Button type="submit" variant="contained">
              Login
            </Button>
            {login.isError && (
              <Typography variant="body2" color="error">
                {login.error instanceof Error
                  ? login.error.message
                  : "Login failed"}
              </Typography>
            )}
          </Box>
        </CardContent>
      </Card>
    </Box>
  );
}
