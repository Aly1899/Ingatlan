using Microsoft.EntityFrameworkCore;
using reat.Persistency;
using reat.Persistency.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace reat.Repository
{
    public class AdRepository<T> : IAdRepository<T> where T : class
    {
        private readonly AdContext _adContext;

        public AdRepository(AdContext adContext)
        {
            _adContext = adContext;
        }
        public async Task<IReadOnlyList<AdModel>> GetAllAds()
        {
            return await _adContext.AdModels.ToListAsync();
        }
    }
}
