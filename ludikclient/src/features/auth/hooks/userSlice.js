// src/features/auth/hooks/userSlice.js
import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  id: null,
  user: null,
  token: null,
  role: null,
  isAuthenticated: false,
  loggedOutManually: false,
  rememberMe: false,
};

const userSlice = createSlice({
  name: "userData",
  initialState,
  reducers: {
    loginSuccess: (state, action) => {
      state.id = action.payload.id;
      state.user = action.payload.nombreUsuario;
      state.token = action.payload.token;
      state.role = action.payload.rol;
      state.isAuthenticated = true;
      state.loggedOutManually = false;
      state.rememberMe = action.payload.rememberMe || false;
    },
    logout: (state) => {
      state.id = null;
      state.user = null;
      state.token = null;
      state.role = null;
      state.isAuthenticated = false;
      state.loggedOutManually = true;
      state.rememberMe = false;
    },
    logoutReset: (state) => {
      state.loggedOutManually = false;
    },
  },
});

export const { loginSuccess, logout, logoutReset } = userSlice.actions;

// Selectors
export const selectUserId = (state) => state.userData.id;
export const selectUser = (state) => state.userData.user;
export const selectUserRole = (state) => state.userData.role;
export const selectIsAuthenticated = (state) => state.userData.isAuthenticated;
export const selectLoggedOutManually = (state) => state.userData.loggedOutManually;

export default userSlice.reducer;
