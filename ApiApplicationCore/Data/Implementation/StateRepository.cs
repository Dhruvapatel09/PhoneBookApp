using ApiApplicationCore.Data.Contract;
using ApiApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiApplicationCore.Data.Implementation
{
    public class StateRepository : IStateRepository
    {
        private readonly IAppDbContext _appDbContext;

        public StateRepository(IAppDbContext context)
        {
            _appDbContext = context;
        }
        public IEnumerable<State> GetAll()
        {
            List<State> standards = _appDbContext.States.Include(c => c.Country).ToList();
            return standards;
        }
        public State GetStateById(int id)
        {
            var student = _appDbContext.States.Include(c => c.Country).FirstOrDefault(c => c.StateId == id);
            return student;
        }
        public List<State> GetAllStateByCountryId(int countryId)
        {
            List<State> countries = _appDbContext.States.Include(p => p.Country).Where(c => c.CountryId == countryId).ToList();
            return countries;
        }
    }
}
