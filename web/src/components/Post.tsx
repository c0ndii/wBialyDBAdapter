import {
  Card,
  CardContent,
  Chip,
  Divider,
  Stack,
  Typography,
} from "@mui/material"
import { Link } from "@tanstack/react-router"
import type { PropsWithChildren, ReactNode } from "react"

function Root({ children }: PropsWithChildren<unknown>) {
  return (
    <Card
      variant="outlined"
      sx={{
        borderRadius: 0,
        borderLeft: 1,
        borderLeftColor: "divider",
        boxShadow: 0,
      }}
    >
      <CardContent sx={{ p: 2.5, "&:last-child": { pb: 2.5 } }}>
        <Stack spacing={1}>{children}</Stack>
      </CardContent>
    </Card>
  )
}

function Title({ children, link }: PropsWithChildren<{ link: string }>) {
  return (
    <Typography
      component={Link}
      to={link}
      variant="h6"
      sx={(theme) => ({
        fontWeight: 700,
        textDecoration: "none",
        color: theme.palette.getContrastText(theme.palette.background.default),
        "&:hover": {
          textDecoration: "underline",
        },
      })}
    >
      {children}
    </Typography>
  )
}

function Description({ children }: PropsWithChildren<unknown>) {
  return (
    <Typography variant="body2" color="text.secondary">
      {children}
    </Typography>
  )
}

function Separator() {
  return <Divider sx={{ my: 1.5 }} />
}

type FooterProps = {
  left?: ReactNode
  right?: ReactNode
}

function Footer({ left, right }: PropsWithChildren<FooterProps>) {
  return (
    <Stack direction="row" alignItems="center" justifyContent="space-between">
      <Stack direction="row" spacing={1}>
        {left}
      </Stack>
      {right}
    </Stack>
  )
}

type TagsProps = {
  tags: string[]
}

function Tags({ tags }: TagsProps) {
  return (
    <>
      {tags.map((tag) => (
        <Chip
          key={tag}
          label={tag}
          size="small"
          variant="outlined"
          color="default"
          sx={{ textTransform: "lowercase" }}
        />
      ))}
    </>
  )
}

function DateText({ children }: PropsWithChildren<unknown>) {
  return (
    <Typography variant="caption" color="text.secondary">
      {children}
    </Typography>
  )
}

export const Post = Object.assign(Root, {
  Title,
  Description,
  Separator,
  Footer,
  Tags,
  DateText,
})
