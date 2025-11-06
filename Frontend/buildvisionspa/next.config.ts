import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* Aspire service discovery rewrite: proxy /api to Gateway */
  async rewrites() {
    const target =
      process.env.services__gateway__https__0 ||
      process.env.services__gateway__http__0;
    if (!target) return [];
    return [
      {
        source: "/api/:path*",
        destination: `${target}/:path*`,
      },
    ];
  },
};

export default nextConfig;
