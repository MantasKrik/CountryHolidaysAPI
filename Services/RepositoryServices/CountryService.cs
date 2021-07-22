using CountryHolidaysAPI.Models;
using CountryHolidaysAPI.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CountryHolidaysAPI.Services.RepositoryServices
{
    public class CountryService : ICountryService
    {
        private ICountryRepositoryExtension _repository;
        private IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;

        public CountryService(ICountryRepositoryExtension repository, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _httpClientFactory = httpClientFactory;

            _httpClient = _httpClientFactory.CreateClient();
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            var repositoryList = await _repository.Get();

            if(repositoryList == null || repositoryList.Count() == 0)
            {
                return await SyncCountriesInternal();
            }
            else
            {
                return repositoryList;
            }
        }

        public async Task SyncCountries()
        {
            var response = await _httpClient.GetAsync(EnricoAPI.GetSupportedCountriesRequest());
            var externalList = await EnricoAPI.ParseCountries(await response.Content.ReadAsStringAsync(), null);

            await _repository.CreateRange(externalList);
        }

        private async Task<IEnumerable<Country>> SyncCountriesInternal()
        {
            var response = await _httpClient.GetAsync(EnricoAPI.GetSupportedCountriesRequest());
            var externalList = await EnricoAPI.ParseCountries(await response.Content.ReadAsStringAsync(), null);

            return externalList;
        }
    }
}
