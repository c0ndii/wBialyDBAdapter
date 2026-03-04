import { useTheme } from "@/hooks/useTheme";
import BedtimeIcon from "@mui/icons-material/Bedtime";
import SunnyIcon from "@mui/icons-material/Sunny";
import { IconButton } from "@mui/material";

export function ThemePicker() {
  const { theme, setTheme } = useTheme();
  const isLight = theme === "light";
  const handleClick = () => {
    // eslint-disable-next-line @typescript-eslint/no-unused-expressions
    isLight ? setTheme("dark") : setTheme("light");
  };

  return (
    <div>
      <IconButton onClick={handleClick} color="inherit">
        {isLight ? <SunnyIcon /> : <BedtimeIcon />}
      </IconButton>
    </div>
  );
}
