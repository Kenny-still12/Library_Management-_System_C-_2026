using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Librarian
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibrarianId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Employee ID is required")]
        [StringLength(20)]
        [Display(Name = "Employee ID")]
        public string EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        // Which library this librarian works at
        [Required]
        [Display(Name = "Library")]
        public int LibraryId { get; set; }
        public virtual Library Library { get; set; }

        // Links to Identity login account
        public string UserId { get; set; }
    }
}