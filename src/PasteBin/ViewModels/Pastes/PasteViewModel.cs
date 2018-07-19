namespace PasteBin.ViewModels.Pastes
{
    using System;

    using PasteBin.Models;
    using PasteBin.Config.Mapping;
    using AutoMapper;

    public class PasteViewModel : IMapFrom<Paste>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Language { get; set; }

        public DateTime Date { get; set; }

        public void CreateMappings(IMapperConfigurationExpression config)
        {
            config.CreateMap<Paste, PasteViewModel>().ForMember(m => m.Language, c => c.MapFrom(post => post.Language.Name));
        }
    }
}