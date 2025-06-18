using Domain.Entities;
using Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.BloodInventoryRepo
{
    public interface IBloodInventoryRepository : IGenericRepository<BloodInventory>
    {
<<<<<<< HEAD
        Task<BloodInventory?> GetByBloodRegisIdAsync(int id);
=======
        Task<BloodInventory?> GetByBloodRegisId(int regisId);
>>>>>>> 7813e3d6e8429ba0d2072fc7e67b73930be3fabd
    }
}
