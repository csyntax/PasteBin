namespace PasteBin.Web.Infrastructure.ViewModels.Pastes
{
    using AutoMapper;

    using Data.Models;
    using Infrastructure.Mapping;

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