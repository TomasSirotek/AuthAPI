using ProductAPI.Domain.BindingModels;
using ProductAPI.Domain.Models;

namespace ProductAPI.Configuration.Token; 

public interface IVerifyJwtToken {
    Task<TokenResponse> VerifyAndGenerateToken(TokenRequest tokenRequest);
}