namespace PasteBin.Web.Infrastructure.ViewModels.Pastes
{ 
    using Data.Models;
    using Infrastructure.Mapping;

    public class LanguageViewModel : IMapFrom<Language>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }
    }
}