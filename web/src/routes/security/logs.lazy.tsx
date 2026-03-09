import { useSecurityLogs } from "@/api/hooks/user";
import { LogsScope, type LogsScopeEnum } from "@/api/types";
import {
  Box,
  Button,
  ButtonGroup,
  Card,
  CardContent,
  CardHeader,
  Chip,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from "@mui/material";
import { createLazyFileRoute } from "@tanstack/react-router";
import { useState } from "react";

export const Route = createLazyFileRoute("/security/logs")({
  component: SecurityLogsView,
});

function SecurityLogsView() {
  const [scope, setScope] = useState<LogsScopeEnum>(LogsScope.Mine);

  const { data: logs = [], isLoading, isError } = useSecurityLogs(100, scope);

  return (
    <Card>
      <CardHeader
        title="Security logs"
        subheader="Recent login attempts (mine or all)"
      />
      <CardContent>
        <Box
          sx={{
            display: "flex",
            gap: 2,
            alignItems: "center",
            mb: 2,
            flexWrap: "wrap",
          }}
        >
          <ButtonGroup size="small" variant="outlined">
            <Button
              variant={scope === LogsScope.Mine ? "contained" : "outlined"}
              onClick={() => setScope(LogsScope.Mine)}
            >
              Mine
            </Button>
            <Button
              variant={scope === LogsScope.All ? "contained" : "outlined"}
              onClick={() => setScope(LogsScope.All)}
            >
              All
            </Button>
          </ButtonGroup>
        </Box>

        {isLoading && (
          <Typography variant="body2" color="text.secondary">
            Loading logs...
          </Typography>
        )}

        {isError && (
          <Typography variant="body2" color="error">
            Failed to load security logs.
          </Typography>
        )}

        {!isLoading && !isError && logs.length === 0 && (
          <Typography variant="body2" color="text.secondary">
            No login logs available.
          </Typography>
        )}

        {!isLoading && !isError && logs.length > 0 && (
          <TableContainer>
            <Table size="small">
              <TableHead>
                <TableRow>
                  <TableCell>Time</TableCell>
                  <TableCell>User ID</TableCell>
                  <TableCell>Login</TableCell>
                  <TableCell>Result</TableCell>
                  <TableCell>Reason</TableCell>
                  <TableCell>Delay</TableCell>
                  <TableCell>IP</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {logs.map((log) => (
                  <TableRow key={log.id}>
                    <TableCell>
                      {new Date(log.attemptedAtUtc).toLocaleString("pl-PL")}
                    </TableCell>
                    <TableCell>{log.userId ?? "-"}</TableCell>
                    <TableCell>{log.loginIdentifier}</TableCell>
                    <TableCell>
                      <Chip
                        size="small"
                        color={log.isSuccessful ? "success" : "error"}
                        label={log.isSuccessful ? "Success" : "Failed"}
                      />
                    </TableCell>
                    <TableCell>{log.failureCategory ?? "-"}</TableCell>
                    <TableCell>{log.appliedDelaySeconds}s</TableCell>
                    <TableCell>{log.ipAddress ?? "-"}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        )}

        <Box sx={{ mt: 2 }}>
          <Typography variant="caption" color="text.secondary">
            Logs include successful and failed attempts for existing and blocked
            access flows.
          </Typography>
        </Box>
      </CardContent>
    </Card>
  );
}
