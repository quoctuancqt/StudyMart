import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react-swc';
import path from 'path';

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  // const env = loadEnv(mode, process.cwd());
  return {
    plugins: [
      react()
    ],
    resolve: {
      alias: {
        "@": path.resolve(__dirname, "./src"),
      },
    },
    // server: {
    //   port: parseInt(env.VITE_PORT),
    //   proxy: {
    //     '/api/v1': {
    //       target: process.env.services__apiservice__http__0 ||
    //         process.env.services__apiservice__https__0,
    //       changeOrigin: true,
    //       secure: false,
    //     }
    //   }
    // },
    build: {
      outDir: 'dist',
      rollupOptions: {
        input: './index.html'
      }
    }
  };
})
