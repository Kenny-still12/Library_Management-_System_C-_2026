using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class GroupMember
    {
        public int GroupMemberId { get; set; }

        [Required]
        [Display(Name = "Student ID")]
        public string StudentId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}