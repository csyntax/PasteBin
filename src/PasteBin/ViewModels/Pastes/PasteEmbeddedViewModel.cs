namespace PasteBin.ViewModels.Pastes
{
    using AutoMapper;

    using PasteBin.Models;
    using PasteBin.Config.Mapping;

    public class PasteEmbeddedViewModel : IMapFrom<Paste>, IHaveCustomMappings
    {
        public string Content { get; set; }

        public string Tag { get; set; }

        public void CreateMappings(IMapperConfigurationExpression config)
        {
            config.CreateMap<Paste, PasteEmbeddedViewModel>().ForMember(m => m.Tag, c => c.MapFrom(p => p.Language.Tag));
        }
    }
}