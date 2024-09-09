using ApiApplicationCore.Dto;

namespace ApiApplicationCore.Services.Contract
{
    public interface ICountryService
    {
        ServiceResponse<CountryDto> GetCountryById(int id);
        ServiceResponse<IEnumerable<CountryDto>> GetCountry();
    }
}
