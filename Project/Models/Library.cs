using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Library
    {
        public int LibraryId { get; set; }

        [Required(ErrorMessage = "Library name is required")]
        [StringLength(100)]
        [Display(Name = "Library Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200)]
        public string Location { get; set; }

        [Required(ErrorMessage = "Operating hours are required")]
        [Display(Name = "Operating Hours")]
        [StringLength(100)]
        public string OperatingHours { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [Phone(ErrorMessage = "Enter a valid phone number")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        // Navigation property — one library has many books
        public virtual ICollection<Book> Books { get; set; }

    }
}