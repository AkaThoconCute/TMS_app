import React from "react";
import {
  Box,
  Button,
  TextField,
  Typography,
  Paper,
  Stack,
} from "@mui/material";

const ResetPassword = () => {
  const handleSubmit = (e: React.BaseSyntheticEvent): void => {
    e.preventDefault();
    const data = new FormData(e.currentTarget);
    console.log("Email to reset:", data.get("email"));
  };

  return (
    <Box sx={{ display: "flex", justifyContent: "center", mt: 8, px: 2 }}>
      <Box sx={{ width: 400 }}>
        <Paper
          elevation={3}
          sx={{ p: 4, borderRadius: 2, width: "100%", boxSizing: "border-box" }}
        >
          <Typography variant="h5" sx={{ fontWeight: 700, mb: 1 }}>
            Reset Password
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
            We'll send a recovery link to your email.
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
                  placeholder="Enter your email"
                  required
                  size="small"
                  autoFocus
                />
              </Box>

              <Button
                fullWidth
                type="submit"
                variant="contained"
                size="large"
                sx={{ textTransform: "none", fontWeight: 700, py: 1.5 }}
              >
                Send Reset Link
              </Button>
            </Stack>
          </Box>
        </Paper>
      </Box>
    </Box>
  );
};

export default ResetPassword;
