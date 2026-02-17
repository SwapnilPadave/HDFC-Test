using MachinTestForHDFC.Database;
using MachinTestForHDFC.Models.CountryStateCity;
using Microsoft.EntityFrameworkCore;

namespace MachinTestForHDFC.Services.CountryStateCity
{
    public class CountryStateCityService : ICountryStateCityService
    {
        private readonly TestDbContext _context;
        public CountryStateCityService(TestDbContext context)
        {
            _context = context;
        }

        public async Task<List<CountryMaster>> GetAllCountryAsync()
        {
            var data = await _context.CountryMaster.ToListAsync();
            return data;
        }

        public async Task<List<StateMaster>> GetAllStatesByCountryIdAsync(int countryId)
        {
            var data = await _context.StateMaster.Where(x => x.CountryId == countryId).ToListAsync();
            return data;
        }

        public async Task<List<CityMaster>> GetAllCityByStateIdAsync(int stateId)
        {
            var data = await _context.CityMaster.Where(x => x.StateId == stateId).ToListAsync();
            return data;
        }
    }
}
