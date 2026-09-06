import assert from "node:assert/strict";
import { readFileSync, readdirSync, existsSync } from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";
import test from "node:test";
import sidebar from "../docs/api/sidebar.mjs";

const root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), "..");
const sourceRoot = path.join(root, "src/RoyalApps.Community.FreeRdp.WinForms");
const apiRoot = path.join(root, "docs/api");
const prefix = "royalapps-community-freerdp-winforms-";
const page = name => readFileSync(path.join(apiRoot, "reference", prefix + name + ".md"), "utf8");
const source = name => readFileSync(path.join(sourceRoot, name), "utf8");

function sourceFiles(directory) {
  return readdirSync(directory, { withFileTypes: true }).flatMap(entry =>
    entry.name === "bin" || entry.name === "obj" ? [] :
    entry.isDirectory() ? sourceFiles(path.join(directory, entry.name)) :
    entry.name.endsWith(".cs") ? [path.join(directory, entry.name)] : []);
}

test("every public type has a generated page and a sidebar entry", () => {
  const expectedTypes = sourceFiles(sourceRoot).flatMap(file => {
    const text = readFileSync(file, "utf8");
    const namespace = text.match(/namespace\s+([\w.]+)\s*;/)?.[1];
    return [...text.matchAll(/^public\s+(?:(?:sealed|static|abstract|partial)\s+)*(?:class|struct|interface|enum)\s+(\w+)/gm)]
      .map(match => (namespace + "." + match[1]).replaceAll(".", "-").toLowerCase());
  });
  const links = sidebar.flatMap(group => group.items.map(item => item.link));
  assert.equal(links.length, expectedTypes.length);
  assert.ok(expectedTypes.length >= 17);
  for (const slug of expectedTypes) {
    assert.ok(existsSync(path.join(apiRoot, "reference", slug + ".md")), slug);
    assert.ok(links.includes("/api/reference/" + slug), slug);
  }
});

test("all public control and configuration properties are documented, including new() initializers", () => {
  for (const [file, slug] of [
    ["FreeRdpControl.cs", "freerdpcontrol"],
    ["Configuration/FreeRdpConfiguration.cs", "configuration-freerdpconfiguration"],
    ["Configuration/GatewayConfiguration.cs", "configuration-gatewayconfiguration"],
    ["Configuration/CertificateConfiguration.cs", "configuration-certificateconfiguration"],
    ["Configuration/CacheConfiguration.cs", "configuration-cacheconfiguration"],
    ["Configuration/ProxyConfiguration.cs", "configuration-proxyconfiguration"],
    ["Configuration/SecurityConfiguration.cs", "configuration-securityconfiguration"]
  ]) {
    const properties = [...source(file).matchAll(/^\s*public\s+[^\r\n]+?\s+(\w+)\s*\{\s*get;/gm)];
    assert.ok(properties.length > 0, file);
    for (const [, property] of properties)
      assert.ok(page(slug).includes("### \u0060" + property + "\u0060"), file + ": " + property);
  }
});

test("Allman methods, constructors, parameter types, and return types are present", () => {
  const control = page("freerdpcontrol");
  for (const name of ["Constructor()", "Connect()", "Disconnect()", "ResetZoom()", "SetZoomLevel(Int32)", "ZoomIn()", "ZoomOut()"])
    assert.ok(control.includes("### \u0060" + name + "\u0060"), name);
  assert.ok(control.includes("Returns: \u0060void\u0060"));
  assert.ok(control.includes("\u0060scalingInPercent\u0060 (\u0060int\u0060): Scaling factor in percent"));
  const credentials = page("verifycredentialseventargs");
  assert.ok(credentials.includes("SetCredentials(String, String, String)"));
  for (const name of ["username", "domain", "password"])
    assert.ok(credentials.includes("\u0060" + name + "\u0060 (\u0060string\u0060?)"));
});

test("diagnostics events, cross-links, and multiple public types in one source file are retained", () => {
  const control = page("freerdpcontrol");
  assert.ok(control.includes("### \u0060DiagnosticOutput\u0060"));
  assert.ok(control.includes("Handler Type:"));
  assert.ok(control.includes("/api/reference/" + prefix + "freerdpdiagnosticeventargs"));
  assert.ok(page("freerdpdiagnosticeventargs").includes("## Used By"));
  for (const name of ["LaunchId", "ProcessId", "Source", "Message"])
    assert.ok(page("freerdpdiagnosticeventargs").includes("### \u0060" + name + "\u0060"));
  assert.ok(page("freerdpdiagnosticsoptions").includes("### \u0060LogLevel\u0060"));
  assert.ok(!existsSync(path.join(apiRoot, "reference", prefix + "freerdpprocessdiagnostics.md")));
});

test("enum numeric values, remarks, and external documentation links are rendered", () => {
  const audio = page("configuration-audioredirectionmode");
  for (const value of ["NotSpecified = -1", "Local = 0", "Server = 1", "None = 2"])
    assert.ok(audio.includes(value), value);
  const configuration = page("configuration-freerdpconfiguration");
  assert.ok(configuration.includes("Command line argument: /audio-mode:"));
  assert.ok(configuration.includes('class="api-member-external-link"'));
  assert.ok(configuration.includes("imsrdpclientadvancedsettings5-audioredirectionmode"));
});

test("all generated API links and source references resolve locally", () => {
  for (const name of readdirSync(path.join(apiRoot, "reference"))) {
    const text = readFileSync(path.join(apiRoot, "reference", name), "utf8");
    for (const [, slug] of text.matchAll(/\]\(\/api\/reference\/([^)#]+)(?:#[^)]*)?\)/g))
      assert.ok(existsSync(path.join(apiRoot, "reference", slug + ".md")), name + " -> " + slug);
    const sourceLink = text.match(/\/blob\/main\/([^#)]+)#L(\d+)/);
    assert.ok(sourceLink, name + ": missing source link");
    const sourceText = readFileSync(path.join(root, sourceLink[1]), "utf8");
    assert.match(sourceText.split(/\r?\n/)[Number(sourceLink[2]) - 1], /^public /);
  }
});

test("generated files retain BOM and CRLF and are marked as generated", () => {
  const files = ["index.md", "sidebar.mjs",
    ...readdirSync(path.join(apiRoot, "reference")).map(name => "reference/" + name)];
  for (const file of files) {
    const bytes = readFileSync(path.join(apiRoot, file));
    assert.deepEqual([...bytes.subarray(0, 3)], [239, 187, 191], file);
    const text = bytes.toString("utf8");
    assert.ok(!/(?<!\r)\n|\r(?!\n)/.test(text), file);
    assert.ok(text.includes("Generated by scripts/generate-api-docs.mjs"), file);
  }
});
