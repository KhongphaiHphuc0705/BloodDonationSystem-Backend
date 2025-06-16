using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Facilities
{
    public class FacilityRepository : GenericRepository<Facility>, IFacilityRepository
    {
        public FacilityRepository(BloodDonationSystemContext context) : base(context)
        {

        }
    }
}
