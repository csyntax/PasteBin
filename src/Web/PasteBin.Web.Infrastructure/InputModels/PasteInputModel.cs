namespace PasteBin.Web.Infrastructure.InputModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using System.Collections.Generic;

    public class PasteInputModel
    {
        public string Title {get; set;}

        public string Content { get; set; }

        public int Language { get; set; }

        public IEnumerable<SelectListItem> Languages { get; set; }
    }
}
