using AuthAPI.Domain.BindingModels;
using AuthAPI.Domain.Models;

namespace AuthAPI.Configuration.Token; 

public interface IVerifyJwtToken {
    Task<TokenResponse> VerifyAndGenerateToken(TokenRequest tokenRequest);
}