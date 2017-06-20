// ---------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Azure.Mobile.Server.Files
{
    /// <summary>
    /// Provides a common abstraction for storage providers.
    /// </summary>
    public abstract class StorageProvider
    {
        /// <summary>
        /// The storage provider name.
        /// </summary>
        abstract public string Name { get; }

        /// <summary>
        /// Gets the files associated with a record.
        /// </summary>
        /// <param name="tableName">The table (entity) name.</param>
        /// <param name="recordId">The entity id.</param>
        /// <param name="containerNameResolver">The instance of an <see cref="IContainerNameResolver"/> used to resolve the storage container name.</param>
        /// <returns>A <see cref="Task{TResult}"/> that completes when the list of files is retrieved.</returns>
        abstract public Task<IEnumerable<MobileServiceFile>> GetRecordFilesAsync(string tableName, string recordId, IContainerNameResolver containerNameResolver);

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="tableName">The table (entity) name.</param>
        /// <param name="recordId">The entity id.</param>
        /// <param name="fileName">The name of the file to be deleted.</param>
        /// <param name="containerNameResolver">The instance of an <see cref="IContainerNameResolver"/> used to resolve the storage container name.</param>
        /// <returns>A <see cref="Task"/> that completes when the delete operation is finished.</returns>
        abstract public Task DeleteFileAsync(string tableName, string recordId, string fileName, IContainerNameResolver containerNameResolver);

        /// <summary>
        /// Retrieves an access token for a given request.
        /// </summary>
        /// <param name="request">The request the requested access </param>
        /// <param name="scope"></param>
        /// <param name="containerNameResolver">The instance of an <see cref="IContainerNameResolver"/> used to resolve the storage container name.</param>
        /// <returns>A <see cref="Task{StorageToken}"/> that completes when the storage token is generated.</returns>
        abstract public Task<StorageToken> GetAccessTokenAsync(StorageTokenRequest request, StorageTokenScope scope, IContainerNameResolver containerNameResolver);
    }
}
