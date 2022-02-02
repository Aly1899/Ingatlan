using reat.Persistency.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace reat.Repository
{
    public interface IAdRepository<T>
    {
        Task<IReadOnlyList<AdModel>> GetAllAds();
        Task<IReadOnlyList<AdModel>> GetNewAds();
        Task<IReadOnlyList<AdModel>> GetNewInactiveAds();
    }
}
