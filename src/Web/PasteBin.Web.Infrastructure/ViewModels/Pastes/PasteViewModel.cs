namespace PasteBin.Web.Infrastructure.ViewModels.Pastes
{
    using System;
    using AutoMapper;

    using Data.Models;
    using Infrastructure.Mapping;

    public class PasteViewModel : IMapFrom<Paste>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public LanguageViewModel Language { get; set; }

        public string User { get; set; }

        public double Bytes {get; set;}

        public DateTime Date { get; set; }

        public void CreateMappings(IMapperConfigurationExpression config)
        {
            config
                   .CreateMap<Paste, PasteViewModel>()
                        .ForMember(m => m.User, c => c.MapFrom(p => p.User.UserName))
                        .ForMember(m => m.Bytes, c => c.MapFrom(p => (double) (p.Content.Length * sizeof(char)) / 1024f));
        }
    }
}