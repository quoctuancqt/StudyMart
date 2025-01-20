import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { QueryClientProvider } from '@tanstack/react-query';
import { AuthProvider } from 'react-oidc-context';
import App from './App.tsx';
import { onSigninCallback, queryClient, userManager } from './config.ts';
import './index.css'
import { ThemeProvider } from './components/theme-provider.tsx';
import { BrowserRouter } from "react-router";

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <AuthProvider userManager={userManager} onSigninCallback={onSigninCallback}>
      <QueryClientProvider client={queryClient}>
        <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
          <BrowserRouter>
            <App />
          </BrowserRouter>
        </ThemeProvider>
      </QueryClientProvider>
    </AuthProvider>
  </StrictMode>,
)
