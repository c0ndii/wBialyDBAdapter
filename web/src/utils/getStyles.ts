import type { Tag } from "@/api/types"
import type { Theme } from "@mui/material"

export function getStyles(tag: Tag, tags: Tag[], theme: Theme) {
  return {
    fontWeight: tags.includes(tag)
      ? theme.typography.fontWeightMedium
      : theme.typography.fontWeightRegular,
  }
}
