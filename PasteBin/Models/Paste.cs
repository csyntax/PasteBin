using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteBin.Models
{
    public class Paste
    {
        public Paste()
        {
            this.CreatedOn = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        public bool Private { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int? LanguageId { get; set; }

        public virtual Language Language { get; set; }
    }
}