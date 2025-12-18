import { AppBar, Box, Toolbar, Typography } from "@mui/material"
import { Link, Outlet } from "@tanstack/react-router"
import { DatabasePicker } from "./DatabasePicker"

export const Layout = () => {
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
                  theme.palette.primary.main
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
                  theme.palette.primary.main
                ),
                "&:hover": { opacity: 0.9 },
              })}
            >
              Gastro
            </Typography>
          </Box>
          <DatabasePicker />
        </Toolbar>
      </AppBar>
      <Box
        sx={(theme) => ({
          display: "flex",
          flexDirection: "column",
          flex: 1,
          backgroundColor: "transparent",
          px: 2,
          py: 3,
        })}
      >
        <Box
          sx={{
            maxWidth: 1180,
            width: "100%",
            mx: "auto",
            display: "flex",
            flexDirection: "column",
            gap: 3,
          }}
        >
          <Outlet />
        </Box>
      </Box>
    </Box>
  )
}
