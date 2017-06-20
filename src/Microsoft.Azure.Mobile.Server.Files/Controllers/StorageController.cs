// ---------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Files.Properties;

namespace Microsoft.Azure.Mobile.Server.Files.Controllers
{
    /// <summary>
    /// Provides a common <see cref="ApiController"/> abstraction for Storage Controllers.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public abstract class StorageController<T> : ApiController
    {
        private StorageProvider storageProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageController{T}"/> class.
        /// </summary>
        public StorageController()
            : this(Constants.StorageConnectionStringName)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageController{T}"/> class, using the
        /// provided <paramref name="connectionStringName"/> as the storage connection string name.
        /// </summary>
        /// <param name="connectionStringName">The connection string name to be used with the underlying provider.</param>
        public StorageController(string connectionStringName)
        {
            if (connectionStringName == null)
            {
                throw new ArgumentNullException("connectionStringName");
            }

            if (connectionStringName.Length == 0)
            {
                throw new ArgumentException(Resources.ConnectionStringNameMayNotBeEmpty);
            }

            var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];

            if (connectionStringSettings == null)
            {
                throw new ConfigurationErrorsException(string.Format(Resources.MissingConnectionString, connectionStringName));
            }

            this.storageProvider = new AzureStorageProvider(connectionStringSettings.ConnectionString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageController{T}"/> class, using the
        /// provided <paramref name="storageProvider"/>.
        /// </summary>
        /// <param name="storageProvider">The storage provider to be used by this controller.</param>
        public StorageController(StorageProvider storageProvider)
        {
            if (storageProvider == null)
            {
                throw new ArgumentNullException("storageProvider");
            }

            this.storageProvider = storageProvider;
        }

        /// <summary>
        /// Gets a storage token for the given entity ID and <see cref="StorageTokenRequest"/>.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="value">The request provided by the client.</param>
        /// <returns></returns>
        public Task<StorageToken> GetStorageTokenAsync(string id, StorageTokenRequest value)
        {
            return GetStorageTokenAsync(id, value, new ContainerNameResolver());
        }

        /// <summary>
        /// Gets a storage token for the given entity ID and <see cref="StorageTokenRequest"/>, using the provided <see cref="IContainerNameResolver"/>.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="value">The request provided by the client.</param>
        /// <param name="containerNameResolver">The instance of an <see cref="IContainerNameResolver"/> used to resolve the storage container name.</param>
        /// <returns></returns>
        public virtual async Task<StorageToken> GetStorageTokenAsync(string id, StorageTokenRequest value, IContainerNameResolver containerNameResolver)
        {
            StorageTokenScope scope = GetStorageScopeForRequest(id, value);

            StorageToken token = await this.storageProvider.GetAccessTokenAsync(value, scope, containerNameResolver);

            return token;
        }

        /// <summary>
        /// Gets the list of files for the provided entity ID.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>A <see cref="Task"/> that completes when the list of <see cref="MobileServiceFile"/> is available.</returns>
        public Task<IEnumerable<MobileServiceFile>> GetRecordFilesAsync(string id)
        {
            return GetRecordFilesAsync(id, new ContainerNameResolver());
        }

        /// <summary>
        /// Gets the list of files for the provided entity ID
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="containerNameResolver">The instance of an <see cref="IContainerNameResolver"/> used to resolve the storage container name.</param>
        /// <returns>A <see cref="Task"/> that completes when the list of <see cref="MobileServiceFile"/> is available.</returns>
        public async Task<IEnumerable<MobileServiceFile>> GetRecordFilesAsync(string id, IContainerNameResolver containerNameResolver)
        {
            return await this.storageProvider.GetRecordFilesAsync(GetTableName(), id, containerNameResolver);
        }

        /// <summary>
        /// Resolves the storage token scope for a given request.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="value">The request provided by the client.</param>
        /// <returns>The resolved scope, based on the provided parameters. The default implementation ALWAYS returns <see cref="StorageTokenScope.Record"/>.</returns>
        protected virtual StorageTokenScope GetStorageScopeForRequest(string id, StorageTokenRequest value)
        {
            return StorageTokenScope.Record;
        }

        private string GetTableName()
        {
            // TODO: This works for this tests, but we need to use the same logic applied by the framework to get the table name.
            return typeof(T).Name;
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="id">The id of the entity that owns the file.</param>
        /// <param name="name">The name of the file to be deleted.</param>
        /// <returns>A <see cref="Task"/> that completes when the delete operation finishes.</returns>
        public Task DeleteFileAsync(string id, string name)
        {
            return DeleteFileAsync(id, name, new ContainerNameResolver());
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="id">The id of the entity that owns the file.</param>
        /// <param name="name">The name of the file to be deleted.</param>
        /// <param name="containerNameResolver">The instance of an <see cref="IContainerNameResolver"/> used to resolve the storage container name.</param>
        /// <returns>A <see cref="Task"/> that completes when the delete operation finishes.</returns>
        public async Task DeleteFileAsync(string id, string name, IContainerNameResolver containerNameResolver)
        {
            await this.storageProvider.DeleteFileAsync(GetTableName(), id, name, containerNameResolver);
        }
    }
}
