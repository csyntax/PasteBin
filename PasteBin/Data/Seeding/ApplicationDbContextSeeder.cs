namespace PasteBin.Data.Seeding
{
    using System.Linq;
    using System.Collections.Generic;

    using PasteBin.Models;

    public class ApplicationDbContextSeeder
    {
        public static void Seed(ApplicationDbContext dbContext)
        {
            if (dbContext.Languages.Any())
            {
                return;
            }

            var languages = new List<Language>
            {
                new Language { Name = "Apache", Tag = "apache" },
                new Language { Name = "Bash", Tag = "bash" },
                new Language { Name = "C#", Tag = "csharp" },
                new Language { Name = "C++", Tag = "cpp" },
                new Language { Name = "CSS", Tag = "css" },
                new Language { Name = "CoffeeScript", Tag = "coffeescript" },
                new Language { Name = "Diff", Tag = "diff" },
                new Language { Name = "HTML, XML", Tag = "html" },
                new Language { Name = "HTTP", Tag = "http" },
                new Language { Name = "Ini", Tag = "ini" },
                new Language { Name = "JSON", Tag = "json" },
                new Language { Name = "Java", Tag = "java" },
                new Language { Name = "JavaScript", Tag = "javascript" },
                new Language { Name = "Makefile", Tag = "makefile" },
                new Language { Name = "Markdown", Tag = "markdown" },
                new Language { Name = "Nginx", Tag = "nginx" },
                new Language { Name = "Objective-C", Tag = "objectivec" },
                new Language { Name = "PHP", Tag = "php" },
                new Language { Name = "Perl", Tag = "perl" },
                new Language { Name = "Python", Tag = "python" },
                new Language { Name = "Ruby", Tag = "ruby" },
                new Language { Name = "SQL", Tag = "sql" }
            };

            foreach (var language in languages)
            {
                dbContext.Languages.Add(language);
                dbContext.SaveChanges();
            }
        }
    }
}