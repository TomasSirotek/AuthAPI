
using AuthAPI.Domain.Models;
using FluentEmail.Core;


namespace AuthAPI.ExternalServices {
    
    public class EmailService : IEmailService{
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        public  EmailService(IConfiguration config,IServiceProvider serviceProvider) {
            _config = config;
            _serviceProvider = serviceProvider;
        }
        
        public void SendEmail(EmailModel emailModel)
        {
            using var scope = _serviceProvider.CreateScope();
            var mailer = scope.ServiceProvider.GetRequiredService<IFluentEmail>();
            
            var email = mailer
                .To(emailModel.EmailTo)
                .Subject(emailModel.Subject)
                .UsingTemplateFromFile(
                    $"{Directory.GetCurrentDirectory()}/wwwroot/EmailTemplates/ConfirmEmailTemplate.cshtml",new
                    {
                        Name = emailModel.Name,
                        Token = emailModel.Body
                    });
          
            email.SendAsync();
        
        }
    }

}