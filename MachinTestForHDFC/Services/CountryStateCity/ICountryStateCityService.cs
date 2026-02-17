using MachinTestForHDFC.Models.CountryStateCity;

namespace MachinTestForHDFC.Services.CountryStateCity
{
    public interface ICountryStateCityService
    {
        Task<List<CountryMaster>> GetAllCountryAsync();
        Task<List<StateMaster>> GetAllStatesByCountryIdAsync(int countryId);
        Task<List<CityMaster>> GetAllCityByStateIdAsync(int stateId);
    }
}
