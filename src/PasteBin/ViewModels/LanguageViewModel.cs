namespace PasteBin.ViewModels
{
    using PasteBin.Models;
    using PasteBin.Config.Mapping;

    public class LanguageViewModel : IMapFrom<Language>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }
    }
}