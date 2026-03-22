using System.Threading.Tasks;

namespace Brewery.Application.Interfaces
{
    public interface IHttpClientService
    {
        Task<TResponse> GetAsync<TResponse>(string url);

        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data);
    }
}
