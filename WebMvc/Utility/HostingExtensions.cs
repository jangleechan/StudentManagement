using WebMvc.Models;

namespace WebMvc.Utility
{
    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
           
            builder.Services.AddSingleton<IStudentRepository, MockStudentRepository>();
            return builder.Build();
        }
    }

}
