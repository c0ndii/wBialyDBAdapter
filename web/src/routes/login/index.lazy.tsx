import { useLogin, useVerifyLogin } from "@/api/hooks/user";
import type { LoginChallengeResponse } from "@/api/types";
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
import { useMemo, useState } from "react";
import { useForm } from "react-hook-form";

const PASSWORD_MANAGER_STORAGE_KEY = "security.passwordManagerEnabled";

export const Route = createLazyFileRoute("/login/")({
  component: LoginView,
});

function LoginView() {
  const [challenge, setChallenge] = useState<LoginChallengeResponse | null>(
    null,
  );
  const [challengeValues, setChallengeValues] = useState<
    Record<number, string>
  >({});
  const [challengeError, setChallengeError] = useState<string | null>(null);

  const isPasswordManagerEnabled = useMemo(() => {
    const stored = localStorage.getItem(PASSWORD_MANAGER_STORAGE_KEY);
    return stored == null ? true : stored === "true";
  }, []);

  const form = useForm<LoginSchema>({
    defaultValues: {
      login: "",
    },
    resolver: zodResolver(loginSchema),
  });

  const login = useLogin();
  const verifyLogin = useVerifyLogin();

  const resetChallenge = () => {
    setChallenge(null);
    setChallengeValues({});
    setChallengeError(null);
    verifyLogin.reset();
    login.reset();
  };

  const handleLogin = form.handleSubmit(async (data) => {
    setChallengeError(null);
    const response = await login.mutateAsync({
      login: data.login,
    });

    setChallenge(response);
    setChallengeValues(
      response.requiredPositions.reduce<Record<number, string>>((acc, pos) => {
        acc[pos] = "";
        return acc;
      }, {}),
    );
  });

  const handleVerify = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!challenge) {
      return;
    }

    const providedCharacters: Record<number, string> = {};

    for (const position of challenge.requiredPositions) {
      const value = challengeValues[position]?.trim() ?? "";
      if (value.length !== 1) {
        setChallengeError(
          "Każda wymagana pozycja musi zawierać dokładnie 1 znak.",
        );
        return;
      }

      providedCharacters[position] = value;
    }

    setChallengeError(null);
    await verifyLogin.mutateAsync({
      login: form.getValues("login"),
      partialPasswordId: challenge.partialPasswordId,
      providedCharacters,
    });
  };

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
            onSubmit={challenge ? handleVerify : handleLogin}
          >
            <TextField
              id="login"
              label="Login"
              autoComplete={isPasswordManagerEnabled ? "username" : "off"}
              {...form.register("login")}
              disabled={!!challenge}
              error={!!form.formState.errors.login}
              helperText={form.formState.errors.login?.message}
              sx={{ width: 350 }}
            />

            {challenge && (
              <>
                <Typography variant="body2" color="text.secondary">
                  Input partial password for positions:{" "}
                  {challenge.requiredPositions.join(", ")}
                </Typography>
                <Box
                  sx={{
                    display: "grid",
                    gap: 1,
                    gridTemplateColumns:
                      "repeat(auto-fill, minmax(52px, 52px))",
                    justifyContent: "center",
                  }}
                >
                  {Array.from(
                    { length: challenge.totalPasswordLength },
                    (_, index) => index + 1,
                  ).map((position) => {
                    const isRequired =
                      challenge.requiredPositions.includes(position);

                    return (
                      <TextField
                        key={position}
                        value={
                          isRequired ? (challengeValues[position] ?? "") : "*"
                        }
                        type={"password"}
                        disabled={!isRequired}
                        onChange={(event) => {
                          const nextValue = event.target.value.slice(0, 1);
                          setChallengeValues((previous) => ({
                            ...previous,
                            [position]: nextValue,
                          }));
                        }}
                        inputProps={{ maxLength: 1 }}
                        sx={{
                          width: 52,
                          "& .MuiInputBase-input": {
                            textAlign: "center",
                            p: 1,
                          },
                        }}
                      />
                    );
                  })}
                </Box>
              </>
            )}

            <Button type="submit" variant="contained">
              {challenge ? "Verify" : "Login"}
            </Button>
            {challenge && (
              <Button type="button" variant="text" onClick={resetChallenge}>
                Change login
              </Button>
            )}
            {login.isError && (
              <Typography variant="body2" color="error">
                {login.error instanceof Error
                  ? login.error.message
                  : "Login failed"}
              </Typography>
            )}
            {challengeError && (
              <Typography variant="body2" color="error">
                {challengeError}
              </Typography>
            )}
            {verifyLogin.isError && (
              <Typography variant="body2" color="error">
                {verifyLogin.error instanceof Error
                  ? verifyLogin.error.message
                  : "Verification failed"}
              </Typography>
            )}
          </Box>
        </CardContent>
      </Card>
    </Box>
  );
}
