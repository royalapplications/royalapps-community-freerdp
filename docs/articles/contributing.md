# Maintaining the Documentation

## Local development

From the repository root, use Node.js 22.18 or newer (CI uses Node.js 24) and the .NET 10 SDK. The API generator builds the Windows-targeted library with cross-targeting enabled; it does not launch a native RDP client.

```sh
npm ci
npm run docs:dev
```

To build and preview the production site:

```sh
npm run docs:build
npm run docs:preview
```

Open the URL printed by VitePress, including the `/royalapps-community-freerdp/` base path.

Both `docs:dev` and `docs:build` regenerate the API reference first. The production build also runs API coverage tests, validates internal links, and creates `docs/.vitepress/dist`. The lockfile pins the dependency tree. The VitePress version, branding, and API generator follow the companion Community RDP documentation site.

To regenerate or validate just the API pages:

```sh
npm run docs:api
npm run docs:test
```

Rerun `docs:api` after changing C# code or XML comments while the development server is running.

## Content layout

- `docs/index.md`: homepage.
- `docs/articles/`: guides and maintenance instructions.
- `docs/api/reference/`: generated pages for public types and their documented members.
- `docs/api/index.md` and `docs/api/sidebar.mjs`: generated type index and namespace navigation.
- `docs/api/control.md`, `configuration.md`, and `diagnostics.md`: hand-maintained usage overviews with links to the generated reference.
- `scripts/generate-api-docs.mjs`: generator adapted from Community RDP, combining public C# declarations with compiled XML documentation.
- `scripts/generate-api-docs.test.mjs`: generated-output coverage, cross-link, signature, and encoding checks.
- `scripts/NuGet.Config`: public-only restore sources for reproducible docs builds independent of developer-specific feeds.
- `docs/.vitepress/`: navigation, theme, and build configuration.
- `docs/public/assets/`: assets copied into the static site.
- `docs/assets/`: retained repository assets used by existing external links.

Update C# XML documentation alongside public API changes and regenerate the reference; do not edit generated pages directly. Commit the generated pages and sidebar with the source changes. Update handwritten guides and overviews when usage changes. The build compiles the library but does not compile embedded C# examples.

The reference includes property types, event handler types, method parameters and return values, enum values, summaries, remarks, type cross-links, and source links. Inherited documentation is resolved when the referenced documentation is available in the library. Framework members inherited from WinForms are not expanded into a separate reference, and missing XML descriptions remain explicitly marked rather than invented.

Keep text files UTF-8 with BOM and CRLF, following the repository convention.

## Validation and deployment

The docs workflow builds on pull requests and pushes to `main` when documentation, source, generator, or dependency files change. It installs .NET 10 as well as Node.js, then regenerates and tests the reference before building the site. Deployment is **manual**: after reviewing and merging changes, run **FreeRDP Docs** from GitHub Actions.

Before the first deployment, select **GitHub Actions** as the repository's **Settings → Pages → Build and deployment → Source**. The workflow uploads a Pages artifact and deploys it without creating or force-pushing a `gh-pages` branch.

The configured deployment address is:

[RoyalApps FreeRDP documentation](https://royalapplications.github.io/royalapps-community-freerdp/)

Creating the workflow does not publish the site. The README links to the deployed documentation pages; their Markdown sources remain in `docs/` for editing. After changing site content, run the deployment workflow to update the published pages.

For another host or a custom domain, adjust `base` in `docs/.vitepress/config.mts` and the deployment instructions. See the [official VitePress deployment guide](https://vitepress.dev/guide/deploy).
