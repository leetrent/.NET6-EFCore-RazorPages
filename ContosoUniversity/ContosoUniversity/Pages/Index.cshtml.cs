using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ContosoUniversity.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
        }

        public void OnGet()
        {
            string logSnippet = "[Index][GET] => ";

            string connectionString = _configuration.GetConnectionString("SchoolContext");

            Console.WriteLine();
            Console.WriteLine($"{logSnippet} (connectionString): '{connectionString}'");
            Console.WriteLine();

            string appsettingsDirectory = _configuration.GetValue<string>("APPSETTINGS_DIRECTORY");

            Console.WriteLine();
            Console.WriteLine($"{logSnippet} (appsettingsDirectory): '{appsettingsDirectory}'");
            Console.WriteLine();
        }
    }
}