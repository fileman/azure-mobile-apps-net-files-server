// ---------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------------------- 

using System;

namespace Microsoft.Azure.Mobile.Server.Files
{
    /// <summary>
    /// Specifies the supported storage permissions.
    /// </summary>
    [Flags]
    public enum StoragePermissions
    {
        /// <summary>
        /// No permission.
        /// </summary>
        None = 0x0,
        /// <summary>
        /// Permission to read from storage.
        /// </summary>
        Read = 0x1,
        /// <summary>
        /// Permission to write to storage.
        /// </summary>
        Write = 0x2,
        /// <summary>
        /// Permission to delete from storage.
        /// </summary>
        Delete = 0x4,
        /// <summary>
        /// Permission to list storage items.
        /// </summary>
        List = 0x8,
        /// <summary>
        /// Permission to read and write to storage.
        /// </summary>
        ReadWrite = Read | Write,
        /// <summary>
        /// Permission to read, write, delete and list storage items.
        /// </summary>
        All = Read | Write | Delete | List
    }
}
