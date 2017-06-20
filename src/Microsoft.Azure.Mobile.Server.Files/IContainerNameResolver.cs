// ---------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Mobile.Server.Files
{
    /// <summary>
    /// Defines the methods used to resolve storage container names.
    /// </summary>
    public interface IContainerNameResolver
    {
        /// <summary>
        /// Resolves the container name for a given file.
        /// </summary>
        /// <param name="tableName">The name of the table (entity type) that owns the file.</param>
        /// <param name="recordId">The ID of the entity that owns that file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>A <see cref="Task{String}"/> that completes when the container name is resolved.</returns>
        Task<string> GetFileContainerNameAsync(string tableName, string recordId, string fileName);

        /// <summary>
        /// Gets the list of containers mapped to a given record/entity.
        /// This is primarily used to list the files associated with a given entity.
        /// </summary>
        /// <param name="tableName">The name of the table (entity type).</param>
        /// <param name="recordId">The ID of the entity.</param>
        /// <returns>A <see cref="Task{IEnumerable}"/> that completes when the list of container names is retrieved.</returns>
        Task<IEnumerable<string>> GetRecordContainerNames(string tableName, string recordId);
    }
}
