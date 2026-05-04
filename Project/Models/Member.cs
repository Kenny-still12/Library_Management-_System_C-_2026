using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        public string Address { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Member Since")]
        public DateTime MembershipDate { get; set; }

        // Links this profile to the Identity login account
        public string UserId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        // One member can have many borrowing transactions
        public virtual ICollection<BorrowingTransaction> BorrowingTransactions { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}