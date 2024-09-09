using ApiApplicationCore.Models;

namespace ApiApplicationCore.Data.Contract
{
    public interface IStateRepository
    {
        IEnumerable<State> GetAll();
        State GetStateById(int id);
        List<State> GetAllStateByCountryId(int countryId);
    }
}
