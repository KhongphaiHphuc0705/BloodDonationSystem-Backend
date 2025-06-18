using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.BloodInventoryRepo
{
    public class BloodInventoryRepository : GenericRepository<BloodInventory>, IBloodInventoryRepository
    {
        public BloodInventoryRepository(BloodDonationSystemContext context) : base(context)
        {
            
        }

        public async Task<BloodInventory?> GetByBloodRegisId(int regisId)
        {
            return await _dbSet.FirstOrDefaultAsync(i => i.RegistrationId == regisId);
        }
    }
}
