import { getStyles } from "@/utils/getStyles"
import {
  Box,
  Chip,
  FormControl,
  InputLabel,
  MenuItem,
  OutlinedInput,
  Select,
  useTheme,
} from "@mui/material"
import {
  Controller,
  type FieldPath,
  type FieldValues,
  type UseFormReturn,
} from "react-hook-form"

export interface Tag {
  id: string
  name: string
}

interface TagsSelectProps<T extends FieldValues> {
  formContext: UseFormReturn<T>
  tags: Tag[] | undefined
  fieldName: FieldPath<T>
  label?: string
}

export const TagsSelect = <T extends FieldValues>({
  formContext,
  tags = [],
  fieldName,
  label = "Tagi",
}: TagsSelectProps<T>) => {
  const theme = useTheme()

  return (
    <Controller
      name={fieldName}
      control={formContext.control}
      render={({ field }) => (
        <FormControl fullWidth>
          <InputLabel id={`${fieldName}-label`}>{label}</InputLabel>
          <Select
            labelId={`${fieldName}-label`}
            multiple
            value={
              Array.isArray(field.value) && field.value.length > 0
                ? field.value.map((tag: Tag) => tag.id)
                : []
            }
            onChange={(e) => {
              const selectedIds = e.target.value as string[]
              const selectedTags =
                tags?.filter((tag) => selectedIds.includes(tag.id)) || []
              field.onChange(selectedTags)
            }}
            input={<OutlinedInput label={label} />}
            renderValue={(selected: string[]) => (
              <Box sx={{ display: "flex", flexWrap: "wrap", gap: 0.5 }}>
                {selected.map((tagId) => {
                  const tag = tags?.find((t) => t.id === tagId)
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
            {tags?.map((tag) => (
              <MenuItem
                key={tag.id}
                value={tag.id}
                style={getStyles(tag, tags, theme)}
              >
                {tag.name}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      )}
    />
  )
}
