namespace PasteBin.ViewModels.PasteViewModels
{
    using AutoMapper;

    using PasteBin.Models;
    using PasteBin.Config.Mapping;

    public class PasteViewModel : IMapFrom<Paste>, IHaveCustomMappings
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string User { get; set; }

        public string Language { get; set; }

        public void CreateMappings(IMapperConfigurationExpression config)
        {
            config.CreateMap<Paste, PasteViewModel>().ForMember(m => m.User, opt => opt.MapFrom(u => u.User.UserName));
            config.CreateMap<Paste, PasteViewModel>().ForMember(m => m.Language, opt => opt.MapFrom(u => u.Language.Name));
        }
    }
}