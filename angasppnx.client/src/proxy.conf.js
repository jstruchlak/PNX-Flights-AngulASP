const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7197'; // Ensure this points to the active backend API

const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast", // This should match the API endpoint you're trying to call
    ],
    target,
    secure: false // Set to false if you're not using SSL or if using self-signed certificates
  }
];

module.exports = PROXY_CONFIG;
