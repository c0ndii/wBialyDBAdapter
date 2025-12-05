import { DatabaseTypes, type DatabaseEnum } from "@/api/types"
import { useDatabase } from "@/hooks/useDatabase"
import CheckIcon from "@mui/icons-material/Check"
import SettingsIcon from "@mui/icons-material/Settings"
import { IconButton } from "@mui/material"
import Menu from "@mui/material/Menu"
import MenuItem from "@mui/material/MenuItem"
import * as React from "react"

export function DatabasePicker() {
  const { setDatabaseType, databaseType } = useDatabase()
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null)
  const open = Boolean(anchorEl)
  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget)
  }
  const handleClose = () => {
    setAnchorEl(null)
  }

  const handleSelect = (value: DatabaseEnum) => {
    setDatabaseType(value)
    handleClose()
  }

  return (
    <div>
      <IconButton onClick={handleClick}>
        <SettingsIcon />
      </IconButton>
      <Menu
        id="demo-positioned-menu"
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "left",
        }}
        transformOrigin={{
          vertical: "top",
          horizontal: "left",
        }}
      >
        {Object.keys(DatabaseTypes).map((key, value) => {
          return (
            <MenuItem
              key={key}
              onClick={() => {
                handleSelect(value as DatabaseEnum)
              }}
              sx={{
                gap: 1,
              }}
            >
              {value === databaseType ? <CheckIcon /> : <></>}
              {key}
            </MenuItem>
          )
        })}
      </Menu>
    </div>
  )
}
