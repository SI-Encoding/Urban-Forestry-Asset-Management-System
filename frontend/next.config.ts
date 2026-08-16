/** @type {import('next').NextConfig} */
const nextConfig = {
  output: 'export',
  images: {
    unoptimized: true, // Required for static export on simple hosts
  },
  // Optional: Ensures clean URLs work better on Apache/Nginx
  trailingSlash: true, 
}

module.exports = nextConfig   