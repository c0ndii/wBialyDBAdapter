import { useTags } from "@/api/hooks/tags"
import { Box, TextField } from "@mui/material"
import {
  Controller,
  type FieldPath,
  type FieldValues,
  type UseFormReturn,
} from "react-hook-form"
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
        {...register("title" as FieldPath<T>)}
        error={!!errors.title}
        helperText={errors.title?.message as unknown as string}
      />

      <TextField
        label="Opis"
        multiline
        minRows={3}
        {...register("description" as FieldPath<T>)}
        error={!!errors.description}
        helperText={errors.description?.message as unknown as string}
      />

      <TextField
        label="Autor"
        {...register("author" as FieldPath<T>)}
        error={!!errors.author}
        helperText={errors.author?.message as unknown as string}
      />

      <TextField
        label="Link"
        {...register("link" as FieldPath<T>)}
        error={!!errors.link}
        helperText={errors.link?.message as unknown as string}
      />

      <TextField
        label="Miejsce"
        {...register("place" as FieldPath<T>)}
        error={!!errors.place}
        helperText={errors.place?.message as unknown as string}
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
        fieldName={"tags" as FieldPath<T>}
        label="Tagi"
      />

      {showAddDate && (
        <input
          type="hidden"
          {...register("addDate" as FieldPath<T>, { valueAsDate: true })}
        />
      )}
    </Box>
  )
}
