// src/app/store.js
import { configureStore, combineReducers } from "@reduxjs/toolkit";
import userReducer from "../features/auth/hooks/userSlice";
import { persistStore, persistReducer, FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER } from "redux-persist";
import storage from "redux-persist/lib/storage"; // usa localStorage

// Combina tus reducers
const rootReducer = combineReducers({
  userData: userReducer,
  // podés agregar más reducers si los tenés
});

// Configuración del persist
const persistConfig = {
  key: "root",
  storage,
  whitelist: ["userData"], // los slices que querés persistir
};

const persistedReducer = persistReducer(persistConfig, rootReducer);

// Crear store con middleware compatible con redux-persist
export const store = configureStore({
  reducer: persistedReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: {
        // Ignora acciones de persist
        ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
      },
    }),
});

export const persistor = persistStore(store);
