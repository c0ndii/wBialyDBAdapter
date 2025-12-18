import { useDeleteEvent, useEvent, useUpdateEvent } from "@/api/hooks/events"
import { EventForm } from "@/components/EventForm"
import { PostDetails } from "@/components/PostDetails"
import { editEventSchema, type EditEventSchema } from "@/schema/events.schema"
import { zodResolver } from "@hookform/resolvers/zod"
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material"
import { createLazyFileRoute } from "@tanstack/react-router"
import { useEffect, useState } from "react"
import { useForm, type UseFormReturn } from "react-hook-form"

export const Route = createLazyFileRoute("/events/$eventId")({
  component: GastroView,
})

function GastroView() {
  const [open, setOpen] = useState(false)
  const { eventId } = Route.useParams()
  const { data } = useEvent(eventId)
  const mutation = useDeleteEvent()
  const { mutateAsync } = useUpdateEvent()

  const handleSubmit = (data: EditEventSchema) => {
    mutateAsync({
      id: data.id,
      data: data,
    })
    form.reset()
    handleClose()
  }

  const form = useForm<EditEventSchema>({
    defaultValues: {
      id: eventId,
      title: "",
      description: "",
      author: "",
      addDate: new Date(),
      link: "",
      place: "",
      eventDate: new Date(),
      tags: [],
    },
    resolver: zodResolver(editEventSchema),
  })

  useEffect(() => {
    if (!data?.data) return

    form.reset({
      id: data.data.id,
      title: data.data.title,
      description: data.data.description,
      author: data.data.author,
      addDate: new Date(data.data.addDate),
      link: data.data.link,
      place: data.data.place,
      eventDate: new Date(data.data.eventDate),
      tags: data.data.tags || [],
    })
  }, [data, form])

  const handleClickOpen = () => {
    if (data?.data) {
      form.reset({
        id: data.data.id,
        title: data.data.title,
        description: data.data.description,
        author: data.data.author,
        addDate: new Date(data.data.addDate),
        link: data.data.link,
        place: data.data.place,
        eventDate: new Date(data.data.eventDate),
        tags: data.data.tags || [],
      })
    }
    setOpen(true)
  }

  const handleClose = () => {
    if (data?.data) {
      form.reset({
        id: data.data.id,
        title: data.data.title,
        description: data.data.description,
        author: data.data.author,
        addDate: new Date(data.data.addDate),
        link: data.data.link,
        place: data.data.place,
        eventDate: new Date(data.data.eventDate),
        tags: data.data.tags || [],
      })
    }
    setOpen(false)
  }

  const handleDelete = () => {
    mutation.mutateAsync(eventId)
  }

  if (!data?.data) return null

  if (!data?.data) return null
  return (
    <Box>
      <PostDetails
        data={data?.data}
        onDelete={handleDelete}
        onEdit={handleClickOpen}
      />
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
          Edytuj
        </DialogTitle>
        <DialogContent sx={{ pt: 0, px: 2.5 }}>
          <EditForm formContext={form} onSubmit={handleSubmit} />
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
            Edytuj
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}

type Props = {
  onSubmit: (value: EditEventSchema) => void
  formContext: UseFormReturn<EditEventSchema>
}

const EditForm = ({ onSubmit, formContext }: Props) => {
  const { register } = formContext

  return (
    <>
      <input type="hidden" {...register("id")} />
      <EventForm
        formContext={formContext}
        onSubmit={onSubmit}
        showAddDate={false}
      />
    </>
  )
}
