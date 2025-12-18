import { useTags } from "@/api/hooks/tags"
import { Box, TextField } from "@mui/material"
import { Controller, type FieldPath, type FieldValues, type UseFormReturn } from "react-hook-form"
import { TagsSelect } from "./TagsSelect"

interface GastroFormProps<T extends FieldValues> {
  formContext: UseFormReturn<T>
  onSubmit: (data: T) => void
  showAddDate?: boolean
  dateFieldName?: string
}

export const GastroForm = <T extends FieldValues>({
  formContext,
  onSubmit,
  showAddDate = true,
  dateFieldName = "day",
}: GastroFormProps<T>) => {
  const { data: tagsData } = useTags({
    pageIndex: 0,
    pageSize: 25,
  })

  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = formContext

  const formatDateInput = (value: unknown) => {
    if (!value) return ""
    const date = value instanceof Date ? value : new Date(value as string)
    return Number.isNaN(date.getTime()) ? "" : date.toISOString().slice(0, 10)
  }

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

      <Controller
        name={dateFieldName as FieldPath<T>}
        control={control}
        render={({ field, fieldState }) => (
          <TextField
            type="date"
            label="Dzień"
            InputLabelProps={{ shrink: true }}
            value={formatDateInput(field.value)}
            onChange={(e) => {
              const next = e.target.value ? new Date(e.target.value) : null
              field.onChange(next)
            }}
            error={!!fieldState.error}
            helperText={fieldState.error?.message}
          />
        )}
      />

      <TagsSelect
        formContext={formContext}
        tags={tagsData?.data}
        fieldName="tags"
        label="Tagi"
      />

      {showAddDate && (
        <input type="hidden" {...register("addDate", { valueAsDate: true })} />
      )}
    </Box>
  )
}
