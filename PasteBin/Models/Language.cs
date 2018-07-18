namespace PasteBin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Language
    {
        public Language()
        {
            this.Pastes = new HashSet<Paste>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Tag { get; set; }

        public ICollection<Paste> Pastes { get; set; }
    }
}