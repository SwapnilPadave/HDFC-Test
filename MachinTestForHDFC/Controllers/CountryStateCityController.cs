using MachinTestForHDFC.ResponseModels;
using MachinTestForHDFC.Services.CountryStateCity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MachinTestForHDFC.Controllers
{
    public class CountryStateCityController : Controller
    {
        private readonly ICountryStateCityService _countryStateCityService;
        private readonly HttpClient _httpClient;
        public CountryStateCityController(ICountryStateCityService countryStateCityService, HttpClient httpClient)
        {
            _countryStateCityService = countryStateCityService;
            _httpClient = httpClient;
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
        public async Task<IActionResult> GetAllStates(int countryId)
        {
            try
            {
                //External api.
                var response = await _httpClient.GetAsync("https://localhost:7295/api/State/GetAll");

                //Check if response is success or not.
                if (!response.IsSuccessStatusCode)
                    return Json(new List<StateMasters>());

                //Read response data from external api.
                var json = await response.Content.ReadAsStringAsync();

                //Deserialize the response from external api response.
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<StateMasters>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

                //Mapping response to our model.
                var states = apiResponse.data ?? new List<StateMasters>();

                //Return json response to page.
                return Json(states);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCityByStateId(int stateId)
        {
            var data = await _countryStateCityService.GetAllCityByStateIdAsync(stateId);
            return Json(data);
        }
    }
}

public class StateMasters
{
    public int Id { get; set; }
    public string StateName { get; set; } = string.Empty;
    public string StateCode { get; set; } = string.Empty;
    public int CountryId { get; set; }
}

//public class ApiResponse<T>
//{
//    public int StatusCode { get; set; }
//    public string Message { get; set; } = string.Empty;
//    public T? Data { get; set; }
//    public List<Errors> Errors { get; set; } = new List<Errors>();
//}

//public class Errors
//{
//    public string PropertyName { get; set; } = string.Empty;

//    public required string[] ErrorMessages { get; set; }
//}
