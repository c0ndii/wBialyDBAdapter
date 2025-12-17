import { useCreateEvent, useEvents } from "@/api/hooks/events"
import { useTags } from "@/api/hooks/tags"
import { PostList } from "@/components/PostList"
import { eventSchema, type EventSchema } from "@/schema/events.schema"
import { getStyles } from "@/utils/getStyles"
import { zodResolver } from "@hookform/resolvers/zod"
import {
  Box,
  Button,
  Chip,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  InputLabel,
  MenuItem,
  OutlinedInput,
  Select,
  TextField,
  useTheme,
} from "@mui/material"
import { createFileRoute } from "@tanstack/react-router"
import { useState } from "react"
import { Controller, useForm, type UseFormReturn } from "react-hook-form"

export const Route = createFileRoute("/")({
  component: HomeView,
})

function HomeView() {
  const [open, setOpen] = useState(false)
  const { data } = useEvents({
    pageIndex: 0,
    pageSize: 5,
  })

  const { mutateAsync } = useCreateEvent()

  const handleSubmit = (data: EventSchema) => {
    mutateAsync(data)
    form.reset()
    handleClose()
  }

  const form = useForm<EventSchema>({
    defaultValues: {
      title: "",
      description: "",
      author: "",
      addDate: new Date(),
      link: "",
      place: "",
      eventDate: new Date(),
      tags: [],
    },
    resolver: zodResolver(eventSchema),
  })

  const handleClickOpen = () => {
    setOpen(true)
  }

  const handleClose = () => {
    setOpen(false)
  }

  return (
    <Box>
      <Box>
        <Button color="success" variant="outlined" onClick={handleClickOpen}>
          + Event
        </Button>
      </Box>
      <PostList events={data?.data} type="event" />
      <Dialog
        open={open}
        onClose={handleClose}
        sx={{
          display: "flex",
          flexDirection: "column",
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <DialogTitle>Dodaj</DialogTitle>
        <DialogContent>
          <CreateForm formContext={form} onSubmit={handleSubmit} />
        </DialogContent>
        <DialogActions>
          <Button color="inherit" onClick={handleClose}>
            Cancel
          </Button>
          <Button
            type="submit"
            form="createProjectForm"
            color="success"
            autoFocus
          >
            Create
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}

type Props = {
  onSubmit: (value: EventSchema) => void
  formContext: UseFormReturn<EventSchema>
}

const CreateForm = ({ onSubmit, formContext }: Props) => {
  const { data } = useTags({
    pageIndex: 0,
    pageSize: 25,
  })
  const theme = useTheme()
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = formContext

  return (
    <Box
      component="form"
      id="createProjectForm"
      sx={{
        paddingTop: 1,
        display: "flex",
        flexDirection: "column",
        gap: 2,
        minWidth: "500px",
      }}
      onSubmit={handleSubmit(onSubmit)}
    >
      <TextField
        label="Tytuł"
        {...register("title")}
        error={!!errors.title}
        helperText={errors.title?.message}
      />

      <TextField
        label="Opis"
        multiline
        minRows={3}
        {...register("description")}
        error={!!errors.description}
        helperText={errors.description?.message}
      />

      <TextField
        label="Autor"
        {...register("author")}
        error={!!errors.author}
        helperText={errors.author?.message}
      />

      <TextField
        label="Link"
        {...register("link")}
        error={!!errors.link}
        helperText={errors.link?.message}
      />

      <TextField
        label="Miejsce"
        {...register("place")}
        error={!!errors.place}
        helperText={errors.place?.message}
      />

      <TextField
        type="date"
        label="Dzień"
        InputLabelProps={{ shrink: true }}
        {...register("eventDate", {
          valueAsDate: true,
        })}
        error={!!errors.eventDate}
        helperText={errors.eventDate?.message}
      />

      <Controller
        name="tags"
        control={formContext.control}
        render={({ field }) => (
          <FormControl fullWidth>
            <InputLabel id="tags-label">Tagi</InputLabel>
            <Select
              labelId="tags-label"
              multiple
              value={
                Array.isArray(field.value) && field.value.length > 0
                  ? field.value.map((tag) => tag.id)
                  : []
              }
              onChange={(e) => {
                const selectedIds = e.target.value as string[]
                const selectedTags =
                  data?.data.filter((tag) => selectedIds.includes(tag.id)) || []
                field.onChange(selectedTags)
              }}
              input={<OutlinedInput label="Tagi" />}
              renderValue={(selected: string[]) => (
                <Box sx={{ display: "flex", flexWrap: "wrap", gap: 0.5 }}>
                  {selected.map((tagId) => {
                    const tag = data?.data.find((t) => t.id === tagId)
                    return tag ? <Chip key={tagId} label={tag.name} /> : null
                  })}
                </Box>
              )}
              MenuProps={{
                PaperProps: {
                  style: {
                    maxHeight: 8 * 4.5 + 48,
                    width: 250,
                  },
                },
              }}
            >
              {data?.data.map((tag) => (
                <MenuItem
                  key={tag.id}
                  value={tag.id}
                  style={getStyles(tag, data.data, theme)}
                >
                  {tag.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        )}
      />

      <input type="hidden" {...register("addDate", { valueAsDate: true })} />
    </Box>
  )
}
