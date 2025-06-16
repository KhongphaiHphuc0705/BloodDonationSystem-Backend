using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Events
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(BloodDonationSystemContext context) : base(context)
        {

        }
    }
}
