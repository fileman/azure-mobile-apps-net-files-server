// ---------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------------------- 

using System;

namespace Microsoft.Azure.Mobile.Server.Files
{
    /// <summary>
    /// Representes a storage token.
    /// </summary>
    public class StorageToken
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="StorageToken"/>.
        /// </summary>
        /// <param name="resourceUri">The resource URI this token applies to.</param>
        /// <param name="entityId">The entity ID that owns the file or resource this token applies to.</param>
        /// <param name="permissions">The permissions supported by this token.</param>
        /// <param name="scope">The scope this token applies to.</param>
        /// <param name="rawToken">The raw token representation for the storage provider this token applies to.</param>
        public StorageToken(Uri resourceUri, string entityId, StoragePermissions permissions, StorageTokenScope scope, string rawToken)
        {
            if (resourceUri == null)
            {
                throw new ArgumentNullException(nameof(resourceUri));
            }

            if (string.IsNullOrEmpty(entityId))
            {
                throw new ArgumentException($"The argument '{nameof(entityId)}' may not be an empty or null string.");
            }

            if (string.IsNullOrEmpty(rawToken))
            {
                throw new ArgumentException($"The argument '{nameof(rawToken)}' may not be an empty or null string.");
            }

            ResourceUri = resourceUri;
            EntityId = entityId;
            Permissions = permissions;
            Scope = scope;
            RawToken = rawToken;
        }

        /// <summary>
        /// Gets the raw token representation for the storage provider this token applies to.
        /// </summary>
        public string RawToken { get; }

        /// <summary>
        /// Gets the resource URI this token applies to.
        /// </summary>
        public Uri ResourceUri { get; }

        /// <summary>
        /// Gets the entity ID that owns the file or resource this token applies to.
        /// </summary>
        public string EntityId { get; }

        /// <summary>
        /// Gets the permissions supported by this token.
        /// </summary>
        public StoragePermissions Permissions { get; }

        /// <summary>
        /// Gets the scope this token applies to.
        /// </summary>
        public StorageTokenScope Scope { get; }
    }
}
