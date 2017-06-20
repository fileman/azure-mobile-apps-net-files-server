// ---------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------------------- 

using System;

namespace Microsoft.Azure.Mobile.Server.Files
{
    /// <summary>
    /// Specifies the scope applied to a <see cref="StorageToken"/>.
    /// </summary>
    public enum StorageTokenScope
    {
        /// <summary>
        /// The token is scoped to a record (all files/containers associated with it).
        /// </summary>
        Record,
        /// <summary>
        /// The token is scoped to a single file.
        /// </summary>
        File
    }
}
