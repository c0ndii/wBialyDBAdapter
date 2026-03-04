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
} from "@mui/material";
import { createLazyFileRoute } from "@tanstack/react-router";
import { useForm } from "react-hook-form";

export const Route = createLazyFileRoute("/login/")({
  component: LoginView,
});

function LoginView() {
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
            autoComplete="off"
            onSubmit={handleLogin}
          >
            <TextField
              id="login"
              label="Login"
              autoComplete="username"
              {...form.register("login")}
              error={!!form.formState.errors.login}
              helperText={form.formState.errors.login?.message}
              sx={{ width: 350 }}
            />
            <TextField
              id="password"
              label="Password"
              type="password"
              autoComplete="new-password"
              {...form.register("password")}
              error={!!form.formState.errors.password}
              helperText={form.formState.errors.password?.message}
              sx={{ width: 350 }}
            />

            <Button type="submit" variant="contained">
              Login
            </Button>
          </Box>
        </CardContent>
      </Card>
    </Box>
  );
}
