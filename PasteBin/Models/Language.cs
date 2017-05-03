using System.ComponentModel.DataAnnotations;

namespace PasteBin.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }
    }
}