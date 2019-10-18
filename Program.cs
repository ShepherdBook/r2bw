using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Sentry;

namespace r2bw
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (SentrySdk.Init("https://f9dc3b2f25a14cfeb3f38841cb42c7fe@sentry.io/1784237"))
            {
                CreateWebHostBuilder(args).Build().Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
