// ---------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Microsoft.Azure.Mobile.Server.Files
{
    /// <summary>
    /// A storage provider that uses Azure Blob Storage as the file store.
    /// </summary>
    public class AzureStorageProvider : StorageProvider
    {
        private readonly static Dictionary<StoragePermissions, SharedAccessBlobPermissions> storagePermissionsMapping;

        private string connectionString;

        static AzureStorageProvider()
        {
            storagePermissionsMapping = new Dictionary<StoragePermissions, SharedAccessBlobPermissions>
            {
                {StoragePermissions.Read, SharedAccessBlobPermissions.Read},
                {StoragePermissions.Write, SharedAccessBlobPermissions.Write},
                {StoragePermissions.Delete, SharedAccessBlobPermissions.Delete},
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageProvider"/> class, using the provided
        /// <paramref name="connectionString"/> to connect to Azure Storage.
        /// </summary>
        /// <param name="connectionString"></param>
        public AzureStorageProvider(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.connectionString = connectionString;
        }

        /// <inheritdoc />
        public override string Name
        {
            get { return "Microsoft Azure Blob Storage"; }
        }

        /// <inheritdoc />
        public async override Task<IEnumerable<MobileServiceFile>> GetRecordFilesAsync(string tableName, string recordId, IContainerNameResolver containerNameResolver)
        {
            if (tableName == null)
            {
                throw new ArgumentException(nameof(tableName));
            }

            if (recordId == null)
            {
                throw new ArgumentException(nameof(recordId));
            }

            if (containerNameResolver == null)
            {
                throw new ArgumentNullException(nameof(containerNameResolver));
            }

            IEnumerable<string> containerNames = await containerNameResolver.GetRecordContainerNames(tableName, recordId);

            var files = new List<MobileServiceFile>();

            foreach (var containerName in containerNames)
            {
                IEnumerable<CloudBlockBlob> blobs = await GetContainerFilesAsync(containerName);

                files.AddRange(blobs.Select(b => MobileServiceFile.FromBlobItem(b, tableName, recordId)));
            }

            return files;
        }

        /// <inheritdoc />
        public async override Task DeleteFileAsync(string tableName, string recordId, string fileName, IContainerNameResolver containerNameResolver)
        {
            if (tableName == null)
            {
                throw new ArgumentException(nameof(tableName));
            }

            if (recordId == null)
            {
                throw new ArgumentException(nameof(recordId));
            }

            if (fileName == null)
            {
                throw new ArgumentException(nameof(fileName));
            }

            if (containerNameResolver == null)
            {
                throw new ArgumentNullException(nameof(containerNameResolver));
            }

            string containerName = await containerNameResolver.GetFileContainerNameAsync(tableName, recordId, fileName);

            CloudBlobContainer container = GetContainer(containerName);

            CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
            await blob.DeleteIfExistsAsync();
        }

        /// <inheritdoc />
        public async override Task<StorageToken> GetAccessTokenAsync(StorageTokenRequest request, StorageTokenScope scope, IContainerNameResolver containerNameResolver)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (containerNameResolver == null)
            {
                throw new ArgumentNullException(nameof(containerNameResolver));
            }

            string containerName = await containerNameResolver.GetFileContainerNameAsync(request.TargetFile.TableName, request.TargetFile.ParentId, request.TargetFile.Name);

            CloudBlobContainer container = GetContainer(containerName);

            var constraints = new SharedAccessBlobPolicy();
            constraints.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1);
            constraints.Permissions = GetBlobAccessPermissions(request.Permissions);

            string sas = null;
            Uri resourceUri = null;
            if (scope == StorageTokenScope.File)
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(request.TargetFile.Name);

                resourceUri = blob.Uri;
                sas = blob.GetSharedAccessSignature(constraints);
            }
            else if (scope == StorageTokenScope.Record)
            {
                resourceUri = container.Uri;
                sas = container.GetSharedAccessSignature(constraints);
            }

            var storageToken = new StorageToken(resourceUri, request.TargetFile.ParentId, request.Permissions, scope, sas);
            
            return storageToken;
        }

        /// <summary>
        /// Gets the list of <see cref="CloudBlockBlob"/> items from a given container.
        /// </summary>
        /// <param name="containerName">The container from which the list should be retrieved.</param>
        /// <returns>A <see cref="Task{IEnumerable}"/> that completes when the list of items is retrieved.</returns>
        protected virtual async Task<IEnumerable<CloudBlockBlob>> GetContainerFilesAsync(string containerName)
        {
            CloudBlobContainer container = GetContainer(containerName);

            IEnumerable<IListBlobItem> blobs = await Task.Run(() => container.ListBlobs(blobListingDetails: BlobListingDetails.Metadata));
            return blobs.OfType<CloudBlockBlob>();
        }

        /// <summary>
        /// Gets a reference to a blob container with the specified name.
        /// </summary>
        /// <param name="containerName">A string containing the name of the container.</param>
        /// <returns>A <see cref="CloudBlockBlob"/> reference to the container specified by <paramref name="containerName"/>.</returns>
        protected CloudBlobContainer GetContainer(string containerName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(this.connectionString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
            return container;
        }

        private SharedAccessBlobPermissions GetBlobAccessPermissions(StoragePermissions storagePermissions)
        {
            SharedAccessBlobPermissions permissions = storagePermissionsMapping
                .Aggregate(SharedAccessBlobPermissions.None, (a, kvp) => (storagePermissions & kvp.Key) == kvp.Key ? a |= kvp.Value : a);

            return permissions;
        }
    }
}
