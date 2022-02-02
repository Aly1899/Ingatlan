using Microsoft.EntityFrameworkCore;
using reat.Persistency;
using reat.Persistency.Entities;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IReadOnlyList<AdModel>> GetNewAds()
        {
            var lastFetchDate = _adContext.FetchDates.OrderByDescending(f => f.EntryDate).First();
            return await _adContext.AdModels.Where(a => a.Created.Date == lastFetchDate.EntryDate.Date).ToListAsync();
        }

        public async Task<IReadOnlyList<AdModel>> GetNewInactiveAds()
        {
            var lastFetchDate = _adContext.FetchDates.OrderByDescending(f => f.EntryDate).First();
            return await _adContext.AdModels.Where(a =>
                    a.Updated.Date == lastFetchDate.EntryDate.Date &&
                    a.IsInactive)
                .ToListAsync();
        }
    }
}
