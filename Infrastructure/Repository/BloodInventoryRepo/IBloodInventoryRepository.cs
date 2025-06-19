using Domain.Entities;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository.BloodInventoryRepo
{
    public interface IBloodInventoryRepository : IGenericRepository<BloodInventory>
    {
        Task<BloodInventory?> GetByBloodRegisIdAsync(int id);
    }
}
