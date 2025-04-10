using System.Collections.Generic;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace RoyalApps.Community.FreeRdp.WinForms;

internal class RdpErrorInfo
{
	private const int ERROR_START = 10000;

	private enum ConnectErrors
	{
		FREERDP_ERROR_PRE_CONNECT_FAILED =								0x00020001,
		FREERDP_ERROR_CONNECT_UNDEFINED =								0x00020002,
		FREERDP_ERROR_POST_CONNECT_FAILED =								0x00020003,

		FREERDP_ERROR_DNS_ERROR =										0x00020004,
		FREERDP_ERROR_DNS_NAME_NOT_FOUND =								0x00020005,

		FREERDP_ERROR_CONNECT_FAILED =									0x00020006,
		FREERDP_ERROR_MCS_CONNECT_INITIAL_ERROR =						0x00020007,
		FREERDP_ERROR_TLS_CONNECT_FAILED =								0x00020008,

		FREERDP_ERROR_AUTHENTICATION_FAILED =							0x00020009,
		FREERDP_ERROR_INSUFFICIENT_PRIVILEGES =							0x0002000A,

		FREERDP_ERROR_CONNECT_CANCELLED =								0x0002000B,
		FREERDP_ERROR_SECURITY_NEGO_CONNECT_FAILED =					0x0002000C,
		FREERDP_ERROR_CONNECT_TRANSPORT_FAILED =						0x0002000D,

		FREERDP_ERROR_CONNECT_PASSWORD_EXPIRED =						0x0002000E,
		FREERDP_ERROR_CONNECT_PASSWORD_CERTAINLY_EXPIRED =				0x0002000F,
		FREERDP_ERROR_CONNECT_CLIENT_REVOKED =							0x00020010,
		FREERDP_ERROR_CONNECT_KDC_UNREACHABLE =							0x00020011,
		FREERDP_ERROR_CONNECT_ACCOUNT_DISABLED =						0x00020012,
		FREERDP_ERROR_CONNECT_PASSWORD_MUST_CHANGE =					0x00020013,
		FREERDP_ERROR_CONNECT_LOGON_FAILURE =							0x00020014,
		FREERDP_ERROR_CONNECT_WRONG_PASSWORD =							0x00020015,
		FREERDP_ERROR_CONNECT_ACCESS_DENIED =							0x00020016,
		FREERDP_ERROR_CONNECT_ACCOUNT_RESTRICTION =						0x00020017,
		FREERDP_ERROR_CONNECT_ACCOUNT_LOCKED_OUT =						0x00020018,
		FREERDP_ERROR_CONNECT_ACCOUNT_EXPIRED =							0x00020019,
		FREERDP_ERROR_CONNECT_LOGON_TYPE_NOT_GRANTED =					0x0002001A,
		FREERDP_ERROR_CONNECT_NO_OR_MISSING_CREDENTIALS =				0x0002001B,
	}

	private enum ProtocolIndependentErrors
	{
		FREERDP_ERROR_RPC_INITIATED_DISCONNECT =						0x00010001,
		FREERDP_ERROR_RPC_INITIATED_LOGOFF =							0x00010002,
		FREERDP_ERROR_IDLE_TIMEOUT =									0x00010003,
		FREERDP_ERROR_LOGON_TIMEOUT =									0x00010004,
		FREERDP_ERROR_DISCONNECTED_BY_OTHER_CONNECTION =				0x00010005,

		FREERDP_ERROR_OUT_OF_MEMORY =									0x00010006,
		FREERDP_ERROR_SERVER_DENIED_CONNECTION =						0x00010007,
		FREERDP_ERROR_SERVER_INSUFFICIENT_PRIVILEGES =					0x00010009,
		FREERDP_ERROR_SERVER_FRESH_CREDENTIALS_REQUIRED =				0x0001000A,

		FREERDP_ERROR_RPC_INITIATED_DISCONNECT_BY_USER =				0x0001000B,
		FREERDP_ERROR_LOGOFF_BY_USER =									0x0001000C,
	}

