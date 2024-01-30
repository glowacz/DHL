import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import fs from 'fs'
const host = 'demo.local'

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    port: 3001,
    https: {
      // key: fs.readFileSync(`/etc/ssl/certs/${host}.key`),
      // cert: fs.readFileSync(`/etc/ssl/certs/${host}.crt`),
      key: './private.key',
      cert: './certificate.crt',
    }
  },
  plugins: [react()],
})
