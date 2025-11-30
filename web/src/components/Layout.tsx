import { AppBar, Box, Toolbar, Typography } from "@mui/material"
import { Link, Outlet } from "@tanstack/react-router"

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
            gap: 2,
          }}
        >
          <Typography
            component={Link}
            variant="h5"
            fontWeight="700"
            to="/"
            sx={(theme) => ({
              textDecoration: "none",
              color: theme.palette.getContrastText(theme.palette.primary.main),
              "&.active": {},
            })}
          >
            wBialy
          </Typography>
          <Link to="/test">test</Link>
        </Toolbar>
      </AppBar>
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          flex: 1,
          flexGrow: 1,
          boxSizing: "border-box",
          marginLeft: "58px",
        }}
      >
        <Outlet />
      </Box>
    </Box>
  )
}
