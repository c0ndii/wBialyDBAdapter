import {
  Avatar,
  Box,
  Checkbox,
  FormControlLabel,
  FormGroup,
  Typography,
} from "@mui/material";

type PermissionUser = {
  id: number;
  username: string;
};

type PermissionsUserListProps = {
  users: PermissionUser[];
  selectedUsers: number[];
  onToggleUser: (userId: number) => void;
};

export const PermissionsUserList = ({
  users,
  selectedUsers,
  onToggleUser,
}: PermissionsUserListProps) => {
  return (
    <FormGroup sx={{ gap: 0.75, mt: 1 }}>
      {users.map((user) => (
        <Box
          key={user.id}
          sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            px: 1,
            py: 0.25,
            borderRadius: 1.5,
            backgroundColor: "action.hover",
          }}
        >
          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <Avatar sx={{ width: 28, height: 28 }}>
              {user.username.charAt(0).toUpperCase()}
            </Avatar>
            <Typography variant="body2">{user.username}</Typography>
          </Box>
          <FormControlLabel
            sx={{ mr: 0 }}
            control={
              <Checkbox
                checked={selectedUsers.includes(user.id)}
                onChange={() => onToggleUser(user.id)}
              />
            }
            label=""
          />
        </Box>
      ))}
    </FormGroup>
  );
};
