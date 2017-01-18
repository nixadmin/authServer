using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AuthServerDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Auth server (Identity4)";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
