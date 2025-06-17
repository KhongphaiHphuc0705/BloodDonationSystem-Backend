using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.EventsDTO
{
    public class NormalEventDTO
    {
        public string Title { get; set; }
        public int MaxOfDonor { get; set; }
        public double EstimatedVolume { get; set; }
        public DateOnly EventTime { get; set; }
    }
}
