import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');
  return {
    plugins: [react()],
    server: {
      port: 5173,
      proxy: {
        '/api': env.API_BASE_URL || 'http://localhost:5000',
        '/chatHub': {
          target: env.SIGNALR_URL || 'http://localhost:5000',
          ws: true,
          changeOrigin: true,
        },
      },
    },
  };
});
