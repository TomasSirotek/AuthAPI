namespace ProductAPI.Configuration.Token;

public interface IJwtToken {
    public string CreateToken(List<string> roles, string userId, double duration);
}