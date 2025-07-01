using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.BloodHistoryDTO
{
    public class EventBloodHistory
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateOnly EventDate { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public DateOnly RegisterDate { get; set; }
    }
}
