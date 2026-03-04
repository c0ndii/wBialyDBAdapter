import {
  useCreateMessage,
  useDeleteMessage,
  useMessages,
  useUpdateMessage,
  useUpdateMessageEditors,
} from "@/api/hooks/message";
import { useUsers } from "@/api/hooks/user";
import { MessageBubble } from "@/components/MessageBubble";
import { PermissionsUserList } from "@/components/PermissionsUserList";
import { useAuth } from "@/hooks/useAuth";
import {
  deleteMessageSchema,
  editMessageSchema,
  messagePermissionsSchema,
  messageSchema,
  type DeleteMessageSchema,
  type EditMessageSchema,
  type MessagePermissionsSchema,
  type MessageSchema,
} from "@/schema/message.schema";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Box,
  Button,
  Card,
  CardContent,
  CardHeader,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  Typography,
} from "@mui/material";
import { createLazyFileRoute } from "@tanstack/react-router";
import { useMemo } from "react";
import { useForm, useWatch } from "react-hook-form";

export const Route = createLazyFileRoute("/messages/")({
  component: MessagesView,
});

type Message = {
  id: number;
  authorId: number;
  author: string;
  content: string;
  modifiedAt: string;
  createdAt: string;
  latestModifyUsername: string;
  isMine: boolean;
  canEdit: boolean;
  canManagePermissions: boolean;
  editableBy: number[];
};

