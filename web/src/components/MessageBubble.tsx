import DeleteOutlineRoundedIcon from "@mui/icons-material/DeleteOutlineRounded";
import EditRoundedIcon from "@mui/icons-material/EditRounded";
import InfoOutlinedIcon from "@mui/icons-material/InfoOutlined";
import ManageAccountsRoundedIcon from "@mui/icons-material/ManageAccountsRounded";
import {
  Avatar,
  Box,
  IconButton,
  Paper,
  Tooltip,
  Typography,
} from "@mui/material";

type MessageBubbleProps = {
  id: number;
  author: string;
  content: string;
  modifiedAt: string;
  createdAt: string;
  latestModifyUsername: string;
  isMine?: boolean;
  canEdit?: boolean;
  canManagePermissions?: boolean;
  onDelete?: (messageId: number) => void;
  onEdit?: (messageId: number) => void;
  onManagePermissions?: (messageId: number) => void;
};

export const MessageBubble = ({
  id,
  author,
  content,
  modifiedAt,
  createdAt,
  latestModifyUsername,
  isMine = false,
  canEdit = false,
  canManagePermissions = false,
  onDelete,
  onEdit,
  onManagePermissions,
}: MessageBubbleProps) => {
  const showActions = Boolean(onDelete || onEdit || onManagePermissions);

  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: isMine ? "flex-end" : "flex-start",
        minWidth: 260,
      }}
    >
      <Box
        sx={{
          display: "flex",
          flexDirection: isMine ? "row-reverse" : "row",
          alignItems: "flex-end",
          gap: 1,
          maxWidth: "75%",
          minWidth: 260,
        }}
      >
        <Avatar sx={{ width: 30, height: 30 }}>
          {author.slice(0, 1).toUpperCase()}
        </Avatar>
        <Box sx={{ minWidth: 220 }}>
          <Paper
            elevation={1}
            sx={(theme) => ({
              px: 1.5,
              py: 1,
              borderRadius: 2,
              backgroundColor: isMine
                ? theme.palette.primary.main
                : theme.palette.background.paper,
              color: isMine
                ? theme.palette.primary.contrastText
                : theme.palette.text.primary,
            })}
          >
            {!isMine && (
              <Typography variant="caption" sx={{ fontWeight: 700 }}>
                {author}
              </Typography>
            )}
            <Typography variant="body2">{content}</Typography>
            <Box sx={{ display: "flex", justifyContent: "flex-end", mt: 0.5 }}>
              <Tooltip
                title={`Edited by ${latestModifyUsername} at ${modifiedAt}`}
              >
                <InfoOutlinedIcon
                  fontSize="small"
                  sx={(theme) => ({
                    opacity: 0.85,
                    color: isMine
                      ? theme.palette.primary.contrastText
                      : theme.palette.text.secondary,
                    cursor: "help",
                  })}
                />
              </Tooltip>
            </Box>
          </Paper>
          <Typography
            variant="caption"
            sx={(theme) => ({
              display: "block",
              mt: 0.5,
              px: 1,
              opacity: 0.75,
              color: theme.palette.text.secondary,
              textAlign: isMine ? "right" : "left",
            })}
          >
            {createdAt}
          </Typography>
        </Box>
        {showActions && (
          <Box sx={{ display: "flex", gap: 0.25, pb: 2.25 }}>
            {canEdit && onEdit && (
              <Tooltip title="Edit message">
                <IconButton size="small" onClick={() => onEdit(id)}>
                  <EditRoundedIcon fontSize="small" />
                </IconButton>
              </Tooltip>
            )}
            {canManagePermissions && onManagePermissions && (
              <Tooltip title="Manage permissions">
                <IconButton
                  size="small"
                  onClick={() => onManagePermissions(id)}
                >
                  <ManageAccountsRoundedIcon fontSize="small" />
                </IconButton>
              </Tooltip>
            )}
            {isMine && onDelete && (
              <Tooltip title="Delete message">
                <IconButton
                  size="small"
                  color="error"
                  onClick={() => onDelete(id)}
                >
                  <DeleteOutlineRoundedIcon fontSize="small" />
                </IconButton>
              </Tooltip>
            )}
          </Box>
        )}
      </Box>
    </Box>
  );
};
