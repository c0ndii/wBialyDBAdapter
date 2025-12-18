import { useCreateEvent, useEvents } from "@/api/hooks/events"
import { EventForm } from "@/components/EventForm"
import { PostList } from "@/components/PostList"
import { eventSchema, type EventSchema } from "@/schema/events.schema"
import { zodResolver } from "@hookform/resolvers/zod"
import AddRoundedIcon from "@mui/icons-material/AddRounded"
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material"
import { createFileRoute } from "@tanstack/react-router"
import { useState } from "react"
import { useForm, type UseFormReturn } from "react-hook-form"

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

  const defaultValues: EventSchema = {
    title: "",
    description: "",
    author: "",
    addDate: new Date(),
    link: "",
    place: "",
    eventDate: new Date(),
    tags: [],
  }

  const form = useForm<EventSchema>({
    defaultValues,
    resolver: zodResolver(eventSchema),
  })

  const handleClickOpen = () => {
    form.reset(defaultValues)
    setOpen(true)
  }

  const handleClose = () => {
    form.reset(defaultValues)
    setOpen(false)
  }

  return (
    <Box>
      <Box>
        <Button
          color="primary"
          variant="contained"
          startIcon={<AddRoundedIcon />}
          onClick={handleClickOpen}
          sx={{
            textTransform: "none",
            borderRadius: 999,
            boxShadow: "0 10px 30px rgba(59,130,246,0.25)",
            px: 2.5,
          }}
        >
          Dodaj event
        </Button>
      </Box>
      <PostList events={data?.data} type="event" />
      <Dialog
        open={open}
        onClose={handleClose}
        PaperProps={{
          sx: {
            borderRadius: 3,
            p: 1,
            boxShadow: "0 20px 60px rgba(15,23,42,0.2)",
            minWidth: 520,
          },
        }}
      >
        <DialogTitle sx={{ fontWeight: 700, pb: 1, px: 2.5 }}>
          Dodaj
        </DialogTitle>
        <DialogContent sx={{ pt: 0, px: 2.5 }}>
          <CreateForm formContext={form} onSubmit={handleSubmit} />
        </DialogContent>
        <DialogActions sx={{ px: 2.5, pb: 2, pt: 1.5, gap: 1 }}>
          <Button color="inherit" onClick={handleClose}>
            Anuluj
          </Button>
          <Button
            type="submit"
            form="createProjectForm"
            color="success"
            autoFocus
          >
            Dodaj
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
  return <EventForm formContext={formContext} onSubmit={onSubmit} />
}
