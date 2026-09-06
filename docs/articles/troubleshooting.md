# Troubleshooting

## No connection window

Check that the control is attached to a live WinForms container with a nonzero client size before calling `Connect()`. Capture exceptions from configuration validation and process startup.

Verify that the bundled client can be extracted to `TempPath`, or that the custom `Executable` exists and has the required supporting files.

## Certificate failure

Use the server's certificate-matching DNS name and a trusted certificate chain. Inspect the certificate outside the wrapper where necessary. Do not automatically accept every `CertificateError`; accepting the retry bypasses certificate checks.

## Authentication or gateway failure

Check destination and gateway credentials separately. The wrapper consumes the supplied values; it does not resolve credential references or inherited gateway objects from a host application.

Record sanitized native error messages and the `DisconnectEventArgs.ExitCode`. A wrapper-recognized credential error can raise `VerifyCredentials`; other native failures may terminate directly.

## No diagnostic output

- Check that the installed package contains the 2.2.4 APIs.
- Set the provider and subscribe before connecting.
- Reconnect after enabling capture or changing the level.
- Ensure the host sink is enabled, can open its file, and is not dropping all records.
- Verify that a custom executable supports the expected FreeRDP logging options.

The ordinary `Logger` and `DiagnosticOutput` are separate. Turning up the host's logger does not by itself enable native capture.

## Reconnect or shutdown investigation

Capture one timestamped reproduction, including:

- wrapper package and native executable versions;
- whether the bundled or a custom executable is used;
- host connection-attempt ID, native launch IDs, and PIDs;
- lifecycle records, disconnect/exit codes, and the exact sequence of user actions;
- whether an internal certificate/credential retry, zoom, or resize created another launch.

Do not assume all output received after reconnect belongs to the new client. Old streams may still be draining.

A forcibly terminated client cannot flush its own internal buffers. The wrapper drains available pipe output asynchronously but cannot guarantee a complete trace after a crash or kill.

## Reporting a problem

Use the [issue tracker](https://github.com/royalapplications/royalapps-community-freerdp/issues). Include a minimal reproduction and sanitized evidence. Review logs before sharing; omit passwords, tokens, clipboard contents, full configuration objects, and command lines.

