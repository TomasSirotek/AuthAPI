using System.Security.Principal;
using Newtonsoft.Json;

namespace ProductAPI.Identity.Models {
    public class AppUser {
        public string Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Email { get; set; }
        
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        
        public List<UserRole> Roles { get; set; }

        public string PasswordHash { get; set; }

        public bool IsActivated { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
       

        public AppUser(string id, string firstName, string lastName, string email, string passwordHash, bool isActivated)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            IsActivated = isActivated;
        }

        public AppUser()
        { }
    }
}
