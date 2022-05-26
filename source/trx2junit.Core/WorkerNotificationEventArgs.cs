// (c) gfoidl, all rights reserved

using System;

namespace gfoidl.Trx2Junit.Core;

/// <summary>
/// <see cref="EventArgs"/> for the <see cref="Worker"/>.
/// </summary>
public class WorkerNotificationEventArgs : EventArgs
{
    /// <summary>
    /// The message that is reported.
    /// </summary>
    public string Message { get; }
    //-------------------------------------------------------------------------
    internal WorkerNotificationEventArgs(string message) => this.Message = message;
}
