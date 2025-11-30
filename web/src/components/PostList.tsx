import { Box } from "@mui/material"
import { Post } from "./Post"

export type JobPost = {
  id: number
  title: string
  description: string
  tags: string[]
  date: string
  premium?: boolean
}

const posts: JobPost[] = [
  {
    id: 1,
    title: "szukam pilarza",
    description:
      "Szukam pilarza z uprawnieniami na 1 dzień do wycinki sosen, sprzęt nie jest wymagany.",
    tags: ["d", "u"],
    date: "dodane: 2025-11-29 15:07:40",
    premium: false,
  },
  {
    id: 2,
    title: "Sprzedawca",
    description:
      "Zatrudnię na okres przedświąteczny osobę z doświadczeniem w handlu do sklepu rybnego na ul. Dubois.",
    tags: ["d", "u"],
    date: "dodane: 2025-11-29 14:59:09",
    premium: false,
  },
  {
    id: 3,
    title: "Pracownicy do sortowni i na magazyn",
    description:
      "Zatrudnimy pracowników na magazyn w Choroszczy. Potrzebujemy osób: do sortowania przesyłek kurierskich - praca na pół etatu dwuzmianowa...",
    tags: ["d", "u"],
    date: "dodane: 2025-11-29 12:24:58",
    premium: false,
  },
  {
    id: 4,
    title: "zatrudnimy księgową",
    description:
      "OBOWIĄZKI: dekretowanie i księgowanie dokumentów, księgowanie wyciągów, kontrola i analiza rozrachunków...",
    tags: ["PREMIUM", "min. 5000zł netto"],
    date: "dodane: 2025-11-28 15:44:02",
    premium: true,
  },
]

export function PostList() {
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
      {posts.map((post) => (
        <Post key={post.id}>
          <Post.Title>{post.title}</Post.Title>

          <Post.Description>{post.description}</Post.Description>

          <Post.Separator />

          <Post.Footer
            left={<Post.Tags tags={post.tags} />}
            right={<Post.DateText>{post.date}</Post.DateText>}
          />
        </Post>
      ))}
    </Box>
  )
}
