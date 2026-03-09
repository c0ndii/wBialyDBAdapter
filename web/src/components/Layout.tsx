import { useLogout } from "@/api/hooks/user";
import { useAuth } from "@/hooks/useAuth";
import MonitorHeartIcon from "@mui/icons-material/MonitorHeart";
import { AppBar, Box, IconButton, Toolbar, Typography } from "@mui/material";
import { Link, Outlet } from "@tanstack/react-router";
import { CurrentUserChip } from "./CurrentUserChip";
import { ThemePicker } from "./ThemePicker";

export const Layout = () => {
  const { isAuthenticated, me } = useAuth();
  const logout = useLogout();

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        minHeight: "100vh",
        width: "100%",
      }}
    >
      <AppBar
        position="static"
        elevation={0}
        sx={{
          background: "rgba(15, 23, 42, 0.92)",
          backdropFilter: "blur(6px)",
          borderBottom: "1px solid rgba(255,255,255,0.08)",
        }}
      >
        <Toolbar
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            maxWidth: 1180,
            width: "100%",
            mx: "auto",
            px: 2,
          }}
        >
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              gap: 2.5,
              height: "100%",
            }}
          >
            <Typography
              component={Link}
              variant="h5"
              fontWeight={700}
              to="/"
              sx={(theme) => ({
                textDecoration: "none",
                letterSpacing: 0.5,
                color: theme.palette.getContrastText(
                  theme.palette.primary.main,
                ),
                "&:hover": { opacity: 0.9 },
              })}
            >
              wBialy
            </Typography>
            <Typography
              component={Link}
              variant="body1"
              to="/gastros"
              sx={(theme) => ({
                textDecoration: "none",
                fontWeight: 600,
                color: theme.palette.getContrastText(
                  theme.palette.primary.main,
                ),
                "&:hover": { opacity: 0.9 },
              })}
            >
              Gastro
            </Typography>
            <Typography
              component={Link}
              variant="body1"
              to="/messages"
              sx={(theme) => ({
                textDecoration: "none",
                fontWeight: 600,
                color: theme.palette.getContrastText(
                  theme.palette.primary.main,
                ),
                "&:hover": { opacity: 0.9 },
              })}
            >
              Messages
            </Typography>
          </Box>
          <Box sx={{ display: "flex", alignItems: "center", gap: 2.5 }}>
            {!isAuthenticated && (
              <>
                <Typography
                  component={Link}
                  variant="body1"
                  to="/register"
                  sx={(theme) => ({
                    textDecoration: "none",
                    fontWeight: 600,
                    color: theme.palette.getContrastText(
                      theme.palette.primary.main,
                    ),
                    "&:hover": { opacity: 0.9 },
                  })}
                >
                  Register
                </Typography>
                <Typography
                  component={Link}
                  variant="body1"
                  to="/login"
                  sx={(theme) => ({
                    textDecoration: "none",
                    fontWeight: 600,
                    color: theme.palette.getContrastText(
                      theme.palette.primary.main,
                    ),
                    "&:hover": { opacity: 0.9 },
                  })}
                >
                  Login
                </Typography>
              </>
            )}
            {isAuthenticated && (
              <>
                <Typography
                  component="button"
                  variant="body1"
                  onClick={() => logout.mutate()}
                  sx={(theme) => ({
                    border: "none",
                    background: "transparent",
                    cursor: "pointer",
                    textDecoration: "none",
                    fontWeight: 600,
                    fontFamily: "inherit",
                    fontSize: "inherit",
                    color: theme.palette.getContrastText(
                      theme.palette.primary.main,
                    ),
                    "&:hover": { opacity: 0.9 },
                  })}
                >
                  Logout
                </Typography>
                {me?.username && (
                  <Typography
                    component={Link}
                    variant="body1"
                    to="/security/settings"
                    sx={(theme) => ({
                      textDecoration: "none",
                      fontWeight: 600,
                      color: theme.palette.getContrastText(
                        theme.palette.primary.main,
                      ),
                      "&:hover": { opacity: 0.9 },
                    })}
                  >
                    {me?.username && <CurrentUserChip username={me.username} />}
                  </Typography>
                )}
                <IconButton
                  component={Link}
                  to="/security/logs"
                  sx={(theme) => ({
                    textDecoration: "none",
                    fontWeight: 600,
                    color: theme.palette.getContrastText(
                      theme.palette.primary.main,
                    ),
                    "&:hover": { opacity: 0.9 },
                  })}
                >
                  <MonitorHeartIcon />
                </IconButton>
              </>
            )}
            <ThemePicker />
          </Box>
          {/* <DatabasePicker /> */}
        </Toolbar>
      </AppBar>
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          flex: 1,
          backgroundColor: (theme) => theme.palette.background.default,
          px: 2,
          py: 3,
        }}
      >
        <Box
          sx={{
            maxWidth: 1180,
            width: "100%",
            mx: "auto",
            display: "flex",
            flexDirection: "column",
            flex: 1,
            gap: 3,
          }}
        >
          <Outlet />
        </Box>
      </Box>
    </Box>
  );
};
