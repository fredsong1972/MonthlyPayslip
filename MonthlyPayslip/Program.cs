using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonthlyPayslip.Config;
using MonthlyPayslip.Models;
using MonthlyPayslip.Repositories;
using MonthlyPayslip.Services;
using Serilog;

namespace MonthlyPayslip
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                throw new Exception("Invalid Arguments");
            }

            var employee = new Employee
            {
                Name = args[0],
                AnnualSalary = int.Parse(args[1])
            };
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            IPayslipService payslipService = serviceProvider.GetService<IPayslipService>();
            var payslip = payslipService.GenerateMonthlyPayslip(employee);
            Console.WriteLine($"Monthly Payslip for: {payslip.EmployeeName}");
            Console.WriteLine($"Gross Monthly Income: ${payslip.GrossMonthlyIncome}");
            Console.WriteLine($"Monthly Income Tax: ${payslip.MonthlyIncomeTax}");
            Console.WriteLine($"Net Monthly Income: ${payslip.NetMonthlyIncome}");
        }
        private static IConfigurationRoot Configuration { get; set; }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger>(Log.Logger)
                .AddTransient<IPayslipService, PayslipService>()
                .AddTransient<IDataContext, DataContext>()
                .AddTransient<IRepository, Repository>();
            services.Configure<DataSourceSettings>(Configuration);
        }
    }
}
