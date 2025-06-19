using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository.VolunteerRepo
{
    public class VolunteerRepository : GenericRepository<Volunteer>, IVolunteerRepository
    {
        public VolunteerRepository(BloodDonationSystemContext context) : base(context)
        {

        }
    }
}
