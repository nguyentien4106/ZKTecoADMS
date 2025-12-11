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
        
        // public int BreakDurationInMinutes { get; set; } = 0;
        
        [Required]
        public int MaximumAllowedLateMinutes { get; set; } = 30;
        
        [Required]
        public int MaximumAllowedEarlyLeaveMinutes { get; set; } = 30;

        [Required]
        public int BreakTimeMinutes { get; set; } = 60;
        
        public bool IsActive { get; set; }
    }
}