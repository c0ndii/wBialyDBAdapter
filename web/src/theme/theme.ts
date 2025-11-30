import { grey } from "@mui/material/colors"
import { createTheme } from "@mui/material/styles"
import { blue } from "./colors"

export const dark = createTheme({
  palette: {
    mode: "dark",
    primary: blue,
    background: {
      default: "#121212",
      paper: "#1D1D1D",
    },
    text: {
      primary: "#ffffff",
      secondary: grey[500],
    },
  },
})

export const light = createTheme({
  palette: {
    mode: "light",
    primary: blue,
    background: {
      default: "#fafafa",
      paper: "#ffffff",
    },
    text: {
      primary: "#000000",
      secondary: grey[800],
    },
  },
})