	private static readonly Dictionary<int, RdpErrorInfo> ConnectionErrorInfos = new() {
		{ (int)ConnectErrors.FREERDP_ERROR_PRE_CONNECT_FAILED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_PRE_CONNECT_FAILED.ToString(), "A configuration error prevented the connection from being established.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_UNDEFINED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_UNDEFINED.ToString(), "A undefined connection error occured.") },
		{ (int)ConnectErrors.FREERDP_ERROR_POST_CONNECT_FAILED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_POST_CONNECT_FAILED.ToString(), "The connection attempt was aborted due to post connect configuration errors.") },
		{ (int)ConnectErrors.FREERDP_ERROR_DNS_ERROR, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_DNS_ERROR.ToString(), "An error occurred while resolving the remote computer's name.") },
		{ (int)ConnectErrors.FREERDP_ERROR_DNS_NAME_NOT_FOUND, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_DNS_NAME_NOT_FOUND.ToString(), "The specified computer name could not be found.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_FAILED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_FAILED.ToString(), "The connection failed.") },
		{ (int)ConnectErrors.FREERDP_ERROR_MCS_CONNECT_INITIAL_ERROR, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_MCS_CONNECT_INITIAL_ERROR.ToString(), "The connection failed at initial MCS connect.") },
		{ (int)ConnectErrors.FREERDP_ERROR_TLS_CONNECT_FAILED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_TLS_CONNECT_FAILED.ToString(), "An error occurred during TLS handshake.") },
		{ (int)ConnectErrors.FREERDP_ERROR_AUTHENTICATION_FAILED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_AUTHENTICATION_FAILED.ToString(), "An authentication error has occurred. Please verify your credentials.") },
		{ (int)ConnectErrors.FREERDP_ERROR_INSUFFICIENT_PRIVILEGES, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_INSUFFICIENT_PRIVILEGES.ToString(), "Insufficient privileges to establish a connection.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_CANCELLED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_CANCELLED.ToString(), "The connection was cancelled.") },
		{ (int)ConnectErrors.FREERDP_ERROR_SECURITY_NEGO_CONNECT_FAILED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_SECURITY_NEGO_CONNECT_FAILED.ToString(), "The connection failed at negotiating security settings.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_TRANSPORT_FAILED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_TRANSPORT_FAILED.ToString(), "The connection's transport layer failed.") },

		// NLA Errors (Reference: https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/quick-reference-troubleshooting-netlogon-error-codes/ba-p/256000)
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_PASSWORD_EXPIRED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_PASSWORD_EXPIRED.ToString(), "An authentication error occurred. Your password is expired.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_PASSWORD_CERTAINLY_EXPIRED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_PASSWORD_CERTAINLY_EXPIRED.ToString(), "An authentication error occurred. Your password is expired.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_CLIENT_REVOKED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_CLIENT_REVOKED.ToString(), "An authentication error occurred. The client has been revoked.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_KDC_UNREACHABLE, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_KDC_UNREACHABLE.ToString(), "An authentication error occurred. The KDC is unreachable.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_ACCOUNT_DISABLED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_ACCOUNT_DISABLED.ToString(), "An authentication error occurred. Your user account is disabled.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_PASSWORD_MUST_CHANGE, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_PASSWORD_MUST_CHANGE.ToString(), "An authentication error occurred. Password change required.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_LOGON_FAILURE, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_LOGON_FAILURE.ToString(), "An authentication error occurred. Login failed. Please verify your credentials.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_WRONG_PASSWORD, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_WRONG_PASSWORD.ToString(), "An authentication error occurred. Wrong password. Please verify your credentials.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_ACCESS_DENIED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_ACCESS_DENIED.ToString(), "An authentication error occurred. Access has been denied.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_ACCOUNT_RESTRICTION, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_ACCOUNT_RESTRICTION.ToString(), "An authentication error occurred. The username and password are correct, but there is an account restriction on the user account (such as valid workstation, valid logon hours, etc.).") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_ACCOUNT_LOCKED_OUT, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_ACCOUNT_LOCKED_OUT.ToString(), "An authentication error occurred. Your user account is locked out.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_ACCOUNT_EXPIRED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_ACCOUNT_EXPIRED.ToString(), "An authentication error occurred. Your user account is expired.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_LOGON_TYPE_NOT_GRANTED, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_LOGON_TYPE_NOT_GRANTED.ToString(), "An authentication error occurred. The requested type of logon has not been granted.") },
		{ (int)ConnectErrors.FREERDP_ERROR_CONNECT_NO_OR_MISSING_CREDENTIALS, new RdpErrorInfo(ConnectErrors.FREERDP_ERROR_CONNECT_NO_OR_MISSING_CREDENTIALS.ToString(), "An authentication error occurred. Credentials invalid or missing.") },

		// Protocol-independent errors
		{ (int)ProtocolIndependentErrors.FREERDP_ERROR_RPC_INITIATED_DISCONNECT, new RdpErrorInfo(ProtocolIndependentErrors.FREERDP_ERROR_RPC_INITIATED_DISCONNECT.ToString(), "The disconnection was initiated by an administrative tool on the server in another session.") },
		{ (int)ProtocolIndependentErrors.FREERDP_ERROR_RPC_INITIATED_LOGOFF, new RdpErrorInfo(ProtocolIndependentErrors.FREERDP_ERROR_RPC_INITIATED_LOGOFF.ToString(), "The disconnection was due to a forced logoff initiated by an administrative tool on the server in another session.") },
		{ (int)ProtocolIndependentErrors.FREERDP_ERROR_IDLE_TIMEOUT, new RdpErrorInfo(ProtocolIndependentErrors.FREERDP_ERROR_IDLE_TIMEOUT.ToString(), "The idle session limit timer on the server has elapsed.") },
		{ (int)ProtocolIndependentErrors.FREERDP_ERROR_LOGON_TIMEOUT, new RdpErrorInfo(ProtocolIndependentErrors.FREERDP_ERROR_LOGON_TIMEOUT.ToString(), "The active session limit timer on the server has elapsed.") },
		{ (int)ProtocolIndependentErrors.FREERDP_ERROR_DISCONNECTED_BY_OTHER_CONNECTION, new RdpErrorInfo(ProtocolIndependentErrors.FREERDP_ERROR_DISCONNECTED_BY_OTHER_CONNECTION.ToString(), "Another user connected to the server, forcing the disconnection of the current connection.") },
		{ (int)ProtocolIndependentErrors.FREERDP_ERROR_OUT_OF_MEMORY, new RdpErrorInfo(ProtocolIndependentErrors.FREERDP_ERROR_OUT_OF_MEMORY.ToString(), "The server ran out of available memory resources.") },
		{ (int)ProtocolIndependentErrors.FREERDP_ERROR_SERVER_DENIED_CONNECTION, new RdpErrorInfo(ProtocolIndependentErrors.FREERDP_ERROR_SERVER_DENIED_CONNECTION.ToString(), "The server denied the connection.") },
		{ (int)ProtocolIndependentErrors.FREERDP_ERROR_SERVER_INSUFFICIENT_PRIVILEGES, new RdpErrorInfo(ProtocolIndependentErrors.FREERDP_ERROR_SERVER_INSUFFICIENT_PRIVILEGES.ToString(), "The user cannot connect to the server due to insufficient access privileges.") },
		{ (int)ProtocolIndependentErrors.FREERDP_ERROR_SERVER_FRESH_CREDENTIALS_REQUIRED, new RdpErrorInfo(ProtocolIndependentErrors.FREERDP_ERROR_SERVER_FRESH_CREDENTIALS_REQUIRED.ToString(), "The server does not accept saved user credentials and requires that the user enter their credentials for each connection.") },

		// Deprecated Connect Errors, all of these should now be included in ConnectErrors but are kept in case they are still needed (no harm)
		{ ERROR_START, new RdpErrorInfo(ERROR_START.ToString(), string.Empty) }
	};

	internal static bool ShouldShowError(int errorCode)
	{
		return errorCode > ERROR_START;
	}

	internal static RdpErrorInfo? ErrorInfoFromErrorCode(uint errorCode)
	{
		return ConnectionErrorInfos.TryGetValue((int)errorCode, out var errInfo) 
			? errInfo 
			: null;
	}

	private string Name { get; }
	private string Message { get; }

	public RdpErrorInfo()
	{
		Name = string.Empty;
		Message = string.Empty;
	}

	private RdpErrorInfo(string name, string message)
	{
		Name = name;
		Message = message;
	}

	public static bool IsCancelledError(uint errorNumber)
	{
		return
			errorNumber == (int)ConnectErrors.FREERDP_ERROR_CONNECT_CANCELLED;
	}

	public static bool IsAuthenticationError(uint errorNumber)
	{
		return
			errorNumber is 
				(int)ConnectErrors.FREERDP_ERROR_AUTHENTICATION_FAILED or 
				(int)ConnectErrors.FREERDP_ERROR_CONNECT_NO_OR_MISSING_CREDENTIALS or 
				(int)ConnectErrors.FREERDP_ERROR_CONNECT_LOGON_FAILURE or 
				(int)ConnectErrors.FREERDP_ERROR_CONNECT_WRONG_PASSWORD or 
				(int)ProtocolIndependentErrors.FREERDP_ERROR_SERVER_INSUFFICIENT_PRIVILEGES;
	}

	public static bool IsPotentialSecurityNegotiationError(uint errorNumber)
	{
		return
			errorNumber is 
				(int)ConnectErrors.FREERDP_ERROR_CONNECT_FAILED or 
				(int)ConnectErrors.FREERDP_ERROR_TLS_CONNECT_FAILED or 
				(int)ConnectErrors.FREERDP_ERROR_SECURITY_NEGO_CONNECT_FAILED;
	}

	public static bool IsUserTriggeredDisconnectError(uint errorNumber)
	{
		return
			errorNumber is 
				(int)ProtocolIndependentErrors.FREERDP_ERROR_LOGOFF_BY_USER or 
				(int)ProtocolIndependentErrors.FREERDP_ERROR_RPC_INITIATED_DISCONNECT_BY_USER;
	}

	public override string ToString()
	{
		return $"{Message} ({@"Error Code"}: {Name})";
	}
}