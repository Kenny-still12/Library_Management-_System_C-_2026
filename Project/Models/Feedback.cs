using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackId { get; set; }

        [Required(ErrorMessage = "Please select a rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        [Display(Name = "Rating (1-5 Stars)")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Please write a comment")]
        [StringLength(500, MinimumLength = 5,
            ErrorMessage = "Comment must be between 5 and 500 characters")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Your Review")]
        public string Comment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Submitted On")]
        public DateTime SubmittedDate { get; set; }

        [Display(Name = "Approved")]
        public bool IsApproved { get; set; }

        // Which book this feedback is for
        [Required]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        // Which member left the feedback
        [Required]
        public int MemberId { get; set; }
        public virtual Member Member { get; set; }
    }
}