using System;

namespace PasteBin.Models
{
    public class Paste
    {
        public Paste()
        {
            this.CreatedDate = DateTime.Now;
            this.ExpiryDate = DateTime.Now.AddDays(3);
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool Private { get; set; }

        public int? LanguageId { get; set; }

        public virtual Language Language { get; set; }
    }
}