---
layout: home

hero:
  name: "RoyalApps FreeRDP"
  text: "FreeRDP sessions inside WinForms"
  tagline: "Host the Windows FreeRDP client with typed settings, lifecycle events, and opt-in diagnostics."
  image:
    src: /assets/RoyalApps_1024.png
    alt: Royal Apps
  actions:
    - theme: brand
      text: Getting Started
      link: /articles/getting-started
    - theme: alt
      text: Diagnostics
      link: /articles/diagnostics
    - theme: alt
      text: API Reference
      link: /api/

features:
  - title: Embedded Sessions
    details: Render wfreerdp.exe inside a WinForms control using its parent-window handle.
  - title: Typed Configuration
    details: Configure destination and gateway credentials, display scaling, certificates, and redirection.
  - title: Opt-in Diagnostics
    details: Capture stdout, stderr, and launch metadata with per-launch settings and correlation IDs.
---

## About the library

`RoyalApps.Community.FreeRdp.WinForms` wraps the Windows FreeRDP executable; it is not an ActiveX wrapper or an in-process FreeRdpKit integration.

The bundled client is extracted before launch. You can also select a compatible `wfreerdp.exe` yourself. Each launched client runs in its own process.

These docs describe the current source targeting **.NET 10 on Windows**. The diagnostics APIs are introduced in **2.2.4**; confirm availability in your installed package.

![FreeRDP demo application](/assets/Screenshot.png)

Start with [Getting Started](/articles/getting-started), then review [Diagnostics](/articles/diagnostics) and the [Support Matrix](/articles/support-matrix).

