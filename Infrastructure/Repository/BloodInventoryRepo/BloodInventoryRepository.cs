using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.BloodInventoryRepo
{
    public class BloodInventoryRepository : GenericRepository<BloodInventory>, IBloodInventoryRepository
    {
        public BloodInventoryRepository(BloodDonationSystemContext context) : base(context)
        {

        }

        public async Task<BloodInventory?> GetByBloodRegisIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(i => i.RegistrationId == id);
        }
    }
}
