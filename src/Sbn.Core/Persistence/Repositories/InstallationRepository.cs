using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Sbn.Cms.Core.Serialization;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public class InstallationRepository : IInstallationRepository
    {
        private readonly IJsonSerializer _jsonSerializer;
        private static HttpClient _httpClient;
        private const string RestApiInstallUrl = "https://our.sbn.com/sbn/api/Installation/Install";

        public InstallationRepository(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public async Task SaveInstallLogAsync(InstallLog installLog)
        {
            try
            {
                if (_httpClient == null)
                    _httpClient = new HttpClient();

                var content = new StringContent(_jsonSerializer.Serialize(installLog), Encoding.UTF8, "application/json");

                await _httpClient.PostAsync(RestApiInstallUrl, content);
            }
            // this occurs if the server for Our is down or cannot be reached
            catch (HttpRequestException)
            { }
        }
    }
}
