import React from "react";
import { Link as RouterLink } from "react-router-dom";
import {
  Box,
  Button,
  TextField,
  Typography,
  Link,
  Paper,
  Stack,
} from "@mui/material";

const Login = () => {
  const handleSubmit = (e: React.BaseSyntheticEvent): void => {
    e.preventDefault();
    const data = new FormData(e.currentTarget);
    console.log(Object.fromEntries(data.entries()));
  };

  return (
    <Box sx={{ display: "flex", justifyContent: "center", mt: 8, px: 2 }}>
      <Box sx={{ width: 400 }}>
        <Paper
          elevation={3}
          sx={{ p: 4, borderRadius: 2, width: "100%", boxSizing: "border-box" }}
        >
          <Typography variant="h5" sx={{ fontWeight: 700, mb: 1 }}>
            Welcome back
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
            Enter your credentials to access your account.
          </Typography>

          <Box component="form" onSubmit={handleSubmit} noValidate>
            <Stack spacing={2.5}>
              <Box>
                <Typography
                  variant="subtitle2"
                  sx={{ mb: 0.5, fontWeight: 700 }}
                >
                  Email Address
                </Typography>
                <TextField
                  fullWidth
                  name="email"
                  type="email"
                  placeholder="name@company.com"
                  required
                  size="small"
                />
              </Box>

              <Box>
                <Box
                  sx={{
                    display: "flex",
                    justifyContent: "space-between",
                    mb: 0.5,
                  }}
                >
                  <Typography variant="subtitle2" sx={{ fontWeight: 700 }}>
                    Password
                  </Typography>
                  <Link
                    component={RouterLink}
                    to="/reset-password"
                    underline="none"
                    sx={{ fontSize: "0.875rem", fontWeight: 600 }}
                  >
                    Forgot?
                  </Link>
                </Box>
                <TextField
                  fullWidth
                  name="password"
                  type="password"
                  placeholder="••••••••"
                  required
                  size="small"
                />
              </Box>

              <Button
                fullWidth
                type="submit"
                variant="contained"
                size="large"
                sx={{ textTransform: "none", fontWeight: 700, py: 1.5 }}
              >
                Sign In
              </Button>
            </Stack>
          </Box>
        </Paper>
      </Box>
    </Box>
  );
};

export default Login;
