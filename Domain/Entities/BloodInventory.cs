using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class BloodInventory
    {
        [Key]
        public int Id { get; set; }
        public float Volume { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime ExpiredDate { get; set; }
        public bool IsAvailable { get; set; }

        public int BloodTypeId { get; set; }
        public BloodComponent BloodComponent { get; set; }
        public Guid? RemoveBy { get; set; }
        public int RegistrationId { get; set; }

        [ForeignKey("BloodTypeId")]
        public virtual BloodType BloodType { get; set; }

        [ForeignKey("RemoveBy")]
        public virtual User RemovedByUser { get; set; }

        [ForeignKey("RegistrationId")]
        public virtual BloodRegistration BloodRegistration { get; set; }
    }
}
