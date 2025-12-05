import type { Event } from "@/api/hooks/events"
import type { Gastro } from "@/api/hooks/gastro"
import {
  Box,
  Button,
  Card,
  CardContent,
  Chip,
  Divider,
  Link as MuiLink,
  Stack,
  Typography,
} from "@mui/material"

type Props = {
  data: Event | Gastro
  onDelete: () => void
  onEdit: () => void
}

export function PostDetails({ data, onDelete, onEdit }: Props) {
  const isEvent = "eventDate" in data

  return (
    <Box
      sx={{
        maxWidth: 880,
        mx: "auto",
        my: 4,
      }}
    >
      <Card variant="outlined" sx={{ borderRadius: 2 }}>
        <CardContent sx={{ p: 3, "&:last-child": { pb: 3 } }}>
          <Stack
            direction="row"
            alignItems="center"
            justifyContent="space-between"
            mb={1}
          >
            <Typography variant="h5" fontWeight={700}>
              {data.title}
            </Typography>

            <Stack direction="row" spacing={1}>
              <Button
                size="small"
                variant="outlined"
                color="primary"
                onClick={onEdit}
              >
                Edytuj
              </Button>
              <Button
                size="small"
                variant="outlined"
                color="error"
                onClick={onDelete}
              >
                Usuń
              </Button>
            </Stack>
          </Stack>

          {data.place && (
            <Typography variant="body2" color="text.secondary" gutterBottom>
              {data.place}
            </Typography>
          )}

          <Typography variant="body1" sx={{ whiteSpace: "pre-line", my: 2 }}>
            {data.description}
          </Typography>

          <Typography variant="body2" sx={{ mb: 1 }}>
            {isEvent
              ? `Data wydarzenia: ${new Date(data.eventDate).toLocaleString(
                  "pl-PL"
                )}`
              : `Dzień: ${new Date(data.day).toLocaleString("pl-PL")}`}
          </Typography>

          <Typography variant="caption" color="text.secondary">
            dodane: {new Date(data.addDate).toLocaleString("pl-PL")}
          </Typography>

          <Divider sx={{ my: 2 }} />

          <Stack direction="row" spacing={1} flexWrap="wrap" sx={{ mb: 2 }}>
            {data.tags?.map((t) => (
              <Chip key={t.name} label={t.name} size="small" />
            ))}
          </Stack>

          {data.author && (
            <Typography variant="body2" sx={{ mb: 1 }}>
              Autor: <b>{data.author}</b>
            </Typography>
          )}

          {data.link && (
            <MuiLink
              href={data.link}
              target="_blank"
              underline="hover"
              variant="body2"
              color="primary"
              sx={{ mt: 1, display: "inline-block" }}
            >
              Zobacz więcej
            </MuiLink>
          )}
        </CardContent>
      </Card>
    </Box>
  )
}
