using Microsoft.AspNetCore.Http;
using Nav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Application.Interfaces
{
    public interface INavService
    {
        Task<IEnumerable<Fund>> GetAllNavByDate(DateTime startDate, DateTime endDate);
        Task AddNavAsync(IFormFile file);
        Task UpdateIsVisible(string fundId, bool isVisible);
        Task UpdateIsVisibleInScheme(string fundId, string schemeId, bool isVisible);
        Task<IEnumerable<Funds>> GetVisibleFunds();
        Task<IEnumerable<Scheme>> GetVisibleSchemes(string fundId);
        Task<IndividualNav> GetIndividualNav(string fundId, string schemeId, DateTime startDate, DateTime endDate);
    }
}
