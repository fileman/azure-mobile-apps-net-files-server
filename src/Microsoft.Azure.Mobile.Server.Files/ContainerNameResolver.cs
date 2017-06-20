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
    /// A container name resolver that applies a 1:1 mapping between records and containers.
    /// </summary>
    public sealed class ContainerNameResolver : IContainerNameResolver
    {
        private string suffix;
        private string prefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerNameResolver"/>.
        /// </summary>
        public ContainerNameResolver()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerNameResolver"/>,
        /// using the provided prefix and suffix when resolving container names.
        /// </summary>
        /// <param name="prefix">The prefix that should be applied to container names.</param>
        /// <param name="suffix">The suffix that should be applied to container names.</param>
        public ContainerNameResolver(string prefix, string suffix)
        {
            this.prefix = prefix;
            this.suffix = suffix;
        }

        /// <inheritdoc />
        public Task<string> GetFileContainerNameAsync(string tableName, string recordId, string fileName)
        {
            return Task.FromResult(GetDefaultContainerName(tableName, recordId));
        }

        /// <inheritdoc />
        public Task<IEnumerable<string>> GetRecordContainerNames(string tableName, string recordId)
        {
            return Task.FromResult<IEnumerable<string>>(new string[] { GetDefaultContainerName(tableName, recordId) });
        }

        private string GetDefaultContainerName(string tableName, string recordId)
        {
            return string.Format("{0}{1}-{2}{3}", prefix, tableName, recordId, suffix).ToLower();
        }
    }
}
