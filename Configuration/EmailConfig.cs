using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
namespace ProductAPI.Configuration; 

public static class EmailConfig {
    
    public static void AddEmailConfiguration(this IServiceCollection services,  IConfiguration configuration)
    {
        services
            .AddFluentEmail(configuration["Email:Sender"])
            .AddRazorRenderer()
            .AddSmtpSender(new SmtpClient(configuration["Email:Server"])
            {
                UseDefaultCredentials = false,
                Port = Convert.ToInt32(configuration["Email:Port"]) ,
                Credentials = new NetworkCredential(configuration["Email:UserName"], configuration["Email:Password"]),
                EnableSsl = true,
            });
    }
    
}