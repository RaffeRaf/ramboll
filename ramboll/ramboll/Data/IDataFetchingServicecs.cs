using ramboll.Models;

namespace ramboll.Data
{
    public interface IDataFetchingServicecs
    {
        public Task<DataResponse> GetDataAsync(List<string> parameters);
    }
}
