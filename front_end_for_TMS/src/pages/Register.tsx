import React from "react";
import {
  Box,
  Button,
  TextField,
  Typography,
  Paper,
  Stack,
} from "@mui/material";

const Register = () => {
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
            Create account
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
            Join us today to get started.
          </Typography>

          <Box component="form" onSubmit={handleSubmit} noValidate>
            <Stack spacing={2.5}>
              <Box>
                <Typography
                  variant="subtitle2"
                  sx={{ mb: 0.5, fontWeight: 700 }}
                >
                  Full Name
                </Typography>
                <TextField
                  fullWidth
                  name="fullName"
                  placeholder="John Doe"
                  required
                  size="small"
                />
              </Box>

              <Box>
                <Typography
                  variant="subtitle2"
                  sx={{ mb: 0.5, fontWeight: 700 }}
                >
                  Email
                </Typography>
                <TextField
                  fullWidth
                  name="email"
                  type="email"
                  placeholder="john@example.com"
                  required
                  size="small"
                />
              </Box>

              <Box>
                <Typography
                  variant="subtitle2"
                  sx={{ mb: 0.5, fontWeight: 700 }}
                >
                  Password
                </Typography>
                <TextField
                  fullWidth
                  name="password"
                  type="password"
                  placeholder="Min. 8 characters"
                  required
                  size="small"
                  inputProps={{ minLength: 8 }}
                />
              </Box>

              <Button
                fullWidth
                type="submit"
                variant="contained"
                size="large"
                sx={{ textTransform: "none", fontWeight: 700, py: 1.5 }}
              >
                Get Started
              </Button>
            </Stack>
          </Box>
        </Paper>
      </Box>
    </Box>
  );
};

export default Register;
