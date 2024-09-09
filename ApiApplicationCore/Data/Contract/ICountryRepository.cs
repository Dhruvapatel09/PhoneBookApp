using ApiApplicationCore.Models;

namespace ApiApplicationCore.Data.Contract
{
    public interface ICountryRepository
    {
        Country GetCountryById(int id);
        IEnumerable<Country> GetAll();
        //int TotalCountries();
    }
}
