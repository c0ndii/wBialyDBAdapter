import { AppBar, Box, Toolbar, Typography } from "@mui/material"
import { Link, Outlet } from "@tanstack/react-router"
import { DatabasePicker } from "./DatabasePicker"

export const Layout = () => {
  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        maxHeight: "100vh",
        minHeight: "100vh",
        width: "100vw",
      }}
    >
      <AppBar position="static">
        <Toolbar
          sx={{
            display: "flex",
            justifyContent: "space-between",
          }}
        >
          <Box
            sx={{
              display: "flex",
              alignItems: "flex-end",
              justifyContent: "flex-start",
              gap: 2,
              height: "100%",
            }}
          >
            <Typography
              component={Link}
              variant="h5"
              fontWeight="700"
              to="/"
              sx={(theme) => ({
                textDecoration: "none",
                color: theme.palette.getContrastText(
                  theme.palette.primary.main
                ),
                "&.active": {},
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
                color: theme.palette.getContrastText(
                  theme.palette.primary.main
                ),
                "&.active": {},
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
          flexGrow: 1,
          boxSizing: "border-box",
          backgroundColor: theme.palette.background.default,
          p: 2,
        })}
      >
        <Outlet />
      </Box>
    </Box>
  )
}
