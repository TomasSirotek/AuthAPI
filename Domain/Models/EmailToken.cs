namespace ProductAPI.Domain.Models; 

public class EmailToken {

    public string Id { get; set; }

    public string UserId { get; set; }

    public string Token { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public bool IsUsed { get; set; }
    
    public EmailToken(string id, string userId, string token, DateTime createdAt,bool isUsed)
    {
        Id = id;
        UserId = userId;
        Token = token;
        CreatedAt = createdAt;
        IsUsed = isUsed;
    }
    public EmailToken()
    {
    }
   
}