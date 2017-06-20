// ---------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------------------- 

using System;
using System.Collections.Generic;
using Microsoft.Azure.Mobile.Server.Files.Properties;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace Microsoft.Azure.Mobile.Server.Files
{
    /// <summary>
    /// Represents a <see cref="MobileServiceFile"/>.
    /// </summary>
    public class MobileServiceFile
    {
        /// <summary>
        /// The file ID.
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// The file name (friendly name)
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// The name of the table this file's parent entity belongs to.
        /// </summary>
        public virtual string TableName { get; set; }

        /// <summary>
        /// The ID of the entity that owns the file.
        /// </summary>
        public virtual string ParentId { get; set; }

        /// <summary>
        /// Gets or sets this file's content MD5 hash.
        /// </summary>
        public virtual string ContentMD5 { get; set; }

        /// <summary>
        /// Gets or sets the file size, in bytes.
        /// </summary>
        public virtual long Length { get; set; }

        /// <summary>
        /// Gets or sets the LastMofified <see cref="DateTimeOffset"/>
        /// </summary>
        public DateTimeOffset? LastModified { get; set; }

        /// <summary>
        /// Gets or sets this file's store URI.
        /// </summary>
        public string StoreUri { get; set; }

        /// <summary>
        /// Gets or sets user defined metadata for this file.
        /// </summary>
        public virtual IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Creates a <see cref="MobileServiceFile"/> from a <see cref="CloudBlockBlob"/>.
        /// </summary>
        /// <param name="item">The instance of a <see cref="CloudBlockBlob"/> to create the file from.</param>
        /// <param name="parentEntityType">The parent entity type (table name).</param>
        /// <param name="parentEntityId">The id of the entity that owns the file.</param>
        /// <returns></returns>
        public static MobileServiceFile FromBlobItem(CloudBlockBlob item, string parentEntityType, string parentEntityId)
        {
            return new MobileServiceFile
            {
                Id = item.Name,
                Name = item.Name,
                TableName = parentEntityType,
                ParentId = parentEntityId,
                Length = item.Properties.Length,
                ContentMD5 = item.Properties.ContentMD5,
                LastModified = item.Properties.LastModified,
                Metadata = item.Metadata,
                StoreUri = item.Uri.LocalPath
            };
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public virtual string FileInfoToken
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                try
                {
                    JsonConvert.PopulateObject(value, this,
                        new JsonSerializerSettings
                        {
                            Error = (a, s) => s.ErrorContext.Handled = true
                        });
                }
                catch { }
            }
        }

        private class NullMobileServiceFile : MobileServiceFile
        {
            public override string TableName
            {
                get
                {
                    return null;
                }
                set { throw new InvalidOperationException(Resources.CannotModifyMobileFileInstance); }
            }

            public override string ContentMD5
            {
                get
                {
                    return null;
                }
                set { throw new InvalidOperationException(Resources.CannotModifyMobileFileInstance); }
            }

            public override string FileInfoToken
            {
                get
                {
                    return null;
                }
                set { throw new InvalidOperationException(Resources.CannotModifyMobileFileInstance); }
            }

            public override string Id
            {
                get
                {
                    return null;
                }
                set { throw new InvalidOperationException(Resources.CannotModifyMobileFileInstance); }
            }

            public override long Length
            {
                get
                {
                    return 0;
                }
                set { throw new InvalidOperationException(Resources.CannotModifyMobileFileInstance); }
            }

            public override IDictionary<string, string> Metadata
            {
                get
                {
                    return null;
                }
                set { throw new InvalidOperationException(Resources.CannotModifyMobileFileInstance); }
            }

            public override string Name
            {
                get
                {
                    return null;
                }
                set { throw new InvalidOperationException(Resources.CannotModifyMobileFileInstance); }
            }
        }
    }

}
