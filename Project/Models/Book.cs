using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace Project.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [StringLength(100)]
        public string Author { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(50)]
        public string Genre { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "ISBN must be 10-20 characters")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Required]
        [Display(Name = "Available for Borrowing")]
        public bool IsAvailable { get; set; }

        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [Display(Name = "Cover Image URL")]
        [StringLength(500)]
        public string CoverImageUrl { get; set; }

        [Required]
        [Range(1000, 2100, ErrorMessage = "Enter a valid year")]
        [Display(Name = "Publication Year")]
        public int PublicationYear { get; set; }

        [Required]
        [Range(1, 10000)]
        [Display(Name = "Total Copies")]
        public int TotalCopies { get; set; }

        // One book can have many borrowing transactions
        public virtual ICollection<BorrowingTransaction> BorrowingTransactions { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }

        // Foreign key — links each book to one library
        [Required(ErrorMessage = "Please select a library")]
        [Display(Name = "Library")]
        public int LibraryId { get; set; }

        // Navigation property — lets us write book.Library.Name in views
        public virtual Library Library { get; set; }
    }
}