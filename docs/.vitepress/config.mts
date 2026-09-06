import { defineConfig } from "vitepress";

const repository = "https://github.com/royalapplications/royalapps-community-freerdp";
const sidebar = [
  {
    text: "Guide",
    items: [
      { text: "Overview", link: "/" },
      { text: "Getting Started", link: "/articles/getting-started" },
      { text: "Configuration", link: "/articles/configuration" },
      { text: "Lifecycle and Events", link: "/articles/lifecycle" },
      { text: "Diagnostics", link: "/articles/diagnostics" },
      { text: "Troubleshooting", link: "/articles/troubleshooting" },
      { text: "Support Matrix", link: "/articles/support-matrix" }
    ]
  },
  {
    text: "API",
    items: [
      { text: "Overview", link: "/api/" },
      { text: "FreeRdpControl", link: "/api/control" },
      { text: "Configuration Types", link: "/api/configuration" },
      { text: "Diagnostics Types", link: "/api/diagnostics" }
    ]
  },
  {
    text: "Contributing",
    items: [{ text: "Documentation", link: "/articles/contributing" }]
  }
];

export default defineConfig({
  title: "RoyalApps FreeRDP",
  description: "Embed wfreerdp.exe in WinForms with typed configuration, lifecycle events, and opt-in diagnostics.",
  base: "/royalapps-community-freerdp/",
  cleanUrls: true,
  // Avoid PostCSS's JSON config discovery, which cannot read a BOM-prefixed package.json.
  // This site needs no extra PostCSS plugins; keep the repository's UTF-8 BOM convention.
  vite: { css: { postcss: { plugins: [] } } },
  themeConfig: {
    logo: "/assets/RoyalApps_1024.png",
    nav: [
      { text: "Guide", link: "/articles/getting-started" },
      { text: "API", link: "/api/" },
      { text: "GitHub", link: repository }
    ],
    sidebar: { "/articles/": sidebar, "/api/": sidebar },
    outline: [2, 3],
    socialLinks: [{ icon: "github", link: repository }],
    search: { provider: "local" },
    editLink: { pattern: repository + "/edit/main/docs/:path", text: "Edit this page on GitHub" },
    footer: {
      message: "MIT Licensed",
      copyright: "Copyright Royal Apps GmbH"
    }
  }
});

