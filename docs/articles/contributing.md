# Maintaining the Documentation

## Local development

From the repository root, use Node.js 22.18 or newer (CI uses Node.js 24):

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

The production build validates internal links and creates `docs/.vitepress/dist`. The lockfile pins the dependency tree. The VitePress version and branding follow the companion Community RDP documentation site.

## Content layout

- `docs/index.md`: homepage.
- `docs/articles/`: guides and maintenance instructions.
- `docs/api/`: focused, hand-maintained host API reference.
- `docs/.vitepress/`: navigation, theme, and build configuration.
- `docs/public/assets/`: assets copied into the static site.
- `docs/assets/`: retained repository assets used by existing external links.

Update guides and API pages alongside public API changes. Verify examples against the source. The documentation build does not generate API pages or compile embedded C# examples.

Keep text files UTF-8 with BOM and CRLF, following the repository convention.

## Validation and deployment

The docs workflow builds on pull requests and pushes to `main`. Deployment is **manual**: after reviewing and merging changes, run **FreeRDP Docs** from GitHub Actions.

Before the first deployment, select **GitHub Actions** as the repository's **Settings → Pages → Build and deployment → Source**. The workflow uploads a Pages artifact and deploys it without creating or force-pushing a `gh-pages` branch.

The configured deployment address is:

`https://royalapplications.github.io/royalapps-community-freerdp/`

Creating the workflow does not publish the site. The README links to the in-repository guides so they remain usable before deployment.

For another host or a custom domain, adjust `base` in `docs/.vitepress/config.mts` and the deployment instructions. See the [official VitePress deployment guide](https://vitepress.dev/guide/deploy).

