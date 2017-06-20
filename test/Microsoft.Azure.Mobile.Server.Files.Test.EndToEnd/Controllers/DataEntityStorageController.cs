using Microsoft.Azure.Mobile.Server.Files;
using Microsoft.Azure.Mobile.Server.Files.Controllers;
using Microsoft.Azure.Mobile.Server.Files.Test.EndToEnd.DataObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Microsoft.Azure.Mobile.Server.Files.Test.EndToEnd.Controllers
{
    [RoutePrefix("tables/dataentity")]
    public class DataEntityStorageController : StorageController<DataEntity>
    {
        [HttpPost]
        [Route("{id}/StorageToken")]
        public async Task<IHttpActionResult> PostStorageTokenRequest(string id, StorageTokenRequest value)
        {
            StorageToken token = await GetStorageTokenAsync(id, value);

            return Ok(token);
        }

        [HttpGet]
        [Route("{id}/MobileServiceFiles")]
        public async Task<IHttpActionResult> GetFiles(string id)
        {
            IEnumerable<MobileServiceFile> files = await GetRecordFilesAsync(id);

            return Ok(files);
        }

        [HttpDelete]
        [Route("{id}/MobileServiceFiles/{name}")]
        public Task Delete(string id, string name)
        {
            return base.DeleteFileAsync(id, name);
        }
    }
}
