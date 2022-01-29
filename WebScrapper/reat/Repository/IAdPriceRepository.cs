using reat.Persistency.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace reat.Repository
{
    public interface IAdPriceRepository
    {
        Task<IReadOnlyList<AdPriceModel>> GetAllAdPrice();
    }
}
