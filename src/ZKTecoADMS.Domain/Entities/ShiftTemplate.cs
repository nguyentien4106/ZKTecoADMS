using System.ComponentModel.DataAnnotations;
using ZKTecoADMS.Domain.Entities.Base;

namespace ZKTecoADMS.Domain.Entities
{
    public class ShiftTemplate : Entity<Guid>
    {
        public Guid ManagerId { get; set; }
        public ApplicationUser Manager { get; set; } = null!;
        
        [Required]
        public string Name { get; set; } = null!;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; }
    }
}