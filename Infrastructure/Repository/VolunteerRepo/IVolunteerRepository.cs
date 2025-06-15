using Domain.Entities;
using Infrastructure.Repository.Base;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.VolunteerRepo
{
    public interface IVolunteerRepository : IGenericRepository<Volunteer>
    {
    }
}
