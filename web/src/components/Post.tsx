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
      sx={{
        borderRadius: 6,
        border: "1px solid",
        borderColor: "rgba(148, 163, 184, 0.25)",
        boxShadow: "0 15px 40px rgba(15, 23, 42, 0.08)",
        background: "linear-gradient(135deg, #fff, #f8fafc)",
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
        letterSpacing: 0.1,
        textDecoration: "none",
        color: theme.palette.text.primary,
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
    <Typography variant="body2" color="text.secondary" sx={{ lineHeight: 1.6 }}>
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
          variant="filled"
          color="default"
          sx={{
            textTransform: "lowercase",
            backgroundColor: "#e2e8f0",
            color: "#0f172a",
          }}
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
