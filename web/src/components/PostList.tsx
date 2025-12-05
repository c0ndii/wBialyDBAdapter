import type { Event } from "@/api/hooks/events"
import type { Gastro } from "@/api/hooks/gastro"
import EventIcon from "@mui/icons-material/Event"
import FastfoodIcon from "@mui/icons-material/Fastfood"
import { Box } from "@mui/material"
import { Post } from "./Post"

export function PostList({
  events = [],
  type,
}: {
  events?: Event[] | Gastro[]
  type: "event" | "gastro"
}) {
  return (
    <Box
      sx={{
        maxWidth: 880,
        mx: "auto",
        my: 4,
        display: "flex",
        flexDirection: "column",
        gap: 2,
      }}
    >
      {events.map((event) => (
        <Post key={event.id}>
          <Box sx={{ display: "flex", justifyContent: "space-between" }}>
            <Post.Title link={`/${type}s/${event.id}`}>
              {event.title}
            </Post.Title>
            {type === "event" ? <EventIcon /> : <FastfoodIcon />}
          </Box>

          <Post.Description>{event.description}</Post.Description>

          <Post.Separator />

          <Post.Footer
            left={<Post.Tags tags={event.tags.map((t) => t.name)} />}
            right={
              <Post.DateText>
                {`dodane: ${new Date(event.addDate).toLocaleString("pl-PL")}`}
              </Post.DateText>
            }
          />
        </Post>
      ))}
    </Box>
  )
}
