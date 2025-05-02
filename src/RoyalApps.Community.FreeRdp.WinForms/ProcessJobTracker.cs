// ReSharper disable CommentTypo

// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license.

/* This is a derivative from multiple answers on https://stackoverflow.com/questions/3342941/kill-child-process-when-parent-process-is-killed */

using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;
using Windows.Win32.System.JobObjects;
using static Windows.Win32.PInvoke;

namespace RoyalApps.Community.FreeRdp.WinForms;

#pragma warning disable SA1629 // xml doc comments must end with periods (we end with a hyperlink).

/// <summary>
/// Allows processes to be automatically killed if this parent process unexpectedly quits
/// (or when an instance of this class is disposed).
/// </summary>
/// <remarks>
/// This "just works" on Windows 8.
/// To support Windows Vista or Windows 7 requires an app.manifest with specific content as described here:
/// https://stackoverflow.com/a/9507862/46926
/// </remarks>
internal class ProcessJobTracker : IDisposable
{
    /// <summary>
    /// The job handle.
    /// </summary>
    /// <remarks>
    /// Closing this handle would close all tracked processes. So we don't do it in this process
    /// so that it happens automatically when our process exits.
    /// </remarks>
    private readonly SafeFileHandle _jobHandle;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessJobTracker"/> class.
    /// </summary>
    public unsafe ProcessJobTracker(string namePrefix)
    {
        // The job name is optional (and can be null) but it helps with diagnostics.
        //  If it's not null, it has to be unique. Use SysInternals' Handle command-line
        //  utility: handle -a ChildProcessTracker
        var jobName = namePrefix + nameof(ProcessJobTracker) + Process.GetCurrentProcess().Id;
        _jobHandle = CreateJobObject(null, jobName);

        var extendedInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION
        {
            BasicLimitInformation = new JOBOBJECT_BASIC_LIMIT_INFORMATION
            {
                LimitFlags = JOB_OBJECT_LIMIT.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE,
            },
        };

        if (!SetInformationJobObject(
                _jobHandle,
                JOBOBJECTINFOCLASS.JobObjectExtendedLimitInformation,
                &extendedInfo,
                (uint)sizeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION)))
        {
            throw new Win32Exception();
        }
    }

    /// <summary>
    /// Ensures a given process is killed when the current process exits.
    /// </summary>
    /// <param name="process">The process whose lifetime should never exceed the lifetime of the current process.</param>
    public void AddProcess(Process process)
    {
        if (process is null)
        {
            throw new ArgumentNullException(nameof(process));
        }

        bool success = AssignProcessToJobObject(_jobHandle, new SafeFileHandle(process.Handle, ownsHandle: false));
        if (!success && !process.HasExited)
        {
            throw new Win32Exception();
        }
    }

    /// <summary>
    /// Kills all processes previously tracked with <see cref="AddProcess(Process)"/> by closing the Windows Job.
    /// </summary>
    public void Dispose()
    {
        _jobHandle.Dispose();
    }
}
