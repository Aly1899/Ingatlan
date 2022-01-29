using Microsoft.EntityFrameworkCore;
using reat.Persistency;
using reat.Persistency.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace reat.Repository
{
    public class AdPriceRepository : IAdPriceRepository
    {
        private readonly AdContext _adContext;

        public AdPriceRepository(AdContext adContext)
        {
            _adContext = adContext;
        }
        public async Task<IReadOnlyList<AdPriceModel>> GetAllAdPrice()
        {
            return await _adContext.AdPriceModels.ToListAsync();
        }
    }
}