function MessagesView() {
  const { me } = useAuth();

  const sendForm = useForm<MessageSchema>({
    defaultValues: {
      content: "",
    },
    resolver: zodResolver(messageSchema),
  });

  const deleteForm = useForm<DeleteMessageSchema>({
    defaultValues: {
      messageId: null,
    },
    resolver: zodResolver(deleteMessageSchema),
  });

  const editForm = useForm<EditMessageSchema>({
    defaultValues: {
      messageId: null,
      content: "",
    },
    resolver: zodResolver(editMessageSchema),
  });

  const permissionsForm = useForm<MessagePermissionsSchema>({
    defaultValues: {
      messageId: null,
      userIds: [],
    },
    resolver: zodResolver(messagePermissionsSchema),
  });

  const { data: messagesResponse, isLoading: isMessagesLoading } =
    useMessages();
  const { data: users = [] } = useUsers();
  const createMessage = useCreateMessage();
  const deleteMessage = useDeleteMessage();
  const updateMessage = useUpdateMessage();
  const updateMessageEditors = useUpdateMessageEditors();

  const messages = useMemo<Message[]>(
    () =>
      (messagesResponse?.messages ?? []).map((message) => ({
        isMine: message.userId === me?.id,
        id: message.id,
        authorId: message.userId,
        author: message.user.username,
        content: message.content,
        modifiedAt: new Date(message.modifiedAt).toLocaleString("pl-PL", {
          dateStyle: "short",
          timeStyle: "medium",
        }),
        createdAt: new Date(message.createdAt).toLocaleString("pl-PL", {
          dateStyle: "short",
          timeStyle: "short",
        }),
        latestModifyUsername: message.latestModifyUsername,
        canEdit:
          message.userId === me?.id ||
          message.canModify.some((user) => user.id === me?.id),
        canManagePermissions: message.userId === me?.id,
        editableBy: message.canModify.map((user) => user.id),
      })),
    [messagesResponse?.messages, me?.id],
  );

  const deleteMessageId = useWatch({
    control: deleteForm.control,
    name: "messageId",
  });
  const editMessageId = useWatch({
    control: editForm.control,
    name: "messageId",
  });
  const permissionsMessageId = useWatch({
    control: permissionsForm.control,
    name: "messageId",
  });
  const selectedPermissionUsers =
    useWatch({
      control: permissionsForm.control,
      name: "userIds",
    }) ?? [];

  const messageToDelete = useMemo(
    () => messages.find((message) => message.id === deleteMessageId),
    [deleteMessageId, messages],
  );

  const messageToManagePermissions = useMemo(
    () => messages.find((message) => message.id === permissionsMessageId),
    [permissionsMessageId, messages],
  );

  const openDeleteDialog = (messageId: number) => {
    deleteForm.setValue("messageId", messageId);
  };

  const openEditDialog = (messageId: number) => {
    const targetMessage = messages.find((message) => message.id === messageId);
    editForm.reset({
      messageId,
      content: targetMessage?.content ?? "",
    });
  };

  const openPermissionsDialog = (messageId: number) => {
    const targetMessage = messages.find((message) => message.id === messageId);
    permissionsForm.reset({
      messageId,
      userIds: targetMessage?.editableBy ?? [],
    });
  };

  const togglePermissionUser = (userId: number) => {
    const currentSelection = permissionsForm.getValues("userIds");
    const updatedSelection = currentSelection.includes(userId)
      ? currentSelection.filter((entry) => entry !== userId)
      : [...currentSelection, userId];

    permissionsForm.setValue("userIds", updatedSelection);
  };

  const handleSend = sendForm.handleSubmit(async (data) => {
    await createMessage.mutateAsync({ content: data.content });
    sendForm.reset();
  });

  const confirmDeleteMessage = deleteForm.handleSubmit(
    async ({ messageId }) => {
      if (!messageId) {
        return;
      }

      await deleteMessage.mutateAsync(messageId);
      deleteForm.reset({ messageId: null });
    },
  );

  const saveMessageEdit = editForm.handleSubmit(
    async ({ messageId, content }) => {
      if (!messageId) {
        return;
      }

      await updateMessage.mutateAsync({
        messageId,
        content: content.trim(),
      });
      editForm.reset({ messageId: null, content: "" });
    },
  );

  const savePermissions = permissionsForm.handleSubmit(
    async ({ messageId, userIds }) => {
      if (!messageId) {
        return;
      }

      await updateMessageEditors.mutateAsync({
        messageId,
        userIds,
      });

      permissionsForm.reset({ messageId: null, userIds: [] });
    },
  );

  const closeDeleteDialog = () => {
    deleteForm.reset({ messageId: null });
  };

  const closeEditDialog = () => {
    editForm.reset({ messageId: null, content: "" });
  };

  const closePermissionsDialog = () => {
    permissionsForm.reset({ messageId: null, userIds: [] });
  };

  const availablePermissionUsers = useMemo(() => {
    if (!messageToManagePermissions) {
      return [];
    }

    return users.filter(
      (user) => user.id !== messageToManagePermissions.authorId,
    );
  }, [messageToManagePermissions, users]);

  const editableByDisplay = useMemo(() => {
    if (!messageToManagePermissions) {
      return "nobody";
    }

    const names = users
      .filter((user) => messageToManagePermissions.editableBy.includes(user.id))
      .map((user) => user.username);

    return names.join(", ") || "nobody";
  }, [messageToManagePermissions, users]);

  return (
    <>
      <Card
        sx={{
          display: "flex",
          flexDirection: "column",
          flex: 1,
          minHeight: 0,
          maxHeight: "85vh",
        }}
      >
        <CardHeader title="Messages" subheader="Conversation preview" />
        <CardContent
          sx={{
            display: "flex",
            flexDirection: "column",
            gap: 2,
            flex: 1,
            minHeight: 0,
            py: 2,
            px: 0,
            "&:last-child": {
              pb: 2,
            },
          }}
        >
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              gap: 1.25,
              flex: 1,
              overflowY: "auto",
              pr: 0.5,
              px: 4,
            }}
          >
            {isMessagesLoading ? (
              <Typography variant="body2" color="text.secondary">
                Loading messages...
              </Typography>
            ) : messages.length === 0 ? (
              <Typography variant="body2" color="text.secondary">
                No messages yet.
              </Typography>
            ) : (
              messages.map((message) => (
                <MessageBubble
                  key={message.id}
                  id={message.id}
                  author={message.author}
                  content={message.content}
                  modifiedAt={message.modifiedAt}
                  createdAt={message.createdAt}
                  latestModifyUsername={message.latestModifyUsername}
                  isMine={message.isMine}
                  canEdit={message.canEdit}
                  canManagePermissions={message.canManagePermissions}
                  onDelete={openDeleteDialog}
                  onEdit={openEditDialog}
                  onManagePermissions={openPermissionsDialog}
                />
              ))
            )}
          </Box>

          <Box
            component="form"
            sx={{
              display: "flex",
              alignItems: "center",
              gap: 1,
              px: 4,
            }}
            onSubmit={handleSend}
          >
            <TextField
              fullWidth
              placeholder="Type your message..."
              size="small"
              {...sendForm.register("content")}
              error={!!sendForm.formState.errors.content}
              helperText={sendForm.formState.errors.content?.message}
            />
            <Button
              type="submit"
              variant="contained"
              disabled={createMessage.isPending}
            >
              Send
            </Button>
          </Box>
        </CardContent>
      </Card>

      <Dialog open={deleteMessageId !== null} onClose={closeDeleteDialog}>
        <DialogTitle>Delete message</DialogTitle>
        <DialogContent>
          <Typography variant="body2">
            Are you sure you want to delete this message?
          </Typography>
          <Typography variant="body2" sx={{ mt: 1, fontWeight: 600 }}>
            {messageToDelete?.content}
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button color="inherit" onClick={closeDeleteDialog}>
            Cancel
          </Button>
          <Button
            color="error"
            variant="contained"
            onClick={confirmDeleteMessage}
          >
            Delete
          </Button>
        </DialogActions>
      </Dialog>

      <Dialog open={editMessageId !== null} onClose={closeEditDialog} fullWidth>
        <DialogTitle>Edit message</DialogTitle>
        <DialogContent>
          <TextField
            fullWidth
            multiline
            minRows={3}
            maxRows={8}
            {...editForm.register("content")}
            error={!!editForm.formState.errors.content}
            helperText={editForm.formState.errors.content?.message}
            sx={{ mt: 1 }}
          />
        </DialogContent>
        <DialogActions>
          <Button color="inherit" onClick={closeEditDialog}>
            Cancel
          </Button>
          <Button
            variant="contained"
            onClick={saveMessageEdit}
            disabled={updateMessage.isPending}
          >
            Save changes
          </Button>
        </DialogActions>
      </Dialog>

      <Dialog
        open={permissionsMessageId !== null}
        onClose={closePermissionsDialog}
        fullWidth
      >
        <DialogTitle>Manage edit permissions</DialogTitle>
        <DialogContent>
          <Typography variant="body2" sx={{ mb: 1 }}>
            Message editable by: {editableByDisplay}
          </Typography>
          <Typography variant="caption" color="text.secondary">
            Checked = user can edit, unchecked = no permission.
          </Typography>
          <PermissionsUserList
            users={availablePermissionUsers}
            selectedUsers={selectedPermissionUsers}
            onToggleUser={togglePermissionUser}
          />
        </DialogContent>
        <DialogActions>
          <Button color="inherit" onClick={closePermissionsDialog}>
            Cancel
          </Button>
          <Button variant="contained" onClick={savePermissions}>
            Save
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
