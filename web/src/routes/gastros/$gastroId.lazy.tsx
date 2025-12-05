import { useDeleteGastro, useGastro, useUpdateGastro } from "@/api/hooks/gastro"
import { PostDetails } from "@/components/PostDetails"
import { editGastroSchema, type EditGastroSchema } from "@/schema/gastro.schema"
import { zodResolver } from "@hookform/resolvers/zod"
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
} from "@mui/material"
import { createLazyFileRoute } from "@tanstack/react-router"
import { useEffect, useState } from "react"
import { useForm, type UseFormReturn } from "react-hook-form"

export const Route = createLazyFileRoute("/gastros/$gastroId")({
  component: GastroView,
})

function GastroView() {
  const [open, setOpen] = useState(false)
  const { gastroId } = Route.useParams()
  const { data } = useGastro(gastroId)
  const mutation = useDeleteGastro()
  const { mutateAsync } = useUpdateGastro()

  const handleSubmit = (data: EditGastroSchema) => {
    mutateAsync({
      id: data.id,
      data: data,
    })
    form.reset()
    handleClose()
  }

  const form = useForm<EditGastroSchema>({
    defaultValues: {
      id: gastroId,
      title: data?.data.title,
      description: data?.data.description,
      author: data?.data.author,
      addDate: data?.data.addDate ? new Date(data.data.addDate) : new Date(),
      link: data?.data.link,
      place: data?.data.place,
      day: data?.data.day ? new Date(data.data.day) : new Date(),
      tags: [],
    },
    resolver: zodResolver(editGastroSchema),
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
      day: new Date(data.data.day),
      tags: [],
    })
  }, [data, form])

  const handleClickOpen = () => {
    setOpen(true)
  }

  const handleClose = () => {
    setOpen(false)
  }

  const handleDelete = () => {
    mutation.mutateAsync(gastroId)
  }

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
        sx={{
          display: "flex",
          flexDirection: "column",
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <DialogTitle>Edytuj</DialogTitle>
        <DialogContent>
          <EditForm formContext={form} onSubmit={handleSubmit} />
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
            Edit
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}

type Props = {
  onSubmit: (value: EditGastroSchema) => void
  formContext: UseFormReturn<EditGastroSchema>
}

const EditForm = ({ onSubmit, formContext }: Props) => {
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
      <input type="hidden" {...register("id")} />

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
        {...register("day", {
          valueAsDate: true,
        })}
        error={!!errors.day}
        helperText={errors.day?.message}
      />

      <input type="hidden" {...register("addDate", { valueAsDate: true })} />

      <input
        type="hidden"
        {...register("tags", {
          value: [],
        })}
      />
    </Box>
  )
}
