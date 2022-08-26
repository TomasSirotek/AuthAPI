using FluentEmail.Core.Models;
using ProductAPI.Domain.Models;

namespace ProductAPI.ExternalServices; 

public interface IEmailService {
    
    void SendEmail(EmailModel emailModel);
}