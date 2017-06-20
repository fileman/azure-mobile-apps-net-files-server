// ---------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------------------- 

using System;

namespace Microsoft.Azure.Mobile.Server.Files
{
    /// <summary>
    /// Represents a storage token request.
    /// </summary>
    public class StorageTokenRequest
    {
        /// <summary>
        /// The permissions requested.
        /// </summary>
        public StoragePermissions Permissions { get; set; }

        /// <summary>
        /// The provider name targeted by this request.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// The target file this request applies to.
        /// </summary>
        public MobileServiceFile TargetFile { get; set; }
    }

}
