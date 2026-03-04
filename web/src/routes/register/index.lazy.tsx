import { useRegister } from "@/api/hooks/user";
import { registerSchema, type RegisterSchema } from "@/schema/register.schema";
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

export const Route = createLazyFileRoute("/register/")({
  component: RegisterView,
});

function RegisterView() {
  const form = useForm<RegisterSchema>({
    defaultValues: {
      login: "",
      password: "",
      confirmPassword: "",
    },
    resolver: zodResolver(registerSchema),
  });

  const register = useRegister();

  const handleRegister = form.handleSubmit((data) =>
    register.mutateAsync({
      login: data.login,
      username: data.username,
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
        <CardHeader title="Register" subheader="Create a new account" />
        <CardContent>
          <Box
            component="form"
            sx={{ display: "flex", flexDirection: "column", gap: 2 }}
            noValidate
            autoComplete="off"
            onSubmit={handleRegister}
          >
            <TextField
              id="username"
              label="Username"
              autoComplete="username"
              {...form.register("username")}
              error={!!form.formState.errors.username}
              helperText={form.formState.errors.username?.message}
              sx={{ width: 350 }}
            />
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
            <TextField
              id="confirmPassword"
              label="Confirm Password"
              type="password"
              autoComplete="new-password"
              {...form.register("confirmPassword")}
              error={!!form.formState.errors.confirmPassword}
              helperText={form.formState.errors.confirmPassword?.message}
              sx={{ width: 350 }}
            />
            <Button type="submit" variant="contained">
              Register
            </Button>
          </Box>
        </CardContent>
      </Card>
    </Box>
  );
}
