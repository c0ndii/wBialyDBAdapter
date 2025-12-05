import { useCreateGastro, useGastros } from "@/api/hooks/gastro"
import { PostList } from "@/components/PostList"
import { type GastroSchema, gastroSchema } from "@/schema/gastro.schema"
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
import { useState } from "react"
import { useForm, type UseFormReturn } from "react-hook-form"

export const Route = createLazyFileRoute("/gastros/")({
  component: GastroView,
})

function GastroView() {
  const [open, setOpen] = useState(false)
  const { data } = useGastros({
    pageIndex: 0,
    pageSize: 5,
  })

  const { mutateAsync } = useCreateGastro()

  const handleSubmit = (data: GastroSchema) => {
    mutateAsync(data)
    form.reset()
    handleClose()
  }

  const form = useForm<GastroSchema>({
    defaultValues: {
      title: "",
      description: "",
      author: "",
      addDate: new Date(),
      link: "",
      place: "",
      day: new Date(),
      tags: [],
    },
    resolver: zodResolver(gastroSchema),
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
          + Gastro
        </Button>
      </Box>
      <PostList events={data?.data} type="gastro" />

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
  onSubmit: (value: GastroSchema) => void
  formContext: UseFormReturn<GastroSchema>
}

const CreateForm = ({ onSubmit, formContext }: Props) => {
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
