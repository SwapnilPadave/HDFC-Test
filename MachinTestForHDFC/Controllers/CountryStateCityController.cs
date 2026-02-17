using MachinTestForHDFC.Services.CountryStateCity;
using Microsoft.AspNetCore.Mvc;

namespace MachinTestForHDFC.Controllers
{
    public class CountryStateCityController : Controller
    {
        private readonly ICountryStateCityService _countryStateCityService;
        public CountryStateCityController(ICountryStateCityService countryStateCityService)
        {
            _countryStateCityService = countryStateCityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountries()
        {
            var data = await _countryStateCityService.GetAllCountryAsync();
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStateByCountryId(int countryId)
        {
            var data = await _countryStateCityService.GetAllStatesByCountryIdAsync(countryId);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCityByStateId(int stateId)
        {
            var data = await _countryStateCityService.GetAllCityByStateIdAsync(stateId);
            return Json(data);
        }
    }
}
