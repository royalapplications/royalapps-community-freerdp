using System;

namespace RoyalApps.Community.FreeRdp.WinForms.Extensions;

/// <summary>
/// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnnetsec/html/dpapiusercredentials.asp
/// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/secauthn/security/creduipromptforcredentials.asp
/// </summary>
[Flags]
internal enum CREDUI_FLAGS
{
    /// <summary>
    /// The caller is requesting that the credential provider return the user name and password in plain text.
    /// This value cannot be combined with SECURE_PROMPT.
    /// </summary>
    CREDUIWIN_GENERIC = 0x1,
    /// <summary>
    /// The Save check box is displayed in the dialog box.
    /// </summary>
    CREDUIWIN_CHECKBOX = 0x2,
    /// <summary>
    /// REQUEST_ADMINISTRATOR
    /// </summary>
    REQUEST_ADMINISTRATOR = 0x4,
    /// <summary>
    /// EXCLUDE_CERTIFICATES
    /// </summary>
    EXCLUDE_CERTIFICATES = 0x8,

    /// <summary>
    /// Only credential providers that support the authentication package specified by the authPackage parameter should be enumerated.
    /// This value cannot be combined with CREDUIWIN_IN_CRED_ONLY.
    /// </summary>
    CREDUIWIN_AUTHPACKAGE_ONLY = 0x10,
    /// <summary>
    /// Only the credentials specified by the InAuthBuffer parameter for the authentication package specified by the authPackage parameter should be enumerated.
    /// If this flag is set, and the InAuthBuffer parameter is NULL, the function fails.
    /// This value cannot be combined with CREDUIWIN_AUTHPACKAGE_ONLY.
    /// </summary>
    CREDUIWIN_IN_CRED_ONLY = 0x20,
    /// <summary>
    /// SHOW_SAVE_CHECK_BOX
    /// </summary>
    SHOW_SAVE_CHECK_BOX = 0x40,
    /// <summary>
    /// ALWAYS_SHOW_UI
    /// </summary>
    ALWAYS_SHOW_UI = 0x80,

    /// <summary>
    /// Credential providers should enumerate only administrators. This value is intended for User Account Control (UAC) purposes only. We recommend that external callers not set this flag.
    /// </summary>
    CREDUIWIN_ENUMERATE_ADMINS = 0x100,
    /// <summary>
    /// Only the incoming credentials for the authentication package specified by the authPackage parameter should be enumerated.
    /// </summary>
    CREDUIWIN_ENUMERATE_CURRENT_USER = 0x200,
    /// <summary>
    /// VALIDATE_USERNAME
    /// </summary>
    VALIDATE_USERNAME = 0x400,
    /// <summary>
    /// COMPLETE_USERNAME
    /// </summary>
    COMPLETE_USERNAME = 0x800,

    /// <summary>
    /// The credential dialog box should be displayed on the secure desktop. This value cannot be combined with CREDUIWIN_GENERIC.
    /// Windows Vista: This value is not supported until Windows Vista with SP1.
    /// </summary>
    CREDUIWIN_SECURE_PROMPT = 0x1000,

    /// <summary>
    /// SERVER_CREDENTIAL
    /// </summary>
    SERVER_CREDENTIAL = 0x4000,
    /// <summary>
    /// EXPECT_CONFIRMATION
    /// </summary>
    EXPECT_CONFIRMATION = 0x20000,
    /// <summary>
    /// GENERIC_CREDENTIALS
    /// </summary>
    GENERIC_CREDENTIALS = 0x40000,
    /// <summary>
    /// USERNAME_TARGET_CREDENTIALS
    /// </summary>
    USERNAME_TARGET_CREDENTIALS = 0x80000,

    /// <summary>
    /// The credential provider should align the credential BLOB pointed to by the refOutAuthBuffer parameter to a 32-bit boundary, even if the provider is running on a 64-bit system.
    /// </summary>
    CREDUIWIN_PACK_32_WOW = 0x10000000,
}
