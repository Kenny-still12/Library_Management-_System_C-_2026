using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public enum BorrowingStatus
    {
        Reserved,
        Borrowed,
        Returned,
        Overdue
    }

    public class BorrowingTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Borrow Date")]
        public DateTime BorrowDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }

        [Required]
        public BorrowingStatus Status { get; set; }

        [Range(0, 10)]
        [Display(Name = "Renewals Used")]
        public int RenewalCount { get; set; }

        [Range(0, 10000)]
        [DataType(DataType.Currency)]
        [Display(Name = "Fine Amount ($)")]
        public decimal FineAmount { get; set; }

        [Display(Name = "Fine Paid")]
        public bool FinePaid { get; set; }

        // Which book was borrowed
        [Required]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        // Which member borrowed it
        [Required]
        public int MemberId { get; set; }
        public virtual Member Member { get; set; }
    }
}