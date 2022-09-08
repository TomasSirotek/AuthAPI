using AuthAPI.Domain.Models;
using FluentEmail.Core.Models;

namespace AuthAPI.ExternalServices; 

public interface IEmailService {
    
    void SendEmail(EmailModel emailModel);
}