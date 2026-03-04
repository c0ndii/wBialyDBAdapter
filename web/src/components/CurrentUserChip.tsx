import { Avatar, Chip } from "@mui/material";

type CurrentUserChipProps = {
  username: string;
};

export const CurrentUserChip = ({ username }: CurrentUserChipProps) => {
  return (
    <Chip
      avatar={<Avatar>{username.slice(0, 1).toUpperCase()}</Avatar>}
      label={username}
      variant="outlined"
      sx={(theme) => ({
        color: theme.palette.getContrastText(theme.palette.primary.main),
        borderColor: "rgba(255,255,255,0.35)",
        "& .MuiChip-avatar": {
          bgcolor: "rgba(255,255,255,0.18)",
          color: theme.palette.getContrastText(theme.palette.primary.main),
        },
      })}
    />
  );
};
