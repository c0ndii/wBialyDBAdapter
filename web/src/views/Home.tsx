import { useEvents } from "@/api/hooks/events"
import { DatabaseTypes } from "@/api/types"
import { PostList } from "@/components/PostList"
import { Box } from "@mui/material"

export const Home = () => {
  const { data } = useEvents({
    pageIndex: 0,
    pageSize: 5,
    databaseType: DatabaseTypes.NoSQL,
  })

  console.log(data)
  return (
    <Box>
      Hello home
      <PostList />
    </Box>
  )
}
